using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLanguagePalService.BLL.Tasks.Quiz;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;
using MyLanguagePalService.DAL.Models;
using Newtonsoft.Json;

namespace MyLanguagePalService.Tests.BLL.Tasks.WriteTranslation
{
    [TestClass]
    public class WriteTranslationTaskServiceGetSettingsTest : WriteTranslationTaskTestBase
    {
        [TestMethod]
        public void GetSettings_ShouldReturnSettingsFromDb()
        {
            /* Arrange */
            var settingsInDb = new List<TaskSettingsDal>()
            {
                new TaskSettingsDal()
                {
                    Id = 1,
                    TaskId = WriteTranslationTaskService.WriteTranslationTaskId,
                    SettingsJson = "{\"languageId\": 1, \"countOfWordsUsed\": 15}"
                }
            };
            var expected = JsonConvert.DeserializeObject<QuizTaskSettings>(settingsInDb[0].SettingsJson);

            CreateTaskSettingsMockDbSet(settingsInDb);

            /* Act */
            var service = CreateService();
            var actualObject = service.GetSettings();

            /* Assert */
            Assert.IsNotNull(actualObject);
            Assert.IsTrue(actualObject is QuizTaskSettings);

            var actual = actualObject as QuizTaskSettings;
            Assert.AreEqual(expected.LanguageId, actual.LanguageId);
            Assert.AreEqual(expected.CountOfWordsUsed, actual.CountOfWordsUsed);
        }

        [TestMethod]
        public void GetSettings_ShouldRespectTaskId()
        {
            /* Arrange */
            var settingsInDb = new List<TaskSettingsDal>()
            {
                new TaskSettingsDal()
                {
                    Id = 1,
                    TaskId = 3,
                    SettingsJson = "{\"languageId\": 2, \"countOfWordsUsed\": 25}"
                },
                new TaskSettingsDal()
                {
                    Id = 1,
                    TaskId = WriteTranslationTaskService.WriteTranslationTaskId,
                    SettingsJson = "{\"languageId\": 1, \"countOfWordsUsed\": 15}"
                },
                new TaskSettingsDal()
                {
                    Id = 1,
                    TaskId = 4,
                    SettingsJson = "{\"languageId\": 2, \"countOfWordsUsed\": 45}"
                }
            };
            var expected = JsonConvert.DeserializeObject<QuizTaskSettings>(settingsInDb[1].SettingsJson);

            CreateTaskSettingsMockDbSet(settingsInDb);

            /* Act */
            var service = CreateService();
            var actualObject = service.GetSettings();

            /* Assert */
            Assert.IsNotNull(actualObject);
            Assert.IsTrue(actualObject is QuizTaskSettings);

            var actual = actualObject as QuizTaskSettings;
            Assert.AreEqual(expected.LanguageId, actual.LanguageId);
            Assert.AreEqual(expected.CountOfWordsUsed, actual.CountOfWordsUsed);
        }

        [TestMethod]
        public void GetSettings_ShouldReturnDefaultSettingsIfNoRowsInDb()
        {
            /* Arrange */


            /* Act */
            var service = CreateService();
            var actualObject = service.GetSettings();

            /* Assert */
            Assert.IsNotNull(actualObject);
            Assert.IsTrue(actualObject is QuizTaskSettings);

            var actual = actualObject as QuizTaskSettings;
            Assert.AreEqual(1, actual.LanguageId);
            Assert.AreEqual(WriteTranslationTaskService.DefaultCountOfWordsUsed, actual.CountOfWordsUsed);
        }
    }
}
