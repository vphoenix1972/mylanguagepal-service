using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;
using MyLanguagePalService.Tests.BLL.Tasks.Sprint;

namespace MyLanguagePalService.Tests.BLL.Tasks.WriteTranslation
{
    [TestClass]
    public class WriteTranslationTaskServiceSetSettingsTest : WriteTranslationTaskTestBase
    {
        [TestMethod]
        public void SetSettings_ShouldAddSettingsInDatabase()
        {
            /* Arrange */
            var mockContext = new Mock<IApplicationDbContext>();

            var settings = new List<WriteTranslationTaskSettingDal>().AsQueryable();

            var sprintTaskSettingsDbSet = CreateMockDbSet(settings);

            mockContext.Setup(x => x.WriteTranslationTaskSettings)
                .Returns(sprintTaskSettingsDbSet.Object);

            var db = mockContext.Object;

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new WriteTranslationTaskService(phrasesService, languagesService, db);
            var input = new WriteTranslationTaskSettingModel()
            {
                LanguageId = 1,
                CountOfWordsUsed = 3
            };
            service.SetSettings(input);

            /* Assert */
            languageServiceMock.Verify(m => m.CheckIfLanguageExists(input.LanguageId), Times.Once);
            sprintTaskSettingsDbSet.Verify(
                m => m.Add(It.Is<WriteTranslationTaskSettingDal>(
                    dal => dal.LanguageId == input.LanguageId &&
                           dal.CountOfWordsUsed == input.CountOfWordsUsed
                )), Times.Once);
        }

        [TestMethod]
        public void SetSettings_ShouldEditSettingsInDatabase_ShouldEditFirstRecord()
        {
            /* Arrange */
            var mockDb = new Mock<IApplicationDbContext>();

            var record1 = new WriteTranslationTaskSettingDal()
            {
                Id = 1,
                LanguageId = 1,
                CountOfWordsUsed = 3
            };

            var record2 = new WriteTranslationTaskSettingDal()
            {
                Id = 2,
                LanguageId = 2,
                CountOfWordsUsed = 4
            };

            var settings = new List<WriteTranslationTaskSettingDal>()
            {
                record1,
                record2
            }.AsQueryable();

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
                LanguageId = 2,
                CountOfWordsUsed = 40
            };
            service.SetSettings(input);

            /* Assert */
            languageServiceMock.Verify(m => m.CheckIfLanguageExists(input.LanguageId), Times.Once);

            // Assert that no records was added
            mockDbSet.Verify(m => m.Add(It.IsAny<WriteTranslationTaskSettingDal>()), Times.Never);

            // Assert that only one record was marked as modified
            mockDb.Verify(m => m.MarkModified(It.IsAny<WriteTranslationTaskSettingDal>()), Times.Once);
            mockDb.Verify(m => m.MarkModified(It.Is<WriteTranslationTaskSettingDal>(
                    dal => dal.LanguageId == input.LanguageId &&
                           dal.CountOfWordsUsed == input.CountOfWordsUsed
                )), Times.Once);

            // Assert that the first record was changed
            Assert.AreEqual(1, record1.Id);
            Assert.AreEqual(input.LanguageId, record1.LanguageId);
            Assert.AreEqual(input.CountOfWordsUsed, record1.CountOfWordsUsed);

            // Assert that the other records was not changed
            Assert.AreEqual(2, record2.Id);
            Assert.AreEqual(2, record2.LanguageId);
            Assert.AreEqual(4, record2.CountOfWordsUsed);
        }

        [TestMethod]
        public void SetSettings_ShouldCheckLanguageId()
        {
            ShouldCheckLanguageId((service, settings) => service.SetSettings(settings));
        }

        [TestMethod]
        public void SetSettings_ShouldCheckCountOfWordsUsed()
        {
            ShouldCheckCountOfWordsUsed((service, settings) => service.SetSettings(settings));
        }
    }
}
