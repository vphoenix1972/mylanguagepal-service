using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;
using MyLanguagePalService.Tests.TestsShared;

namespace MyLanguagePalService.Tests.BLL.Tasks.WriteTranslation
{
    public abstract class WriteTranslationTaskTestBase : TestBase
    {
        public void ShouldCheckLanguageId(Action<WriteTranslationTaskService, WriteTranslationTaskSettingModel> method)
        {
            /* Arrange */
            var mockDb = new Mock<IApplicationDbContext>();

            var settings = new List<WriteTranslationTaskSettingDal>().AsQueryable();

            var mockDbSet = CreateMockDbSet(settings);

            mockDb.Setup(x => x.WriteTranslationTaskSettings)
                .Returns(mockDbSet.Object);

            var db = mockDb.Object;

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = new Mock<ILanguagesService>();
            languageServiceMock.Setup(m => m.CheckIfLanguageExists(It.IsAny<int>())).Returns(false);
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new WriteTranslationTaskService(phrasesService, languagesService, db);
            var input = new WriteTranslationTaskSettingModel()
            {
                LanguageId = 3,
                CountOfWordsUsed = 40
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
            AssertValidationFailedException(vfeCaught, nameof(WriteTranslationTaskSettingModel.LanguageId));
        }


        public void ShouldCheckCountOfWordsUsed(Action<WriteTranslationTaskService, WriteTranslationTaskSettingModel> method)
        {
            /* Arrange */
            var mockDb = new Mock<IApplicationDbContext>();

            var settings = new List<WriteTranslationTaskSettingDal>().AsQueryable();

            var mockDbSet = CreateMockDbSet(settings);

            mockDb.Setup(x => x.WriteTranslationTaskSettings)
                .Returns(mockDbSet.Object);

            var db = mockDb.Object;

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new WriteTranslationTaskService(phrasesService, languagesService, db);
            var input = new WriteTranslationTaskSettingModel()
            {
                LanguageId = 1,
                CountOfWordsUsed = SprintTaskService.MinCountOfWordsUsed - 1
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
            AssertValidationFailedException(vfeCaught, nameof(WriteTranslationTaskSettingModel.CountOfWordsUsed));

            /* Act */
            input = new WriteTranslationTaskSettingModel()
            {
                LanguageId = 1,
                CountOfWordsUsed = WriteTranslationTaskService.MaxCountOfWordsUsed + 1
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
            AssertValidationFailedException(vfeCaught, nameof(WriteTranslationTaskSettingModel.CountOfWordsUsed));
        }

        public Mock<IDbSet<WriteTranslationTaskJournalRecordDal>> CreateWriteTranslationTaskJournalRecordsMockDbSet(IList<WriteTranslationTaskJournalRecordDal> data)
        {
            return CreateMockDbSet(data, db => db.WriteTranslationTaskJournal);
        }
    }
}
