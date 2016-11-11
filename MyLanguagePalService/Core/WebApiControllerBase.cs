using System.Web.Http;

namespace MyLanguagePalService.Core
{
    [AddResponseHeader(headerName: WebApiHeaderName, headerValue: "true")]
    public abstract class WebApiControllerBase : ApiController
    {
        public const string WebApiHeaderName = "X-Web-Api";
    }
}