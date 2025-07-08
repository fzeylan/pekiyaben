using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PekiYaBen.WebSite.Extensions;
using PekiYaBen.WebSite.Helpers;
using PekiYaBen.WebSite.Models;
using PekiYaBen.WebSite.Models.EntityModels;
using PekiYaBen.WebSite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using static PekiYaBen.WebSite.Enums.General;

namespace PekiYaBen.WebSite.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        PekiYaBenDBEntities entities = new PekiYaBenDBEntities();

        //string rootUrl = "http://localhost:63067";
        string rootUrl = "https://www.pekiyaben.com";

        [HttpGet]
        [AllowAnonymous]
        [Route("hesabim/giris")]
        public ActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated && returnUrl != null)
                return Redirect(returnUrl);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("applogin/")]
        public ActionResult AppLogin(string u, string t)
        {
            var user = entities.AppUsers.FirstOrDefault(x => x.Email == u && x.Token == t && x.Status);
            if (user != null) // is validated
            {
                if (!user.Status)
                {
                    TempData["Result"] = new MethodResult { Succeed = false, Message = "Hesabınız aktif değil. <br/>Bir hata olduğunu düşünüyorsanız lütfen iletişime geçiniz!" };
                    return RedirectToAction("Login", "Account");
                }

                //Sign In
                var now = DateTime.UtcNow.ToLocalTime();

                var ticket = new FormsAuthenticationTicket(
                    1,
                    user.Email,
                    now,
                    now.Add(FormsAuthentication.Timeout),
                    true,
                    user.Email,
                    FormsAuthentication.FormsCookiePath);

                var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                cookie.HttpOnly = true;
                if (ticket.IsPersistent)
                {
                    cookie.Expires = ticket.Expiration;
                }
                cookie.Secure = FormsAuthentication.RequireSSL;
                cookie.Path = FormsAuthentication.FormsCookiePath;
                if (FormsAuthentication.CookieDomain != null)
                {
                    cookie.Domain = FormsAuthentication.CookieDomain;
                }

                HttpContext.Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Result"] = new MethodResult { Succeed = false, Message = "Geçersiz kullanıcı bilgisi." };
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("hesabim/giris")]
        public ActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var password = HashHelper.ConvertToMd5x(model.Password);
                var user = entities.AppUsers.FirstOrDefault(x => x.Email == model.Email && x.Password == password && x.Status);
                if (user != null) // is validated
                {
                    if (!user.Status)
                    {
                        TempData["Result"] = new MethodResult { Succeed = false, Message = "Hesabınız aktif değil. <br/>Bir hata olduğunu düşünüyorsanız lütfen iletişime geçiniz!" };
                        return View("Login");
                    }

                    //Sign In
                    var now = DateTime.UtcNow.ToLocalTime();

                    var ticket = new FormsAuthenticationTicket(
                        1,
                        user.Email,
                        now,
                        now.Add(FormsAuthentication.Timeout),
                        true,
                        user.Email,
                        FormsAuthentication.FormsCookiePath);

                    var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    cookie.HttpOnly = true;
                    if (ticket.IsPersistent)
                    {
                        cookie.Expires = ticket.Expiration;
                    }
                    cookie.Secure = FormsAuthentication.RequireSSL;
                    cookie.Path = FormsAuthentication.FormsCookiePath;
                    if (FormsAuthentication.CookieDomain != null)
                    {
                        cookie.Domain = FormsAuthentication.CookieDomain;
                    }

                    HttpContext.Response.Cookies.Add(cookie);

                    if (!string.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Result"] = new MethodResult { Succeed = false, Message = "Kullanıcı adı veya şifreniz yanlış. Lütfen bilgileri kontrol ediniz." };
                    ViewData["ReturnUrl"] = returnUrl;
                    return View("Login");
                }
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult LoginMod(string email, string pass)
        {
            var password = HashHelper.ConvertToMd5x(pass);
            var user = entities.AppUsers.FirstOrDefault(x => x.Email == email && x.Password == password && x.Status);
            MethodResult result = new MethodResult();
            if (user != null) // is validated
            {
                if (!user.Status)
                {
                    result = new MethodResult { Succeed = false, Message = "Hesabınız aktif değil. <br/>Bir hata olduğunu düşünüyorsanız lütfen iletişime geçiniz!" };
                    return Json(result);
                }

                //Sign In
                var now = DateTime.UtcNow.ToLocalTime();

                var ticket = new FormsAuthenticationTicket(
                    1,
                    user.Email,
                    now,
                    now.Add(FormsAuthentication.Timeout),
                    true,
                    user.Email,
                    FormsAuthentication.FormsCookiePath);

                var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                cookie.HttpOnly = true;
                if (ticket.IsPersistent)
                {
                    cookie.Expires = ticket.Expiration;
                }
                cookie.Secure = FormsAuthentication.RequireSSL;
                cookie.Path = FormsAuthentication.FormsCookiePath;
                if (FormsAuthentication.CookieDomain != null)
                {
                    cookie.Domain = FormsAuthentication.CookieDomain;
                }

                HttpContext.Response.Cookies.Add(cookie);

                result = new MethodResult { Succeed = true, Message = "" };
                return Json(result);
            }
            else
            {
                result = new MethodResult { Succeed = false, Message = "Kullanıcı adı veya şifreniz yanlış. Lütfen bilgileri kontrol ediniz." };
                return Json(result);
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult SignMod(string fullName, string email, string pass, string newPass, bool agreement)
        {
            MethodResult result = new MethodResult();

            if (agreement == false)
            {
                result = new MethodResult { Succeed = false, Message = "Üyelik sözleşmesi onaylanmadan üye kaydı yapılamaz" };
                return Json(result);
            }

            if (string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(newPass))
            {
                result = new MethodResult { Succeed = false, Message = "Şifre ve tekrar alanları boş olamaz." };
                return Json(result);
            }

            if (string.IsNullOrEmpty(email))
            {
                result = new MethodResult { Succeed = false, Message = "E-posta alanı dolu olmalıdır." };
                return Json(result);
            }

            if (!IsValidMail(email))
            {
                result = new MethodResult { Succeed = false, Message = "Lütfen doğru bir mail adresi giriniz." };
                return Json(result);
            }

            if (pass != newPass)
            {
                result = new MethodResult { Succeed = false, Message = "Şifre ve tekrar alanları aynı olmalıdır." };
                return Json(result);
            }

            var user = entities.AppUsers.FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                result = new MethodResult { Succeed = false, Message = "Bu e-posta adresi kullanımda. Şifrenizi hatırlamıyorsanız şifre hatırlatma adımlarını takip edebilirsiniz." };
                return Json(result);
            }
            else
            {
                user = new AppUser
                {
                    FullName = fullName,
                    Email = email,
                    Password = HashHelper.ConvertToMd5x(pass),
                    RegisterDate = DateTime.Now,
                    ProfilePhoto = ImageExtensions.ImageFromFilePath(Server.MapPath("/Content/assets/img/profile.jpg")),
                    Status = true
                };

                user = entities.AppUsers.Add(user);
                entities.SaveChanges();
            }

            result = new MethodResult { Succeed = true, Message = "Kayıt işleminiz gerçekleşti." };
            return Json(result);
        }

        [HttpPost]
        public JsonResult SaveInvoice(int invoiceType, string personalID, string companyName, string taxOffice, string taxNumber, string invoiceAddress)
        {
            MethodResult result = new MethodResult();
            try
            {
                AppUser user = AuthenticationHelper.GetAuthenticatedUser();
                if (invoiceType == (int)InvoiceType.Corporate)
                {
                    if (string.IsNullOrEmpty(companyName) || string.IsNullOrEmpty(taxNumber) || string.IsNullOrEmpty(taxOffice))
                    {
                        result = new MethodResult { Succeed = false, Message = "Firma bilgilerini eksiksiz giriniz." };
                        return Json(result);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(personalID) || !TcKimlikDogrulama(personalID))
                    {
                        result = new MethodResult { Succeed = false, Message = "Geçersiz kimlik numarası" };
                        return Json(result);
                    }
                }

                if (string.IsNullOrEmpty(invoiceAddress))
                {
                    result = new MethodResult { Succeed = false, Message = "Fatura adresini eksiksiz giriniz." };
                    return Json(result);
                }

                var invoice = entities.InvoiceInfoes.FirstOrDefault(x => x.Id == user.Id);

                if (invoice == null)
                {
                    invoice = new InvoiceInfo
                    {
                        AppUserId = user.Id,
                        InvoiceType = invoiceType,
                        PersonalID = personalID,
                        CompanyName = companyName,
                        TaxNumber = taxNumber,
                        TaxOffice = taxOffice,
                        InvoiceAddress = invoiceAddress
                    };
                    entities.InvoiceInfoes.Add(invoice);

                    result = new MethodResult { Message = "Fatura bilgileriniz güncellendi", Succeed = true };
                    entities.SaveChanges();
                }
                else
                {
                    invoice.InvoiceType = invoiceType;
                    invoice.PersonalID = personalID;
                    invoice.CompanyName = companyName;
                    invoice.TaxNumber = taxNumber;
                    invoice.TaxOffice = taxOffice;
                    invoice.InvoiceAddress = invoiceAddress;

                    result = new MethodResult { Message = "Fatura bilgileriniz güncellendi", Succeed = true };
                    entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result = new MethodResult { Message = "Fatura bilgileri güncellenirken hata oluştu", Succeed = false };
            }
            return Json(result);

        }


        [HttpPost]
        public JsonResult FindInvoice()
        {
            AppUser user = AuthenticationHelper.GetAuthenticatedUser();

            MethodResult result = new MethodResult();
            result = new MethodResult { Message = "", Succeed = true };

            var invoiceInfo = entities.InvoiceInfoes.FirstOrDefault(x => x.AppUserId == user.Id);

            if (invoiceInfo == null)
                result = new MethodResult { Message = "", Succeed = false };

            return Json(result);
        }

        public bool IsValidMail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult AuthenticatedUser()
        {
            AppUser user = AuthenticationHelper.GetAuthenticatedUser();
            MethodResult result = new MethodResult();

            if (user == null)
            {
                result = new MethodResult { Succeed = false, Message = "Hesabınız aktif değil. <br/>Bir hata olduğunu düşünüyorsanız lütfen iletişime geçiniz!" };
                return Json(result);
            }
            else
            {
                result = new MethodResult { Succeed = true, Message = "" };
                return Json(result);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("hesabim/kayit-ol")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("hesabim/kayit-ol")]
        public ActionResult Register(RegisterUserViewModel request)
        {
            MethodResult result = new MethodResult();

            if (!request.ConfirmAgreement)
            {
                TempData["Result"] = new MethodResult { Succeed = false, Message = "Üyelik sözleşmesi onaylanmadan üye kaydı yapılamaz" };
                return View(request);
            }

            if (string.IsNullOrEmpty(request.NewPassword) || string.IsNullOrEmpty(request.NewPasswordAgain))
            {
                TempData["Result"] = new MethodResult { Succeed = false, Message = "Şifre ve tekrar alanları boş olamaz." };
                return View(request);
            }

            if (request.NewPassword != request.NewPasswordAgain)
            {
                TempData["Result"] = new MethodResult { Succeed = false, Message = "Şifre ve tekrar alanları aynı olmalıdır." };
                return View(request);
            }

            var user = entities.AppUsers.FirstOrDefault(x => x.Email == request.Email);
            if (user != null)
            {
                TempData["Result"] = new MethodResult { Succeed = false, Message = "Bu e-posta adresi kullanımda. Şifrenizi hatırlamıyorsanız şifre hatırlatma adımlarını takip edebilirsiniz." };
                return View(request);
            }
            else
            {

                user = new AppUser
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Password = HashHelper.ConvertToMd5x(request.NewPassword),
                    RegisterDate = DateTime.Now,
                    ProfilePhoto = ImageExtensions.ImageFromFilePath(Server.MapPath("/Content/assets/img/profile.jpg")),
                    Status = true
                };

                user = entities.AppUsers.Add(user);
                entities.SaveChanges();
            }

            TempData["Result"] = new MethodResult { Succeed = true, Message = "Kayıt işleminiz gerçekleşti." };
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("hesabim/sifre-hatirlat")]
        public ActionResult RemindPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("hesabim/sifre-hatirlat")]
        public ActionResult RemindPassword(RemindPasswordViewModel request)
        {
            MethodResult result = new MethodResult();

            if (string.IsNullOrEmpty(request.Email))
            {
                TempData["Result"] = new MethodResult { Succeed = false, Message = "E-posta alanı boş olamaz." };
                return View(request);
            }

            var user = entities.AppUsers.FirstOrDefault(x => x.Email == request.Email && x.Status == true);
            if (user == null)
            {
                TempData["Result"] = new MethodResult { Succeed = false, Message = "Bu e-posta adresi ile kayıtlı kulanıcı bulunamadı!" };
                return View(request);
            }
            else
            {
                var newPassword = HashHelper.CreatePassword(8);
                user.Password = HashHelper.ConvertToMd5x(newPassword);

                entities.SaveChanges();

                var mailbody = MailHelper.PrepareMailBody("Şifremi Unuttum", $"<p>Talebiniz üzerine şifreniz sıfırlanmıştır. </p> <p>Yeni Şifreniz : {newPassword}</p>");
                //FCMService.SendNotification(new List<string> { existingUser.FCMToken }, "Şifremi Unuttum", $" Yeni Şifreniz : {newPassword}");

                MailHelper.SendMail(user.Email, "Peki Ya Ben şifremi unuttum", mailbody);

                TempData["Result"] = new MethodResult { Succeed = true, Message = "Yeni şifreniz kayıtlı e-posta adresinize gönderildi" };
                return View();
            }
        }

        [HttpGet]
        [Route("hesabim/cikis")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("hesabim/profilim")]
        public ActionResult EditProfile()
        {
            AppUser user = AuthenticationHelper.GetAuthenticatedUser();

            UserUpdateViewModel model = new UserUpdateViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                Gender = (Gender?)user.Gender,
                PhoneNumber = user.PhoneNumber,
                ProfilePhoto = user.ProfilePhoto != null ? Convert.ToBase64String(user.ProfilePhoto) : null,
                Occupation = user.Occupation,
                OccupationStatus = (OccupationStatus?)user.OccupationStatus,
                City = user.City,
                CommunicationPermission = user.CommunicationPermission
            };

            InvoiceUpdateViewModel invoiceInfo = new InvoiceUpdateViewModel();

            var invoice = entities.InvoiceInfoes.FirstOrDefault(x => x.AppUserId == user.Id);
            if (invoice != null)
            {
                invoiceInfo = new InvoiceUpdateViewModel
                {
                    Id = invoice.Id,
                    AppUserId = invoice.AppUserId,
                    InvoiceType = (InvoiceType)invoice.InvoiceType,
                    PersonalID = invoice.PersonalID,
                    CompanyName = invoice.CompanyName,
                    TaxNumber = invoice.TaxNumber,
                    TaxOffice = invoice.TaxOffice,
                    InvoiceAddress = invoice.InvoiceAddress
                };
            }
            ViewBag.InvoiceInfo = invoiceInfo;
            return View(model);
            //
        }

        [HttpPost]
        [Route("hesabim/profilim")]
        public ActionResult EditProfile(UserUpdateViewModel request)
        {
            try
            {
                AppUser user = AuthenticationHelper.GetAuthenticatedUser();
                var updateUser = entities.AppUsers.FirstOrDefault(x => x.Id == user.Id);


                if (!string.IsNullOrEmpty(request.NewPassword) && !string.IsNullOrEmpty(request.NewPasswordAgain) && !string.IsNullOrEmpty(request.Password))
                {
                    if (request.NewPassword != request.NewPasswordAgain)
                    {
                        TempData["Result"] = new MethodResult { Succeed = false, Message = "Şifre ve tekrar alanları aynı olmalıdır." };
                        return RedirectToAction("EditProfile");
                    }

                    if (HashHelper.ConvertToMd5x(request.Password) != updateUser.Password)
                    {
                        TempData["Result"] = new MethodResult { Succeed = false, Message = "Eski şifrenizi yanlış girdiniz." };
                        return RedirectToAction("EditProfile");
                    }
                }

                if (!String.IsNullOrEmpty(request.NewPassword))
                {
                    updateUser.Password = HashHelper.ConvertToMd5x(request.NewPassword);
                }

                updateUser.FullName = request.FullName;
                updateUser.DateOfBirth = request.DateOfBirth;
                updateUser.Gender = (int?)request.Gender;
                updateUser.PhoneNumber = request.PhoneNumber;
                updateUser.Occupation = request.Occupation;
                updateUser.OccupationStatus = (int?)request.OccupationStatus;
                updateUser.City = request.City;


                entities.SaveChanges();

                var model = new UserUpdateViewModel
                {
                    Id = updateUser.Id,
                    FullName = updateUser.FullName,
                    Email = updateUser.Email,
                    DateOfBirth = updateUser.DateOfBirth,
                    Gender = (Gender?)updateUser.Gender,
                    PhoneNumber = updateUser.PhoneNumber,
                    ProfilePhoto = Convert.ToBase64String(updateUser.ProfilePhoto),
                    Occupation = updateUser.Occupation,
                    OccupationStatus = (OccupationStatus?)updateUser.OccupationStatus,
                    City = updateUser.City,
                    CommunicationPermission = updateUser.CommunicationPermission
                };

                TempData["Result"] = new MethodResult
                {
                    Message = "Profil bilgileri başarılı bir şekilde güncellendi.",
                    Succeed = true,
                    Data = updateUser
                };

                return RedirectToAction("EditProfile");
            }
            catch (Exception ex)
            {
                TempData["Result"] = new MethodResult
                {
                    Message = "Profil bilgileri güncellenirken hata oluştu",
                    Succeed = false
                };
            }
            return RedirectToAction("EditProfile");
        }

        [HttpPost]
        [Route("hesabim/fatura-bilgileri")]
        public ActionResult EditInvoice(InvoiceUpdateInputModel info)
        {
            try
            {
                var request = info.InvoiceInfo;
                AppUser user = AuthenticationHelper.GetAuthenticatedUser();

                if (request.InvoiceType == InvoiceType.Corporate)
                {
                    if (string.IsNullOrEmpty(request.CompanyName) || string.IsNullOrEmpty(request.TaxNumber) || string.IsNullOrEmpty(request.TaxOffice))
                    {
                        TempData["Result"] = new MethodResult { Succeed = false, Message = "Firma bilgilerini eksiksiz giriniz." };
                        return RedirectToAction("EditProfile");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(request.PersonalID) || !TcKimlikDogrulama(request.PersonalID))
                    {
                        TempData["Result"] = new MethodResult { Succeed = false, Message = "Geçersiz kimlik numarası" };
                        return RedirectToAction("EditProfile");
                    }
                }

                var invoice = entities.InvoiceInfoes.FirstOrDefault(x => x.Id == user.Id);

                if (invoice == null)
                {
                    invoice = new InvoiceInfo
                    {
                        AppUserId = user.Id,
                        InvoiceType = request.InvoiceType.ToInt32(),
                        PersonalID = request.PersonalID,
                        CompanyName = request.CompanyName,
                        TaxNumber = request.TaxNumber,
                        TaxOffice = request.TaxOffice,
                        InvoiceAddress = request.InvoiceAddress
                    };
                    entities.InvoiceInfoes.Add(invoice);

                    TempData["Result"] = new MethodResult { Message = "Fatura bilgileriniz güncellendi", Succeed = true };
                    entities.SaveChanges();
                }
                else
                {
                    invoice.InvoiceType = request.InvoiceType.ToInt32();
                    invoice.PersonalID = request.PersonalID;
                    invoice.CompanyName = request.CompanyName;
                    invoice.TaxNumber = request.TaxNumber;
                    invoice.TaxOffice = request.TaxOffice;
                    invoice.InvoiceAddress = request.InvoiceAddress;

                    TempData["Result"] = new MethodResult { Message = "Fatura bilgileriniz güncellendi", Succeed = true };
                    entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["Result"] = new MethodResult { Message = "Fatura bilgileri güncellenirken hata oluştu", Succeed = false };
            }
            return RedirectToAction("EditProfile");
        }

        [HttpPost]
        [Route("hesabim/profil-fotografi")]
        public ActionResult UdpateProfilePhoto()
        {
            var user = AuthenticationHelper.GetAuthenticatedUser();
            var cropped = Request.Form["croppedImage"].ToString().Replace("data:image/png;base64,", "");
            if (cropped.Length > 0)
            {
                user.ProfilePhoto = Convert.FromBase64String(cropped);
                var updateUser = entities.AppUsers.FirstOrDefault(x => x.Id == user.Id);
                updateUser.ProfilePhoto = user.ProfilePhoto;
                entities.SaveChanges();
            }

            return RedirectToAction("EditProfile");
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]

        [HttpGet]
        [Route("hesabim/satin-aldiklarim")]
        public ActionResult MyProducts()
        {
            AppUser user = AuthenticationHelper.GetAuthenticatedUser();
            var model = entities.Transactions.Where(x => x.AppUserId == user.Id && x.Status > 0).OrderBy(x => x.TransactionDate).ToList();
            ViewBag.Products = GeneralHelpers.GetProductList().Where(x => x.Status == 1).ToList();
            return View(model);
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]

        [HttpGet]
        [Route("hesabim/randevularim")]
        [Route("hesabim/randevularim/{id}")]
        public ActionResult MyInterviews(int id = 0)
        {
            AppUser user = AuthenticationHelper.GetAuthenticatedUser();
            var model = entities.CoachCalendarUsers.Where(x => x.AppUserId == user.Id && x.Status > 0);
            var calendar = entities.CoachCalendars.Where(x => model.Any(y => y.CoachCalendarId == x.Id) && x.Status == 1);
            var coaches = entities.Coaches;
            ViewBag.Products = GeneralHelpers.GetProductList().Where(x => x.Status == 1).ToList();
            ViewBag.Calendar = calendar.ToList();
            ViewBag.Coaches = coaches.ToList();
            ViewBag.ModifiedId = id;
            return View(model.ToList());
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        [HttpGet]
        [Route("online-kocluk-ve-atolye")]
        [Route("online-kocluk-ve-atolye/{id}/{title}")]
        [AllowAnonymous]
        public ActionResult NewInterview(int id = 0, string title = "")
        {
            var products = GeneralHelpers.GetProductList().Where(x => x.Type == 3 && x.Status == 1).OrderBy(x => x.SortOrder).ToList();

            Product selectedProduct = null;
            if (id > 0 && !String.IsNullOrEmpty(title))
            {
                selectedProduct = products.FirstOrDefault(x => x.Id == id);
                if (selectedProduct.Title.FriendlyUrl() != title)
                    return Redirect("/Hata");
            }

            ViewBag.SelectedProduct = selectedProduct;
            return View(products.ToList());
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        [HttpGet]
        [Route("koclar")]
        [AllowAnonymous]
        public ActionResult _Coaches(string interviewType)
        {
            var products = GeneralHelpers.GetProductList().Where(x => x.Type == 3 && x.Status == 1).OrderBy(x => x.SortOrder).ToList();
            var coaches = entities.Coaches.Where(x => x.CoachingInfo.Contains(interviewType) && x.Status == 1).OrderByDescending(x => x.Status).ThenBy(x => x.FullName).ToList();

            ViewBag.Products = products;
            ViewBag.SelectedProduct = products.FirstOrDefault(x => x.Title == interviewType && x.Status == 1);
            return PartialView(coaches.ToList());
        }

        [HttpGet]
        [Route("randevu-takvimi/{id}/{title}/{coachId}/{coachName}")]
        [Route("randevu-takvimi/{id}/{title}/{coachId}/{coachName}/{operation}")]
        [Route("randevu-takvimi/{id}/{title}/{coachId}/{coachName}/{operation}/{transactionId}")]
        [AllowAnonymous]
        public ActionResult InterviewCalendar(int id, string title, int coachId, string coachName, string operation = "yeni", int transactionId = 0)
        {
            Product selectedProduct = GeneralHelpers.GetProductList().FirstOrDefault(x => x.Id == id && x.Type == 3 && x.Status == 1);
            if (selectedProduct.Title.FriendlyUrl() != title)
                return Redirect("/Hata");

            ViewBag.SelectedProduct = selectedProduct;

            var coach = entities.Coaches.FirstOrDefault(x => x.Id == coachId);
            if (coach.FullName.FriendlyUrl() != coachName)
                return Redirect("/Hata");

            ViewBag.Operation = operation;
            ViewBag.TransactionId = transactionId;
            return View(coach);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("musait-randevular")]
        public ActionResult GetCalendar(int id, string product, DateTime dateStart)
        {
            List<object> response = new List<object>();
            try
            {

                DateTime nextAvailableDate = DateTime.Now.AddHours(4);
                DateTime startDate = dateStart < nextAvailableDate ? nextAvailableDate : dateStart;
                DateTime endDate = startDate.AddDays(7);

                var coachCalendar = entities.CoachCalendars.Where(x => x.AvailableDate > startDate && x.CoachId == id && x.AvailableDate < endDate && x.Status == 1);
                var coachCalendarUser = entities.CoachCalendarUsers.Where(x => coachCalendar.Any(y => y.Id == x.CoachCalendarId));


                foreach (var cc in coachCalendar)
                {
                    var ccu = coachCalendarUser.Where(x => x.CoachCalendarId == cc.Id); //
                    var participantCount = ccu.Where(x => x.Status >= 1).Count(); //aktif olarak kaç kişi katılmış

                    if ((!cc.InterviewType.Contains("Atölye") && (cc.InterviewType == "Genel" || cc.InterviewType == product) && !product.Contains("Atölye")
                        ||
                        (product.Contains("Atölye") && cc.InterviewType == product))
                        && cc.ParticipantLimit > participantCount
                        )
                    {
                        response.Add(new
                        {
                            title = (cc.CoachIsFull == true ? "Dolu Randevu" : "Randevu Al"),
                            start = cc.AvailableDate.ToString("yyyy-MM-dd HH:mm"),
                            end = cc.AvailableDate.AddHours(1).ToString("yyyy-MM-dd HH:mm"),
                            allDay = false,
                            editable = false,
                            id = cc.Id,
                            color = (cc.CoachIsFull == true ? "#ff4d7e" : "#19a5c9"),
                            overlap = (cc.CoachIsFull == true ? false : true)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Json(response);
        }

        private string GenerateOrderId(int length)
        {
            string timeStamp = DateTime.Now.ToString("hhmmss");
            string allowedChars = "ST123456789ABCDEFGHJKLMNOPRSTUIVYZWX";
            var random = new Random();
            return new string(Enumerable.Repeat(allowedChars, length).Select(x => x[random.Next(x.Length)]).ToArray()) + timeStamp;
        }

        [HttpGet]
        [Route("hesabim/randevu-al/{cid}/{product}")]
        [Route("hesabim/randevu-al/{cid}/{product}/{transactionId}")]
        public ActionResult Payment(int cid, string product, int transactionId = 0)
        {
            //TODO: Satınalma işlemlerindeki kontrolleri API'deki gibi yapalım.
            ViewBag.ShowPayment = false;

            AppUser user = AuthenticationHelper.GetAuthenticatedUser();
            var invoiceInfo = entities.InvoiceInfoes.FirstOrDefault(x => x.AppUserId == user.Id);

            if (invoiceInfo == null)
            {
                TempData["Result"] = new MethodResult { Message = "Randevu satın alabilmek için fatura bilgilerinizi kaydetmiş olmanız gerekmektedir.", Succeed = false };
                return RedirectToAction("EditProfile");
            }

            if (user == null)
            {
                TempData["Result"] = new MethodResult { Message = "Kullanıcı bulunamadı", Succeed = false };
                return View();
            }

            if (String.IsNullOrEmpty(user.PhoneNumber))
            {
                TempData["Result"] = new MethodResult { Message = "Randevu satın alabilmek için telefon numaranızı kaydetmiş olmanız gerekmektedir", Succeed = false };
                return RedirectToAction("EditProfile");

            }

            var selectedProduct = GeneralHelpers.GetProductList().AsEnumerable().FirstOrDefault(x => x.Title.FriendlyUrl() == product && x.Status == 1 && x.Type == 3);
            if (selectedProduct == null)
            {
                TempData["Result"] = new MethodResult { Message = "İşlem yapmak istediğiniz ürün bulunamadı", Succeed = false };
                return View();
            }

            var coachCalendar = entities.CoachCalendars.FirstOrDefault(x => x.Id == cid && x.Status == 1);
            if (coachCalendar == null)
            {
                TempData["Result"] = new MethodResult { Message = "Koçumuz maalesef randevu almak istediğiniz ürün için belirtilen tarihte müsait değil. Başka bir tarih/saat için randevu almayı deneyebilirsiniz", Succeed = false };
                return View();
            }

            DateTime AvailableDate = coachCalendar.AvailableDate;

            if (AvailableDate < DateTime.Now.AddHours(4))
            {
                {
                    TempData["Result"] = new MethodResult { Message = "Koçluk görüşmeleri için en erken 4 saat sonrasına randevu alabilirsiniz.", Succeed = false };
                    return View();
                }
            }

            var coachCalendarUser = entities.CoachCalendarUsers.Where(x => x.CoachCalendarId == coachCalendar.Id);
            var ccu = coachCalendarUser.Where(x => x.CoachCalendarId == coachCalendar.Id); //
            var participantCount = ccu.Where(x => x.Status >= 1).Count(); //aktif olarak kaç kişi katılmış

            if ((!coachCalendar.InterviewType.Contains("Atölye") && (coachCalendar.InterviewType == "Genel" || coachCalendar.InterviewType == selectedProduct.Title) && !selectedProduct.Title.Contains("Atölye")
                ||
                (selectedProduct.Title.Contains("Atölye") && coachCalendar.InterviewType == selectedProduct.Title))
                && coachCalendar.ParticipantLimit > participantCount
                )
            {
                //ödeme gerekli mi?
                var availableTransaction = entities.Transactions.FirstOrDefault(x => x.AppUserId == user.Id && x.ProductId == selectedProduct.Id && x.Status == 1);
                if (availableTransaction != null) //ürün müsait satın almadan geçelim.
                {
                    var userCalendar = entities.CoachCalendarUsers.FirstOrDefault(x => x.TransactionId == availableTransaction.Id && x.Status == 1);
                    if (userCalendar != null)
                    {
                        userCalendar.CoachCalendarId = coachCalendar.Id;
                        availableTransaction.Status = 10; //kullanıldı;
                        entities.SaveChanges();

                        TempData["Result"] = new MethodResult { Message = "Koçluk Görüşmesi güncelleme talebiniz alınmıştır.", Succeed = true };

                        return RedirectToAction("MyInterviews");
                    }
                    else
                    {
                        entities.CoachCalendarUsers.Add(new CoachCalendarUser
                        {
                            AppUserId = user.Id,
                            CoachCalendarId = coachCalendar.Id,
                            InterviewType = product,
                            TransactionId = availableTransaction.Id,
                            Status = 2,
                        });

                        availableTransaction.Status = 10; //kullanıldı;
                        entities.SaveChanges();

                        TempData["Result"] = new MethodResult { Message = "Koçluk Görüşmesi talebiniz alınmıştır.", Succeed = true };

                        return RedirectToAction("MyInterviews");
                    }
                }

                //Ödemeye ilerle
                // API Entegrasyon Bilgileri
                string merchant_id = "190881";
                string merchant_key = "gZoXu9wzc7FB2Tpy";
                string merchant_salt = "8SCg3j85eib3hoQF";

                string emailstr = user.Email;
                //
                // Tahsil edilecek tutar. 9.99 için 9.99 * 100 = 999 gönderilmelidir.
                //int payment_amountstr = 999;
                int payment_amountstr = (int)selectedProduct.Price * 100; //fiyat * 100 kuruşş şeklinde olacak.

                // Sipariş numarası: Her işlemde benzersiz olmalıdır!! Bu bilgi bildirim sayfanıza yapılacak bildirimde geri gönderilir.
                string merchant_oid = GenerateOrderId(8);
                //
                // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız ad ve soyad bilgisi
                string user_namestr = user.FullName;//invoiceInfo.InvoiceType == InvoiceType.Individual.ToInt32() ? user.FullName : invoiceInfo.CompanyName;
                //
                // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız adres bilgisi
                string user_addressstr = "Adres belirtilmedi"; //invoiceInfo.InvoiceAddress;
                //
                // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız telefon bilgisi
                string user_phonestr = user.PhoneNumber;
                //
                // Başarılı ödeme sonrası müşterinizin yönlendirileceği sayfa
                // !!! Bu sayfa siparişi onaylayacağınız sayfa değildir! Yalnızca müşterinizi bilgilendireceğiniz sayfadır!
                // !!! Siparişi onaylayacağız sayfa "Bildirim URL" sayfasıdır (Bakınız: 2.ADIM Klasörü).
                string merchant_ok_url = rootUrl + "/hesabim/odeme-basarili";
                //
                // Ödeme sürecinde beklenmedik bir hata oluşması durumunda müşterinizin yönlendirileceği sayfa
                // !!! Bu sayfa siparişi iptal edeceğiniz sayfa değildir! Yalnızca müşterinizi bilgilendireceğiniz sayfadır!
                // !!! Siparişi iptal edeceğiniz sayfa "Bildirim URL" sayfasıdır (Bakınız: 2.ADIM Klasörü).
                string merchant_fail_url = rootUrl + "/hesabim/odeme-basarisiz";
                //        
                // !!! Eğer bu örnek kodu sunucuda değil local makinanızda çalıştırıyorsanız
                // buraya dış ip adresinizi (https://www.whatismyip.com/) yazmalısınız. Aksi halde geçersiz paytr_token hatası alırsınız.
                string user_ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (user_ip == "37.156.246.88" || user_ip == null)
                {
                    user_ip = Request.ServerVariables["REMOTE_ADDR"];
                }
                //
                // ÖRNEK $user_basket oluşturma - Ürün adedine göre object'leri çoğaltabilirsiniz
                object[][] user_basket = {
                    new object[] {selectedProduct.Title, selectedProduct.Price.ToString("#0.00"), 1} // 1. ürün (Ürün Ad - Birim Fiyat - Adet)
                };

                // İşlem zaman aşımı süresi - dakika cinsinden
                string timeout_limit = "30";
                //
                // Hata mesajlarının ekrana basılması için entegrasyon ve test sürecinde 1 olarak bırakın. Daha sonra 0 yapabilirsiniz.
                string debug_on = "0";
                //
                // Mağaza canlı modda iken test işlem yapmak için 1 olarak gönderilebilir.
                string test_mode = "0";
                //
                // Taksit yapılmasını istemiyorsanız, sadece tek çekim sunacaksanız 1 yapın
                string no_installment = "1";

                //
                // Sayfada görüntülenecek taksit adedini sınırlamak istiyorsanız uygun şekilde değiştirin.
                // Sıfır (0) gönderilmesi durumunda yürürlükteki en fazla izin verilen taksit geçerli olur.
                string max_installment = "0";
                //
                // Para birimi olarak TL, EUR, USD gönderilebilir. USD ve EUR kullanmak için kurumsal@paytr.com 
                // üzerinden bilgi almanız gerekmektedir. Boş gönderilirse TL geçerli olur.
                string currency = "TL";
                //
                // Türkçe için tr veya İngilizce için en gönderilebilir. Boş gönderilirse tr geçerli olur.
                string lang = "";


                // Gönderilecek veriler oluşturuluyor
                NameValueCollection data = new NameValueCollection();
                data["merchant_id"] = merchant_id;
                data["user_ip"] = user_ip;
                data["merchant_oid"] = merchant_oid;
                data["email"] = emailstr;
                data["payment_amount"] = payment_amountstr.ToString();

                // Sepet içerği oluşturma fonksiyonu, değiştirilmeden kullanılabilir.
                string user_basket_json = JsonConvert.SerializeObject(user_basket);
                string user_basketstr = Convert.ToBase64String(Encoding.UTF8.GetBytes(user_basket_json));
                data["user_basket"] = user_basketstr;

                // Token oluşturma fonksiyonu, değiştirilmeden kullanılmalıdır.
                string Birlestir = string.Concat(merchant_id, user_ip, merchant_oid, emailstr, payment_amountstr.ToString(), user_basketstr, no_installment, max_installment, currency, test_mode, merchant_salt);
                HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchant_key));
                byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(Birlestir));
                data["paytr_token"] = Convert.ToBase64String(b);

                data["debug_on"] = debug_on;
                data["test_mode"] = test_mode;
                data["no_installment"] = no_installment;
                data["max_installment"] = max_installment;
                data["user_name"] = user_namestr;
                data["user_address"] = user_addressstr;
                data["user_phone"] = user_phonestr;
                data["merchant_ok_url"] = merchant_ok_url;
                data["merchant_fail_url"] = merchant_fail_url;
                data["timeout_limit"] = timeout_limit;
                data["currency"] = currency;
                data["lang"] = lang;

                var newTransaction = new Transaction
                {
                    AppUserId = user.Id,
                    ProductId = selectedProduct.Id,
                    MarketId = 3,
                    TransactionId = merchant_oid,
                    TransactionDetails = "booked",
                    TransactionDate = DateTime.Now,
                    TransactionPrice = selectedProduct.Price,
                    Status = 0 //passive
                };
                entities.Transactions.Add(newTransaction);
                entities.SaveChanges();

                entities.CoachCalendarUsers.Add(new CoachCalendarUser
                {
                    AppUserId = user.Id,
                    CoachCalendarId = coachCalendar.Id,
                    InterviewType = product,
                    TransactionId = newTransaction.Id,
                    Status = 0, //passive
                });

                entities.SaveChanges();

                //string resultauth = "";
                //try
                //{
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    byte[] result = client.UploadValues("https://www.paytr.com/odeme/api/get-token", "POST", data);
                    string ResultAuthTicket = Encoding.UTF8.GetString(result);
                    //resultauth = ResultAuthTicket;
                    dynamic json = JValue.Parse(ResultAuthTicket);

                    if (json.status == "success")
                    {
                        ViewBag.Src = "https://www.paytr.com/odeme/guvenli/" + json.token + "";
                    }
                    else
                    {
                        Console.WriteLine("PAYTR IFRAME failed. reason:" + json.reason + "");
                    }
                }
                //}
                //catch (Exception ex)
                //{
                //    TempData["Result"] = new MethodResult { Message = resultauth + " | " + JsonConvert.SerializeObject(data.ToDictionary()) + " | " + ex.Message, Succeed = false };
                //}

                ViewBag.ShowPayment = true;

                return View(selectedProduct);

            }
            else
            {
                TempData["Result"] = new MethodResult { Message = "Koçumuz maalesef randevu almak istediğiniz ürün için belirtilen tarihte müsait değil.Başka bir tarih / saat için randevu almayı deneyebilirsiniz", Succeed = false };
                return View();
            }
        }

        [HttpGet]
        [Route("hesabim/randevu-duzenle/{cid}/{product}/{transactionId}")]
        public ActionResult EditMeeting(int cid, string product, int transactionId = 0)
        {
            ViewBag.ShowPayment = false;

            AppUser user = AuthenticationHelper.GetAuthenticatedUser();

            if (user == null)
            {
                TempData["Result"] = new MethodResult { Message = "Kullanıcı bulunamadı", Succeed = false };
                return View();
            }

            var selectedProduct = GeneralHelpers.GetProductList().AsEnumerable().FirstOrDefault(x => x.Title.FriendlyUrl() == product && x.Status == 1 && x.Type == 3);
            if (selectedProduct == null)
            {
                TempData["Result"] = new MethodResult { Message = "İşlem yapmak istediğiniz ürün bulunamadı", Succeed = false };
                return View();
            }

            var coachCalendar = entities.CoachCalendars.FirstOrDefault(x => x.Id == cid && x.Status == 1);
            if (coachCalendar == null)
            {
                TempData["Result"] = new MethodResult { Message = "Koçumuz maalesef randevu almak istediğiniz ürün için belirtilen tarihte müsait değil. Başka bir tarih/saat için randevu almayı deneyebilirsiniz", Succeed = false };
                return View();
            }

            DateTime AvailableDate = coachCalendar.AvailableDate;

            if (AvailableDate < DateTime.Now.AddHours(4))
            {
                TempData["Result"] = new MethodResult { Message = "Koçluk görüşmeleri için en erken 4 saat sonrasına randevu alabilirsiniz.", Succeed = false };
                return View();
            }

            var coachCalendarUser = entities.CoachCalendarUsers.Where(x => x.CoachCalendarId == coachCalendar.Id);
            var ccu = coachCalendarUser.Where(x => x.CoachCalendarId == coachCalendar.Id); //
            var participantCount = ccu.Where(x => x.Status >= 1).Count(); //aktif olarak kaç kişi katılmış

            if ((!coachCalendar.InterviewType.Contains("Atölye") && (coachCalendar.InterviewType == "Genel" || coachCalendar.InterviewType == selectedProduct.Title) && !product.Contains("Atölye")
                ||
                (product.Contains("Atölye") && coachCalendar.InterviewType == selectedProduct.Title))
                && coachCalendar.ParticipantLimit > participantCount
                )
            {
                var availableTransaction = entities.Transactions.FirstOrDefault(x => x.Id == transactionId && x.AppUserId == user.Id && x.ProductId == selectedProduct.Id && x.Status == 10);
                if (availableTransaction != null) //ürün müsait satın almadan geçelim.
                {
                    var userCalendar = entities.CoachCalendarUsers.FirstOrDefault(x => x.TransactionId == availableTransaction.Id && x.Status == 2);
                    if (userCalendar != null)
                    {
                        userCalendar.CoachCalendarId = coachCalendar.Id;
                        entities.SaveChanges();

                        StringBuilder sb = new StringBuilder();
                        var coach = entities.Coaches.FirstOrDefault(x => x.Id == coachCalendar.CoachId);

                        sb.AppendFormat("Kullanıcı : {0}<br/>", user.FullName).AppendFormat("E-posta : {0}<br/>", user.Email).AppendFormat("Telefon : {0}<br/><br/><br/>", user.PhoneNumber);
                        sb.Append("<table width='100%'><tr><th><h3>Form Verileri</h3></th></tr>");
                        sb.AppendFormat("<tr><th>Koç</th><td>: {0}</td></tr>", coach.FullName);
                        sb.AppendFormat("<tr><th>Görüşme Tarihi</th><td>: {0}</td></tr>", coachCalendar.AvailableDate)
                        .AppendFormat("<tr><th>Koçluk Tipi</th><td>: {0}</td></tr>", userCalendar.InterviewType)
                        .Append("</table>");

                        var mailbody = MailHelper.PrepareMailBody("Koçluk Görüşmesi Güncelleme Talebi", sb.ToString());

                        MailHelper.SendMail("fzeylan@yahoo.com", "Koçluk Görüşmesi Güncelleme", mailbody);
                        MailHelper.SendMail("form@pekiyaben.com", "Koçluk Görüşmesi Güncelleme", mailbody);

                        TempData["Result"] = new MethodResult { Message = "Koçluk Görüşmesi güncelleme talebiniz alınmıştır.", Succeed = true };

                        return RedirectToAction("MyInterviews", new { id = userCalendar.Id });
                    }
                }
                else
                {
                    TempData["Result"] = new MethodResult { Message = "İşlem yapmak istediğiniz randevu ile giriş yaptığınız kullanıcı bilgileri eşleşmedi", Succeed = true };
                    return View();
                }
            }
            else
            {
                TempData["Result"] = new MethodResult { Message = "Koçumuz maalesef randevu almak istediğiniz ürün için belirtilen tarihte müsait değil.Başka bir tarih / saat için randevu almayı deneyebilirsiniz", Succeed = false };
                return View();
            }
            return View();
        }

        [HttpGet]
        [Route("hesabim/randevu-iptal/{cid}/{product}/{transactionId}")]
        public ActionResult CancelMeeting(int cid, string product, int transactionId = 0)
        {
            ViewBag.ShowPayment = false;

            AppUser user = AuthenticationHelper.GetAuthenticatedUser();

            if (user == null)
            {
                TempData["Result"] = new MethodResult { Message = "Kullanıcı bulunamadı", Succeed = false };
                return View();
            }

            var selectedProduct = GeneralHelpers.GetProductList().AsEnumerable().FirstOrDefault(x => x.Title.FriendlyUrl() == product && x.Status == 1 && x.Type == 3);
            if (selectedProduct == null)
            {
                TempData["Result"] = new MethodResult { Message = "İşlem yapmak istediğiniz ürün bulunamadı", Succeed = false };
                return View();
            }

            var coachCalendar = entities.CoachCalendars.FirstOrDefault(x => x.Id == cid && x.Status == 1);
            if (coachCalendar == null)
            {
                TempData["Result"] = new MethodResult { Message = "Koçumuz maalesef randevu almak istediğiniz ürün için belirtilen tarihte müsait değil. Başka bir tarih/saat için randevu almayı deneyebilirsiniz", Succeed = false };
                return View();
            }

            DateTime AvailableDate = coachCalendar.AvailableDate;

            if (AvailableDate < DateTime.Now.AddHours(24))
            {
                TempData["Result"] = new MethodResult { Message = "Randevunuza 24 saatten az bir zaman kaldığından iptal edilemez.", Succeed = false };
                return View();
            }

            var availableTransaction = entities.Transactions.FirstOrDefault(x => x.Id == transactionId && x.AppUserId == user.Id && x.ProductId == selectedProduct.Id && x.Status == 10);
            if (availableTransaction != null) //işlem var silebiliriz.
            {
                var userCalendar = entities.CoachCalendarUsers.FirstOrDefault(x => x.TransactionId == availableTransaction.Id && x.Status == 2);
                if (userCalendar != null)
                {
                    entities.CoachCalendarUsers.Remove(userCalendar);
                    availableTransaction.Status = 1; //kullanılabilir;

                    entities.SaveChanges();

                    StringBuilder sb = new StringBuilder();
                    var coach = entities.Coaches.FirstOrDefault(x => x.Id == coachCalendar.CoachId);

                    sb.AppendFormat("Kullanıcı : {0}<br/>", user.FullName).AppendFormat("E-posta : {0}<br/>", user.Email).AppendFormat("Telefon : {0}<br/><br/><br/>", user.PhoneNumber);
                    sb.Append("<table width='100%'><tr><th><h3>Form Verileri</h3></th></tr>");
                    sb.AppendFormat("<tr><th>Koç</th><td>: {0}</td></tr>", coach.FullName);
                    sb.AppendFormat("<tr><th>Görüşme Tarihi</th><td>: {0}</td></tr>", coachCalendar.AvailableDate)
                    .AppendFormat("<tr><th>Koçluk Tipi</th><td>: {0}</td></tr>", userCalendar.InterviewType)
                    .Append("</table>");

                    var mailbody = MailHelper.PrepareMailBody("Koçluk Görüşmesi İptal Talebi", sb.ToString());

                    MailHelper.SendMail("fzeylan@yahoo.com", "Koçluk Görüşmesi İptal ", mailbody);
                    MailHelper.SendMail("form@pekiyaben.com", "Koçluk Görüşmesi İptal ", mailbody);


                    TempData["Result"] = new MethodResult { Message = "Koçluk Görüşmesi iptal talebiniz alınmıştır.", Succeed = true };

                    return RedirectToAction("MyInterviews", new { id = userCalendar.Id });
                }
            }
            else
            {
                TempData["Result"] = new MethodResult { Message = "İşlem yapmak istediğiniz randevu ile giriş yaptığınız kullanıcı bilgileri eşleşmedi", Succeed = true };
                return View();
            }
            return View();
        }

        [HttpGet]
        [Route("hesabim/odeme-basarili")]
        public ActionResult PaymentSucceeded()
        {
            return View();
        }

        [HttpGet]
        [Route("hesabim/odeme-basarisiz")]
        public ActionResult PaymentFailed()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("hesabim/odeme-onayi")]
        public ActionResult PaymentNotification()
        {
            string merchant_key = "gZoXu9wzc7FB2Tpy";
            string merchant_salt = "8SCg3j85eib3hoQF";

            // POST değerleri ile hash oluştur.
            string merchant_oid = Request.Form["merchant_oid"];
            string status = Request.Form["status"];
            string total_amount = Request.Form["total_amount"];
            string hash = Request.Form["hash"];

            string Birlestir = string.Concat(merchant_oid, merchant_salt, status, total_amount);
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchant_key));
            byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(Birlestir));
            string token = Convert.ToBase64String(b);

            // Oluşturulan hash'i, paytr'dan gelen post içindeki hash ile karşılaştır (isteğin paytr'dan geldiğine ve değişmediğine emin olmak için)
            // Bu işlemi yapmazsanız maddi zarara uğramanız olasıdır.
            if (hash.ToString() != token)
            {
                return View("PAYTR notification failed: bad hash");
            }

            var transaction = entities.Transactions.FirstOrDefault(x => x.TransactionId == merchant_oid);
            if (transaction == null || transaction.Status > 0)
                return View();

            var userCalendar = entities.CoachCalendarUsers.FirstOrDefault(x => x.TransactionId == transaction.Id);
            if (userCalendar == null || userCalendar.Status > 0)
                return View();

            if (status == "success") //Ödeme Onaylandı
            {
                transaction.Status = 1;
                transaction.TransactionDetails = JsonConvert.SerializeObject(Request.Form.ToDictionary());

                userCalendar.Status = 1;
                entities.SaveChanges();

                transaction.Status = 10;
                userCalendar.Status = 2;
                entities.SaveChanges();

                var user = entities.AppUsers.FirstOrDefault(x => x.Id == transaction.AppUserId);
                var calendar = entities.CoachCalendars.FirstOrDefault(x => x.Id == userCalendar.CoachCalendarId);
                var coach = entities.Coaches.FirstOrDefault(x => x.Id == calendar.CoachId);

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Kullanıcı : {0}<br/>", user.FullName).AppendFormat("E-posta : {0}<br/>", user.Email).AppendFormat("Telefon : {0}<br/><br/><br/>", user.PhoneNumber);
                sb.Append("<table width='100%'><tr><th><h3>Form Verileri</h3></th></tr>");
                sb.AppendFormat("<tr><th>Koç</th><td>: {0}</td></tr>", coach.FullName);
                sb.AppendFormat("<tr><th>Görüşme Tarihi</th><td>: {0}</td></tr>", calendar.AvailableDate)
                .AppendFormat("<tr><th>Koçluk Tipi</th><td>: {0}</td></tr>", calendar.InterviewType)
                .Append("</table>");

                var mailbody = MailHelper.PrepareMailBody("Koçluk Görüşmesi Talebi", sb.ToString());

                MailHelper.SendMail("fzeylan@yahoo.com", "Koçluk Görüşmesi Talebi", mailbody);
                MailHelper.SendMail("form@pekiyaben.com", "Koçluk Görüşmesi Talebi", mailbody);

                return View();
            }
            else //Ödemeye Onay Verilmedi
            {

                transaction.TransactionDetails = JsonConvert.SerializeObject(Request.Form.ToDictionary());
                entities.SaveChanges();

                return View();
            }
        }

        private bool TcKimlikDogrulama(string pTCKimlik)
        {
            if (pTCKimlik.Length < 11)
                return false;
            else if (pTCKimlik.Substring(0, 1) == "0")
                return false;

            int toplam1 = Convert.ToInt32(pTCKimlik[0].ToString()) + Convert.ToInt32(pTCKimlik[2].ToString()) + Convert.ToInt32(pTCKimlik[4].ToString()) + Convert.ToInt32(pTCKimlik[6].ToString()) + Convert.ToInt32(pTCKimlik[8].ToString());
            int toplam2 = Convert.ToInt32(pTCKimlik[1].ToString()) + Convert.ToInt32(pTCKimlik[3].ToString()) + Convert.ToInt32(pTCKimlik[5].ToString()) + Convert.ToInt32(pTCKimlik[7].ToString());

            int sonuc = (toplam1 * 7 - toplam2) % 10;

            if (sonuc.ToString() != pTCKimlik[9].ToString())
                return false;

            int toplam3 = 0;
            for (int i = 0; i < 10; i++)
                toplam3 += Convert.ToInt32(pTCKimlik[i].ToString());

            if ((toplam3 % 10).ToString() != pTCKimlik[10].ToString())
                return false;

            return true;
        }
    }
}