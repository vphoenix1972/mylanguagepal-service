using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MyLanguagePalService.Areas.Site.Models.Controllers.Phrase;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;
using WebGrease.Css.Extensions;

namespace MyLanguagePalService.Areas.Site.Controllers
{
    public class PhrasesController : Controller
    {
        private const int MaxPhraseLength = 100;

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
        //public ActionResult Index()
        //{
        //    return View(_db.Phrases.Select(dal => new IndexPhraseVm()
        //    {
        //        Id = dal.Id,
        //        Text = dal.Text
        //    }));
        //}

        //// GET: Phrases/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var phraseDal = _db.Phrases.Find(id);
        //    if (phraseDal == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(new DetailsVm()
        //    {
        //        Id = phraseDal.Id,
        //        Text = phraseDal.Text,
        //        Translations = phraseDal.Translations.Select(dal => dal.Text).ToList()
        //    });
        //}

        //// GET: Phrases/Create
        //public ActionResult Create()
        //{
        //    var languagesOptions = GetLanguagesOptions();

        //    var defaultVm = new CreateVm()
        //    {
        //        LanguageId = languagesOptions.First().Id,
        //        LanguageOptions = languagesOptions
        //    };

        //    return View(defaultVm);
        //}

        //// POST: Phrases/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(CreateIm inputModel)
        //{
        //    // *** Request validation ***
        //    if (inputModel.LanguageId == null)
        //    {
        //        // User cannot send request without language id using UI
        //        // Unsupported request
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var languageId = inputModel.LanguageId.Value;
        //    var isLanguageExists = _db.Languages.Any(dal => dal.Id == languageId);
        //    if (!isLanguageExists)
        //    {
        //        // User cannot select non-existing request id in UI
        //        // Unsupported request
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    // Phrase text validation
        //    if (string.IsNullOrWhiteSpace(inputModel.Text))
        //    {
        //        ModelState.AddModelError(nameof(CreateVm.Text), "Phrase cannot be empty");
        //        return View(ToCreateVm(inputModel));
        //    }

        //    inputModel.Text = inputModel.Text.Trim();

        //    if (inputModel.Text.Length > MaxPhraseLength)
        //    {
        //        ModelState.AddModelError(nameof(CreateVm.Text), "Phrase is too long");
        //        return View(ToCreateVm(inputModel));
        //    }

        //    // Validate translations
        //    IList<string> translationsInputs = null;
        //    if (inputModel.Translations != null)
        //    {
        //        translationsInputs = ParseTranslations(inputModel.Translations);
        //        if (translationsInputs.Any(ts => ts.Length > MaxPhraseLength))
        //        {
        //            ModelState.AddModelError(nameof(CreateVm.Translations), "One of translations is too long");
        //            return View(ToCreateVm(inputModel));
        //        }
        //    }

        //    // Check that the phrase does not exist
        //    if (_db.Phrases.Any(dal => dal.Text == inputModel.Text))
        //    {
        //        ModelState.AddModelError(nameof(CreateVm.Text), "The phrase already exists");
        //        return View(ToCreateVm(inputModel));
        //    }

        //    // *** Phrase creation ***
        //    var newPhraseDal = new PhraseDal()
        //    {
        //        Text = inputModel.Text,
        //        LanguageId = inputModel.LanguageId.Value
        //    };

        //    // Add & find translations
        //    if (translationsInputs != null && translationsInputs.Any())
        //    {
        //        var translationsDals = new List<PhraseDal>();
        //        foreach (var translationInput in translationsInputs)
        //        {
        //            var existingTranslationDal = _db.Phrases.FirstOrDefault(dal => dal.Text == translationInput);
        //            if (existingTranslationDal != null)
        //            {
        //                // Use existing translation
        //                translationsDals.Add(existingTranslationDal);

        //                // Also add new phrase as translation for existing phrase
        //                existingTranslationDal.Translations.Add(newPhraseDal);
        //            }
        //            else
        //            {
        //                // Create new translation
        //                var newTranslationDal = new PhraseDal()
        //                {
        //                    Text = translationInput,
        //                    // ToDo: Since now only two languages, determine translation language this way
        //                    LanguageId =
        //                        _db.Languages.Single(languageDal => languageDal.Id != inputModel.LanguageId.Value).Id,
        //                    Translations = new List<PhraseDal>() { newPhraseDal } // Use the new phrase as a translation
        //                };

        //                _db.Phrases.Add(newTranslationDal);
        //                translationsDals.Add(newTranslationDal);
        //            }
        //        }
        //        newPhraseDal.Translations = translationsDals;
        //    }

        //    // Create new phrase in the database
        //    _db.Phrases.Add(newPhraseDal);

        //    _db.SaveChanges();

        //    return RedirectToAction("Index");
        //}

        //// GET: Phrases/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var phraseDal = _db.Phrases.Find(id);
        //    if (phraseDal == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(ToEditVm(phraseDal));
        //}

        //// POST: Phrases/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(EditIm inputModel)
        //{
        //    // *** Request validation ***
        //    if (inputModel.Id == null)
        //    {
        //        // Id was not specified or was incorrect
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    // Find phrase in database
        //    var phraseDal = _db.Phrases.Find(inputModel.Id);
        //    if (phraseDal == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    // Phrase text validation
        //    if (string.IsNullOrWhiteSpace(inputModel.Text))
        //    {
        //        ModelState.AddModelError(nameof(EditVm.Text), "Phrase cannot be empty");
        //        return View(ToEditVm(inputModel));
        //    }

        //    inputModel.Text = inputModel.Text.Trim();

        //    if (inputModel.Text.Length > MaxPhraseLength)
        //    {
        //        ModelState.AddModelError(nameof(EditVm.Text), "Phrase is too long");
        //        return View(ToEditVm(inputModel));
        //    }

        //    // Validate translations
        //    IList<string> translationsInputs = null;
        //    if (inputModel.Translations != null)
        //    {
        //        translationsInputs = ParseTranslations(inputModel.Translations);
        //        if (translationsInputs.Any(ts => ts.Length > MaxPhraseLength))
        //        {
        //            ModelState.AddModelError(nameof(EditVm.Translations), "One of translations is too long");
        //            return View(ToEditVm(inputModel));
        //        }
        //    }

        //    // Check that the phrase does not exist
        //    if (phraseDal.Text != inputModel.Text)
        //    {
        //        if (_db.Phrases.Any(dal => dal.Text == inputModel.Text))
        //        {
        //            ModelState.AddModelError(nameof(EditVm.Text), "The phrase already exists");
        //            return View(ToEditVm(inputModel));
        //        }
        //    }

        //    // *** Phrase modification ***
        //    phraseDal.Text = inputModel.Text;

        //    // Modify translations
        //    var newTranslations = new List<PhraseDal>();
        //    var oldTranslations = phraseDal.Translations.ToList();
        //    if (translationsInputs != null && translationsInputs.Any())
        //    {
        //        // Merge translations
        //        // Check if user removed a translation
        //        foreach (var existingTranslation in oldTranslations)
        //        {
        //            var userTranslation = translationsInputs.FirstOrDefault(s => s == existingTranslation.Text);
        //            if (userTranslation == null)
        //            {
        //                // User has removed the translation

        //                // Remove this phrase from phrases for which it is the translation
        //                existingTranslation.Translations.Remove(phraseDal);

        //                continue;
        //            }

        //            // Keep existing translation
        //            newTranslations.Add(existingTranslation);
        //        }

        //        // Check for new translations
        //        foreach (var translationInput in translationsInputs)
        //        {
        //            var existingTranslation = oldTranslations.FirstOrDefault(dal => dal.Text == translationInput);
        //            if (existingTranslation != null)
        //            {
        //                // Already exists
        //                continue;
        //            }

        //            // New translation
        //            var existingTranslationDal = _db.Phrases.FirstOrDefault(dal => dal.Text == translationInput);
        //            if (existingTranslationDal != null)
        //            {
        //                // Use existing translation
        //                newTranslations.Add(existingTranslationDal);

        //                // Also add new phrase as translation for existing phrase
        //                existingTranslationDal.Translations.Add(phraseDal);
        //            }
        //            else
        //            {
        //                // Create new translation
        //                var newTranslationDal = new PhraseDal()
        //                {
        //                    Text = translationInput,
        //                    // ToDo: Since now only two languages, determine translation language this way
        //                    LanguageId =
        //                        _db.Languages.Single(languageDal => languageDal.Id != phraseDal.LanguageId).Id,
        //                    Translations = new List<PhraseDal>() { phraseDal } // Use the new phrase as a translation
        //                };

        //                _db.Phrases.Add(newTranslationDal);
        //                newTranslations.Add(newTranslationDal);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // User removed all translations

        //        // Remove this phrase from phrases for which it is the translation
        //        phraseDal.Translations.ForEach(ts => ts.Translations.Remove(phraseDal));

        //        // Remove translations for the phrase
        //        newTranslations.Clear();
        //    }
        //    phraseDal.Translations = newTranslations;

        //    // Modify the phrase in the database
        //    _db.Entry(phraseDal).State = EntityState.Modified;
        //    _db.SaveChanges();

        //    return RedirectToAction("Index");
        //}

        //// GET: Phrases/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var phraseDal = _db.Phrases.Find(id);
        //    if (phraseDal == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ToDeleteVm(phraseDal));
        //}

        //// POST: Phrases/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    var phraseDal = _db.Phrases.Find(id);
        //    if (phraseDal == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    // Remove this phrase from phrases for which it is the translation
        //    phraseDal.Translations.ForEach(ts => ts.Translations.Remove(phraseDal));

        //    // Remove the phrase
        //    _db.Phrases.Remove(phraseDal);

        //    _db.SaveChanges();

        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private IList<LanguageOptionVm> GetLanguagesOptions()
        //{
        //    return _db.Languages.Select(ToLanguageOption).ToList();
        //}

        //private IList<string> ParseTranslations(string input)
        //{
        //    return input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //                .Select(s => s.Trim())
        //                .Where(s => !string.IsNullOrWhiteSpace(s))
        //                .ToList();
        //}

        //private EditVm ToEditVm(PhraseDal phraseDal)
        //{
        //    return new EditVm()
        //    {
        //        Id = phraseDal.Id,
        //        Text = phraseDal.Text,
        //        Translations = string.Join(", ", phraseDal.Translations.Select(dal => dal.Text))
        //    };
        //}

        //private CreateVm ToCreateVm(CreateIm inputModel)
        //{
        //    return new CreateVm()
        //    {
        //        // Checked in Create()
        //        // ReSharper disable once PossibleInvalidOperationException
        //        LanguageId = inputModel.LanguageId.Value,
        //        Text = inputModel.Text,
        //        Translations = inputModel.Translations,
        //        LanguageOptions = GetLanguagesOptions()
        //    };
        //}

        //private EditVm ToEditVm(EditIm inputModel)
        //{
        //    return new EditVm()
        //    {
        //        // Checked in Edit()
        //        // ReSharper disable once PossibleInvalidOperationException
        //        Id = inputModel.Id.Value,
        //        Text = inputModel.Text,
        //        Translations = inputModel.Translations
        //    };
        //}

        //private DeleteVm ToDeleteVm(PhraseDal phraseDal)
        //{
        //    return new DeleteVm()
        //    {
        //        Text = phraseDal.Text
        //    };
        //}

        //private LanguageOptionVm ToLanguageOption(LanguageDal dal)
        //{
        //    return new LanguageOptionVm()
        //    {
        //        Id = dal.Id,
        //        Name = dal.Name
        //    };
        //}
    }
}
