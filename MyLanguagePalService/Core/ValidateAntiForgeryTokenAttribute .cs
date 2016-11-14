using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MyLanguagePalService.Core
{
    public sealed class ValidateAntiForgeryTokenAttribute : ActionFilterAttribute
    {
        private const string HeaderTokenName = "X-XSRF-Token";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            var headers = actionContext.Request.Headers;

            try
            {
                IEnumerable<string> tokens;
                if (headers.TryGetValues(HeaderTokenName, out tokens))
                {
                    var headerToken = tokens.FirstOrDefault();
                    var cookie = headers.GetCookies().Select(c => c[AntiForgeryConfig.CookieName]).FirstOrDefault();
                    var cookieToken = cookie?.Value;
                    AntiForgery.Validate(cookieToken, headerToken);
                }
                else
                {
                    throw new InvalidOperationException("Anti forgery token was not found");
                }
            }
            catch
            {
                actionContext.Response = new HttpResponseMessage
                {
                    RequestMessage = actionContext.ControllerContext.Request,
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = "Invalid Token"
                };
            }

            base.OnActionExecuting(actionContext);
        }
    }
}