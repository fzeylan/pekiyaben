using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Helpers;
using Emoda.PekiYaBen.Business.Queries;
using Emoda.PekiYaBen.Entity.Comment;
using Emoda.PekiYaBen.Web.Models.DataTable;
using OLCA.Infrastructure.CQS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Emoda.PekiYaBen.Web.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private IQueryDispatcher queryDispatcher;
        private ICommandDispatcher commandDispatcher;

        public CommentController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Yorum Listesi";
            ViewBag.PageHeader = "Yorum Listesi";
            ViewBag.ActivePage = "CommentList";

            return View();
        }

        public string GetComments(DataTableParameters dataParameter)
        {
            GetCommentListQuery query = new GetCommentListQuery();
            query.Search = dataParameter.Search.Value;

            string json;
            try
            {
                var resultSet = new DataTableResultSet<CommentListItem>();
                var listResult = queryDispatcher.Dispatch<GetCommentListQuery, QueryListResult<CommentListItem>>(query);
                resultSet.draw = dataParameter.Draw;
                resultSet.recordsTotal = listResult.Result.FirstOrDefault() != null ? listResult.Result.FirstOrDefault().RecordCount : 0;
                resultSet.recordsFiltered = resultSet.recordsTotal;
                resultSet.data = listResult.Result;

                json = resultSet.ToJSON();
            }
            catch (Exception ex)
            {
                DataTableResultError<CommentListItem> error = new DataTableResultError<CommentListItem>();
                error.error = "Bir hata oluştu. Lütfen sonra tekrar deneyiniz";
                json = error.ToJSON();
            }

            return json;
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Yorum Düzenle";
            ViewBag.PageHeader = "Yorum Düzenle";
            ViewBag.ActivePage = "CommentList";

            LoadListForEdit();
            QuerySingleResult<CommentInfo> product = queryDispatcher.Dispatch<GetCommentInfoQuery, QuerySingleResult<CommentInfo>>(new GetCommentInfoQuery(id));

            return View(product.Result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CommentInfo info)
        {
            ViewBag.Title = "Yorum Düzenle";
            ViewBag.PageHeader = "Yorum Düzenle";
            ViewBag.ActivePage = "CommentList";
            
            LoadListForEdit();
            CommentInfo product;
            try
            {
                QuerySingleResult<CommentInfo> productQuery = queryDispatcher.Dispatch<GetCommentInfoQuery, QuerySingleResult<CommentInfo>>(new GetCommentInfoQuery(info.Id));
                product = (CommentInfo)productQuery.Result;

                var command = new UpdateCommentCommand
                {
                    Id = info.Id,
                    CoachId = info.CoachId,
                    ProductId = info.ProductId,
                    FullName = info.FullName,
                    Title = info.Title,
                    Comment = info.Comment,
                    Stars = info.Stars,
                    CreatedDate = info.CreatedDate
                };
                product = (CommentInfo)commandDispatcher.Dispatch<UpdateCommentCommand, CommentInfo>(command);
            }
            catch (Exception ex)
            {
             
                return View();
            }

            return RedirectToAction("Edit", new { id = product.Id });
        }

        public void LoadListForEdit()
        {
            DataAccess da = new DataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["PekiYaBen"].ConnectionString);
            var coaches = da.GetDataReader("Select * FROM Coach Where Status = 1 Order By FullName ASC", new List<SqlParameter> { });
            var products = da.GetDataReader("Select * FROM Product Where Status = 1 and Type = 3 Order By Title ASC", new List<SqlParameter> { });

            List<SelectListItem> productList = new List<SelectListItem>();
            foreach (DataRow row in products.Rows)
            {
                SelectListItem item = new SelectListItem();
                item.Text = row["Title"].ToString();
                item.Value = row["Id"].ToString();
                productList.Add(item);
            }
            List<SelectListItem> coachList = new List<SelectListItem>();
            foreach (DataRow row in coaches.Rows)
            {
                SelectListItem item = new SelectListItem();
                item.Text = row["FullName"].ToString();
                item.Value = row["Id"].ToString();
                coachList.Add(item);
            }

            ViewBag.Products = productList;
            ViewBag.Coaches = coachList;
        }


    }
}
