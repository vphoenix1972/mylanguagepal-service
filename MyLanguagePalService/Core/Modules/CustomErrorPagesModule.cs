using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MyLanguagePalService.Controllers;

namespace MyLanguagePalService.Core.Modules
{
    /// <summary>
    /// Enables support for CustomErrors ResponseRewrite mode in MVC.
    /// </summary>
    public class CustomErrorPagesModule : IHttpModule
    {
        private HttpContext HttpContext => HttpContext.Current;

        public void Init(HttpApplication application)
        {
            application.EndRequest += Application_EndRequest;
        }

        public void Dispose()
        {

        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Response;

            // Check status code
            var statusCode = response.StatusCode;
            if (statusCode < 400)
            {
                // Request finished successfully
                return;
            }

            // Show custom error
            switch (statusCode)
            {
                // 401
                case 401:
                    ShowErrorPage("Http401", response);
                    break;
                // 403
                case 403:
                    ShowErrorPage("Http403", response);
                    break;
                // 404
                case 404:
                    ShowErrorPage("Http404", response);
                    break;
                default:
                    var exception = GetServerError();
                    ShowErrorPage("Http500", response, exception);
                    break;
            }
        }

        private Exception GetServerError()
        {
            return HttpContext.Server.GetLastError();
        }

        private void ShowErrorPage(string action, HttpResponse response, Exception exception = null)
        {
            response.Clear();

            // Create route to error controller
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", action);

            // Pass exception details to the target error View.
            if (exception != null)
            {
                routeData.Values.Add("error", exception);

                // Clear the error on server.
                HttpContext.Server.ClearError();
            }

            // Avoid IIS7 getting in the middle
            HttpContext.Response.TrySkipIisCustomErrors = true;
            HttpContext.Response.Headers["Content-type"] = "text/html";

            // Call target Controller and pass the routeData.
            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(
                 new HttpContextWrapper(HttpContext), routeData));
        }
    }
}