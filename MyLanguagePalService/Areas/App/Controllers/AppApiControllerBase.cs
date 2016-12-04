using System.Web.Http;
using MyLanguagePalService.Core;

namespace MyLanguagePalService.Areas.App.Controllers
{
    public class AppApiControllerBase : WebApiControllerBase
    {
        protected IHttpActionResult PhraseNotFound(int id)
        {
            return NotFound($"Phrase '{id}' does not exist");
        }
    }
}