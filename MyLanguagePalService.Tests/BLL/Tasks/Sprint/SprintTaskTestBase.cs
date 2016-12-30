using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;
using MyLanguagePalService.Tests.TestsShared;

namespace MyLanguagePalService.Tests.BLL.Tasks.Sprint
{
    public abstract class SprintTaskTestBase : TestBase
    {
        public void ShouldCheckLanguageId(Action<SprintTaskService, SprintTaskSettings> method)
        {
            /* Arrange */
            var mockDb = new Mock<IApplicationDbContext>();

            var settings = new List<SprintTaskSettingDal>().AsQueryable();

            var sprintTaskSettingsDbSet = CreateMockDbSet(settings);

            mockDb.Setup(x => x.SprintTaskSettings)
                .Returns(sprintTaskSettingsDbSet.Object);

            var db = mockDb.Object;

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = new Mock<ILanguagesService>();
            languageServiceMock.Setup(m => m.CheckIfLanguageExists(It.IsAny<int>())).Returns(false);
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, db);
            var input = new SprintTaskSettings()
            {
                LanguageId = 3,
                CountOfWordsUsed = 40,
                TotalTimeForTask = 60
            };

            ValidationFailedException vfeCaught = null;
            try
            {
                method(service, input);
            }
            catch (ValidationFailedException vfe)
            {
                vfeCaught = vfe;
            }

            /* Assert */
            languageServiceMock.Verify(m => m.CheckIfLanguageExists(input.LanguageId), Times.Once);
            AssertValidationFailedException(vfeCaught, nameof(SprintTaskSettings.LanguageId));
        }

        public void ShouldCheckTotalTimeForTask(Action<SprintTaskService, SprintTaskSettings> method)
        {
            /* Arrange */
            var mockDb = new Mock<IApplicationDbContext>();

            var settings = new List<SprintTaskSettingDal>().AsQueryable();

            var sprintTaskSettingsDbSet = CreateMockDbSet(settings);

            mockDb.Setup(x => x.SprintTaskSettings)
                .Returns(sprintTaskSettingsDbSet.Object);

            var db = mockDb.Object;

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, db);
            var input = new SprintTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = 40,
                TotalTimeForTask = SprintTaskService.MinTotalTimeForTask - 1
            };

            ValidationFailedException vfeCaught = null;
            try
            {
                method(service, input);
            }
            catch (ValidationFailedException vfe)
            {
                vfeCaught = vfe;
            }

            /* Assert */
            AssertValidationFailedException(vfeCaught, nameof(SprintTaskSettings.TotalTimeForTask));
        }

        public void ShouldCheckCountOfWordsUsed(Action<SprintTaskService, SprintTaskSettings> method)
        {
            /* Arrange */
            var mockDb = new Mock<IApplicationDbContext>();

            var settings = new List<SprintTaskSettingDal>().AsQueryable();

            var sprintTaskSettingsDbSet = CreateMockDbSet(settings);

            mockDb.Setup(x => x.SprintTaskSettings)
                .Returns(sprintTaskSettingsDbSet.Object);

            var db = mockDb.Object;

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, db);
            var input = new SprintTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = SprintTaskService.MinCountOfWordsUsed - 1,
                TotalTimeForTask = 10
            };

            ValidationFailedException vfeCaught = null;
            try
            {
                method(service, input);
            }
            catch (ValidationFailedException vfe)
            {
                vfeCaught = vfe;
            }

            /* Assert */
            AssertValidationFailedException(vfeCaught, nameof(SprintTaskSettings.CountOfWordsUsed));

            /* Act */
            input = new SprintTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = SprintTaskService.MaxCountOfWordsUsed + 1,
                TotalTimeForTask = 10
            };

            vfeCaught = null;
            try
            {
                method(service, input);
            }
            catch (ValidationFailedException vfe)
            {
                vfeCaught = vfe;
            }

            /* Assert */
            AssertValidationFailedException(vfeCaught, nameof(SprintTaskSettings.CountOfWordsUsed));
        }

        public Mock<IDbSet<SprintTaskJournalRecordDal>> CreateSprintTaskJournalRecordsMockDbSet(IList<SprintTaskJournalRecordDal> data)
        {
            return CreateMockDbSet(data, db => db.SprintTaskJournal);
        }
    }
}
