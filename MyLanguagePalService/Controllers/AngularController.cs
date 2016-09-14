using System.Web.Mvc;

namespace MyLanguagePalService.Controllers
{
    public class AngularController : SecuredSiteControllerBase
    {
        // GET: Angular
        public ActionResult Index()
        {
            return View();
        }
    }
}