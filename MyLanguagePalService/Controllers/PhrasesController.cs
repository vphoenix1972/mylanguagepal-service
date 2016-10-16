using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;
using MyLanguagePalService.Models.Controllers.Phrase;

namespace MyLanguagePalService.Controllers
{
    public class PhrasesController : Controller
    {
        private readonly IApplicationDbContext _db;

        public PhrasesController()
        {
            _db = new ApplicationDbContext();
        }

        public PhrasesController(IApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Phrases
        public ActionResult Index()
        {
            return View(_db.Phrases.Select(ToVm).ToList());
        }

        // GET: Phrases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var phraseDal = _db.Phrases.Find(id);
            if (phraseDal == null)
            {
                return HttpNotFound();
            }
            return View(ToVm(phraseDal));
        }

        // GET: Phrases/Create
        public ActionResult Create()
        {
            var languagesOptions = GetLanguagesOptions();

            var defaultVm = new PhraseVm()
            {
                LanguageId = languagesOptions.First().Id
            };
            ViewBag.LanguagesOptions = languagesOptions;

            return View(defaultVm);
        }

        // POST: Phrases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhraseVm phraseVm)
        {
            // *** Request validation ***
            if (phraseVm.LanguageId == null)
            {
                // User cannot send request without language id using UI
                // Unsupported request
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var languageId = phraseVm.LanguageId.Value;
            var isLanguageExists = _db.Languages.Any(dal => dal.Id == languageId);
            if (!isLanguageExists)
            {
                // User cannot select non-existing request id in UI
                // Unsupported request
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // User input validation
            if (!ModelState.IsValid)
            {
                ViewBag.LanguagesOptions = GetLanguagesOptions();
                return View(phraseVm);
            }

            // *** Phrase creation ***
            //var translations = _db.Phrases.Where(p => p.Text == phraseVm.Translations).ToList();

            // Create new phrase in the database
            _db.Phrases.Add(new PhraseDal()
            {
                Id = phraseVm.Id,
                Text = phraseVm.Text,
                LanguageId = phraseVm.LanguageId.Value
            });
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Phrases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var phraseDal = _db.Phrases.Find(id);
            if (phraseDal == null)
            {
                return HttpNotFound();
            }
            return View(ToVm(phraseDal));
        }

        // POST: Phrases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhraseVm phraseVm)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(ToDal(phraseVm)).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phraseVm);
        }

        // GET: Phrases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var phraseDal = _db.Phrases.Find(id);
            if (phraseDal == null)
            {
                return HttpNotFound();
            }
            return View(ToVm(phraseDal));
        }

        // POST: Phrases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var phraseDal = _db.Phrases.Find(id);
            _db.Phrases.Remove(phraseDal);
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

        private IEnumerable<LanguageOptionVm> GetLanguagesOptions()
        {
            return _db.Languages.Select(ToLanguageOption);
        }

        private PhraseVm ToVm(PhraseDal dal)
        {
            return new PhraseVm()
            {
                Id = dal.Id,
                Text = dal.Text
            };
        }

        private PhraseDal ToDal(PhraseVm vm)
        {
            return new PhraseDal()
            {
                Id = vm.Id,
                Text = vm.Text
            };
        }

        private LanguageOptionVm ToLanguageOption(LanguageDal dal)
        {
            return new LanguageOptionVm()
            {
                Id = dal.Id,
                Name = dal.Name
            };
        }
    }
}
