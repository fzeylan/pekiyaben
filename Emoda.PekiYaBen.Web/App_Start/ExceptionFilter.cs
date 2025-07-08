using Emoda.PekiYaBen.Entity;
using Emoda.PekiYaBen.Web.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;

namespace Emoda.PekiYaBen.Web.App_Start
{
    public class PekiYaBenExceptionHandler : ExceptionHandler
    {
        private static ILog logger = LogManager.GetLogger(typeof(ExceptionFilterAttribute));

        public override void Handle(ExceptionHandlerContext context)
        {
            //Check the Exception Type
            ApiResult result = new ApiResult();
            if (context.Exception is EmodaException)
            {
                //The Response Message Set by the Action During Ececution
                var res = context.Exception.Message;
                var inner = (EmodaException)context.Exception.InnerException;
                logException(context, inner);
                result.Error = inner.Message;
            }
            else
            {
                logException(context, context.Exception);

                result.Error = "İşlem sırasında bir hata oluştu. Lütfen sonra tekrar deneyiniz.";
            }

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(result))
            };
            //Create the Error Response

            context.Result = new ApiExceptionResult(context.Request, response);
        }

        public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            Handle(context);
            return Task.FromResult(0);
        }
        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }

        private static void logException(ExceptionHandlerContext context, Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Request : {0} \n", context.Request);
            sb.Append("Params");
            var data = context.Request.Content.ReadAsStringAsync().Result;
            try
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                foreach(var KeyVal in dict)
                {
                    sb.AppendFormat("{0} : {1}\n", KeyVal.Key, KeyVal.Value);
                }
            } catch
            {
                sb.Append(data);
            }            

            var claims = context.Request.GetOwinContext().Authentication.User.Claims;
            var id = claims.FirstOrDefault(x => x.Type == "Id");
            sb.AppendFormat("User : {0} \nClaims: ", id);

            foreach (var c in claims)
            {
                sb.Append(c.Type + ",");
            }
            sb.Append("\n\r");
            //actionExecutedContext.ActionContext.ActionArguments.AsEnumerable();
            logger.Error(sb.ToString(), ex);
        }

        public class ApiExceptionResult : IHttpActionResult
        {
            private HttpRequestMessage _request;
            private HttpResponseMessage _httpResponseMessage;

            public ApiExceptionResult(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
            {
                _request = request;
                _httpResponseMessage = httpResponseMessage;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_httpResponseMessage);
            }
        }
    }
}