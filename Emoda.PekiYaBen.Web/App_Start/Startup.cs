using Emoda.PekiYaBen.Web.App_Start;
using Emoda.PekiYaBen.Web.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Facebook;
using Newtonsoft.Json.Serialization;
using OLCA.Infrastructure.CQS;
using Owin;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Emoda.PekiYaBen.Web.Startup))]
namespace Emoda.PekiYaBen.Web
{
    // Servis çalışmaya başlarken Owin pipeline'ını ayağa kaldırabilmek için Startup'u hazırlıyoruz.
    public class Startup
    {
        Container container;
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();

            container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().Where(x => x.FullName.StartsWith("OLCA") || x.FullName.StartsWith("Emoda"));
            // Register a custom PropertySelectionBehavior to enable property injection.
            container.Options.PropertySelectionBehavior =
                new ImportAttributePropertySelectionBehavior();

            container.Register(
                     typeof(ICommandHandler<,>),
                     assemblies);

            container.Register(
                typeof(IQueryHandler<,>),
                assemblies);

            IOCCommandDispatcher commandDipatcher = new IOCCommandDispatcher(container);
            IOCQueryDispatcher queryDispatcher = new IOCQueryDispatcher(container);

            container.RegisterSingleton<ICommandDispatcher>(() => commandDipatcher);
            container.RegisterSingleton<IQueryDispatcher>(() => queryDispatcher);

            // This is an extension method from the integration package as well.
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            httpConfiguration.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            container.RegisterWebApiControllers(httpConfiguration);
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            httpConfiguration.Filters.Add(new ModelValidationFilter());

            //DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            httpConfiguration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

            ConfigureOAuth(appBuilder, commandDipatcher);
            /*httpConfiguration.SuppressDefaultHostAuthentication();*/
            /*httpConfiguration.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));*/

            appBuilder.UseWebApi(httpConfiguration);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            httpConfiguration.EnsureInitialized();
        }

        private void ConfigureOAuth(IAppBuilder appBuilder, ICommandDispatcher commandDispatcher)
        {
            appBuilder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationMode= Microsoft.Owin.Security.AuthenticationMode.Active,
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                CookiePath = "/",
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/LogOff"),
                CookieName = "pekiYaBen",
                CookieSecure = CookieSecureOption.SameAsRequest
            });

            appBuilder.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
        }
    }

    public class ImportAttributePropertySelectionBehavior : IPropertySelectionBehavior
    {
        public bool SelectProperty(Type serviceType, PropertyInfo propertyInfo)
        {
            // Makes use of the System.ComponentModel.Composition assembly
            return (typeof(Controller).IsAssignableFrom(serviceType) || typeof(ApiController).IsAssignableFrom(serviceType) ||
                typeof(ClaimsAuthorize).IsAssignableFrom(serviceType)) &&
                   propertyInfo.GetCustomAttributes<ImportAttribute>().Any();
        }
    }
}