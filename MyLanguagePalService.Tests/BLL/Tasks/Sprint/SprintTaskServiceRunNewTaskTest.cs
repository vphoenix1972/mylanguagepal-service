using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.Tests.BLL.Tasks.Sprint
{
    [TestClass]
    public class SprintTaskServiceRunNewTaskTest : SprintTaskTestBase
    {
        [TestMethod]
        public void RunNewTask_ShouldCheckLanguageId()
        {
            ShouldCheckLanguageId((service, settings) => service.SetSettings(settings));
        }

        [TestMethod]
        public void RunNewTask_ShouldCheckTotalTimeForTask()
        {
            ShouldCheckTotalTimeForTask((service, settings) => service.SetSettings(settings));
        }

        [TestMethod]
        public void RunNewTask_ShouldCheckCountOfWordsUsed()
        {
            ShouldCheckCountOfWordsUsed((service, settings) => service.SetSettings(settings));
        }

        [TestMethod]
        public void RunNewTask_ShouldReturnWordsOrderedByDelta()
        {
            /* Arrange */
            var date = DateTime.UtcNow;
            var records = new List<SprintTaskJournalRecordDal>()
            {
                new SprintTaskJournalRecordDal() {Id = 1, PhraseId = 1, LastRepetitonTime = date, CorrectWrongAnswersDelta = 2},
                new SprintTaskJournalRecordDal() {Id = 2, PhraseId = 2, LastRepetitonTime = date, CorrectWrongAnswersDelta = 1},
                new SprintTaskJournalRecordDal() {Id = 3, PhraseId = 3, LastRepetitonTime = date, CorrectWrongAnswersDelta = -1},
                new SprintTaskJournalRecordDal() {Id = 4, PhraseId = 4, LastRepetitonTime = date, CorrectWrongAnswersDelta = 0},
                new SprintTaskJournalRecordDal() {Id = 5, PhraseId = 5, LastRepetitonTime = date, CorrectWrongAnswersDelta = -2}
            };

            CreateSprintTaskJournalRecordsMockDbSet(records);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {Id = 1, LanguageId = 1, Text = "Phrase 1", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[0] } },
                new PhraseDal() {Id = 2, LanguageId = 1, Text = "Phrase 2", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[1] } },
                new PhraseDal() {Id = 3, LanguageId = 1, Text = "Phrase 3", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[2] } },
                new PhraseDal() {Id = 4, LanguageId = 1, Text = "Phrase 4", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[3] } },
                new PhraseDal() {Id = 5, LanguageId = 1, Text = "Phrase 5", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[4] } }
            };

            CreatePhrasesMockDbSet(phrases);

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, Db);
            var settings = new SprintTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = SprintTaskService.MaxCountOfWordsUsed,
                TotalTimeForTask = SprintTaskService.MinTotalTimeForTask
            };
            var actual = service.RunNewTask(settings);

            /* Assert */
            AssertSprintTaskRunModelContract(actual);
            Assert.AreEqual(phrases.Count, actual.Phrases.Count);

            Assert.AreEqual(phrases[0].Id, actual.Phrases[4].Id);
            Assert.AreEqual(phrases[1].Id, actual.Phrases[3].Id);
            Assert.AreEqual(phrases[2].Id, actual.Phrases[1].Id);
            Assert.AreEqual(phrases[3].Id, actual.Phrases[2].Id);
            Assert.AreEqual(phrases[4].Id, actual.Phrases[0].Id);
        }

        [TestMethod]
        public void RunNewTask_ShouldIncludeWordsWithoutRecords()
        {
            /* Arrange */
            var date = DateTime.UtcNow;
            var records = new List<SprintTaskJournalRecordDal>()
            {
                new SprintTaskJournalRecordDal() {Id = 1, PhraseId = 1, LastRepetitonTime = date, CorrectWrongAnswersDelta = 2},
                new SprintTaskJournalRecordDal() {Id = 2, PhraseId = 2, LastRepetitonTime = date, CorrectWrongAnswersDelta = 1},
                new SprintTaskJournalRecordDal() {Id = 3, PhraseId = 3, LastRepetitonTime = date, CorrectWrongAnswersDelta = -1},
                new SprintTaskJournalRecordDal() {Id = 5, PhraseId = 5, LastRepetitonTime = date, CorrectWrongAnswersDelta = -2}
            };

            CreateSprintTaskJournalRecordsMockDbSet(records);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {Id = 1, LanguageId = 1, Text = "Phrase 1", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[0] } },
                new PhraseDal() {Id = 2, LanguageId = 1, Text = "Phrase 2", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[1] } },
                new PhraseDal() {Id = 3, LanguageId = 1, Text = "Phrase 3", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[2] } },
                new PhraseDal() {Id = 4, LanguageId = 1, Text = "Phrase 4", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() },
                new PhraseDal() {Id = 5, LanguageId = 1, Text = "Phrase 5", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[3] } }
            };

            CreatePhrasesMockDbSet(phrases);

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, Db);
            var settings = new SprintTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = SprintTaskService.MaxCountOfWordsUsed,
                TotalTimeForTask = SprintTaskService.MinTotalTimeForTask
            };
            var actual = service.RunNewTask(settings);

            /* Assert */
            AssertSprintTaskRunModelContract(actual);
            Assert.AreEqual(phrases.Count, actual.Phrases.Count);

            Assert.AreEqual(phrases[0].Id, actual.Phrases[4].Id);
            Assert.AreEqual(phrases[1].Id, actual.Phrases[3].Id);
            Assert.AreEqual(phrases[2].Id, actual.Phrases[1].Id);
            Assert.AreEqual(phrases[3].Id, actual.Phrases[2].Id);
            Assert.AreEqual(phrases[4].Id, actual.Phrases[0].Id);
        }

        [TestMethod]
        public void RunNewTask_ShouldRespectLanguageId()
        {
            /* Arrange */
            var date = DateTime.UtcNow;
            var records = new List<SprintTaskJournalRecordDal>()
            {
                new SprintTaskJournalRecordDal() {Id = 1, PhraseId = 1, LastRepetitonTime = date, CorrectWrongAnswersDelta = 2},
                new SprintTaskJournalRecordDal() {Id = 2, PhraseId = 2, LastRepetitonTime = date, CorrectWrongAnswersDelta = 1},
                new SprintTaskJournalRecordDal() {Id = 3, PhraseId = 3, LastRepetitonTime = date, CorrectWrongAnswersDelta = 0},
                new SprintTaskJournalRecordDal() {Id = 4, PhraseId = 4, LastRepetitonTime = date, CorrectWrongAnswersDelta = -1},
                new SprintTaskJournalRecordDal() {Id = 5, PhraseId = 5, LastRepetitonTime = date, CorrectWrongAnswersDelta = -2}
            };

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {Id = 1, LanguageId = 1, Text = "Phrase 1", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[0] } },
                new PhraseDal() {Id = 2, LanguageId = 1, Text = "Phrase 2", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[1] } },
                new PhraseDal() {Id = 3, LanguageId = 2, Text = "Phrase 3", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[2] } },
                new PhraseDal() {Id = 4, LanguageId = 2, Text = "Phrase 4", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[3] }},
                new PhraseDal() {Id = 5, LanguageId = 2, Text = "Phrase 5", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[4] } }
            };

            CreatePhrasesMockDbSet(phrases);

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, Db);
            var settings = new SprintTaskSettings()
            {
                LanguageId = 2,
                CountOfWordsUsed = SprintTaskService.MaxCountOfWordsUsed,
                TotalTimeForTask = SprintTaskService.MinTotalTimeForTask
            };
            var actual = service.RunNewTask(settings);

            /* Assert */
            AssertSprintTaskRunModelContract(actual);
            Assert.AreEqual(3, actual.Phrases.Count);

            Assert.AreEqual(phrases[2].Id, actual.Phrases[2].Id);
            Assert.AreEqual(phrases[3].Id, actual.Phrases[1].Id);
            Assert.AreEqual(phrases[4].Id, actual.Phrases[0].Id);
        }

        [TestMethod]
        public void RunNewTask_ShouldRespectLimit()
        {
            /* Arrange */
            var date = DateTime.UtcNow;
            var records = new List<SprintTaskJournalRecordDal>()
            {
                new SprintTaskJournalRecordDal() {Id = 1, PhraseId = 1, LastRepetitonTime = date, CorrectWrongAnswersDelta = 2},
                new SprintTaskJournalRecordDal() {Id = 2, PhraseId = 2, LastRepetitonTime = date, CorrectWrongAnswersDelta = 1},
                new SprintTaskJournalRecordDal() {Id = 3, PhraseId = 3, LastRepetitonTime = date, CorrectWrongAnswersDelta = 0},
                new SprintTaskJournalRecordDal() {Id = 4, PhraseId = 4, LastRepetitonTime = date, CorrectWrongAnswersDelta = -1},
                new SprintTaskJournalRecordDal() {Id = 5, PhraseId = 5, LastRepetitonTime = date, CorrectWrongAnswersDelta = -2}
            };

            CreateSprintTaskJournalRecordsMockDbSet(records);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {Id = 1, LanguageId = 1, Text = "Phrase 1", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[0] } },
                new PhraseDal() {Id = 2, LanguageId = 1, Text = "Phrase 2", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[1] } },
                new PhraseDal() {Id = 3, LanguageId = 1, Text = "Phrase 3", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[2] } },
                new PhraseDal() {Id = 4, LanguageId = 1, Text = "Phrase 4", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[3] } },
                new PhraseDal() {Id = 5, LanguageId = 1, Text = "Phrase 5", SprintTaskJournalRecords = new List<SprintTaskJournalRecordDal>() { records[4] } }
            };

            CreatePhrasesMockDbSet(phrases);

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, Db);
            var settings = new SprintTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = 3,
                TotalTimeForTask = SprintTaskService.MinTotalTimeForTask
            };
            var actual = service.RunNewTask(settings);

            /* Assert */
            AssertSprintTaskRunModelContract(actual);
            Assert.AreEqual(3, actual.Phrases.Count);

            Assert.AreEqual(phrases[2].Id, actual.Phrases[2].Id);
            Assert.AreEqual(phrases[3].Id, actual.Phrases[1].Id);
            Assert.AreEqual(phrases[4].Id, actual.Phrases[0].Id);
        }

        [TestMethod]
        public void RunNewTask_ShouldReturnEmptyListIfNoPhrases()
        {
            /* Arrange */
            var records = new List<SprintTaskJournalRecordDal>();
            CreateSprintTaskJournalRecordsMockDbSet(records);

            var phrases = new List<PhraseDal>();
            CreatePhrasesMockDbSet(phrases);

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, languagesService, Db);
            var settings = new SprintTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = SprintTaskService.MaxCountOfWordsUsed,
                TotalTimeForTask = SprintTaskService.MinTotalTimeForTask
            };
            var actual = service.RunNewTask(settings);

            /* Assert */
            AssertSprintTaskRunModelContract(actual);
            Assert.AreEqual(0, actual.Phrases.Count);
        }

        // ReSharper disable once UnusedParameter.Local
        private void AssertSprintTaskRunModelContract(SprintTaskRunModel actual)
        {
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Phrases);
        }
    }
}
