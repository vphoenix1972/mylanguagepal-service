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
            return View();
        }

        // POST: Phrases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Text")] PhraseVm phraseVm)
        {
            if (ModelState.IsValid)
            {
                _db.Phrases.Add(ToDal(phraseVm));
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phraseVm);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Text")] PhraseVm phraseVm)
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
    }
}
