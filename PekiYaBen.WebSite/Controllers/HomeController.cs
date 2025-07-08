using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PekiYaBen.WebSite.Extensions;
using PekiYaBen.WebSite.Helpers;
using PekiYaBen.WebSite.Models;
using PekiYaBen.WebSite.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace PekiYaBen.WebSite.Controllers
{
    public class HomeController : Controller
    {
        PekiYaBenDBEntities entities = new PekiYaBenDBEntities();
        int linkPerPage = 4;

        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult Thumbnail(string path, int width, int height)
        {
            // TODO: the filename could be passed as argument of course
            var imageFile = Server.MapPath("~/") + path.Replace("/", "\\");


            //var imageFile = Server.MapPath("~/") + path.Replace("/", "\\");


            //using (var srcImage = Image.FromFile(imageFile))
            //using (var webPFileStream = new System.IO.MemoryStream())
            //{
            //    using (var imageFactory = new ImageFactory(preserveExifData: false))
            //    {
            //        imageFactory.Load(srcImage)
            //                .Format(new WebPFormat())
            //                .Quality(100)
            //                .Save(webPFileStream);

            //        return File(webPFileStream.ToArray(), "image/webp");
            //    }
            //}



            using (var srcImage = Image.FromFile(imageFile))
            using (var newImage = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(newImage))
            using (var stream = new MemoryStream())
            using (var imageFactory = new ImageFactory(preserveExifData: false))
            {
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(srcImage, new Rectangle(0, 0, width, height));
                newImage.Save(stream, ImageFormat.Png);

                imageFactory.Load(newImage)
                            .Format(new WebPFormat())
                            .Quality(100)
                            .Save(stream);


                return File(stream.ToArray(), "image/webp");
            }
        }

        [HttpGet]
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult Index()
        {

            var coaches = entities.Coaches.Where(x => x.ShowMainPage == true).OrderByDescending(x => x.Status).ThenBy(x => x.FullName).ToList();

            var coacheList = coaches.Where(x => x.Status == 1).ToList();

            var products = GeneralHelpers.GetProductList();

            ViewBag.Products = products;

            var lastContent = GeneralHelpers.GetContentList().OrderByDescending(x => x.CreateDate).FirstOrDefault(x => x.Status == 1);
            ViewBag.LastContent = lastContent;

            var selectedCategory = entities.ContentCategories.FirstOrDefault(x => x.Id == lastContent.CategoryId && x.Status == 1);
            ViewBag.SelectedCategory = selectedCategory;

            var lastContentCoach = coaches.FirstOrDefault(x => x.Id == lastContent.CoachId);
            ViewBag.LastContent = lastContent;
            ViewBag.LastContentCoach = lastContentCoach;

            var parameters = entities.GeneralParameters.ToList();
            ViewBag.KoclukSaati = parameters.Where(x => x.Parameter == "KoclukSaati").Select(x => x.Value).FirstOrDefault();
            ViewBag.Kullanici = parameters.Where(x => x.Parameter == "Kullanici").Select(x => x.Value).FirstOrDefault();
            ViewBag.SaatAtolye = parameters.Where(x => x.Parameter == "SaatAtolye").Select(x => x.Value).FirstOrDefault();
            ViewBag.Ulke = parameters.Where(x => x.Parameter == "Ulke").Select(x => x.Value).FirstOrDefault();
            ViewBag.MainPageVideo = parameters.Where(x => x.Parameter == "MainPageVideo").Select(x => x.Value).FirstOrDefault();
            ViewBag.AnaSayfaBaslik = parameters.Where(x => x.Parameter == "AnaSayfaBaslik").Select(x => x.Value).FirstOrDefault();
            ViewBag.AnaSayfaAltAciklama = parameters.Where(x => x.Parameter == "AnaSayfaAltAciklama").Select(x => x.Value).FirstOrDefault();
            return View(coacheList);
        }


        [Route("PekiYaben")]
        public ActionResult About()
        {
            return View();
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        [Route("Kocluk-Ekibimiz")]
        [Route("Kocluk-Ekibimiz/{id}/{title}")]
        public ActionResult Coaches(int? id, string title = "")
        {
            var products = GeneralHelpers.GetProductList().Where(x => x.Type == 3 && !x.Title.Contains("Atölye") && !x.Title.Contains("Sertifika Programı") && x.Status == 1).OrderBy(x => x.SortOrder).ToList();
            ViewBag.Products = products;

            Product selectedProduct;
            if (id == null)
                selectedProduct = products.FirstOrDefault();
            else
                selectedProduct = products.FirstOrDefault(x => x.Id == id);
            ViewBag.SelectedProduct = selectedProduct;

            if (!string.IsNullOrEmpty(title) && selectedProduct.Title.FriendlyUrl() != title)
                return Redirect("/Hata");

            var coaches = entities.Coaches.Where(x => x.CoachingInfo.Contains(selectedProduct.Title) && x.Status == 1).OrderByDescending(x => x.Status).ThenBy(x => x.FullName).ToList();

            List<Comment> comments = new List<Comment>();
            if (id == null)
                comments = entities.Comments.OrderByDescending(x => x.CreatedDate).ToList();
            else
                comments = entities.Comments.Where(x => x.ProductId == id).OrderByDescending(x => x.CreatedDate).ToList();

            ViewBag.Comments = comments;

            return View(coaches);
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        [Route("Kocluk-Ekibimiz/{id}/{title}/{coachId}/{coachName}")]
        public ActionResult Coach(int id, string title, int coachId, string coachName)
        {
            var products = GeneralHelpers.GetProductList().Where(x => x.Type == 3 && x.Status == 1).OrderBy(x => x.SortOrder).ToList();
            ViewBag.Products = products;

            Product selectedProduct;
            selectedProduct = products.FirstOrDefault(x => x.Id == id);

            if (selectedProduct.Title.FriendlyUrl() != title)
                return Redirect("/Hata");


            var contents = GeneralHelpers.GetContentList().Where(x => x.Status == 1 && x.CoachId == coachId).ToList();

            ViewBag.VideoCount = contents.Where(x => x.Type == 1).Count();
            ViewBag.BlogCount = contents.Where(x => x.Type == 0).Count();
            ViewBag.PodcastCount = contents.Where(x => x.Type == 2).Count();
            ViewBag.SelectedProduct = selectedProduct;

            List<Comment> comments = new List<Comment>();
            if (coachId > 0)
                comments = entities.Comments.Where(x => x.CoachId == coachId).OrderByDescending(x => x.CreatedDate).ToList();

            ViewBag.Comments = comments;
            ViewBag.CommentCount = comments.Count;

            var coach = entities.Coaches.FirstOrDefault(x => x.Id == coachId);
            if (coach.FullName.FriendlyUrl() != coachName)
                return Redirect("/Hata");

            return View(coach);
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        [Route("atolye/{id}/{title}")]
        [Route("kocluk/{id}/{title}")]
        [Route("sertifika/{id}/{title}")]
        public ActionResult Product(int id, string title)
        {
            var products = GeneralHelpers.GetProductList().Where(x => x.Type == 3 && x.Status == 1).OrderBy(x => x.SortOrder).ToList();
            ViewBag.Products = products;
            Product selectedProduct = entities.Products.FirstOrDefault(x => x.Id == id);
            if (selectedProduct.Title.FriendlyUrl() != title)
                return Redirect("/Hata");

            List<Content> cntLst = new List<Content>();

            List<Content> contents = GeneralHelpers.GetContentList().Where(x => x.Status == 1).ToList();
            foreach (var item in contents)
            {
                if (!String.IsNullOrEmpty(item.ProductIds))
                {
                    List<int> prd = JsonConvert.DeserializeObject<List<int>>(item.ProductIds);
                    Boolean bl = prd.Exists(y => y == id);
                    if (bl) { cntLst.Add(item); }
                    if (cntLst.Count >= 10) { continue; }
                }
            }

            var coaches = entities.Coaches.OrderByDescending(x => x.Status).ThenBy(x => x.FullName).ToList();
            ViewBag.Coaches = coaches;
            ViewBag.Contents = cntLst;

            return View(selectedProduct);
        }

        [Route("Kisisel-Durum-Analizi")]
        public ActionResult KDA()
        {
            return View();
        }

        [Route("Kisisel-Strateji-Belirleme")]
        public ActionResult KSB()
        {
            return View();
        }

        [Route("Bilincalti-Uygulamalari")]
        public ActionResult BU()
        {
            return View();
        }

        [Route("Kocluk-Gorusmeleri")]
        public ActionResult KG()
        {
            return View();
        }

        [Route("Sikca-Sorulanlar")]
        public ActionResult FAQ()
        {
            var faq = entities.FAQs.Where(x => x.Status == 1).OrderBy(x => x.SortOrder).ToList();
            return View(faq);
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        [Route("Kisisel-Gelisim/Yazilar")]
        [Route("Kisisel-Gelisim/Yazilar/{page}")]
        [Route("Kisisel-Gelisim/Yazilar/{id}/{title}")]
        [Route("Kisisel-Gelisim/Yazilar/{id}/{title}/{page}")]
        [Route("Kisisel-Gelisim/Yazilar/{yazarid}/yazar_{yazaradi}")]
        [Route("Kisisel-Gelisim/Yazilar/{id}/{title}/{yazarid}/yazar_{yazaradi}")]
        [Route("Kisisel-Gelisim/Yazilar/{id}/{title}/{page}/{yazarid}/yazar_{yazaradi}")]

        public ActionResult Blog(int? id, int? yazarid, string yazarAdi, string title = "", int page = 1)
        {
            var categories = entities.ContentCategories.Where(x => x.Type == 0 && x.Status == 1).OrderBy(x => x.Title).ToList();

            ContentCategory selectedContent;
            if (id == null)
                selectedContent = categories.FirstOrDefault();
            else
                selectedContent = categories.FirstOrDefault(x => x.Id == id);

            ViewBag.SelectedContent = selectedContent;
            ViewBag.Categories = categories;
            ViewBag.Type = 0;

            var coaches = entities.Coaches.OrderByDescending(x => x.Status).ThenBy(x => x.FullName).ToList();
            ViewBag.Coaches = coaches;

            List<Content> blogs = new List<Content>();
            if (yazarid != null)
            {
                blogs = GeneralHelpers.GetContentList().Where(x => x.CategoryId == selectedContent.Id && x.Type == 0 && x.Status == 1 && x.CoachId == yazarid).ToList();
                ViewBag.YazarId = yazarid;
                ViewBag.YazarAdi = coaches.Where(x => x.Id == yazarid).FirstOrDefault().FullName;
            }
            else
            {
                blogs = GeneralHelpers.GetContentList().Where(x => x.CategoryId == selectedContent.Id && x.Type == 0 && x.Status == 1).ToList();
                ViewBag.YazarId = null;
                ViewBag.YazarAdi = null;
            }

            //foreach (var blog in blogs)
            //{
            //    System.IO.File.WriteAllBytes(Server.MapPath("/") + "\\Content\\Assets\\img\\blog\\" + blog.Title.FriendlyUrl() + ".png", Convert.FromBase64String(blog.Image.Replace("data:image/png;base64,", "")));
            //    blog.Image = "/Content/Assets/img/blog/" + blog.Title.FriendlyUrl() + ".png";
            //    entities.SaveChanges();
            //}

            ViewBag.PageCount = blogs.Count();
            ViewBag.Page = page;

            string pageLink = "";
            if (id > 0 && !string.IsNullOrEmpty(title))
                pageLink = $"/Kisisel-Gelisim/Yazilar/{id}/{title}/";
            else
                pageLink = $"/Kisisel-Gelisim/Yazilar/";

            ViewBag.PageLink = pageLink;

            return View(blogs.OrderByDescending(x => x.CreateDate).Skip((page - 1) * linkPerPage).Take(linkPerPage).ToList());
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        [Route("Kisisel-Gelisim/Videolar")]
        [Route("Kisisel-Gelisim/Videolar/{page}")]
        [Route("Kisisel-Gelisim/Videolar/{id}/{title}")]
        [Route("Kisisel-Gelisim/Videolar/{id}/{title}/page")]
        [Route("Kisisel-Gelisim/Videolar/{yazarid}/yazar_{yazaradi}")]
        [Route("Kisisel-Gelisim/Videolar/{id}/{title}/{yazarid}/yazar_{yazaradi}")]
        [Route("Kisisel-Gelisim/Videolar/{id}/{title}/{page}/{yazarid}/yazar_{yazaradi}")]
        public ActionResult Video(int? id, int? yazarid, string yazarAdi, string title = "", int page = 1)
        {
            var categories = entities.ContentCategories.Where(x => x.Type == 1 && x.Status == 1).OrderBy(x => x.Title).ToList();

            ContentCategory selectedContent;
            if (id == null)
                selectedContent = categories.FirstOrDefault();
            else
                selectedContent = categories.FirstOrDefault(x => x.Id == id);

            ViewBag.SelectedContent = selectedContent;
            ViewBag.Categories = categories;
            ViewBag.Type = 1;

            var coaches = entities.Coaches.OrderByDescending(x => x.Status).ThenBy(x => x.FullName).ToList();
            ViewBag.Coaches = coaches;

            List<Content> blogs = new List<Content>();
            if (yazarid != null)
            {
                blogs = GeneralHelpers.GetContentList().Where(x => x.CategoryId == selectedContent.Id && x.Type == 1 && x.Status == 1 && x.CoachId == yazarid).ToList();
                ViewBag.YazarId = yazarid;
                ViewBag.YazarAdi = coaches.Where(x => x.Id == yazarid).FirstOrDefault().FullName;
            }
            else
            {
                blogs = GeneralHelpers.GetContentList().Where(x => x.CategoryId == selectedContent.Id && x.Type == 1 && x.Status == 1).ToList();
                ViewBag.YazarId = null;
                ViewBag.YazarAdi = null;
            }

            //foreach (var blog in blogs)
            //{
            //    System.IO.File.WriteAllBytes(Server.MapPath("/") + "\\Content\\Assets\\img\\blog\\" + blog.Title.FriendlyUrl() + ".png", Convert.FromBase64String(blog.Image.Replace("data:image/png;base64,", "")));
            //    blog.Image = "/Content/Assets/img/blog/" + blog.Title.FriendlyUrl() + ".png";
            //    entities.SaveChanges();
            //}

            ViewBag.PageCount = blogs.Count();
            ViewBag.Page = page;

            string pageLink = "";
            if (id > 0 && !string.IsNullOrEmpty(title))
                pageLink = $"/Kisisel-Gelisim/Videolar/{id}/{title}/";
            else
                pageLink = $"/Kisisel-Gelisim/Videolar/";

            ViewBag.PageLink = pageLink;
            return View("Blog", blogs.OrderByDescending(x => x.CreateDate).Skip((page - 1) * linkPerPage).Take(linkPerPage).ToList());

        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        [Route("Kisisel-Gelisim/Podcastler")]
        [Route("Kisisel-Gelisim/Podcastler/{page}")]
        [Route("Kisisel-Gelisim/Podcastler/{id}/{title}")]
        [Route("Kisisel-Gelisim/Podcastler/{id}/{title}/{page}")]
        [Route("Kisisel-Gelisim/Podcastler/{yazarid}/yazar_{yazaradi}")]
        [Route("Kisisel-Gelisim/Podcastler/{id}/{title}/{yazarid}/yazar_{yazaradi}")]
        [Route("Kisisel-Gelisim/Podcastler/{id}/{title}/{page}/{yazarid}/yazar_{yazaradi}")]

        public ActionResult Podcast(int? id, int? yazarid, string yazarAdi, string title = "", int page = 1)
        {
            var categories = entities.ContentCategories.Where(x => x.Type == 2 && x.Status == 1).OrderBy(x => x.Title).ToList();

            ContentCategory selectedContent;
            if (id == null)
                selectedContent = categories.FirstOrDefault();
            else
                selectedContent = categories.FirstOrDefault(x => x.Id == id);

            ViewBag.SelectedContent = selectedContent;
            ViewBag.Categories = categories;
            ViewBag.Type = 2;

            var coaches = entities.Coaches.OrderByDescending(x => x.Status).ThenBy(x => x.FullName).ToList();
            ViewBag.Coaches = coaches;

            List<Content> blogs = new List<Content>();
            if (yazarid != null)
            {
                blogs = GeneralHelpers.GetContentList().Where(x => x.CategoryId == selectedContent.Id && x.Type == 2 && x.Status == 1 && x.CoachId == yazarid).ToList();
                ViewBag.YazarId = yazarid;
                ViewBag.YazarAdi = coaches.Where(x => x.Id == yazarid).FirstOrDefault().FullName;
            }
            else
            {
                blogs = GeneralHelpers.GetContentList().Where(x => x.CategoryId == selectedContent.Id && x.Type == 2 && x.Status == 1).ToList();
                ViewBag.YazarId = null;
                ViewBag.YazarAdi = null;
            }

            //foreach (var blog in blogs)
            //{
            //    System.IO.File.WriteAllBytes(Server.MapPath("/") + "\\Content\\Assets\\img\\blog\\" + blog.Title.FriendlyUrl() + ".png", Convert.FromBase64String(blog.Image.Replace("data:image/png;base64,", "")));
            //    blog.Image = "/Content/Assets/img/blog/" + blog.Title.FriendlyUrl() + ".png";
            //    entities.SaveChanges();
            //}

            ViewBag.PageCount = blogs.Count();
            ViewBag.Page = page;

            string pageLink = "";
            if (id > 0 && !string.IsNullOrEmpty(title))
                pageLink = $"/Kisisel-Gelisim/Videolar/{id}/{title}/";
            else
                pageLink = $"/Kisisel-Gelisim/Videolar/";

            ViewBag.PageLink = pageLink;
            return View("Blog", blogs.OrderByDescending(x => x.CreateDate).Skip((page - 1) * linkPerPage).Take(linkPerPage).ToList());
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        [Route("Kisisel-Gelisim/Yazilar/{id}/{title}/{contentId}/{contentTitle}")]

        public ActionResult BlogDetail(int? id, string title, int contentId, string contentTitle)
        {
            var categories = entities.ContentCategories.Where(x => x.Type == 0 && x.Status == 1).OrderBy(x => x.Title).ToList();

            ContentCategory selectedContent;
            if (id == null)
                selectedContent = categories.FirstOrDefault();
            else
                selectedContent = categories.FirstOrDefault(x => x.Id == id);

            if (selectedContent.Title.FriendlyUrl() != title)
                return Redirect("/Hata");

            ViewBag.SelectedContent = selectedContent;
            ViewBag.Categories = categories;

            var coaches = entities.Coaches.OrderByDescending(x => x.Status).ThenBy(x => x.FullName).ToList();
            ViewBag.Coaches = coaches;

            var content = entities.Contents.FirstOrDefault(x => x.Id == contentId && x.Type == 0 && x.Status == 1);
            if (content.Title.FriendlyUrl() != contentTitle)
                return Redirect("/Hata");

            return View(content);
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        [Route("Kisisel-Gelisim/Videolar/{id}/{title}/{contentId}/{contentTitle}")]
        public ActionResult VideoDetail(int? id, string title, int contentId, string contentTitle)
        {
            var categories = entities.ContentCategories.Where(x => x.Type == 1 && x.Status == 1).OrderBy(x => x.Title).ToList();

            ContentCategory selectedContent;
            if (id == null)
                selectedContent = categories.FirstOrDefault();
            else
                selectedContent = categories.FirstOrDefault(x => x.Id == id);

            if (selectedContent.Title.FriendlyUrl() != title)
                return Redirect("/Hata");

            ViewBag.SelectedContent = selectedContent;
            ViewBag.Categories = categories;

            var coaches = entities.Coaches.OrderByDescending(x => x.Status).ThenBy(x => x.FullName).ToList();
            ViewBag.Coaches = coaches;

            var content = entities.Contents.FirstOrDefault(x => x.Id == contentId && x.Type == 1 && x.Status == 1);
            if (content.Title.FriendlyUrl() != contentTitle)
                return Redirect("/Hata");

            return View("BlogDetail", content);
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        [Route("Kisisel-Gelisim/Podcastler/{id}/{title}/{contentId}/{contentTitle}")]
        public ActionResult PodcastDetail(int? id, string title, int contentId, string contentTitle)
        {
            var categories = entities.ContentCategories.Where(x => x.Type == 2 && x.Status == 1).OrderBy(x => x.Title).ToList();

            ContentCategory selectedContent;
            if (id == null)
                selectedContent = categories.FirstOrDefault();
            else
                selectedContent = categories.FirstOrDefault(x => x.Id == id);

            if (selectedContent.Title.FriendlyUrl() != title)
                return Redirect("/Hata");

            ViewBag.SelectedContent = selectedContent;
            ViewBag.Categories = categories;

            var coaches = entities.Coaches.OrderByDescending(x => x.Status).ThenBy(x => x.FullName).ToList();
            ViewBag.Coaches = coaches;

            var content = entities.Contents.FirstOrDefault(x => x.Id == contentId && x.Type == 2 && x.Status == 1);
            if (content.Title.FriendlyUrl() != contentTitle)
                return Redirect("/Hata");

            return View("BlogDetail", content);
        }

        [Route("Iletisim")]
        public ActionResult Contact()
        {
            return View();
        }

        [Route("Iletisim")]
        [HttpPost]
        public ActionResult Contact(FormData request)
        {
            try
            {

                var mailbody = $"Ad Soyad : {request.Name}<br/>E-posta : {request.Email}<br>Telefon : {request.Phone}<br/>Mesaj : {request.Message}";
                MailHelper.SendMail("iletisim@pekiyaben.com", "Peki Ya Ben İletişim Formu", mailbody);
                MailHelper.SendMail("fzeylan@yahoo.com", "Peki Ya Ben İletişim Formu", mailbody);
                ViewBag.Message = "Mesajınız başarılı bir şekilde iletildi.";
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Mesaj gönderimi sırasında hata oluştu.Lütfen form verilerini kontrol ediniz.";
                return View();
            }
        }

        [Route("Hata")]
        public ViewResult Error()
        {
            Response.StatusCode = 404;
            return View("Error");
        }


        [OutputCache(Duration = 600, VaryByParam = "*")]
        [Route("Yazar-Yorumlari/{id}/{title}/{coachId}/{coachName}")]
        public ActionResult Comment(int id, string title, int coachId, string coachName)
        {
            var products = GeneralHelpers.GetProductList().Where(x => x.Type == 3 && x.Status == 1).OrderBy(x => x.SortOrder).ToList();

            Coach selectedCoach =
            selectedCoach = entities.Coaches.FirstOrDefault(x => x.Id == coachId);
            if (selectedCoach == null)
                return Redirect("/Hata");


            Product selectedProduct;
            selectedProduct = products.FirstOrDefault(x => x.Id == id);

            if (selectedProduct.Title.FriendlyUrl() != title)
                return Redirect("/Hata");


            ViewBag.SelectedProduct = selectedProduct;


            ViewBag.SelectedCoach = selectedCoach;
            ViewBag.Products = products;

            List<Comment> comments = new List<Comment>();

            comments = entities.Comments.Where(x => x.CoachId == coachId).OrderByDescending(x => x.CreatedDate).ToList();

            return View(comments);

        }



    }
}