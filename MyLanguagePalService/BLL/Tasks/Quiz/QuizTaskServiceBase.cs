using System;
using System.Collections.Generic;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.Core;
using MyLanguagePalService.Core.Extensions;
using MyLanguagePalService.DAL;

namespace MyLanguagePalService.BLL.Tasks.Quiz
{
    public abstract class QuizTaskServiceBase<TSettings, TRunModel, TAnswers, TSummary> : TaskServiceBase<TSettings, TRunModel, TAnswers, TSummary>
        where TSettings : class, new() where TAnswers : class
    {
        protected QuizTaskServiceBase(IPhrasesService phrasesService, ILanguagesService languagesService, IApplicationDbContext db, int taskId, string name) :
            base(phrasesService, languagesService, db, taskId, name)
        {
        }

        protected QuizTaskSettings DefaultSettings(int defaultCountOfWordsUsed)
        {
            return new QuizTaskSettings()
            {
                LanguageId = LanguagesService.GetDefaultLanguage().Id,
                CountOfWordsUsed = defaultCountOfWordsUsed
            };
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
    }
}