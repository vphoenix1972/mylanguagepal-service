using System;
using System.Net;
using System.Web;
using NLog;

namespace MyLanguagePalService.Core.Modules
{
    public class LoggingModule : IHttpModule
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private HttpContext HttpContext => HttpContext.Current;

        public void Init(HttpApplication application)
        {
            application.Error += Application_Error;
        }

        public void Dispose()
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = HttpContext.Server.GetLastError();

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
    }
}