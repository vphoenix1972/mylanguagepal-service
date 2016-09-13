using System;
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
            var ex = HttpContext.Server.GetLastError();
            logger.Fatal(ex, "Internal server error");
        }
    }
}