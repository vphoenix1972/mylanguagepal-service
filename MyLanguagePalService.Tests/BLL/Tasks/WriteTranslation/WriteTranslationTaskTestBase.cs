using MyLanguagePal.Core.Framework;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks;
using MyLanguagePalService.BLL.Tasks.Quiz;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;

namespace MyLanguagePalService.Tests.BLL.Tasks.WriteTranslation
{
    public abstract class WriteTranslationTaskTestBase : TaskTestBase<QuizTaskSettings>
    {
        protected WriteTranslationTaskTestBase()
        {
            TaskId = WriteTranslationTaskService.WriteTranslationTaskId;
        }

        protected override ITaskService CreateService(IFramework framework = null,
            IPhrasesService phrasesService = null,
            ILanguagesService languagesService = null)
        {
            if (framework == null)
                framework = CreateFrameworkStub().Object;
            if (phrasesService == null)
                phrasesService = CreatePhrasesServiceStub().Object;
            if (languagesService == null)
                languagesService = GetLanguageServiceStub().Object;

            return new WriteTranslationTaskService(framework, phrasesService, languagesService, Db);
        }
    }
}
