using System.Web.Mvc;

namespace MyLanguagePalService.Controllers
{
    public class AboutController : SiteControllerBase
    {
        // GET: About
        public ActionResult Index()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}