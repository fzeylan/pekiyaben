using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Queries;
using Emoda.PekiYaBen.Entity.Coaching;
using Emoda.PekiYaBen.Web.Models.DataTable;
using OLCA.Infrastructure.CQS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Emoda.PekiYaBen.Web.Controllers
{
    [Authorize]
    public class CoachController : Controller
    {
        private IQueryDispatcher queryDispatcher;
        private ICommandDispatcher commandDispatcher;

        private string[] columns = new string[] {
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


        private string[] interviewColumns = new string[] {
            "CoachName",
            "InterviewType",
            "UserName",
            "AvailableDate",
            "TransactionDate",
            "Price",
            "Status"
        };

        public CoachController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Koç Listesi";
            ViewBag.PageHeader = "Koç Listesi";
            ViewBag.ActivePage = "CoachList";

            return View();
        }

        public string GetCoaches(DataTableParameters dataParameter)
        {
            GetCoachListQuery query = new GetCoachListQuery();
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
                var resultSet = new DataTableResultSet<CoachListItem>();
                var listResult = queryDispatcher.Dispatch<GetCoachListQuery, QueryListResult<CoachListItem>>(query);
                resultSet.draw = dataParameter.Draw;
                resultSet.recordsTotal = listResult.Result.First().RecordCount;
                resultSet.recordsFiltered = resultSet.recordsTotal;
                resultSet.data = listResult.Result;

                json = resultSet.ToJSON();
            }
            catch (Exception ex)
            {
                DataTableResultError<CoachListItem> error = new DataTableResultError<CoachListItem>();
                error.error = "Bir hata oluştu. Lütfen sonra tekrar deneyiniz";
                json = error.ToJSON();
            }

            return json;
        }

        public ActionResult Interviews()
        {
            ViewBag.Title = "Koçluk Randevuları";
            ViewBag.PageHeader = "Koçluk Randevuları";
            ViewBag.ActivePage = "CoachList";

            return View();
        }

        public string GetInterviews(DataTableParameters dataParameter)
        {
            GetInterviewListQuery query = new GetInterviewListQuery();
            query.Search = dataParameter.Search.Value;
            query.PageNumber = dataParameter.Start / dataParameter.Length + 1;
            query.PageSize = dataParameter.Length;

            StringBuilder sb = new StringBuilder();
            foreach (var order in dataParameter.Order)
            {
                sb.AppendFormat("{0} {1},", interviewColumns[order.Column], order.Dir);
            }
            int length = sb.Length;
            if (length > 0)
            {
                query.OrderBy = "ORDER BY " + sb.ToString().Substring(0, length - 1);
            }
            else
                query.OrderBy = "ORDER BY AvailableDate desc";

            string json;
            try
            {
                var resultSet = new DataTableResultSet<InterviewListItem>();
                var listResult = queryDispatcher.Dispatch<GetInterviewListQuery, QueryListResult<InterviewListItem>>(query);
                resultSet.draw = dataParameter.Draw;
                resultSet.recordsTotal = listResult.Result.First().RecordCount;
                resultSet.recordsFiltered = resultSet.recordsTotal;
                resultSet.data = listResult.Result;

                json = resultSet.ToJSON();
            }
            catch (Exception ex)
            {
                DataTableResultError<InterviewListItem> error = new DataTableResultError<InterviewListItem>();
                error.error = "Bir hata oluştu. Lütfen sonra tekrar deneyiniz";
                json = error.ToJSON();
            }

            return json;
        }

        [HttpGet]
        public ActionResult Calendar()
        {
            ViewBag.Title = "Randevu Takvimi";
            ViewBag.PageHeader = "Randevu Takvimi";
            ViewBag.ActivePage = "CoachList";

            return View();
        }


        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Koç Düzenle";
            ViewBag.PageHeader = "Koç Düzenle";
            ViewBag.ActivePage = "CoachList";

            QuerySingleResult<CoachInfo> coach = queryDispatcher.Dispatch<GetCoachInfoQuery, QuerySingleResult<CoachInfo>>(new GetCoachInfoQuery(id));

            GetProductInfoListQuery query = new GetProductInfoListQuery();
            query.PageSize = 100;
            var products = queryDispatcher.Dispatch<GetProductInfoListQuery, QueryListResult<ProductInfo>>(query).Result;

            ViewBag.Products = products.Where(x => x.Type == Entity.Enums.General.ProductType.Meeting).ToList();

            return View(coach.Result);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(CoachInfo info)
        {
            ViewBag.Title = "Koç Düzenle";
            ViewBag.PageHeader = "Koç Düzenle";
            ViewBag.ActivePage = "CoachList";

            CoachInfo coach;
            try
            {
                QuerySingleResult<CoachInfo> coachQuery = queryDispatcher.Dispatch<GetCoachInfoQuery, QuerySingleResult<CoachInfo>>(new GetCoachInfoQuery(info.Id));
                coach = (CoachInfo)coachQuery.Result;

                var command = new UpdateCoachCommand
                {
                    Id = info.Id,
                    FullName = info.FullName,
                    Email = info.Email,
                    DateOfBirth = info.DateOfBirth,
                    PhoneNumber = info.PhoneNumber,
                    Title = info.Title,
                    ProfilePhoto = info.ProfilePhoto,
                    Description = info.Description,
                    EducationInfo = info.EducationInfo,
                    CoachingInfo = (info.CoachingInfo.Length > 0 ? info.CoachingInfo.Substring(0, info.CoachingInfo.Length - 1) : ""),
                    CertificateInfo = info.CertificateInfo,
                    Status = info.Status,
                    ShowMainPage = info.ShowMainPage
                };
                coach = (CoachInfo)commandDispatcher.Dispatch<UpdateCoachCommand, CoachInfo>(command);
            }
            catch (Exception)
            {
                GetProductInfoListQuery query = new GetProductInfoListQuery();
                query.PageSize = 100;
                var products = queryDispatcher.Dispatch<GetProductInfoListQuery, QueryListResult<ProductInfo>>(query).Result;

                ViewBag.Products = products.Where(x => x.Type == Entity.Enums.General.ProductType.Meeting).ToList();
                return View();
            }

            return RedirectToAction("Edit", new { id = coach.Id });

            //GetProductListQuery query = new GetProductListQuery();
            //query.PageSize = 100;
            //var products = queryDispatcher.Dispatch<GetProductListQuery, QueryListResult<ProductInfo>>(query).Result;

            //ViewBag.Products = products.Where(x => x.Type == Entity.Enums.General.ProductType.Meeting).ToList();
            //return View(coach);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddCoachCalendar(CoachCalendarInfo info)
        {
            int id = 0;
            UpdateCoachCalendarCommand command = new UpdateCoachCalendarCommand();
            try
            {
                command = new UpdateCoachCalendarCommand
                {
                    //Id = info.Id,
                    CoachId = info.CoachId,
                    AvailableDate = info.AvailableDate,
                    InterviewType = info.InterviewType,
                    ParticipantLimit = info.ParticipantLimit,
                    Status = Entity.Enums.General.CoachInterviewStatus.Active,
                    CoachIsFull = info.CoachIsFull
                };
                id = commandDispatcher.Dispatch<UpdateCoachCalendarCommand, int>(command);
            }
            catch (Exception ex)
            {
            }
            return Json(command);
        }

        [Authorize]
        [HttpPost]
        public ActionResult RemoveCoachCalendar(CoachCalendarInfo info)
        {
            bool response = false;
            try
            {
                var command = new RemoveCoachCalendarCommand
                {
                    Id = info.Id
                };
                response = commandDispatcher.Dispatch<RemoveCoachCalendarCommand, bool>(command);
            }
            catch (Exception ex)
            {
            }
            return Json(response);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetCoachCalendar(CoachCalendarInfo info)
        {
            List<object> response = new List<object>();
            int calendarId = 0;
            try
            {
                GetCoachCalendarListQuery q = new GetCoachCalendarListQuery(info.Id, info.StartDate, info.EndDate);
                var calendar = queryDispatcher.Dispatch<GetCoachCalendarListQuery, QueryListResult<CoachCalendarInfo>>(q).Result;

                foreach (var cal in calendar)
                {
                    var participantList = calendar.Where(x => x.Id == cal.Id && !String.IsNullOrEmpty(x.FullName)).Select(x => x.FullName).ToList();
                    var participants = String.Join(", ", participantList);

                    if (participants.Length > 0)
                    {
                        response.Add(new
                        {
                            title = (cal.CoachIsFull == true ? ("Dolu Randevu"): (cal.InterviewType + " (" + participantList.Count + "/" + cal.ParticipantLimit + ") " + participants)),
                            start = cal.AvailableDate.ToString("yyyy-MM-dd HH:mm"),
                            end = cal.AvailableDate.AddHours(1).ToString("yyyy-MM-dd HH:mm"),
                            allDay = false,
                            editable = false,
                            id = cal.Id,
                            color = (cal.CoachIsFull == true ? "#ff4d7e" : "#19a5c9")
                        });
                    }
                    else
                    {
                        response.Add(new
                        {
                            title = (cal.CoachIsFull == true ? ("Dolu Randevu") : cal.InterviewType + " (" + cal.ParticipantLimit + " kişi)"),
                            start = cal.AvailableDate.ToString("yyyy-MM-dd HH:mm"),
                            end = cal.AvailableDate.AddHours(1).ToString("yyyy-MM-dd HH:mm"),
                            allDay = false,
                            editable = false,
                            id = cal.Id,
                            color = (cal.CoachIsFull == true ? "#ff4d7e" : "#19a5c9")
                        });
                    }
                    calendarId = cal.Id;
                }
            }
            catch (Exception ex)
            {
            }
            return Json(response.Distinct().ToList());
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetCalendar(CoachCalendarInfo info)
        {
            List<object> response = new List<object>();
            try
            {
                GetCalendarListQuery q = new GetCalendarListQuery(info.Id, info.StartDate, info.EndDate);
                var calendar = queryDispatcher.Dispatch<GetCalendarListQuery, QueryListResult<CoachCalendarInfo>>(q).Result;

                foreach (var cal in calendar)
                {
                    response.Add(new
                    {
                        title = "Görüşme var",
                        start = cal.AvailableDate.ToString("yyyy-MM-dd HH:mm"),
                        end = cal.AvailableDate.AddHours(1).ToString("yyyy-MM-dd HH:mm"),
                        allDay = false,
                        editable = false,
                        eventcolor = "#378006",
                        id = cal.Id
                    });
                }
            }
            catch (Exception ex)
            {
            }
            return Json(response);
        }
    }
}
