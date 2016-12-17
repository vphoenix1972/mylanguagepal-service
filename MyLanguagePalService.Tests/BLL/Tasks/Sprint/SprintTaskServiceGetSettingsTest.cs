using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;
using MyLanguagePalService.Tests.TestsShared;

namespace MyLanguagePalService.Tests.BLL.Tasks.Sprint
{
    [TestClass]
    public class SprintTaskServiceGetSettingsTest
    {
        [TestMethod]
        public void GetSettings_ShouldReturnSettingsIfOneSettingInDb()
        {
            /* Arrange */
            var mockContext = new Mock<IApplicationDbContext>();

            var expected = new SprintTaskSettingDal()
            {
                Id = 1,
                LanguageId = 1,
                CountOfWordsUsed = 3,
                TotalTimeForTask = 6
            };

            var settings = new List<SprintTaskSettingDal>()
            {
                expected
            }.AsQueryable();

            var mockSet = TestUtils.CreateMockDbSet(settings);

            mockContext.Setup(x => x.SprintTaskSettings)
                .Returns(mockSet.Object);

            var db = mockContext.Object;

            var phrasesService = TestUtils.GetStub<IPhrasesService>();
            var languagesService = TestUtils.GetStub<ILanguagesService>();

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, db);
            var actual = service.GetSettings();

            /* Assert */
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.LanguageId, actual.LanguageId);
            Assert.AreEqual(expected.CountOfWordsUsed, actual.CountOfWordsUsed);
            Assert.AreEqual(expected.TotalTimeForTask, actual.TotalTimeForTask);
        }

        [TestMethod]
        public void GetSettings_ShouldReturnFirstSettingsIfMoreThanOneRowInDb()
        {
            /* Arrange */
            var mockContext = new Mock<IApplicationDbContext>();

            var expected = new SprintTaskSettingDal()
            {
                Id = 1,
                LanguageId = 1,
                CountOfWordsUsed = 3,
                TotalTimeForTask = 6
            };
            var settingsRow2 = new SprintTaskSettingDal()
            {
                Id = 1,
                LanguageId = 2,
                CountOfWordsUsed = 5,
                TotalTimeForTask = 7
            };

            var settings = new List<SprintTaskSettingDal>()
            {
                expected,
                settingsRow2

            }.AsQueryable();

            var mockSet = TestUtils.CreateMockDbSet(settings);

            mockContext.Setup(x => x.SprintTaskSettings)
                .Returns(mockSet.Object);

            var db = mockContext.Object;

            var phrasesService = TestUtils.GetStub<IPhrasesService>();
            var languagesService = TestUtils.GetStub<ILanguagesService>();

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, db);
            var actual = service.GetSettings();

            /* Assert */
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.LanguageId, actual.LanguageId);
            Assert.AreEqual(expected.CountOfWordsUsed, actual.CountOfWordsUsed);
            Assert.AreEqual(expected.TotalTimeForTask, actual.TotalTimeForTask);
        }

        [TestMethod]
        public void GetSettings_ShouldReturnDefaultSettingsIfNoRowsInDb()
        {
            /* Arrange */
            var expected = new SprintTaskSettingDal()
            {
                Id = 1,
                LanguageId = 1,
                CountOfWordsUsed = 30,
                TotalTimeForTask = 60
            };

            var mockContext = new Mock<IApplicationDbContext>();

            var settings = new List<SprintTaskSettingDal>().AsQueryable();

            var mockSet = TestUtils.CreateMockDbSet(settings);

            mockContext.Setup(x => x.SprintTaskSettings)
                .Returns(mockSet.Object);

            var db = mockContext.Object;

            var phrasesService = TestUtils.GetStub<IPhrasesService>();

            var languageServiceMock = new Mock<ILanguagesService>();
            languageServiceMock.SetupAllProperties();
            languageServiceMock.Setup(m => m.GetDefaultLanguage()).Returns(new Language()
            {
                Id = 1,
                Name = "English"
            });
            var languagesService = languageServiceMock.Object;


            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, db);
            var actual = service.GetSettings();

            /* Assert */
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.LanguageId, actual.LanguageId);
            Assert.AreEqual(expected.CountOfWordsUsed, actual.CountOfWordsUsed);
            Assert.AreEqual(expected.TotalTimeForTask, actual.TotalTimeForTask);
        }
    }
}
