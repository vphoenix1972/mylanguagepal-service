using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using JetBrains.Annotations;
using MyLanguagePal.Shared.Exceptions;
using MyLanguagePal.Shared.Extensions.Dictionary;

namespace MyLanguagePalService.Core
{
    [AddResponseHeader(headerName: WebApiHeaderName, headerValue: "true")]
    //[ValidateAntiForgeryToken]
    public abstract class WebApiControllerBase : ApiController
    {
        public const string WebApiHeaderName = "X-Web-Api";

        protected IHttpActionResult NotFound(string message)
        {
            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
        }

        protected internal virtual IHttpActionResult UnprocessableEntity(ValidationFailedException vfe)
        {
            vfe.Errors.ForEach(e => ModelState.AddModelError(e.FieldName, e.Error));
            return UnprocessableEntity(ModelState);
        }

        protected internal virtual IHttpActionResult UnprocessableEntity([NotNull] ModelStateDictionary modelState)
        {
            return ResponseMessage(Request.CreateErrorResponse((HttpStatusCode)422, modelState));
        }

        protected internal virtual IHttpActionResult BadRequest([NotNull] MlpException exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            var data = new Dictionary<string, object>();
            data.Extend(exception, DictionaryExtendOptions.CreateIsSameOrSubClass(typeof(MlpException)));
            data[nameof(Exception.Message)] = exception.Message;

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, data));
        }

        protected internal virtual IHttpActionResult NotFound([NotNull] object data)
        {
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, data));
        }
    }
}