using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;
using MyLanguagePalService.Tests.TestsShared;

namespace MyLanguagePalService.Tests.BLL.Tasks.WriteTranslation
{
    [TestClass]
    public class WriteTranslationTaskServiceGetSettingsTest : TestBase
    {
        [TestMethod]
        public void GetSettings_ShouldReturnSettingsIfOneSettingInDb()
        {
            /* Arrange */
            var mockContext = new Mock<IApplicationDbContext>();

            var expected = new WriteTranslationTaskSettingDal()
            {
                Id = 1,
                LanguageId = 1,
                CountOfWordsUsed = 3
            };

            var settings = new List<WriteTranslationTaskSettingDal>()
            {
                expected
            }.AsQueryable();

            var mockSet = CreateMockDbSet(settings);

            mockContext.Setup(x => x.WriteTranslationTaskSettings)
                .Returns(mockSet.Object);

            var db = mockContext.Object;

            var phrasesService = GetStubObject<IPhrasesService>();
            var languagesService = GetStubObject<ILanguagesService>();

            /* Act */
            var service = new WriteTranslationTaskService(phrasesService, languagesService, db);
            var actual = service.GetSettings();

            /* Assert */
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.LanguageId, actual.LanguageId);
            Assert.AreEqual(expected.CountOfWordsUsed, actual.CountOfWordsUsed);
        }

        [TestMethod]
        public void GetSettings_ShouldReturnFirstSettingsIfMoreThanOneRowInDb()
        {
            /* Arrange */
            var mockContext = new Mock<IApplicationDbContext>();

            var expected = new WriteTranslationTaskSettingDal()
            {
                Id = 1,
                LanguageId = 1,
                CountOfWordsUsed = 3
            };
            var settingsRow2 = new WriteTranslationTaskSettingDal()
            {
                Id = 1,
                LanguageId = 2,
                CountOfWordsUsed = 5
            };

            var settings = new List<WriteTranslationTaskSettingDal>()
            {
                expected,
                settingsRow2

            }.AsQueryable();

            var mockSet = CreateMockDbSet(settings);

            mockContext.Setup(x => x.WriteTranslationTaskSettings)
                .Returns(mockSet.Object);

            var db = mockContext.Object;

            var phrasesService = GetStubObject<IPhrasesService>();
            var languagesService = GetStubObject<ILanguagesService>();

            /* Act */
            var service = new WriteTranslationTaskService(phrasesService, languagesService, db);
            var actual = service.GetSettings();

            /* Assert */
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.LanguageId, actual.LanguageId);
            Assert.AreEqual(expected.CountOfWordsUsed, actual.CountOfWordsUsed);
        }

        [TestMethod]
        public void GetSettings_ShouldReturnDefaultSettingsIfNoRowsInDb()
        {
            /* Arrange */
            var expected = new WriteTranslationTaskSettingDal()
            {
                Id = 1,
                LanguageId = 1,
                CountOfWordsUsed = WriteTranslationTaskService.DefaultCountOfWordsUsed
            };

            var mockContext = new Mock<IApplicationDbContext>();

            var settings = new List<WriteTranslationTaskSettingDal>().AsQueryable();

            var mockSet = CreateMockDbSet(settings);

            mockContext.Setup(x => x.WriteTranslationTaskSettings)
                .Returns(mockSet.Object);

            var db = mockContext.Object;

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = new Mock<ILanguagesService>();
            languageServiceMock.SetupAllProperties();
            languageServiceMock.Setup(m => m.GetDefaultLanguage()).Returns(new Language()
            {
                Id = 1,
                Name = "English"
            });
            var languagesService = languageServiceMock.Object;


            /* Act */
            var service = new WriteTranslationTaskService(phrasesService, languagesService, db);
            var actual = service.GetSettings();

            /* Assert */
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.LanguageId, actual.LanguageId);
            Assert.AreEqual(expected.CountOfWordsUsed, actual.CountOfWordsUsed);
        }
    }
}
