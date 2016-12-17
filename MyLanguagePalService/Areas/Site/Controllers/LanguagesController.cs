using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MyLanguagePalService.Areas.Site.Models.Controllers.Language;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.Areas.Site.Controllers
{
    public class LanguagesController : Controller
    {
        private readonly IApplicationDbContext _db;

        public LanguagesController()
        {
            _db = new ApplicationDbContext();
        }

        public LanguagesController(IApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Languages
        public ActionResult Index()
        {
            return View(_db.Languages.Select(ToVm).ToList());
        }

        // GET: Languages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var languageDal = _db.Languages.Find(id);
            if (languageDal == null)
            {
                return HttpNotFound();
            }
            return View(ToVm(languageDal));
        }

        // GET: Languages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Languages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] LanguageVm languageVm)
        {
            if (ModelState.IsValid)
            {
                _db.Languages.Add(ToDal(languageVm));
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(languageVm);
        }

        // GET: Languages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var languageDal = _db.Languages.Find(id);
            if (languageDal == null)
            {
                return HttpNotFound();
            }
            return View(ToVm(languageDal));
        }

        // POST: Languages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] LanguageVm languageVm)
        {
            if (ModelState.IsValid)
            {
                _db.MarkModified(ToDal(languageVm));
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(languageVm);
        }

        // GET: Languages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var languageDal = _db.Languages.Find(id);
            if (languageDal == null)
            {
                return HttpNotFound();
            }
            return View(ToVm(languageDal));
        }

        // POST: Languages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var languageDal = _db.Languages.Find(id);
            _db.Languages.Remove(languageDal);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private LanguageVm ToVm(LanguageDal languageDal)
        {
            return new LanguageVm()
            {
                Id = languageDal.Id,
                Name = languageDal.Name
            };
        }

        private LanguageDal ToDal(LanguageVm languageVm)
        {
            return new LanguageDal()
            {
                Id = languageVm.Id,
                Name = languageVm.Name
            };
        }
    }
}
