using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NLog;

namespace MyLanguagePalService
{
    public class MvcApplication : HttpApplication
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            // *** Configure Autofac ***
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            // *** Configure MVC ***
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Handle all unhandled exceptions here
        /// </summary>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception == null)
                return;

            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() < 500)
                return; // Ignore all non-server errors

            logger.Fatal(exception, "Internal server error.");
        }
    }
}
