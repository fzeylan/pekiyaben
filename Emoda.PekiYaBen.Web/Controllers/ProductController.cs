using Emoda.PekiYaBen.Business.Commands;
using Emoda.PekiYaBen.Business.DTOs;
using Emoda.PekiYaBen.Business.Helpers;
using Emoda.PekiYaBen.Business.Queries;
using Emoda.PekiYaBen.Entity.Coaching;
using Emoda.PekiYaBen.Entity.Helpers;
using Emoda.PekiYaBen.Web.Models.DataTable;
using OLCA.Infrastructure.CQS;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Emoda.PekiYaBen.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private IQueryDispatcher queryDispatcher;
        private ICommandDispatcher commandDispatcher;

        private string[] columns = new string[] {
            "Code",
            "Type",
            "Title",
            "Price",
            "Status"
        };

        public ProductController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Ürün Listesi";
            ViewBag.PageHeader = "Ürün Listesi";
            ViewBag.ActivePage = "ProductList";

            return View();
        }

        public string GetProducts(DataTableParameters dataParameter)
        {
            GetProductListQuery query = new GetProductListQuery();
            query.Search = dataParameter.Search.Value;

            string json;
            try
            {
                var resultSet = new DataTableResultSet<ProductListItem>();
                var listResult = queryDispatcher.Dispatch<GetProductListQuery, QueryListResult<ProductListItem>>(query);
                resultSet.draw = dataParameter.Draw;
                resultSet.recordsTotal = listResult.Result.First().RecordCount;
                resultSet.recordsFiltered = resultSet.recordsTotal;
                resultSet.data = listResult.Result;

                json = resultSet.ToJSON();
            }
            catch (Exception ex)
            {
                DataTableResultError<ProductListItem> error = new DataTableResultError<ProductListItem>();
                error.error = "Bir hata oluştu. Lütfen sonra tekrar deneyiniz";
                json = error.ToJSON();
            }

            return json;
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Ürün Düzenle";
            ViewBag.PageHeader = "Ürün Düzenle";
            ViewBag.ActivePage = "ProductList";

            QuerySingleResult<ProductInfo> product = queryDispatcher.Dispatch<GetProductInfoQuery, QuerySingleResult<ProductInfo>>(new GetProductInfoQuery(id));

            return View(product.Result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ProductInfo info)
        {
            ViewBag.Title = "Koç Düzenle";
            ViewBag.PageHeader = "Koç Düzenle";
            ViewBag.ActivePage = "ProductList";

            ProductInfo product;
            try
            {
                QuerySingleResult<ProductInfo> productQuery = queryDispatcher.Dispatch<GetProductInfoQuery, QuerySingleResult<ProductInfo>>(new GetProductInfoQuery(info.Id));
                product = (ProductInfo)productQuery.Result;


                var path = "";

                //C:\inetpub\wwwroot\pekiyaben.com\http\Content\assets\img\blog
                //C:\inetpub\wwwroot\pekiyaben.com\subdomains\yonetim\Content\

                var command = new UpdateProductCommand
                {
                    Id = info.Id,
                    Code = info.Code,
                    Title = info.Title,
                    Type = info.Type,
                    Description = info.Description,
                    File = info.File,
                    ContentDescription = info.ContentDescription,

                    Price = info.Price,
                    SortOrder = info.SortOrder,
                    Status = info.Status,
                    ShowMainPage = info.ShowMainPage
                };

                if (info.ContentImage != null)
                {
                    path = Server.MapPath("/").Replace(@"subdomains\yonetim\", @"http\") + "content\\assets\\img\\product\\" + info.Title.ToSEOFriendly(200) + ".png";
                    ImageHelper.SaveImage(info.ContentImage, path);
                    command.ContentImage = "/Content/assets/img/product/" + info.Title.ToSEOFriendly(200) + ".png";
                }

                if (info.Image != null)
                    command.Image = info.Image;

                product = (ProductInfo)commandDispatcher.Dispatch<UpdateProductCommand, ProductInfo>(command);
            }
            catch (Exception ex)
            {
                return View();
            }

            return RedirectToAction("Edit", new { id = product.Id });
        }
    }
}
