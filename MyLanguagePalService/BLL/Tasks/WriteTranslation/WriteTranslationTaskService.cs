using System;
using System.Collections.Generic;
using System.Linq;
using MyLanguagePal.Core.Framework;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.Quiz;
using MyLanguagePalService.DAL;

namespace MyLanguagePalService.BLL.Tasks.WriteTranslation
{
    public sealed class WriteTranslationTaskService : QuizTaskServiceBase<QuizTaskSettings, QuizTaskRunModel, QuizTaskAnswersModel, QuizTaskSummary>
    {
        public const int WriteTranslationTaskId = 1;
        public const string WriteTranslationTaskName = "writeTranslation";
        public const int MinCountOfWordsUsed = 1;
        public const int MaxCountOfWordsUsed = 1000;
        public const int DefaultCountOfWordsUsed = 30;

        public WriteTranslationTaskService(IFramework framework,
            IPhrasesService phrasesService, ILanguagesService languagesService, IApplicationDbContext db) :
            base(framework, phrasesService, languagesService, db, WriteTranslationTaskId, WriteTranslationTaskName)
        {
        }

        protected override QuizTaskSettings SetSettingsImpl(QuizTaskSettings settings)
        {
            Assert(settings);

            return base.SetSettingsImpl(settings);
        }

        protected override QuizTaskRunModel RunNewTaskImpl(QuizTaskSettings settings)
        {
            Assert(settings);

            /* Logic */
            var phrases = Db.Phrases
                .Where(p => p.LanguageId == settings.LanguageId)
                .ToList();
            var levels = GetTaskKnowledgeLevels();

            var phrasesToRepeat = GetPhrasesToRepeat(phrases, levels, settings);

            var result = new QuizTaskRunModel()
            {
                Phrases = phrasesToRepeat.Select(p => new Phrase(p)).ToList()
            };

            return result;
        }

        protected override QuizTaskSummary FinishTaskImpl(QuizTaskSettings settings, QuizTaskAnswersModel result)
        {
            /* Validation */
            Assert(settings);
            Assert(result);

            /* Check answers */
            var taskResults = new List<QuizTaskResult<Phrase>>();
            for (var i = 0; i < result.Answers.Count; i++)
            {
                var answer = result.Answers[i];
                if (answer == null)
                    throw new ArgumentNullException($"{result.Answers}[{i}]");

                var phrase = PhrasesService.GetPhrase(answer.PhraseId);
                if (phrase == null)
                    throw new ArgumentException($"Phrase in {result.Answers}[{i}] does not exist");

                if (phrase.LanguageId != settings.LanguageId)
                    throw new ArgumentException($"Phrase in {result.Answers}[{i}] has different language from language in settings.");

                var isCorrect = answer.Answers.Count > 0;
                foreach (var input in answer.Answers)
                {
                    isCorrect &= PhrasesService.GetTranslations(phrase).Any(ts => ts.Phrase.Text == input);
                    if (!isCorrect)
                        break;
                }

                taskResults.Add(new QuizTaskResult<Phrase>()
                {
                    Entity = new Phrase(phrase),
                    IsCorrect = isCorrect
                });
            }

            UpdateKnowledgeLevels(taskResults);

            return new QuizTaskSummary()
            {
                Results = taskResults
            };
        }

        protected override QuizTaskSettings DefaultSettings()
        {
            return DefaultSettings(DefaultCountOfWordsUsed);
        }

        private void Assert(QuizTaskSettings settings)
        {
            Assert(settings, MinCountOfWordsUsed, MaxCountOfWordsUsed);
        }
    }
}