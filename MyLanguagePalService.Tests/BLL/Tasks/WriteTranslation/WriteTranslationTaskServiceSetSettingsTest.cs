using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyLanguagePalService.BLL.Tasks.Quiz;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;
using MyLanguagePalService.DAL.Models;
using Newtonsoft.Json;

namespace MyLanguagePalService.Tests.BLL.Tasks.WriteTranslation
{
    [TestClass]
    public class WriteTranslationTaskServiceSetSettingsTest : WriteTranslationTaskTestBase
    {
        [TestMethod]
        public void SetSettings_ShouldCheckLanguageId()
        {
            ShouldCheckLanguageId((service, settings) => service.SetSettings(settings));
        }

        [TestMethod]
        public void SetSettings_ShouldCheckCountOfWordsUsed()
        {
            ShouldCheckCountOfWordsUsed(
                (service, settings) => service.SetSettings(settings),
                WriteTranslationTaskService.MinCountOfWordsUsed,
                WriteTranslationTaskService.MaxCountOfWordsUsed);
        }

        [TestMethod]
        public void SetSettings_ShouldAddSettingsInDatabase()
        {
            /* Arrange */
            var taskSettingsDbSetMock = CreateTaskSettingsMockDbSet();

            var languageServiceMock = GetLanguageServiceStub();

            /* Act */
            var service = CreateService(languagesService: languageServiceMock.Object);
            var input = new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = 3
            };
            service.SetSettings(input);

            /* Assert */
            languageServiceMock.Verify(m => m.CheckIfLanguageExists(input.LanguageId), Times.Once);
            taskSettingsDbSetMock.Verify(
                m => m.Add(It.Is<TaskSettingsDal>(
                    dal => dal.TaskId == WriteTranslationTaskService.WriteTranslationTaskId &&
                           JsonConvert.DeserializeObject<QuizTaskSettings>(dal.SettingsJson).LanguageId == input.LanguageId &&
                           JsonConvert.DeserializeObject<QuizTaskSettings>(dal.SettingsJson).CountOfWordsUsed == input.CountOfWordsUsed
                )), Times.Once);
        }

        [TestMethod]
        public void SetSettings_ShouldEditSettingsInDatabase_ShouldEditFirstRecord()
        {
            /* Arrange */
            var settings = new List<QuizTaskSettings>()
            {
                new QuizTaskSettings()
                {
                    LanguageId = 1,
                    CountOfWordsUsed = 15
                },
                new QuizTaskSettings()
                {
                    LanguageId = 2,
                    CountOfWordsUsed = 25
                },
                new QuizTaskSettings()
                {
                    LanguageId = 2,
                    CountOfWordsUsed = 50
                }
            };

            var settingsInDb = new List<TaskSettingsDal>()
            {
                new TaskSettingsDal()
                {
                    Id = 1,
                    TaskId = 3,
                    SettingsJson = JsonConvert.SerializeObject(settings[0])
                },
                new TaskSettingsDal()
                {
                    Id = 2,
                    TaskId = WriteTranslationTaskService.WriteTranslationTaskId,
                    SettingsJson = JsonConvert.SerializeObject(settings[1])
                },
                new TaskSettingsDal()
                {
                    Id = 3,
                    TaskId = 2,
                    SettingsJson = JsonConvert.SerializeObject(settings[2])
                }
            };

            var taskSettingsDbSetMock = CreateTaskSettingsMockDbSet(settingsInDb);

            var languageServiceMock = GetLanguageServiceStub();

            /* Act */
            var service = CreateService(languagesService: languageServiceMock.Object);
            var input = new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = 10
            };
            service.SetSettings(input);

            /* Assert */
            languageServiceMock.Verify(m => m.CheckIfLanguageExists(input.LanguageId), Times.Once);

            // Assert that no records was added
            taskSettingsDbSetMock.Verify(m => m.Add(It.IsAny<TaskSettingsDal>()), Times.Never);

            // Assert that only one record was marked as modified
            DbMock.Verify(m => m.MarkModified(It.IsAny<TaskSettingsDal>()), Times.Once);
            DbMock.Verify(m => m.MarkModified(It.Is<TaskSettingsDal>(
                    dal => dal == settingsInDb[1]
                )), Times.Once);

            // Assert that the target record was changed
            Assert.AreEqual(2, settingsInDb[1].Id);
            Assert.AreEqual(WriteTranslationTaskService.WriteTranslationTaskId, settingsInDb[1].TaskId);
            Assert.IsNotNull(settingsInDb[1].SettingsJson);

            var actual = JsonConvert.DeserializeObject<QuizTaskSettings>(settingsInDb[1].SettingsJson);

            Assert.AreEqual(input.LanguageId, actual.LanguageId);
            Assert.AreEqual(input.CountOfWordsUsed, actual.CountOfWordsUsed);

            // Assert that the other records was not changed
            Assert.AreEqual(1, settingsInDb[0].Id);
            Assert.AreEqual(3, settingsInDb[0].TaskId);
            Assert.IsNotNull(settingsInDb[0].SettingsJson);

            var actualSettings1 = JsonConvert.DeserializeObject<QuizTaskSettings>(settingsInDb[0].SettingsJson);

            Assert.AreEqual(settings[0].LanguageId, actualSettings1.LanguageId);
            Assert.AreEqual(settings[0].CountOfWordsUsed, actualSettings1.CountOfWordsUsed);


            Assert.AreEqual(3, settingsInDb[2].Id);
            Assert.AreEqual(2, settingsInDb[2].TaskId);
            Assert.IsNotNull(settingsInDb[2].SettingsJson);

            var actualSettings3 = JsonConvert.DeserializeObject<QuizTaskSettings>(settingsInDb[2].SettingsJson);

            Assert.AreEqual(settings[2].LanguageId, actualSettings3.LanguageId);
            Assert.AreEqual(settings[2].CountOfWordsUsed, actualSettings3.CountOfWordsUsed);
        }
    }
}
