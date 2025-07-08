using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Helpers;
using Emoda.PekiYaBen.Business.Queries;
using Emoda.PekiYaBen.Entity.Helpers;
using Emoda.PekiYaBen.Web.Models;
using Emoda.PekiYaBen.Web.Models.DataTable;
using Emoda.PekiYaBen.Web.Models.EntityModels;
using OLCA.Infrastructure.CQS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace Emoda.PekiYaBen.Web.Controllers
{

    public class HomeController : Controller
    {
        
        private IQueryDispatcher queryDispatcher;

        private string[] columns = new string[] {
            "Id",
            "LastPurchaseDate",
            "FullName",
            "Email",
            "DateOfBirth",
            "Gender",
            "PhoneNumber",
            "Occupation",
            "OccupationStatus",
            "City",
            "CommunicationPermission",
            "CommunicationPermissionUpdate",
            "LastPurchaseDate",
            "SocialMedia",
            "RegisterDate",
            "LastLoginDate",
            "Status",
        };

        private string[] notificationColumns = new string[] {
            "FullName",
            "Email",
            "DateOfBirth",
            "Gender",
            "PhoneNumber",
            "Occupation",
            "OccupationStatus",
            "City",
            "CommunicationPermission",
            "CommunicationPermissionUpdate",
            "LastPurchaseDate",
            "SocialMedia",
            "RegisterDate",
            "Status"
        };

        
        public HomeController(IQueryDispatcher queryDispatcher)
        {
            this.queryDispatcher = queryDispatcher;

            //FormController.SendMail("erman.olca@gmail.com", "ışğüçö", "<p>Türkçe karakterler denemesi ıüğşçö</p>");
        }

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Dashboard";
            ViewBag.PageHeader = "Dashboard";
            ViewBag.ActivePage = "Index";

            DataAccess da = new DataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString);
            var Today = DateTime.Today.ToSQLDate(false);
            var ThisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToSQLDate(false);

            var dailyMembers = da.ExecuteScalar("Select count(*) FROM AppUser WHERE RegisterDate>='" + Today + "'", new List<SqlParameter> { });
            var monthlyMembers = da.ExecuteScalar("Select count(*) FROM AppUser WHERE RegisterDate>='" + ThisMonth + "'", new List<SqlParameter> { });
            var dailySales = da.ExecuteScalar("Select count(*) FROM [Transaction] WHERE status>0 AND TransactionDate >='" + Today + "'", new List<SqlParameter> { });
            var dailySalesAmount = da.ExecuteScalar("Select Sum(TransactionPrice) FROM [Transaction] WHERE status>0 AND TransactionDate >='" + Today + "'", new List<SqlParameter> { });
            var monthlySales = da.ExecuteScalar("Select count(*) FROM [Transaction] WHERE status>0 AND TransactionDate >='" + ThisMonth + "'", new List<SqlParameter> { });
            var monthlySalesAmount = da.ExecuteScalar("Select Sum(TransactionPrice) FROM [Transaction] WHERE status>0 AND TransactionDate >='" + ThisMonth + "'", new List<SqlParameter> { });

            ViewBag.DailyMembers = dailyMembers;
            ViewBag.MonthlyMembers = monthlyMembers;
            ViewBag.DailySales = dailySales;
            ViewBag.DailySalesAmount = dailySalesAmount;
            ViewBag.MonthlySales = monthlySales;
            ViewBag.MonthlySalesAmount = monthlySalesAmount;

            return View();
        }

        [Authorize]
        public ActionResult Users()
        {
            ViewBag.Title = "Kullanıcı Listesi";
            ViewBag.PageHeader = "Kullanıcı Listesi";
            ViewBag.ActivePage = "UserList";

            return View();
        }

        [Authorize]
        public ActionResult Notification()
        {
            ViewBag.Title = "Bildirim Gönderimi";
            ViewBag.PageHeader = "Bildirim Gönderimi";
            ViewBag.ActivePage = "Notification";

            return View();
        }

        [Authorize]
        public ActionResult GeneralSettings()
        {
            ViewBag.Title = "Genel Ayarlar";
            ViewBag.PageHeader = "Genel Ayarlar";
            ViewBag.ActivePage = "GeneralSettings";

            //PekiYaBenDBEntities ee = new PekiYaBenDBEntities();
            using (PekiYaBenDBEntities ent = new PekiYaBenDBEntities())
            {
                ent.Configuration.LazyLoadingEnabled = false;
                ent.Configuration.ProxyCreationEnabled = false;

                try
                {
                    var parameters = ent.GeneralParameters.ToList();
                    ViewBag.KoclukSaati = parameters.Where(x => x.Parameter == "KoclukSaati").Select(x => x.Value).FirstOrDefault();
                    ViewBag.Kullanici = parameters.Where(x => x.Parameter == "Kullanici").Select(x => x.Value).FirstOrDefault();
                    ViewBag.SaatAtolye = parameters.Where(x => x.Parameter == "SaatAtolye").Select(x => x.Value).FirstOrDefault();
                    ViewBag.Ulke = parameters.Where(x => x.Parameter == "Ulke").Select(x => x.Value).FirstOrDefault();
                    ViewBag.MainPageVideo = parameters.Where(x => x.Parameter == "MainPageVideo").Select(x => x.Value).FirstOrDefault();
                    ViewBag.AnaSayfaBaslik = parameters.Where(x => x.Parameter == "AnaSayfaBaslik").Select(x => x.Value).FirstOrDefault();
                    ViewBag.AnaSayfaAltAciklama = parameters.Where(x => x.Parameter == "AnaSayfaAltAciklama").Select(x => x.Value).FirstOrDefault();
                }
                catch (Exception ex)
                {}
            }

            return View();
        }

        [ValidateInput(false)]
        public JsonResult SaveGeneralParameters(string parameter, string value)
        {
            //PekiYaBenDBEntities ee = new PekiYaBenDBEntities();
            using (PekiYaBenDBEntities ent = new PekiYaBenDBEntities())
            {
                ent.Configuration.LazyLoadingEnabled = false;
                ent.Configuration.ProxyCreationEnabled = false;

                try
                {
                    var genPar = ent.GeneralParameters.Where(x => x.Parameter == parameter).FirstOrDefault();
                    if (genPar != null)
                    {
                        genPar.Value = value;
                        ent.SaveChanges();                        
                    }

                    return new ApiResult{ Data = "", Error = null };
                }
                catch (Exception ex)
                {
                    return new ApiResult
                    {
                        Error = "Bir hata oluştu, lütfen sonra tekrar deneyiniz." //+ ex.ToString()
                    };
                }
            }
        }


        [HttpPost]
        public ActionResult Notification(NotificationModel model)
        {
            if (!base.ModelState.IsValid)
            {
                string error = string.Join("; ", from x in base.ModelState.Values.SelectMany((ModelState x) => x.Errors)
                                                 select x.ErrorMessage);
                return new ApiResult
                {
                    Error = error
                };
            }
            SaveNotificationMessageCommand command = new SaveNotificationMessageCommand
            {
                Title = model.Title,
                Message = model.Message,
                Content = model.Content,
                //DueDate = model.DueDate,
                UserInfoId = model.Users
            };
            try
            {
                var allUSers = command.UserInfoId == null || command.UserInfoId.Length == 0;
                if (!allUSers)
                {
                    FCMService.SendNotification(command.UserInfoId.ToList<string>(), command.Message, command.Title);
                }

                return new ApiResult
                {
                    Data = new
                    {
                        NotificationId = 0
                    },
                    Error = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    Error = "Bir hata oluştu, lütfen sonra tekrar deneyiniz." + ex.ToString()
                };
            }
        }



        [HttpPost]
        public ActionResult SendAllMessages(String title, String message )
        {
            if (!base.ModelState.IsValid)
            {
                string error = string.Join("; ", from x in base.ModelState.Values.SelectMany((ModelState x) => x.Errors)
                                                 select x.ErrorMessage);
                return new ApiResult
                {
                    Error = error
                };
            }

            DataAccess da = new DataAccess(ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString);

            var coaches = da.GetDataReader("Select FCMToken FROM AppUser Where Status = 1 and FCMToken is not null and CommunicationPermission = 1", new List<SqlParameter> { });

            List<string> tokens = new List<string>();
            foreach (DataRow row in coaches.Rows)
            {
                tokens.Add(row["FCMToken"].ToString());
            }

            try
            {
                if (tokens.Count() > 0)
                {
                    FCMService.SendNotification(tokens, message, title);
                }

                return new ApiResult
                {
                    Data = new { NotificationId = 0 },
                    Error = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    Error = "Bir hata oluştu, lütfen sonra tekrar deneyiniz." //+ ex.ToString()
                };
            }
        }

        [Authorize]
        public string GetUsers(DataTableParameters dataParameter)
        {
            GetUserListQuery query = new GetUserListQuery();
            query.Search = dataParameter.Search.Value;
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
                query.OrderBy = "ORDER BY RegisterDate desc";

            string json;
            try
            {
                var resultSet = new DataTableResultSet<UserListItem>();
                var listResult = queryDispatcher.Dispatch<GetUserListQuery, QueryListResult<UserListItem>>(query);
                resultSet.draw = dataParameter.Draw;
                resultSet.recordsTotal = listResult.Result.First().RecordCount;
                resultSet.recordsFiltered = resultSet.recordsTotal;
                resultSet.data = listResult.Result;
                /*foreach (var item in listResult.Result)
                {
                    var columns = new List<string>();
                    columns.Add("");
                    columns.Add(item.NameSurname);
                    columns.Add(item.Email);
                    columns.Add(item.BirthDate.ToShortDateString());
                    columns.Add(item.Sex == "e"? "Erkek": (item.Sex == "k" ? "Kadın" : ""));
                    columns.Add(item.CellPhone);
                    columns.Add(item.Job);
                    columns.Add(item.City);
                    columns.Add(item.WorkStatus);
                    columns.Add(item.ComPermission? item.ComLastDate.ToLongDateString() :"");

                    resultSet.data  .Add(columns);
                }*/
                json = resultSet.ToJSON();
            }
            catch (Exception ex)
            {
                DataTableResultError<UserListItem> error = new DataTableResultError<UserListItem>();
                error.error = "Bir hata oluştu. Lütfen sonra tekrar deneyiniz";
                json = error.ToJSON();
            }

            return json;
        }

        [Authorize]
        public string GetNotificationUsers(DataTableParameters dataParameter)
        {
            GetNotificationUserListQuery query = new GetNotificationUserListQuery();
            query.Search = dataParameter.Search.Value;
            query.PageNumber = dataParameter.Start / dataParameter.Length + 1;
            query.PageSize = dataParameter.Length;

            StringBuilder sb = new StringBuilder();
            foreach (var order in dataParameter.Order)
            {
                sb.AppendFormat("{0} {1},", notificationColumns[order.Column], order.Dir);
            }
            int length = sb.Length;
            if (length > 0)
            {
                query.OrderBy = "ORDER BY " + sb.ToString().Substring(0, length - 1);
            }
            else
                query.OrderBy = "ORDER BY RegisterDate desc";

            string json;
            try
            {
                var resultSet = new DataTableResultSet<UserListItem>();
                var listResult = queryDispatcher.Dispatch<GetNotificationUserListQuery, QueryListResult<UserListItem>>(query);
                resultSet.draw = dataParameter.Draw;
                resultSet.recordsTotal = listResult.Result.First().RecordCount;
                resultSet.recordsFiltered = resultSet.recordsTotal;
                resultSet.data = listResult.Result;
                json = resultSet.ToJSON();
            }
            catch (Exception ex)
            {
                DataTableResultError<UserListItem> error = new DataTableResultError<UserListItem>();
                error.error = "Bir hata oluştu. Lütfen sonra tekrar deneyiniz";
                json = error.ToJSON();
            }

            return json;
        }

        [Authorize]
        public JsonResult GetPayments(int userId)
        {
            GetUserPaymentsListQuery query = new GetUserPaymentsListQuery();
            query.UserId = userId;

            try
            {
                var listResult = queryDispatcher.Dispatch<GetUserPaymentsListQuery, QueryListResult<PaymentItem>>(query);
                return Json(listResult.Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json("Bir hata oluştu, lütfen sonra tekrar deneyin.", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
