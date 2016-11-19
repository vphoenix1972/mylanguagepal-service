using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using MyLanguagePalService.Areas.App.Models.Controller.PhrasesApi;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.Areas.App.Controllers
{
    [RoutePrefix("api/phrases")]
    public class PhrasesApiController : WebApiControllerBase
    {
        private const int MaxPhraseLength = 100;

        private readonly IApplicationDbContext _db;

        public PhrasesApiController()
        {
            _db = new ApplicationDbContext();
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<PhrasesApiGetAllAm> GetAllPhrases()
        {
            return _db.Phrases.Select(dal => new PhrasesApiGetAllAm()
            {
                Id = dal.Id,
                Text = dal.Text
            });
        }


        [Route("{id:int}")]
        [HttpGet]
        public IHttpActionResult GetPhrase(int id)
        {
            var phraseDal = _db.Phrases.Find(id);
            if (phraseDal == null)
            {
                return NotFound();
            }

            return Ok(new PhrasesApiDetailsAm()
            {
                Id = phraseDal.Id,
                Text = phraseDal.Text,
                Translations = phraseDal.Translations.Select(dal => dal.Text).ToList()
            });
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult CreatePhrase(PhrasesApiCreateIm inputModel)
        {
            // *** Request validation ***
            if (inputModel == null)
            {
                ModelState.AddModelError("Phrase", "Phrase cannot be null");
                return BadRequest(ModelState);
            }

            if (inputModel.LanguageId == null)
            {
                // User cannot send request without language id using UI
                // Unsupported request
                ModelState.AddModelError(nameof(PhrasesApiCreateIm.LanguageId), "Language id cannot be null");
                return BadRequest(ModelState);
            }

            var languageId = inputModel.LanguageId.Value;
            var isLanguageExists = _db.Languages.Any(dal => dal.Id == languageId);
            if (!isLanguageExists)
            {
                // User cannot select non-existing request id in UI
                // Unsupported request
                ModelState.AddModelError(nameof(PhrasesApiCreateIm.LanguageId), $"Language with id {languageId} does not exist");
                return BadRequest(ModelState);
            }

            // Phrase text validation
            if (string.IsNullOrWhiteSpace(inputModel.Text))
            {
                ModelState.AddModelError(nameof(PhrasesApiCreateIm.Text), "Phrase cannot be empty");
                return UnprocessableEntity(ModelState);
            }

            inputModel.Text = inputModel.Text.Trim();

            if (inputModel.Text.Length > MaxPhraseLength)
            {
                ModelState.AddModelError(nameof(PhrasesApiCreateIm.Text), "Phrase is too long");
                return UnprocessableEntity(ModelState);
            }

            // Validate translations
            if (inputModel.Translations != null)
            {
                // Trim translations
                inputModel.Translations = inputModel.Translations.Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();

                if (inputModel.Translations.Any(ts => ts.Length > MaxPhraseLength))
                {
                    ModelState.AddModelError(nameof(PhrasesApiCreateIm.Translations), "One of translations is too long");
                    return UnprocessableEntity(ModelState);
                }
            }

            // Check that the phrase does not exist
            if (_db.Phrases.Any(dal => dal.Text == inputModel.Text))
            {
                ModelState.AddModelError(nameof(PhrasesApiCreateIm.Text), "The phrase already exists");
                return UnprocessableEntity(ModelState);
            }

            // *** Phrase creation ***
            var newPhraseDal = new PhraseDal()
            {
                Text = inputModel.Text,
                LanguageId = inputModel.LanguageId.Value
            };

            // Add & find translations
            if (inputModel.Translations != null && inputModel.Translations.Any())
            {
                var translationsDals = new List<PhraseDal>();
                foreach (var translationInput in inputModel.Translations)
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
                                _db.Languages.Single(languageDal => languageDal.Id != inputModel.LanguageId.Value).Id,
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

            return Ok();
        }

        [Route("{id:int}")]
        [HttpPut]
        public IHttpActionResult UpdatePhrase(int id, PhrasesApiCreateIm inputModel)
        {
            // *** Request validation ***

            // Find phrase in database
            var phraseDal = _db.Phrases.Find(id);
            if (phraseDal == null)
            {
                return NotFound();
            }

            // Phrase text validation
            if (string.IsNullOrWhiteSpace(inputModel.Text))
            {
                ModelState.AddModelError(nameof(PhrasesApiCreateIm.Text), "Phrase cannot be empty");
                return UnprocessableEntity(ModelState);
            }

            inputModel.Text = inputModel.Text.Trim();

            if (inputModel.Text.Length > MaxPhraseLength)
            {
                ModelState.AddModelError(nameof(PhrasesApiCreateIm.Text), "Phrase is too long");
                return UnprocessableEntity(ModelState);
            }

            // Validate translations
            if (inputModel.Translations != null)
            {
                // Trim translations
                inputModel.Translations = inputModel.Translations.Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();

                if (inputModel.Translations.Any(ts => ts.Length > MaxPhraseLength))
                {
                    ModelState.AddModelError(nameof(PhrasesApiCreateIm.Translations), "One of translations is too long");
                    return UnprocessableEntity(ModelState);
                }
            }

            // Check that the phrase does not exist
            if (phraseDal.Text != inputModel.Text)
            {
                if (_db.Phrases.Any(dal => dal.Text == inputModel.Text))
                {
                    ModelState.AddModelError(nameof(PhrasesApiCreateIm.Text), "The phrase already exists");
                    return UnprocessableEntity(ModelState);
                }
            }

            // *** Phrase modification ***
            phraseDal.Text = inputModel.Text;

            // Modify translations
            var newTranslations = new List<PhraseDal>();
            var oldTranslations = phraseDal.Translations.ToList();
            if (inputModel.Translations != null && inputModel.Translations.Any())
            {
                // Merge translations
                // Check if user removed a translation
                foreach (var existingTranslation in oldTranslations)
                {
                    var userTranslation = inputModel.Translations.FirstOrDefault(s => s == existingTranslation.Text);
                    if (userTranslation == null)
                    {
                        // User has removed the translation

                        // Remove this phrase from phrases for which it is the translation
                        existingTranslation.Translations.Remove(phraseDal);

                        continue;
                    }

                    // Keep existing translation
                    newTranslations.Add(existingTranslation);
                }

                // Check for new translations
                foreach (var translationInput in inputModel.Translations)
                {
                    var existingTranslation = oldTranslations.FirstOrDefault(dal => dal.Text == translationInput);
                    if (existingTranslation != null)
                    {
                        // Already exists
                        continue;
                    }

                    // New translation
                    var existingTranslationDal = _db.Phrases.FirstOrDefault(dal => dal.Text == translationInput);
                    if (existingTranslationDal != null)
                    {
                        // Use existing translation
                        newTranslations.Add(existingTranslationDal);

                        // Also add new phrase as translation for existing phrase
                        existingTranslationDal.Translations.Add(phraseDal);
                    }
                    else
                    {
                        // Create new translation
                        var newTranslationDal = new PhraseDal()
                        {
                            Text = translationInput,
                            // ToDo: Since now only two languages, determine translation language this way
                            LanguageId =
                                _db.Languages.Single(languageDal => languageDal.Id != phraseDal.LanguageId).Id,
                            Translations = new List<PhraseDal>() { phraseDal } // Use the new phrase as a translation
                        };

                        _db.Phrases.Add(newTranslationDal);
                        newTranslations.Add(newTranslationDal);
                    }
                }
            }
            else
            {
                // User removed all translations

                // Remove this phrase from phrases for which it is the translation
                phraseDal.Translations.ForEach(ts => ts.Translations.Remove(phraseDal));

                // Remove translations for the phrase
                newTranslations.Clear();
            }
            phraseDal.Translations = newTranslations;

            // Modify the phrase in the database
            _db.Entry(phraseDal).State = EntityState.Modified;
            _db.SaveChanges();

            return Ok();
        }

        [Route("{id:int}")]
        [HttpDelete]
        public IHttpActionResult DeletePhrase(int id)
        {
            /* Validation */
            var dal = _db.Phrases.Find(id);
            if (dal == null)
            {
                return NotFound();
            }

            /* Delete the phrase */

            // Remove this phrase from phrases for which it is the translation
            dal.Translations.ForEach(ts => ts.Translations.Remove(dal));

            // Remove the phrase
            _db.Phrases.Remove(dal);

            _db.SaveChanges();

            return Ok();
        }
    }
}
