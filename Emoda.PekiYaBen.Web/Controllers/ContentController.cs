using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Queries;
using Emoda.PekiYaBen.Web.Models.DataTable;
using Emoda.PekiYaBen.Entity.Content;
using OLCA.Infrastructure.CQS;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Emoda.PekiYaBen.Entity.Enums;
using Emoda.PekiYaBen.Business.Helpers;
using Emoda.PekiYaBen.Entity.Helpers;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Entity.Coaching;
using Newtonsoft.Json;
using System.IO;

namespace Emoda.PekiYaBen.Web.Controllers
{
    [Authorize]
    public class ContentController : Controller
    {
        private IQueryDispatcher queryDispatcher;
        private ICommandDispatcher commandDispatcher;

        public ContentController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        private string[] columns = new string[] {
            "Image",
            "Title",
            "Summary",
            "Category",
            "Status",
            "CreateDate"
        };

        public ActionResult Index(int id = 0)
        {
            ViewBag.Title = (General.ContentType)id + " Listesi";
            ViewBag.PageHeader = ViewBag.Title;
            ViewBag.ActivePage = "ContentList";

            return View(id);
        }

        public string GetContent(DataTableParameters dataParameter, int type)
        {
            GetContentListQuery query = new GetContentListQuery();
            query.Search = type.ToString();
            query.PageNumber = dataParameter.Start / dataParameter.Length + 1;
            query.PageSize = dataParameter.Length;

            StringBuilder sb = new StringBuilder();
            foreach (var order in dataParameter.Order)
            {
                sb.AppendFormat("{0} {1},", columns[order.Column], order.Dir);
            }
            int length = sb.Length;
            if (length > 0)
            {
                query.OrderBy = "ORDER BY " + sb.ToString().Substring(0, length - 1);
            }
            else
                query.OrderBy = "ORDER BY CreateDate desc";

            string json;
            try
            {
                var resultSet = new DataTableResultSet<ContentListItem>();
                var listResult = queryDispatcher.Dispatch<GetContentListQuery, QueryListResult<ContentListItem>>(query);
                resultSet.draw = dataParameter.Draw;
                resultSet.recordsTotal = listResult.Result.First().RecordCount;
                resultSet.recordsFiltered = resultSet.recordsTotal;
                resultSet.data = listResult.Result;
                //.Select(x => new ContentListItem
                //{
                //    Id = x.Id,
                //    CategoryId = x.CategoryId,
                //    Category = x.Category,
                //    Title = x.Title,
                //    Summary = x.Summary,
                //    Detail = x.Detail,
                //    Type = x.Type,
                //    CreateDate = x.CreateDate,
                //    Status = x.Status,
                //    Image = String.To
                //});

                json = resultSet.ToJSON();
            }
            catch (Exception ex)
            {
                DataTableResultError<ContentListItem> error = new DataTableResultError<ContentListItem>();
                error.error = "Bir hata oluştu. Lütfen sonra tekrar deneyiniz";
                json = error.ToJSON();
            }

            return json;
        }

        [HttpGet]
        public ActionResult Edit(int id = 0, int type = 0)
        {
            QuerySingleResult<ContentInfo> product = queryDispatcher.Dispatch<GetContentInfoQuery, QuerySingleResult<ContentInfo>>(new GetContentInfoQuery(id));
            var contentType = (product.Entity == null ? (General.ContentType)type : product.Entity.Type);
            ViewBag.Title = contentType + " Düzenle";
            ViewBag.PageHeader = ViewBag.Title;
            ViewBag.ActivePage = "ContentList";

            LoadPageMainLists(contentType);

            return View(product.Result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ContentInfo info, List<string> selectedObjects)
        {
            ContentInfo product;
            try
            {

                string productIds = String.Empty;
                if (selectedObjects != null)
                {
                    productIds = JsonConvert.SerializeObject(selectedObjects);
                }

                QuerySingleResult<ContentInfo> productQuery = queryDispatcher.Dispatch<GetContentInfoQuery, QuerySingleResult<ContentInfo>>(new GetContentInfoQuery(info.Id));
                product = (ContentInfo)productQuery.Result;

                var path = "";

                //C:\inetpub\wwwroot\pekiyaben.com\http\Content\assets\img\blog
                //C:\inetpub\wwwroot\pekiyaben.com\subdomains\yonetim\Content\

                path = Server.MapPath("/").Replace(@"subdomains\yonetim\", @"http\") + "content\\assets\\img\\blog\\" + info.Title.ToSEOFriendly(200) + ".png";

                ImageHelper.SaveImage(info.Image, path);

                var command = new UpdateContentCommand
                {
                    Id = info.Id,
                    Title = info.Title,
                    CategoryId = info.CategoryId,
                    CoachId = info.CoachId,
                    Type = info.Type,
                    Image = "/Content/assets/img/blog/" + info.Title.ToSEOFriendly(200) + ".png",
                    Summary = info.Summary,
                    Detail = info.Detail,
                    MetaDescription = info.MetaDescription,
                    MetaKeywords = info.MetaKeywords,
                    Status = info.Status,
                    ProductIds = productIds
                };

                product = (ContentInfo)commandDispatcher.Dispatch<UpdateContentCommand, ContentInfo>(command);

                LoadPageMainLists(info.Type);


            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", new { id = ex.Message });
            }
            return RedirectToAction("Edit", new { id = product.Id });
        }

        public void LoadPageMainLists(General.ContentType contentType)
        {
            DataAccess da = new DataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString);
            var categories = da.GetDataReader("Select Id, Title FROM ContentCategory WHERE Status=1 and Type=" + contentType.GetValue(), new List<SqlParameter> { });

            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (DataRow row in categories.Rows)
            {
                SelectListItem item = new SelectListItem();
                item.Text = row["Title"].ToString();
                item.Value = row["Id"].ToString();
                categoryList.Add(item);
            }

            GetProductInfoListQuery query = new GetProductInfoListQuery();
            query.PageSize = 100;
            var products = queryDispatcher.Dispatch<GetProductInfoListQuery, QueryListResult<ProductInfo>>(query).Result;

            ViewBag.Products = products.Where(x => x.Type == Entity.Enums.General.ProductType.Meeting).ToList();

            ViewBag.Type = contentType;
            ViewBag.Categories = categoryList;

            var coaches = da.GetDataReader("Select Id, FullName FROM Coach Order By FullName ASC", new List<SqlParameter> { });

            List<SelectListItem> coachList = new List<SelectListItem>();
            foreach (DataRow row in coaches.Rows)
            {
                SelectListItem item = new SelectListItem();
                item.Text = row["FullName"].ToString();
                item.Value = row["Id"].ToString();
                coachList.Add(item);
            }

            ViewBag.Coaches = coachList;
        }

        public ActionResult Categories()
        {
            ViewBag.Title = "Kategori Listesi";
            ViewBag.PageHeader = ViewBag.Title;
            ViewBag.ActivePage = "ContentList";

            return View();
        }

        public string GetCategories(DataTableParameters dataParameter)
        {
            GetCategoryListQuery query = new GetCategoryListQuery();
            query.PageNumber = dataParameter.Start / dataParameter.Length + 1;
            query.PageSize = dataParameter.Length;
            query.OrderBy = "ORDER BY Type ASC, Title ASC";
            string json;
            try
            {
                var resultSet = new DataTableResultSet<CategoryListItem>();
                var listResult = queryDispatcher.Dispatch<GetCategoryListQuery, QueryListResult<CategoryListItem>>(query);
                resultSet.draw = dataParameter.Draw;
                resultSet.recordsTotal = listResult.Result.First().RecordCount;
                resultSet.recordsFiltered = resultSet.recordsTotal;
                resultSet.data = listResult.Result;

                json = resultSet.ToJSON();
            }
            catch (Exception ex)
            {
                DataTableResultError<ContentListItem> error = new DataTableResultError<ContentListItem>();
                error.error = "Bir hata oluştu. Lütfen sonra tekrar deneyiniz";
                json = error.ToJSON();
            }

            return json;
        }

        [HttpGet]
        public ActionResult EditCategory(int id = 0)
        {
            QuerySingleResult<ContentCategoryInfo> category = queryDispatcher.Dispatch<GetContentCategoryInfoQuery, QuerySingleResult<ContentCategoryInfo>>(new GetContentCategoryInfoQuery(id));
            ViewBag.Title = "Kategori Düzenle";
            ViewBag.PageHeader = ViewBag.Title;
            ViewBag.ActivePage = "ContentList";

            return View(category.Result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditCategory(ContentCategoryInfo info)
        {
            ContentCategoryInfo product;
            try
            {
                QuerySingleResult<ContentCategoryInfo> productQuery = queryDispatcher.Dispatch<GetContentCategoryInfoQuery, QuerySingleResult<ContentCategoryInfo>>(new GetContentCategoryInfoQuery(info.Id));
                product = (ContentCategoryInfo)productQuery.Result;

                var command = new UpdateContentCategoryCommand
                {
                    Id = info.Id,
                    Title = info.Title,
                    Type = info.Type,
                    Status = info.Status
                };
                product = (ContentCategoryInfo)commandDispatcher.Dispatch<UpdateContentCategoryCommand, ContentCategoryInfo>(command);
            }
            catch (Exception ex)
            {
                return View();
            }

            return RedirectToAction("EditCategory", new { id = product.Id });
        }
    }
}
