using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Queries;
using Emoda.PekiYaBen.Entity.FAQ;
using Emoda.PekiYaBen.Web.Models.DataTable;
using OLCA.Infrastructure.CQS;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Emoda.PekiYaBen.Web.Controllers
{
    [Authorize]
    public class FAQController : Controller
    {
        private IQueryDispatcher queryDispatcher;
        private ICommandDispatcher commandDispatcher;

        public FAQController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "SSS Listesi";
            ViewBag.PageHeader = "SSS Listesi";
            ViewBag.ActivePage = "SSSList";

            return View();
        }

        public string GetFAQ(DataTableParameters dataParameter)
        {
            GetFAQListQuery query = new GetFAQListQuery();
            query.Search = dataParameter.Search.Value;

            string json;
            try
            {
                var resultSet = new DataTableResultSet<FAQListItem>();
                var listResult = queryDispatcher.Dispatch<GetFAQListQuery, QueryListResult<FAQListItem>>(query);
                resultSet.draw = dataParameter.Draw;
                resultSet.recordsTotal = listResult.Result.FirstOrDefault() != null ? listResult.Result.FirstOrDefault().RecordCount : 0;
                resultSet.recordsFiltered = resultSet.recordsTotal;
                resultSet.data = listResult.Result;

                json = resultSet.ToJSON();
            }
            catch (Exception ex)
            {
                DataTableResultError<FAQListItem> error = new DataTableResultError<FAQListItem>();
                error.error = "Bir hata oluştu. Lütfen sonra tekrar deneyiniz";
                json = error.ToJSON();
            }

            return json;
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Title = "SSS Listesi";
            ViewBag.PageHeader = "SSS Listesi";
            ViewBag.ActivePage = "SSSList";

            QuerySingleResult<FAQInfo> product = queryDispatcher.Dispatch<GetFAQInfoQuery, QuerySingleResult<FAQInfo>>(new GetFAQInfoQuery(id));

            return View(product.Result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FAQInfo info)
        {
            ViewBag.Title = "SSS Listesi";
            ViewBag.PageHeader = "SSS Listesi";
            ViewBag.ActivePage = "SSSList";

            FAQInfo product;
            try
            {
                QuerySingleResult<FAQInfo> productQuery = queryDispatcher.Dispatch<GetFAQInfoQuery, QuerySingleResult<FAQInfo>>(new GetFAQInfoQuery(info.Id));
                product = (FAQInfo)productQuery.Result;

                var command = new UpdateFAQCommand
                {
                    Id = info.Id,
                    SortOrder = info.SortOrder,
                    Title = info.Title,
                    Description = info.Description,
                    Status = info.Status
                };
                product = (FAQInfo)commandDispatcher.Dispatch<UpdateFAQCommand, FAQInfo>(command);
            }
            catch (Exception ex)
            {
             
                return View();
            }

            return RedirectToAction("Edit", new { id = product.Id });
        }
    }
}
