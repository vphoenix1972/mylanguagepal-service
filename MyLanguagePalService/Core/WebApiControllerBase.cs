using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace MyLanguagePalService.Core
{
    [AddResponseHeader(headerName: WebApiHeaderName, headerValue: "true")]
    public abstract class WebApiControllerBase : ApiController
    {
        public const string WebApiHeaderName = "X-Web-Api";

        protected internal virtual IHttpActionResult UnprocessableEntity(ModelStateDictionary modelState)
        {
            return ResponseMessage(Request.CreateErrorResponse((HttpStatusCode)422, modelState));
        }
    }
}