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

        // HttpApplication can be recreated during Application_Start and Application_BeginRequest
        // So it is nesessary to use static variable
        private static bool _isFailedToStart;

        protected void Application_Start()
        {
            try
            {
                // *** Configure Autofac ***
                //var builder = new ContainerBuilder();

                //// Register your MVC controllers.
                //builder.RegisterControllers(typeof(MvcApplication).Assembly);

                //// Set the dependency resolver to be Autofac.
                //var container = builder.Build();
                //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


                // *** Configure MVC ***
                AreaRegistration.RegisterAllAreas();
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
            }
            catch (Exception e)
            {
                // Mark service as unavailable
                _isFailedToStart = true;

                // Log exception
                logger.Fatal(e, "Server has failed to start");
            }

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Log request
            DumpRequest(Request);

            // If server has failed to start, show 503 to user
            if (_isFailedToStart)
            {
                // Stop request processing
                Response.End();
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            // Show 503 to user if the server is unavailable
            if (_isFailedToStart)
            {
                ShowErrorPage(503);
            }
            else
            {
                // If there was a error (except 503), show custom error page to user
                var response = Response;

                // Check status code
                var statusCode = response.StatusCode;
                if (statusCode >= 400)
                {
                    ShowErrorPage(statusCode);
                }

                // else request finished successfully
            }

            // Dump response
            DumpResponse(Response);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            // ASP MVC raises an HttpException for non-existing controller or action
            // Don't treat them as fatal errors
            var httpException = exception as HttpException;
            if (httpException != null)
            {
                var statusCode = httpException.GetHttpCode();
                if (statusCode == 404)
                    return; // Ignore exception
            }

            logger.Fatal(exception, "Internal server error");
        }

        private void ShowErrorPage(int statusCode)
        {
            // Main handler for http errors

            Response.StatusCode = statusCode;

            switch (statusCode)
            {
                // 400
                case 400:
                    ShowErrorPage("Http400");
                    break;
                // 401
                case 401:
                    ShowErrorPage("Http401");
                    break;
                // 403
                case 403:
                    ShowErrorPage("Http403");
                    break;
                // 404
                case 404:
                    ShowErrorPage("Http404");
                    break;
                case 503:
                    ShowErrorPage("Http503");
                    break;
                default:
                    var exception = Server.GetLastError();
                    ShowErrorPage("Http500", exception);
                    break;
            }
        }

        private void ShowErrorPage(string action, Exception exception = null)
        {
            Response.Clear();

            // Create route to error controller
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", action);

            // Pass exception details to the target error View.
            if (exception != null)
            {
                routeData.Values.Add("error", exception);

                // Clear the error on server.
                Server.ClearError();
            }

            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;
            Response.Headers["Content-type"] = "text/html";

            // Call target Controller and pass the routeData.
            // ToDo: Causes System.ObjectDisposedException when Autofac is used
            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(
                 new HttpContextWrapper(Context), routeData));
        }

        private void DumpRequest(HttpRequest request)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug(
                    "********************* Begin Request *********************" + Environment.NewLine +
                    $"{request.HttpMethod} {request.RawUrl}" + Environment.NewLine +
                    Environment.NewLine
                );
            }
        }

        private void DumpResponse(HttpResponse response)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug(
                    "********************* End Request *********************" + Environment.NewLine +
                    $"{response.StatusCode}" + Environment.NewLine +
                    Environment.NewLine
                );
            }
        }
    }
}
