using Microsoft.Ajax.Utilities;
using PekiYaBen.WebSite.Extensions;
using PekiYaBen.WebSite.Helpers;
using PekiYaBen.WebSite.Models;
using PekiYaBen.WebSite.Models.EntityModels;
using PekiYaBen.WebSite.Models.HepsiBurada;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PekiYaBen.WebSite.Controllers
{
    public class OrderController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        PekiYaBenDBEntities entities = new PekiYaBenDBEntities();

        [Route("orders")]
        [HttpPost]
        public JsonResult orders(List<OrderItem> items)
        {
            var lineItems = new List<object>();

            if (items == null)
                return null;

            //try
            //{
            //    var request = entities.HBRequests.Add(new HBRequest
            //    {
            //        Request = Newtonsoft.Json.JsonConvert.SerializeObject(items),
            //        Created = DateTime.Now
            //    });

            //    entities.SaveChanges();

            //    foreach (var item in items)
            //    {
            //        var order = new HBOrder
            //        {
            //            Guid = item.Id,
            //            OrderId = item.OrderNumber,
            //            OrderDate = item.OrderDate,
            //            SKU = item.SKU,
            //            Quantity = item.Quantity,
            //            TotalPrice = item.TotalPrice.Amount,
            //            UnitPrice = item.UnitPrice.Amount,
            //            Discount = item.HBDiscount.TotalPrice.Amount,
            //            Vat = item.Vat.Amount,
            //            VatRate = item.VatRate,
            //            InvoiceName = item.Invoice.Address.Name,
            //            IdentityNo = item.Invoice.TurkishIdentityNumber,
            //            TaxNo = item.Invoice.TaxNumber,
            //            TaxOffice = item.Invoice.TaxOffice,
            //            Address = item.Invoice.Address.Address,
            //            CountryCode = item.Invoice.Address.CountryCode,
            //            Email = item.Invoice.Address.Email,
            //            PhoneNumber = item.Invoice.Address.PhoneNumber,
            //            AlternatePhoneNumber = item.Invoice.Address.AlternatePhoneNumber,
            //            District = item.Invoice.Address.District,
            //            City = item.Invoice.Address.City,
            //            Town = item.Invoice.Address.Town,
            //            Status = item.Status
            //        };

            //        entities.HBOrders.Add(order);
            //        entities.SaveChanges();

            //        lineItems.Add(new { id = order.Guid, quantity = "1" });

            //        StringBuilder sb = new StringBuilder();
            //        sb.AppendFormat("Kullanıcı : {0}<br/>", order.InvoiceName).AppendFormat("E-posta : {0}<br/>", order.Email).AppendFormat("Telefon : {0}<br/>", order.PhoneNumber);

            //        var mailbody = MailHelper.PrepareMailBody("Hepsi Burada yeni sipariş.", sb.ToString());

            //        MailHelper.SendMail("fzeylan@yahoo.com", "Hepsi Burada yeni sipariş.", mailbody);
            //        //MailHelper.SendMail("form@pekiyaben.com", "Hepsi Burada yeni sipariş.", mailbody);
            //    }


                //Kalemi hazırla
            //    string username = ConfigurationSettings.AppSettings["HBUsername"].ToString();
            //    string password = ConfigurationSettings.AppSettings["HBPassword"].ToString();
            //    string merchant = ConfigurationSettings.AppSettings["HBMerchant"].ToString();

            //    var credentials = Encoding.ASCII.GetBytes($"{username}:{password}");
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));

            //    var body = new
            //    {
            //        parcelQuantity = "1",
            //        deci = "1",
            //        lineItemRequests = lineItems
            //    };

            //    var url = $"https://oms-external-sit.hepsiburada.com/packages/merchantid/{merchant}";

            //    var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            //    var result = client.PostAsync(url, content).Result;

            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}

            return Json("OK");
        }
    }
}