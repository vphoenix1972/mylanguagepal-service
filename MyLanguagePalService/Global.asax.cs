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

        /// <summary>
        /// Handle all unhandled exceptions here
        /// </summary>
        protected void Application_Error(object sender, EventArgs e)
        {
            //Response.TrySkipIisCustomErrors = true;

            //var exception = Server.GetLastError();
            //if (exception == null)
            //    return;

            //var httpException = exception as HttpException;
            //if (httpException != null && httpException.GetHttpCode() < 500)
            //    return; // Ignore all non-server errors

            //logger.Fatal(exception, "Internal server error.");

            Exception exception = Server.GetLastError();

            // Log the exception.
            //ILogger logger = Container.Resolve<ILogger>();
            //logger.Error(exception);

            Response.Clear();

            HttpException httpException = exception as HttpException;

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");

            if (httpException == null)
            {
                routeData.Values.Add("action", "Http500");
            }
            else //It's an Http Exception, Let's handle it.
            {
                switch (httpException.GetHttpCode())
                {
                    // ToDo: Custom handling of 401
                    // http://stackoverflow.com/questions/781861/customerrors-does-not-work-when-setting-redirectmode-responserewrite
                    // http://stackoverflow.com/questions/619895/how-can-i-properly-handle-404-in-asp-net-mvc
                    case 401:
                        routeData.Values.Add("action", "Http401");
                        break;
                    case 403:
                        routeData.Values.Add("action", "Http403");
                        break;
                    case 404:
                        routeData.Values.Add("action", "Http404");
                        break;
                    case 500:
                        routeData.Values.Add("action", "Http500");
                        break;
                    // Here you can handle Views to other error codes.
                    // I choose a General error template  
                    default:
                        routeData.Values.Add("action", "Http500");
                        break;
                }
            }

            // Pass exception details to the target error View.
            routeData.Values.Add("error", exception);

            // Clear the error on server.
            Server.ClearError();

            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;
            Response.Headers["Content-type"] = "text/html";

            // Call target Controller and pass the routeData.
            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(
                 new HttpContextWrapper(Context), routeData));
        }
    }
}
