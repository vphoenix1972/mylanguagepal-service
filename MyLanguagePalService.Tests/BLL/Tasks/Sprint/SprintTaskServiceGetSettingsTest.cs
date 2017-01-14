using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.DAL.Models;
using Newtonsoft.Json;

namespace MyLanguagePalService.Tests.BLL.Tasks.Sprint
{
    [TestClass]
    public class SprintTaskServiceGetSettingsTest : SprintTaskTestBase
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
                    TaskId = SprintTaskService.SprintTaskId,
                    SettingsJson = "{\"languageId\": 1, \"countOfWordsUsed\": 15, \"totalTimeForTask\": 30}"
                }
            };
            var expected = JsonConvert.DeserializeObject<SprintTaskSettings>(settingsInDb[0].SettingsJson);

            CreateTaskSettingsMockDbSet(settingsInDb);

            /* Act */
            var service = CreateService();
            var actualObject = service.GetSettings();

            /* Assert */
            Assert.IsNotNull(actualObject);
            Assert.IsTrue(actualObject is SprintTaskSettings);

            var actual = actualObject as SprintTaskSettings;
            Assert.AreEqual(expected.LanguageId, actual.LanguageId);
            Assert.AreEqual(expected.CountOfWordsUsed, actual.CountOfWordsUsed);
            Assert.AreEqual(expected.TotalTimeForTask, actual.TotalTimeForTask);
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
                    TaskId = 98,
                    SettingsJson = "{\"languageId\": 2, \"countOfWordsUsed\": 25}"
                },
                new TaskSettingsDal()
                {
                    Id = 1,
                    TaskId = SprintTaskService.SprintTaskId,
                    SettingsJson = "{\"languageId\": 1, \"countOfWordsUsed\": 15, \"totalTimeForTask\": 30}"
                },
                new TaskSettingsDal()
                {
                    Id = 1,
                    TaskId = 99,
                    SettingsJson = "{\"languageId\": 2, \"countOfWordsUsed\": 45}"
                }
            };
            var expected = JsonConvert.DeserializeObject<SprintTaskSettings>(settingsInDb[1].SettingsJson);

            CreateTaskSettingsMockDbSet(settingsInDb);

            /* Act */
            var service = CreateService();
            var actualObject = service.GetSettings();

            /* Assert */
            Assert.IsNotNull(actualObject);
            Assert.IsTrue(actualObject is SprintTaskSettings);

            var actual = actualObject as SprintTaskSettings;
            Assert.AreEqual(expected.LanguageId, actual.LanguageId);
            Assert.AreEqual(expected.CountOfWordsUsed, actual.CountOfWordsUsed);
            Assert.AreEqual(expected.TotalTimeForTask, actual.TotalTimeForTask);
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
            Assert.IsTrue(actualObject is SprintTaskSettings);

            var actual = actualObject as SprintTaskSettings;
            Assert.AreEqual(1, actual.LanguageId);
            Assert.AreEqual(SprintTaskService.DefaultCountOfWordsUsed, actual.CountOfWordsUsed);
            Assert.AreEqual(SprintTaskService.DefaultTotalTimeForTask, actual.TotalTimeForTask);
        }
    }
}
