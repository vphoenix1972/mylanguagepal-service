using MyLanguagePal.Core.Framework;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;

namespace MyLanguagePalService.Tests.BLL.Tasks.WriteTranslation
{
    public abstract class WriteTranslationTaskTestBase : TaskTestBase
    {
        public WriteTranslationTaskService CreateService(IFramework framework = null,
            IPhrasesService phrasesService = null,
            ILanguagesService languagesService = null)
        {
            if (framework == null)
                framework = GetFrameworkStub().Object;
            if (phrasesService == null)
                phrasesService = GetPhrasesServiceStub().Object;
            if (languagesService == null)
                languagesService = GetLanguageServiceStub().Object;

            return new WriteTranslationTaskService(framework, phrasesService, languagesService, Db);
        }



        //public void ShouldCheckCountOfWordsUsed(Action<WriteTranslationTaskService, WriteTranslationTaskSettings> method)
        //{
        //    /* Arrange */
        //    var mockDb = new Mock<IApplicationDbContext>();

        //    var settings = new List<WriteTranslationTaskSettingDal>().AsQueryable();

        //    var mockDbSet = CreateMockDbSet(settings);

        //    mockDb.Setup(x => x.WriteTranslationTaskSettings)
        //        .Returns(mockDbSet.Object);

        //    var db = mockDb.Object;

        //    var phrasesService = GetStubObject<IPhrasesService>();

        //    var languageServiceMock = GetLanguageServiceStub();
        //    var languagesService = languageServiceMock.Object;

        //    /* Act */
        //    var service = new WriteTranslationTaskService(phrasesService, languagesService, db);
        //    var input = new WriteTranslationTaskSettings()
        //    {
        //        LanguageId = 1,
        //        CountOfWordsUsed = SprintTaskService.MinCountOfWordsUsed - 1
        //    };

        //    ValidationFailedException vfeCaught = null;
        //    try
        //    {
        //        method(service, input);
        //    }
        //    catch (ValidationFailedException vfe)
        //    {
        //        vfeCaught = vfe;
        //    }

        //    /* Assert */
        //    AssertValidationFailedException(vfeCaught, nameof(WriteTranslationTaskSettings.CountOfWordsUsed));

        //    /* Act */
        //    input = new WriteTranslationTaskSettings()
        //    {
        //        LanguageId = 1,
        //        CountOfWordsUsed = WriteTranslationTaskService.MaxCountOfWordsUsed + 1
        //    };

        //    vfeCaught = null;
        //    try
        //    {
        //        method(service, input);
        //    }
        //    catch (ValidationFailedException vfe)
        //    {
        //        vfeCaught = vfe;
        //    }

        //    /* Assert */
        //    AssertValidationFailedException(vfeCaught, nameof(WriteTranslationTaskSettings.CountOfWordsUsed));
        //}

        //public Mock<IDbSet<WriteTranslationTaskJournalRecordDal>> CreateWriteTranslationTaskJournalRecordsMockDbSet(IList<WriteTranslationTaskJournalRecordDal> data)
        //{
        //    return CreateMockDbSet(data, db => db.WriteTranslationTaskJournal);
        //}
    }
}
