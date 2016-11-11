using System;
using System.Web.Http.Filters;
using JetBrains.Annotations;

namespace MyLanguagePalService.Core
{
    public class AddResponseHeaderAttribute : ActionFilterAttribute
    {
        private readonly string _headerName;
        private readonly string _headerValue;

        public AddResponseHeaderAttribute([NotNull] string headerName, [CanBeNull] string headerValue)
        {
            if (headerName == null)
                throw new ArgumentNullException(nameof(headerName));

            _headerName = headerName;
            _headerValue = headerValue;
        }

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            context.Response.Headers.Add(_headerName, _headerValue);
        }
    }
}