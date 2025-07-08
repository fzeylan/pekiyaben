using Emoda.PekiYaBen.Web.App_Start;
using Emoda.PekiYaBen.Web.Filters;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Emoda.PekiYaBen.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Services.Replace(typeof(IExceptionHandler), new PekiYaBenExceptionHandler());
            //config.MessageHandlers.Add(new PreflightRequestsHandler()); 
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            ); 
        }
    }
}
