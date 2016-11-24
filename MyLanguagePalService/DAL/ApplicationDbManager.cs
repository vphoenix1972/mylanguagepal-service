using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using MyLanguagePalService.Areas.App.Models.Controller.PhrasesApi;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL.Dto;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.DAL
{
    public class ApplicationDbManager : IApplicationDbManager
    {
        private const int MaxPhraseLength = 100;

        private readonly IApplicationDbContext _db;

        public ApplicationDbManager(IApplicationDbContext db)
        {
            _db = db;
        }

        public IList<PhraseDal> GetPhrases()
        {
            return _db.Phrases.ToList();
        }

        public PhraseDal GetPhrase(int id)
        {
            return _db.Phrases.Find(id);
        }

        public IList<TranslationDto> GetTranslations(PhraseDal phraseDal)
        {
            if (phraseDal == null)
                throw new ArgumentNullException(nameof(phraseDal));

            var result = GetTranslationsFor(phraseDal);

            return result.Select(t => new TranslationDto()
            {
                Phrase = t.TranslationPhrase,
                Prevalence = t.Prevalence
            }).ToList();
        }

        public void CreatePhrase(string text, int languageId, IList<string> translations = null)
        {
            /* Validation */
            AssertPhraseText(text);
            AssertLanguageId(languageId);
            AssertTranslations(translations);
            AssertPhraseNotExist(text);

            /* Phrase creation */
            var newPhraseDal = new PhraseDal()
            {
                Text = text,
                LanguageId = languageId
            };

            if (translations != null && translations.Any())
            {
                var translationsDals = new List<TranslationDal>();
                foreach (var translationInput in translations)
                {
                    var existingPhraseDal = _db.Phrases.FirstOrDefault(dal => dal.Text == translationInput);
                    if (existingPhraseDal != null)
                    {
                        // Use existing translation
                        translationsDals.Add(new TranslationDal()
                        {
                            ForPhrase = newPhraseDal,
                            TranslationPhrase = existingPhraseDal
                        });
                    }
                    else
                    {
                        // Create new translation
                        var newPhraseAsTranslation = new PhraseDal()
                        {
                            Text = translationInput,
                            // ToDo: Since now only two languages, determine translation language this way
                            LanguageId =
                                _db.Languages.Single(languageDal => languageDal.Id != languageId).Id
                        };

                        _db.Phrases.Add(newPhraseAsTranslation);
                        translationsDals.Add(new TranslationDal()
                        {
                            ForPhrase = newPhraseDal,
                            TranslationPhrase = newPhraseAsTranslation
                        });
                    }
                }
                newPhraseDal.Translations = translationsDals;
            }

            // Create new phrase in the database
            _db.Phrases.Add(newPhraseDal);

            _db.SaveChanges();
        }

        public void UpdatePhrase(PhraseDal phrase, string text, IList<string> translations)
        {
            /* Validation */
            if (phrase == null)
                throw new ArgumentNullException(nameof(phrase));

            AssertPhraseText(text);
            AssertTranslations(translations);

            // Check that the phrase does not exist
            if (phrase.Text != text)
            {
                AssertPhraseNotExist(text);
            }

            // *** Phrase modification ***
            phrase.Text = text;

            //  Modify translations
            var oldTranslations = GetTranslationsFor(phrase);
            if (translations != null && translations.Any())
            {
                // Merge translations
                // Check if user removed a translation
                foreach (var existingTranslation in oldTranslations)
                {
                    var userTranslation = translations.FirstOrDefault(s => s == existingTranslation.TranslationPhrase.Text);
                    if (userTranslation == null)
                    {
                        // User has removed the translation

                        // Remove this phrase from phrases for which it is the translation
                        var translationsToDelete = _db.Translations.Where(t =>
                            (t.ForPhraseId == phrase.Id && t.TranslationPhrase.Id == existingTranslation.TranslationPhraseId) ||
                            (t.ForPhraseId == existingTranslation.TranslationPhrase.Id && t.TranslationPhraseId == phrase.Id)).ToList();
                        translationsToDelete.ForEach(t => _db.Translations.Remove(t));
                    }
                }

                // Check for new translations
                foreach (var translationInput in translations)
                {
                    var existingTranslation = oldTranslations.FirstOrDefault(t => t.TranslationPhrase.Text == translationInput);
                    if (existingTranslation != null)
                    {
                        // Already exists
                        continue;
                    }

                    // New translation
                    var existingPhrase = _db.Phrases.FirstOrDefault(dal => dal.Text == translationInput);
                    if (existingPhrase != null)
                    {
                        // Use existing phrase
                        phrase.Translations.Add(new TranslationDal()
                        {
                            ForPhrase = phrase,
                            TranslationPhrase = existingPhrase
                        });
                    }
                    else
                    {
                        // Create new phrase
                        var newPhraseDal = new PhraseDal()
                        {
                            Text = translationInput,
                            // ToDo: Since now only two languages, determine translation language this way
                            LanguageId =
                                _db.Languages.Single(languageDal => languageDal.Id != phrase.LanguageId).Id
                        };

                        phrase.Translations.Add(new TranslationDal()
                        {
                            ForPhrase = phrase,
                            TranslationPhrase = newPhraseDal
                        });

                        _db.Phrases.Add(newPhraseDal);
                    }
                }
            }
            else
            {
                // User removed all translations

                RemoveAllTranslations(phrase);
            }

            // Modify the phrase in the database
            _db.Entry(phrase).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeletePhrase(PhraseDal phraseDal)
        {
            if (phraseDal == null)
                throw new ArgumentNullException(nameof(phraseDal));

            RemoveAllTranslations(phraseDal);

            // Remove the phrase
            _db.Phrases.Remove(phraseDal);

            _db.SaveChanges();
        }

        private IList<TranslationDal> GetTranslationsFor(PhraseDal phraseDal)
        {
            var result = new List<TranslationDal>();

            // Get translations
            var translations = phraseDal.Translations.ToList();
            result.AddRange(translations.Select(t => new TranslationDal()
            {
                ForPhrase = phraseDal,
                ForPhraseId = phraseDal.Id,
                TranslationPhrase = _db.Phrases.Single(p => p.Id == t.TranslationPhraseId),
                TranslationPhraseId = t.TranslationPhraseId
            }));

            // Get translations from phrases for which this phrase is a translation
            translations = phraseDal.PhrasesTranslatedBy.ToList();
            result.AddRange(translations.Select(t => new TranslationDal()
            {
                ForPhrase = phraseDal,
                ForPhraseId = phraseDal.Id,
                TranslationPhrase = _db.Phrases.Single(p => p.Id == t.ForPhraseId),
                TranslationPhraseId = t.ForPhraseId
            }));

            return result;
        }

        private void RemoveAllTranslations(PhraseDal phraseDal)
        {
            // Remove the translations for this phrase
            phraseDal.Translations.ToList().ForEach(t => _db.Translations.Remove(t));

            // Remove the translations for which this phrase is a translation
            phraseDal.PhrasesTranslatedBy.ToList().ForEach(t => _db.Translations.Remove(t));
        }

        private void AssertPhraseText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ValidationFailedException(nameof(text), "Phrase cannot be empty");
            }

            if (text.Length > MaxPhraseLength)
            {
                throw new ValidationFailedException(nameof(text), "Phrase is too long");
            }
        }

        private void AssertLanguageId(int languageId)
        {
            var isLanguageExists = _db.Languages.Any(dal => dal.Id == languageId);
            if (!isLanguageExists)
            {
                throw new ValidationFailedException(nameof(languageId), $"Language with id {languageId} does not exist");
            }
        }

        private void AssertTranslations(IList<string> translations)
        {
            if (translations == null)
                return;

            if (translations.Any(string.IsNullOrWhiteSpace))
                throw new ValidationFailedException(nameof(translations), "Translation cannot be empty");

            if (translations.Any(ts => ts.Length > MaxPhraseLength))
                throw new ValidationFailedException(nameof(translations), "One of translations is too long");
        }

        private void AssertPhraseNotExist(string text)
        {
            if (_db.Phrases.Any(dal => dal.Text == text))
            {
                throw new ValidationFailedException(nameof(text), "The phrase already exists");
            }
        }
    }
}