using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using MyLanguagePalService.Controllers;
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
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
