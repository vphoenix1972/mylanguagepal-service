using MyLanguagePal.Core.Framework;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks;
using MyLanguagePalService.BLL.Tasks.Sprint;

namespace MyLanguagePalService.Tests.BLL.Tasks.Sprint
{
    public abstract class SprintTaskTestBase : TaskTestBase<SprintTaskSettings>
    {
        protected SprintTaskTestBase()
        {
            TaskId = SprintTaskService.SprintTaskId;
        }

        protected override ITaskService CreateService(IFramework framework = null,
            IPhrasesService phrasesService = null,
            ILanguagesService languagesService = null)
        {
            if (framework == null)
                framework = GetFrameworkStub().Object;
            if (phrasesService == null)
                phrasesService = GetPhrasesServiceStub().Object;
            if (languagesService == null)
                languagesService = GetLanguageServiceStub().Object;

            return new SprintTaskService(framework, phrasesService, languagesService, Db);
        }
    }
}
