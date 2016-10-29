using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;
using MyLanguagePalService.Models.Controllers.Phrase;
using WebGrease.Css.Extensions;

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
            return View(_db.Phrases.Select(
                dal => new IndexPhraseVm()
                {
                    Id = dal.Id,
                    Text = dal.Text
                }
                ).ToList());
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

            var vm = new DetailsVm()
            {
                Id = phraseDal.Id,
                Text = phraseDal.Text,
                Translations = phraseDal.Translations.Select(ts => ts.Text)
            };

            return View(vm);
        }

        // GET: Phrases/Create
        public ActionResult Create()
        {
            var languagesOptions = GetLanguagesOptions();

            var defaultVm = new PhraseIm()
            {
                LanguageId = languagesOptions.First().Id
            };
            ViewBag.LanguagesOptions = languagesOptions;

            return View(defaultVm);
        }

        // POST: Phrases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhraseIm phraseIm)
        {
            // *** Request validation ***
            if (phraseIm.LanguageId == null)
            {
                // User cannot send request without language id using UI
                // Unsupported request
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var languageId = phraseIm.LanguageId.Value;
            var isLanguageExists = _db.Languages.Any(dal => dal.Id == languageId);
            if (!isLanguageExists)
            {
                // User cannot select non-existing request id in UI
                // Unsupported request
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Prepare language options for view
            ViewBag.LanguagesOptions = GetLanguagesOptions();

            // User input validation
            if (!ModelState.IsValid)
            {
                return View(phraseIm);
            }

            // Phrase text validation
            if (string.IsNullOrWhiteSpace(phraseIm.Text))
            {
                ModelState.AddModelError(nameof(PhraseIm.Text), "Phrase cannot be empty");
                return View(phraseIm);
            }

            phraseIm.Text = phraseIm.Text.Trim();

            var maxPhraseLength = 100;
            if (phraseIm.Text.Length > maxPhraseLength)
            {
                ModelState.AddModelError(nameof(PhraseIm.Text), "Phrase is too long");
                return View(phraseIm);
            }

            // Validate translations
            IList<string> translationsInputs = null;
            if (phraseIm.Translations != null)
            {
                translationsInputs = ParseTranslations(phraseIm.Translations);
                if (translationsInputs.Any(ts => ts.Length > maxPhraseLength))
                {
                    ModelState.AddModelError(nameof(PhraseIm.Translations), "One of translations is too long");
                    return View(phraseIm);
                }
            }

            // Check that the phrase does not exist
            if (_db.Phrases.Any(dal => dal.Text == phraseIm.Text))
            {
                ModelState.AddModelError(nameof(PhraseIm.Text), "The phrase already exists");
                return View(phraseIm);
            }


            // *** Phrase creation ***
            var newPhraseDal = new PhraseDal()
            {
                Text = phraseIm.Text,
                LanguageId = phraseIm.LanguageId.Value
            };

            // Add & find translations
            if (translationsInputs != null && translationsInputs.Any())
            {
                var translationsDals = new List<PhraseDal>();
                foreach (var translationInput in translationsInputs)
                {
                    var existingTranslationDal = _db.Phrases.FirstOrDefault(dal => dal.Text == translationInput);
                    if (existingTranslationDal != null)
                    {
                        // Use existing translation
                        translationsDals.Add(existingTranslationDal);

                        // Also add new phrase as translation for existing phrase
                        existingTranslationDal.Translations.Add(newPhraseDal);
                    }
                    else
                    {
                        // Create new translation
                        var newTranslationDal = new PhraseDal()
                        {
                            Text = translationInput,
                            // ToDo: Since now only two languages, determine translation language this way
                            LanguageId =
                                _db.Languages.Single(languageDal => languageDal.Id != phraseIm.LanguageId.Value).Id,
                            Translations = new List<PhraseDal>() { newPhraseDal } // Use the new phrase as a translation
                        };

                        _db.Phrases.Add(newTranslationDal);
                        translationsDals.Add(newTranslationDal);
                    }
                }
                newPhraseDal.Translations = translationsDals;
            }

            // Create new phrase in the database
            _db.Phrases.Add(newPhraseDal);

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
            return View(ToIm(phraseDal));
        }

        // POST: Phrases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhraseIm phraseIm)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(ToDal(phraseIm)).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phraseIm);
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
            return View(ToIm(phraseDal));
        }

        // POST: Phrases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var phraseDal = _db.Phrases.Find(id);
            if (phraseDal == null)
            {
                return HttpNotFound();
            }

            // Remove this phrase from phrases for which it is the translation
            phraseDal.Translations.ForEach(ts => ts.Translations.Remove(phraseDal));

            // Remove the phrase
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

        private IList<string> ParseTranslations(string input)
        {
            return input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToList();
        }

        private PhraseIm ToIm(PhraseDal dal)
        {
            return new PhraseIm()
            {
                Id = dal.Id,
                Text = dal.Text
            };
        }

        private PhraseDal ToDal(PhraseIm im)
        {
            return new PhraseDal()
            {
                Id = im.Id,
                Text = im.Text
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
