using Autofac;
using CustomerOrderManagement.API.DependencyInjection;
using CustomerOrderManagement.API.Logging;
using Serilog;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
namespace CustomerOrderManagement.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LoggerConfig.Configure();
            //Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File(
            //            path: "Logs/api-.log",
            //            rollingInterval: RollingInterval.Day,
            //            retainedFileCountLimit: 30)
            //            .CreateLogger();

            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            AutofacConfig.Register();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
