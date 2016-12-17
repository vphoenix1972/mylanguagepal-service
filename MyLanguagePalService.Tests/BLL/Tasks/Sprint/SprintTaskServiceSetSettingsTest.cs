using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestClass]
    public class SprintTaskServiceSetSettingsTest
    {
        [TestMethod]
        public void SetSettings_ShouldAddSettingsInDatabase()
        {
            /* Arrange */
            var mockContext = new Mock<IApplicationDbContext>();

            var settings = new List<SprintTaskSettingDal>().AsQueryable();

            var sprintTaskSettingsDbSet = TestUtils.CreateMockDbSet(settings);

            mockContext.Setup(x => x.SprintTaskSettings)
                .Returns(sprintTaskSettingsDbSet.Object);

            var db = mockContext.Object;

            var phrasesService = TestUtils.GetStub<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceMock();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, db);
            var input = new SprintTaskSettingModel()
            {
                LanguageId = 1,
                CountOfWordsUsed = 3,
                TotalTimeForTask = 6
            };
            service.SetSettings(input);

            /* Assert */
            languageServiceMock.Verify(m => m.CheckIfLanguageExists(input.LanguageId), Times.Once);
            sprintTaskSettingsDbSet.Verify(
                m => m.Add(It.Is<SprintTaskSettingDal>(
                    dal => dal.LanguageId == input.LanguageId &&
                           dal.CountOfWordsUsed == input.CountOfWordsUsed &&
                           dal.TotalTimeForTask == input.TotalTimeForTask
                )), Times.Once);
        }

        [TestMethod]
        public void SetSettings_ShouldEditSettingsInDatabase_ShouldEditFirstRecord()
        {
            /* Arrange */
            var mockDb = new Mock<IApplicationDbContext>();

            var record1 = new SprintTaskSettingDal()
            {
                Id = 1,
                LanguageId = 1,
                CountOfWordsUsed = 3,
                TotalTimeForTask = 6
            };

            var record2 = new SprintTaskSettingDal()
            {
                Id = 2,
                LanguageId = 2,
                CountOfWordsUsed = 4,
                TotalTimeForTask = 7
            };

            var settings = new List<SprintTaskSettingDal>()
            {
                record1,
                record2
            }.AsQueryable();

            var sprintTaskSettingsDbSet = TestUtils.CreateMockDbSet(settings);

            mockDb.Setup(x => x.SprintTaskSettings)
                .Returns(sprintTaskSettingsDbSet.Object);

            var db = mockDb.Object;

            var phrasesService = TestUtils.GetStub<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceMock();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, db);
            var input = new SprintTaskSettingModel()
            {
                LanguageId = 2,
                CountOfWordsUsed = 40,
                TotalTimeForTask = 60
            };
            service.SetSettings(input);

            /* Assert */
            languageServiceMock.Verify(m => m.CheckIfLanguageExists(input.LanguageId), Times.Once);

            // Assert that no records was added
            sprintTaskSettingsDbSet.Verify(m => m.Add(It.IsAny<SprintTaskSettingDal>()), Times.Never);

            // Assert that only one record was marked as modified
            mockDb.Verify(m => m.MarkModified(It.IsAny<SprintTaskSettingDal>()), Times.Once);
            mockDb.Verify(m => m.MarkModified(It.Is<SprintTaskSettingDal>(
                    dal => dal.LanguageId == input.LanguageId &&
                           dal.CountOfWordsUsed == input.CountOfWordsUsed &&
                           dal.TotalTimeForTask == input.TotalTimeForTask
                )), Times.Once);

            // Assert that the first record was changed
            Assert.AreEqual(1, record1.Id);
            Assert.AreEqual(input.LanguageId, record1.LanguageId);
            Assert.AreEqual(input.CountOfWordsUsed, record1.CountOfWordsUsed);
            Assert.AreEqual(input.TotalTimeForTask, record1.TotalTimeForTask);

            // Assert that the other records was not changed
            Assert.AreEqual(2, record2.Id);
            Assert.AreEqual(2, record2.LanguageId);
            Assert.AreEqual(4, record2.CountOfWordsUsed);
            Assert.AreEqual(7, record2.TotalTimeForTask);
        }

        [TestMethod]
        public void SetSettings_ShouldCheckLanguageId()
        {
            /* Arrange */
            var mockDb = new Mock<IApplicationDbContext>();

            var settings = new List<SprintTaskSettingDal>().AsQueryable();

            var sprintTaskSettingsDbSet = TestUtils.CreateMockDbSet(settings);

            mockDb.Setup(x => x.SprintTaskSettings)
                .Returns(sprintTaskSettingsDbSet.Object);

            var db = mockDb.Object;

            var phrasesService = TestUtils.GetStub<IPhrasesService>();

            var languageServiceMock = new Mock<ILanguagesService>();
            languageServiceMock.Setup(m => m.CheckIfLanguageExists(It.IsAny<int>())).Returns(false); ;
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, db);
            var input = new SprintTaskSettingModel()
            {
                LanguageId = 3,
                CountOfWordsUsed = 40,
                TotalTimeForTask = 60
            };

            ValidationFailedException vfeCaught = null;
            try
            {
                service.SetSettings(input);
            }
            catch (ValidationFailedException vfe)
            {
                vfeCaught = vfe;
            }

            /* Assert */
            languageServiceMock.Verify(m => m.CheckIfLanguageExists(input.LanguageId), Times.Once);
            TestUtils.AssertValidationFailedException(vfeCaught, nameof(SprintTaskSettingModel.LanguageId));
        }

        [TestMethod]
        public void SetSettings_ShouldCheckTotalTimeForTask()
        {
            /* Arrange */
            var mockDb = new Mock<IApplicationDbContext>();

            var settings = new List<SprintTaskSettingDal>().AsQueryable();

            var sprintTaskSettingsDbSet = TestUtils.CreateMockDbSet(settings);

            mockDb.Setup(x => x.SprintTaskSettings)
                .Returns(sprintTaskSettingsDbSet.Object);

            var db = mockDb.Object;

            var phrasesService = TestUtils.GetStub<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceMock();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, db);
            var input = new SprintTaskSettingModel()
            {
                LanguageId = 1,
                CountOfWordsUsed = 40,
                TotalTimeForTask = 1
            };

            ValidationFailedException vfeCaught = null;
            try
            {
                service.SetSettings(input);
            }
            catch (ValidationFailedException vfe)
            {
                vfeCaught = vfe;
            }

            /* Assert */
            TestUtils.AssertValidationFailedException(vfeCaught, nameof(SprintTaskSettingModel.TotalTimeForTask));
        }

        [TestMethod]
        public void SetSettings_ShouldCheckCountOfWordsUsed()
        {
            /* Arrange */
            var mockDb = new Mock<IApplicationDbContext>();

            var settings = new List<SprintTaskSettingDal>().AsQueryable();

            var sprintTaskSettingsDbSet = TestUtils.CreateMockDbSet(settings);

            mockDb.Setup(x => x.SprintTaskSettings)
                .Returns(sprintTaskSettingsDbSet.Object);

            var db = mockDb.Object;

            var phrasesService = TestUtils.GetStub<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceMock();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, db);
            var input = new SprintTaskSettingModel()
            {
                LanguageId = 1,
                CountOfWordsUsed = 0,
                TotalTimeForTask = 10
            };

            ValidationFailedException vfeCaught = null;
            try
            {
                service.SetSettings(input);
            }
            catch (ValidationFailedException vfe)
            {
                vfeCaught = vfe;
            }

            /* Assert */
            TestUtils.AssertValidationFailedException(vfeCaught, nameof(SprintTaskSettingModel.CountOfWordsUsed));

            /* Act */
            input = new SprintTaskSettingModel()
            {
                LanguageId = 1,
                CountOfWordsUsed = 1001,
                TotalTimeForTask = 10
            };

            vfeCaught = null;
            try
            {
                service.SetSettings(input);
            }
            catch (ValidationFailedException vfe)
            {
                vfeCaught = vfe;
            }

            /* Assert */
            TestUtils.AssertValidationFailedException(vfeCaught, nameof(SprintTaskSettingModel.CountOfWordsUsed));
        }

        private static Mock<ILanguagesService> GetLanguageServiceMock()
        {
            var languageServiceMock = new Mock<ILanguagesService>();
            languageServiceMock.Setup(m => m.CheckIfLanguageExists(It.IsAny<int>())).Returns(true);
            return languageServiceMock;
        }
    }
}
