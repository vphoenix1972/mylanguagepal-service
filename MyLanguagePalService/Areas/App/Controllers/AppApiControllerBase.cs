using System.Web.Http;
using JetBrains.Annotations;
using MyLanguagePalService.BLL;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;

namespace MyLanguagePalService.Areas.App.Controllers
{
    public abstract class AppApiControllerBase : WebApiControllerBase
    {
        protected AppApiControllerBase()
        {
            var db = new ApplicationDbContext();
            UnitOfWork = new UnitOfWork(db);
        }

        [NotNull]
        protected IUnitOfWork UnitOfWork { get; }

        protected override void Dispose(bool disposing)
        {
            UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        protected IHttpActionResult PhraseNotFound(int id)
        {
            return NotFound($"Phrase '{id}' does not exist");
        }
    }
}