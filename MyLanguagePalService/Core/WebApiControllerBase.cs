using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using JetBrains.Annotations;

namespace MyLanguagePalService.Core
{
    [AddResponseHeader(headerName: WebApiHeaderName, headerValue: "true")]
    [ValidateAntiForgeryToken]
    public abstract class WebApiControllerBase : ApiController
    {
        public const string WebApiHeaderName = "X-Web-Api";

        protected internal virtual IHttpActionResult UnprocessableEntity(ValidationFailedException vfe)
        {
            vfe.Errors.ForEach(e => ModelState.AddModelError(e.FieldName, e.Error));
            return UnprocessableEntity(ModelState);
        }

        protected internal virtual IHttpActionResult UnprocessableEntity([NotNull] ModelStateDictionary modelState)
        {
            return ResponseMessage(Request.CreateErrorResponse((HttpStatusCode)422, modelState));
        }
    }
}