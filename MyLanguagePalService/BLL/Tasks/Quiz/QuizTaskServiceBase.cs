using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MyLanguagePal.Core.Framework;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Extensions;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Tasks.Quiz
{
    public abstract class QuizTaskServiceBase<TSettings, TRunModel, TAnswers, TSummary> : TaskServiceBase<TSettings, TRunModel, TAnswers, TSummary>
        where TSettings : class, new() where TAnswers : class
    {
        protected QuizTaskServiceBase([NotNull] IFramework framework, IPhrasesService phrasesService, ILanguagesService languagesService, IApplicationDbContext db, int taskId, string name) :
            base(phrasesService, languagesService, db, taskId, name)
        {
            if (framework == null)
                throw new ArgumentNullException(nameof(framework));

            Framework = framework;
        }

        [NotNull]
        protected IFramework Framework { get; set; }

        protected QuizTaskSettings DefaultSettings(int defaultCountOfWordsUsed)
        {
            return new QuizTaskSettings()
            {
                LanguageId = LanguagesService.GetDefaultLanguage().Id,
                CountOfWordsUsed = defaultCountOfWordsUsed
            };
        }

        protected IList<PhraseDal> GetPhrasesToRepeat(IList<PhraseDal> phrases, IList<KnowledgeLevelDal> levels, QuizTaskSettings settings)
        {
            var currentDate = Framework.UtcNow;

            var phrasesToRepeat = new List<PhraseDal>();

            foreach (var phrase in phrases)
            {
                var knowledgeLevel = levels.SingleOrDefault(l => l.PhraseId == phrase.Id);
                if (knowledgeLevel == null)
                {
                    // Phrase has been used in task yet
                    phrasesToRepeat.Add(phrase);
                    if (phrasesToRepeat.Count >= settings.CountOfWordsUsed)
                        break; // Limit reached
                }
                else
                {
                    if (knowledgeLevel.CurrentLevel <= (currentDate - knowledgeLevel.LastRepetitonTime).Days)
                    {
                        // It is time to repeat the phrase
                        phrasesToRepeat.Add(phrase);
                        if (phrasesToRepeat.Count >= settings.CountOfWordsUsed)
                            break; // Limit reached
                    }

                    // Skip phrase - knowledge level is ok
                }
            }

            return phrasesToRepeat;
        }

        protected void UpdateKnowledgeLevels(IList<QuizTaskResult<Phrase>> taskResults)
        {
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
                                l.CurrentLevel = 1;
                            }

                            if (l.CurrentLevel > 30)
                                l.CurrentLevel = 30; // MAX                            

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
        }

        protected void Assert(QuizTaskSettings settings, int minCountOfWordsUsed, int maxCountOfWordsUsed)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (!LanguagesService.CheckIfLanguageExists(settings.LanguageId))
                throw new ValidationFailedException(nameof(settings.LanguageId), GetLanguageNotExistString(settings.LanguageId));

            if (settings.CountOfWordsUsed < minCountOfWordsUsed || settings.CountOfWordsUsed > maxCountOfWordsUsed)
            {
                throw new ValidationFailedException(nameof(settings.CountOfWordsUsed),
                    $"Count of words used must be between {minCountOfWordsUsed} and {maxCountOfWordsUsed} words");
            }
        }

        protected void Assert(QuizTaskAnswersModel result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            if (result.Answers == null)
                throw new ArgumentNullException(nameof(result.Answers));
        }

        private int GetCoef(int? previousLevel)
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