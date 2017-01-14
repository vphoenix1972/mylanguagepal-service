using System;
using System.Collections.Generic;
using System.Linq;
using MyLanguagePal.Core.Framework;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.Core;
using MyLanguagePalService.Core.Extensions;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Extensions;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Tasks.Quiz
{
    public abstract class QuizTaskServiceBase<TSettings, TRunModel, TAnswers, TSummary> : TaskServiceBase<TSettings, TAnswers, TSummary>
        where TSettings : QuizTaskSettings, new()
        where TRunModel : QuizTaskRunModel, new()
        where TAnswers : class
        where TSummary : QuizTaskSummary, new()
    {
        public const int MinCountOfWordsUsed = 1;
        public const int MaxCountOfWordsUsed = 1000;
        public const int DefaultCountOfWordsUsed = 30;

        protected QuizTaskServiceBase(IFramework framework, IPhrasesService phrasesService, ILanguagesService languagesService, IApplicationDbContext db) :
            base(framework, phrasesService, languagesService, db)
        {

        }

        public override object SetSettings(object settings)
        {
            var typedSettings = settings.FromJObjectTo<TSettings>();
            if (typedSettings == null)
                throw new ArgumentException(nameof(settings));

            Assert(typedSettings);

            return base.SetSettings(typedSettings);
        }

        public override object RunNewTask(object runSettings)
        {
            var settings = runSettings.FromJObjectTo<TSettings>();
            if (settings == null)
                throw new ArgumentException(nameof(runSettings));

            Assert(settings);

            /* Logic */
            var phrases = Db.Phrases
                .Where(p => p.LanguageId == settings.LanguageId)
                .ToList();
            var levels = GetTaskKnowledgeLevels();

            var currentDate = Framework.UtcNow;

            var phrasesToRepeat = new List<PhraseDal>();

            foreach (var phrase in phrases)
            {
                var knowledgeLevel = levels.SingleOrDefault(l => l.PhraseId == phrase.Id);
                if (knowledgeLevel == null)
                {
                    // Phrase not has been used in task yet
                    phrasesToRepeat.Add(phrase);
                    if (phrasesToRepeat.Count >= settings.CountOfWordsUsed)
                        break; // Limit reached
                }
                else
                {
                    if (knowledgeLevel.CurrentLevel <= (currentDate - knowledgeLevel.LastRepetitonTime).TotalSeconds)
                    {
                        // It is time to repeat the phrase
                        phrasesToRepeat.Add(phrase);
                        if (phrasesToRepeat.Count >= settings.CountOfWordsUsed)
                            break; // Limit reached
                    }

                    // Skip phrase - knowledge level is ok
                }
            }

            var result = new TRunModel()
            {
                Phrases = phrasesToRepeat.Select(p => new PhraseWithTranslations(p)).ToList()
            };

            return result;
        }

        protected override TSummary FinishTaskImpl(TSettings settings, TAnswers result)
        {
            /* Validation */
            Assert(settings);

            var taskResults = CheckAnswers(settings, result);

            foreach (var answer in taskResults)
            {
                Db.AddOrUpdate(
                    dbSetGetter: db => db.KnowledgeLevels,
                    searcher: dbSet => dbSet.FirstOrDefault(l => l.TaskId == TaskId && l.PhraseId == answer.Entity.Id),
                    setter: l =>
                    {
                        l.PhraseId = answer.Entity.Id;
                        l.TaskId = TaskId;
                        l.LastRepetitonTime = DateTime.UtcNow;

                        var previousLevel = l.CurrentLevel;

                        if (answer.IsCorrect)
                        {
                            if (l.CurrentLevel >= 1)
                            {
                                l.CurrentLevel *= 2 + GetCoef(l.PreviousLevel);
                            }
                            else
                            {
                                l.CurrentLevel = 24;
                            }

                            var maxHours = 24 * 30; // 30 days
                            if (l.CurrentLevel > maxHours)
                                l.CurrentLevel = maxHours; // MAX                            

                        }
                        else
                        {
                            l.PreviousLevel = l.CurrentLevel;
                            l.CurrentLevel = 0;
                        }

                        answer.Improvement = l.CurrentLevel - previousLevel;
                    }
                );
            }

            return new TSummary()
            {
                Results = taskResults
            };
        }

        protected override TSettings DefaultSettings()
        {
            return new TSettings()
            {
                LanguageId = LanguagesService.GetDefaultLanguage().Id,
                CountOfWordsUsed = DefaultCountOfWordsUsed
            };
        }

        protected abstract IList<QuizTaskResult<Phrase>> CheckAnswers(TSettings settings, TAnswers result);

        protected void Assert(QuizTaskSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (!LanguagesService.CheckIfLanguageExists(settings.LanguageId))
                throw new ValidationFailedException(nameof(settings.LanguageId), GetLanguageNotExistString(settings.LanguageId));

            if (settings.CountOfWordsUsed < MinCountOfWordsUsed || settings.CountOfWordsUsed > MaxCountOfWordsUsed)
            {
                throw new ValidationFailedException(nameof(settings.CountOfWordsUsed),
                    $"Count of words used must be between {MinCountOfWordsUsed} and {MaxCountOfWordsUsed} words");
            }
        }

        protected void Assert(QuizTaskAnswersModel result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            if (result.Answers == null)
                throw new ArgumentNullException(nameof(result.Answers));
        }

        private int GetCoef(double? previousLevel)
        {
            if (!previousLevel.HasValue)
                return 0;

            if (previousLevel.Value < 4)
                return 0;

            if (previousLevel.Value >= 4)
                return 1;

            return 2;
        }
    }
}