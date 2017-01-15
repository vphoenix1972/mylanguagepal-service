using System;
using System.Collections.Generic;
using System.Linq;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Phrases
{
    public class PhrasesService : IPhrasesService
    {
        public const int PrevalenceMax = 40;

        private const int MaxPhraseLength = 100;

        private readonly IApplicationDbContext _db;

        public PhrasesService(IApplicationDbContext db)
        {
            _db = db;
        }

        public IList<PhraseDal> GetPhrasesDals()
        {
            return _db.Phrases.ToList();
        }

        public IList<Phrase> GetPhrases()
        {
            return _db.Phrases.Select(Phrase.MapFrom).ToList();
        }

        public IList<Translation> GetTranslations(int phraseId)
        {
            var phraseDal = _db.Phrases.Find(phraseId);
            if (phraseDal == null)
                throw new ArgumentException($"Phrase with id '${phraseId}' not found");

            return GetTranslations(phraseDal).Select(bbl => new Translation()
            {
                Phrase = Phrase.MapFrom(bbl.Phrase),
                Prevalence = bbl.Prevalence
            }).ToList();
        }

        public IList<TranslationModelBbl> GetTranslations(PhraseDal phraseDal)
        {
            if (phraseDal == null)
                throw new ArgumentNullException(nameof(phraseDal));

            var result = GetTranslationsFor(phraseDal);

            return result.Select(t => new TranslationModelBbl()
            {
                Phrase = t.TranslationPhrase,
                Prevalence = t.Prevalence
            }).ToList();
        }

        public PhraseDal GetPhrase(int id)
        {
            return _db.Phrases.Find(id);
        }

        public int CreatePhrase(string text, int languageId, IList<TranslationImBll> translations = null)
        {
            /* Validation */
            text = PreparePhraseText(text);
            translations = PreparePhraseTranslations(translations);

            AssertPhraseText(text);
            AssertLanguageId(languageId);
            AssertTranslations(translations, languageId);
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
                    var existingPhraseDal = _db.Phrases.FirstOrDefault(dal => dal.Text == translationInput.Text);
                    if (existingPhraseDal != null)
                    {
                        // Use existing translation
                        translationsDals.Add(CreateTranslationDal(newPhraseDal, existingPhraseDal, translationInput));
                    }
                    else
                    {
                        // Create new translation
                        var newPhraseAsTranslation = new PhraseDal()
                        {
                            Text = translationInput.Text,
                            // ToDo: Since now only two languages, determine translation language this way
                            LanguageId =
                                _db.Languages.Single(languageDal => languageDal.Id != languageId).Id
                        };

                        _db.Phrases.Add(newPhraseAsTranslation);
                        translationsDals.Add(CreateTranslationDal(newPhraseDal, newPhraseAsTranslation, translationInput));
                    }
                }
                newPhraseDal.Translations = translationsDals;
            }

            // Create new phrase in the database
            _db.Phrases.Add(newPhraseDal);

            _db.SaveChanges();

            return newPhraseDal.Id;
        }

        public void UpdatePhrase(PhraseDal phrase, string text, IList<TranslationImBll> translations)
        {
            /* Validation */
            if (phrase == null)
                throw new ArgumentNullException(nameof(phrase));

            text = PreparePhraseText(text);
            translations = PreparePhraseTranslations(translations);

            AssertPhraseText(text);
            AssertTranslations(translations, phrase.LanguageId);

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
                    var userTranslation = translations.FirstOrDefault(t => t.Text == existingTranslation.TranslationPhrase.Text);
                    if (userTranslation == null)
                    {
                        // User has removed the translation

                        // Remove this phrase from phrases for which it is the translation

                        var translationToDelete = _db.Translations.Find(existingTranslation.Id);
                        _db.Translations.Remove(translationToDelete);
                    }
                }

                // Check for new translations
                foreach (var translationInput in translations)
                {
                    var existingTranslation = oldTranslations.FirstOrDefault(t => t.TranslationPhrase.Text == translationInput.Text);
                    if (existingTranslation != null)
                    {
                        // Already exists - merge

                        var actualTranslation = _db.Translations.Find(existingTranslation.Id);
                        actualTranslation.Prevalence = translationInput.Prevalence;

                        _db.MarkModified(actualTranslation);

                        continue;
                    }

                    // New translation
                    var existingPhrase = _db.Phrases.FirstOrDefault(dal => dal.Text == translationInput.Text);
                    if (existingPhrase != null)
                    {
                        // Use existing phrase
                        phrase.Translations.Add(CreateTranslationDal(phrase, existingPhrase, translationInput));
                    }
                    else
                    {
                        // Create new phrase
                        var newPhraseDal = new PhraseDal()
                        {
                            Text = translationInput.Text,
                            // ToDo: Since now only two languages, determine translation language this way
                            LanguageId =
                                _db.Languages.Single(languageDal => languageDal.Id != phrase.LanguageId).Id
                        };

                        phrase.Translations.Add(CreateTranslationDal(phrase, newPhraseDal, translationInput));

                        _db.Phrases.Add(newPhraseDal);
                    }
                }
            }
            else
            {
                // User removed all translations

                RemoveDependencies(phrase);
            }

            // Modify the phrase in the database
            _db.MarkModified(phrase);
            _db.SaveChanges();
        }

        public void DeletePhrase(PhraseDal phraseDal)
        {
            if (phraseDal == null)
                throw new ArgumentNullException(nameof(phraseDal));

            RemoveDependencies(phraseDal);

            // Remove the phrase
            _db.Phrases.Remove(phraseDal);

            _db.SaveChanges();
        }

        public bool CheckIfPhraseExists(int id)
        {
            return _db.Phrases.Find(id) != null;
        }

        private IList<TranslationDal> GetTranslationsFor(PhraseDal phraseDal)
        {
            var result = new List<TranslationDal>();

            // Get translations
            var translations = phraseDal.Translations.ToList();
            result.AddRange(translations.Select(t => new TranslationDal()
            {
                Id = t.Id,
                ForPhrase = phraseDal,
                ForPhraseId = phraseDal.Id,
                TranslationPhrase = _db.Phrases.Single(p => p.Id == t.TranslationPhraseId),
                TranslationPhraseId = t.TranslationPhraseId,
                Prevalence = t.Prevalence
            }));

            // Get translations from phrases for which this phrase is a translation
            translations = phraseDal.PhrasesTranslatedBy.ToList();
            result.AddRange(translations.Select(t => new TranslationDal()
            {
                Id = t.Id,
                ForPhrase = phraseDal,
                ForPhraseId = phraseDal.Id,
                TranslationPhrase = _db.Phrases.Single(p => p.Id == t.ForPhraseId),
                TranslationPhraseId = t.ForPhraseId,
                Prevalence = t.Prevalence
            }));

            return result;
        }

        private void RemoveDependencies(PhraseDal phraseDal)
        {
            // Remove the translations for this phrase
            phraseDal.Translations.ToList().ForEach(t => _db.Translations.Remove(t));

            // Remove the translations for which this phrase is a translation
            phraseDal.PhrasesTranslatedBy.ToList().ForEach(t => _db.Translations.Remove(t));

            // Remove tasks knowledge levels
            phraseDal.KnowledgeLevels.ToList().ForEach(l => _db.KnowledgeLevels.Remove(l));
        }

        private TranslationDal CreateTranslationDal(PhraseDal newPhraseDal, PhraseDal existingPhraseDal, TranslationImBll translationInput)
        {
            return new TranslationDal()
            {
                ForPhrase = newPhraseDal,
                TranslationPhrase = existingPhraseDal,
                Prevalence = translationInput.Prevalence
            };
        }

        private string PreparePhraseText(string text)
        {
            return text?.Trim();
        }

        private IList<TranslationImBll> PreparePhraseTranslations(IList<TranslationImBll> translations)
        {
            return translations?
                .Where(t => !string.IsNullOrWhiteSpace(t.Text))
                .Select(t => new TranslationImBll()
                {
                    Text = t.Text.Trim(),
                    Prevalence = PrepareTranslationPrevalence(t.Prevalence)
                })
                .ToList();
        }

        private int PrepareTranslationPrevalence(int prevalence)
        {
            if (prevalence < 0)
                return 0;
            if (prevalence > PrevalenceMax)
                return PrevalenceMax;

            return prevalence;
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

        private void AssertTranslations(IList<TranslationImBll> translations, int phraseLanguageId)
        {
            if (translations == null)
                return;

            for (var i = 0; i < translations.Count; i++)
            {
                var translation = translations[i];

                // Check emptyness
                if (string.IsNullOrWhiteSpace(translation.Text))
                    throw new ValidationFailedException($"Translations[{i}]", "Translation cannot be empty");

                // Check length
                if (translation.Text.Length > MaxPhraseLength)
                    throw new ValidationFailedException($"Translations[{i}]", "Translation is too long");

                // Check that none of translations has the same language as the language of the phrase
                if (_db.Phrases.Any(p => p.Text == translation.Text &&
                                         p.LanguageId == phraseLanguageId))
                {
                    throw new ValidationFailedException($"Translations[{i}]",
                        "Translation cannot have the same language as the language of the phrase");
                }
            }
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