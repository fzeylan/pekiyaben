using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Entity;
using Emoda.PekiYaBen.Entity.User;
using log4net;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using OLCA.Infrastructure.CQS;
using System;
using System.ComponentModel.Design;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Emoda.PekiYaBen.Web.Controllers
{
    public class AccountController : Controller
    {
        private static ILog logger = LogManager.GetLogger(typeof(AccountController));
        private ICommandDispatcher commandDispatcher;

        public AccountController(ICommandDispatcher commandDispatcher)
        {
            this.commandDispatcher = commandDispatcher;
        }
        // GET: Account
        public ActionResult Login()
        {
            return View("Login", new ValidateUserCommand());
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Login(ValidateUserCommand command)
        {
            if (ModelState.IsValid)
            {
                AdminUserInfo userInfo;
                try
                {
                    userInfo = commandDispatcher.Dispatch<ValidateUserCommand, AdminUserInfo>(command);
                }
                catch (EmodaException ex)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", ex.Message);

                    return View("Login", command);
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    logger.Error("Login Hatası", ex);
                    ModelState.AddModelError("", "Bir hata oluştu lütfen sonra tekrar deneyiniz" + ex.ToString());
                    return View("Login", command);
                }

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
                    new Claim(ClaimTypes.Email, userInfo.Email),
                    new Claim(ClaimTypes.Name, userInfo.FullName)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType); // create a new user identity

                var auth = Request.GetOwinContext().Authentication;
                auth.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true, // set to true if you want `remember me` feature
                }, identity);

                return RedirectToAction("Index", "Home");
            }

            return View("Login", command);
        }

        [Authorize]
        public ActionResult LogOff()
        {
            Authentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult RenewPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RenewPassword(RenewPasswordCommand command)
        {
            var newPassword = commandDispatcher.Dispatch<RenewPasswordCommand, string>(command);
            string result = "İşlem sırasında hata oluştu , lütfen tekrar deneyiniz";
            if (!string.IsNullOrEmpty(newPassword))
            {
                result = "Şifreniz Sıfırlanmıştır";
                SendMail(command.Email, result, string.Format("<p>Talebiniz üzerine şifreniz sıfırlanmıştır. </p> <p>Yeni Şifreniz : {0}</p>", newPassword));
            }

            ViewBag.Result = result;
            return View();
        }

        //[Authorize]
        //public ApiResult Post([FromBody]UpdatePasswordCommand command)
        //{
        //    var newPassword = CommandDispatcher.Dispatch<UpdatePasswordCommand, string>(command);

        //    if (!string.IsNullOrEmpty(newPassword))
        //    {
                
        //    }

        //    ApiResult result = new ApiResult()
        //    {
        //        Data = "",
        //    };

        //    return result;
        //}

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }

        }

        internal static void SendMail(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            var mailFrom = new MailAddress(ConfigurationManager.AppSettings["FromEmail"], "Peki Ya Ben", Encoding.UTF8);
            var mailTo = new MailAddress(to);

            mail.Sender = new MailAddress(ConfigurationManager.AppSettings["FromEmail"]);
            mail = new MailMessage(mailFrom, mailTo);
            mail.Attachments.Clear();
            mail.Bcc.Clear();
            //if (BCC != null)
            //{
            //    foreach (string item in BCC)
            //    {
            //        email.Bcc.Add(item);
            //    }
            //}
            //mail.Priority = MailPriority.High;
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(body));

            SmtpClient mailSender = new SmtpClient();
            string host = ConfigurationManager.AppSettings["Host"];
            mailSender.Host = host;
            mailSender.UseDefaultCredentials = true;
            mailSender.EnableSsl = false;

            string password = ConfigurationManager.AppSettings["Password"];
            mailSender.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FromEmail"], password);
            mailSender.DeliveryMethod = SmtpDeliveryMethod.Network;
            mailSender.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            mailSender.Send(mail);
        }
    }
}