using Newtonsoft.Json;
using System.Web;
using System.Web.Mvc;

namespace Emoda.PekiYaBen.Web.Models
{
    public class ApiResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (!string.IsNullOrEmpty(Error))
                response.Output.Write(JsonConvert.SerializeObject(new { error = this.Error }));
            else if (Data != null || Token != null)
                response.Output.Write(JsonConvert.SerializeObject(new { data = this.Data, token=this.Token }));
            else
                response.Output.Write(JsonConvert.SerializeObject(new { }));
        }
        public string Error { get; set; }

        public string Token { get; set; }
    }
}