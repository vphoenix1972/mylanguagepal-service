using System.Web.Mvc;

namespace MyLanguagePalService.Areas.Site.Controllers
{
    [Authorize]
    public abstract class SecuredSiteControllerBase : Controller
    {
         
    }
}