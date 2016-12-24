using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;
using MyLanguagePalService.DAL.Models;
using MyLanguagePalService.Tests.BLL.Tasks.WriteTranslation;

namespace MyLanguagePalService.Tests.BLL.Tasks.WriteTranslation
{
    [TestClass]
    public class WriteTranslationTaskServiceRunNewTaskTest : WriteTranslationTaskTestBase
    {
        [TestMethod]
        public void RunNewTask_ShouldCheckLanguageId()
        {
            ShouldCheckLanguageId((service, settings) => service.SetSettings(settings));
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
            var records = new List<WriteTranslationTaskJournalRecordDal>()
            {
                new WriteTranslationTaskJournalRecordDal() {Id = 1, PhraseId = 1, LastRepetitonTime = date, CorrectWrongAnswersDelta = 2},
                new WriteTranslationTaskJournalRecordDal() {Id = 2, PhraseId = 2, LastRepetitonTime = date, CorrectWrongAnswersDelta = 1},
                new WriteTranslationTaskJournalRecordDal() {Id = 3, PhraseId = 3, LastRepetitonTime = date, CorrectWrongAnswersDelta = -1},
                new WriteTranslationTaskJournalRecordDal() {Id = 4, PhraseId = 4, LastRepetitonTime = date, CorrectWrongAnswersDelta = 0},
                new WriteTranslationTaskJournalRecordDal() {Id = 5, PhraseId = 5, LastRepetitonTime = date, CorrectWrongAnswersDelta = -2}
            };

            CreateWriteTranslationTaskJournalRecordsMockDbSet(records);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {Id = 1, LanguageId = 1, Text = "Phrase 1", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[0] } },
                new PhraseDal() {Id = 2, LanguageId = 1, Text = "Phrase 2", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[1] } },
                new PhraseDal() {Id = 3, LanguageId = 1, Text = "Phrase 3", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[2] } },
                new PhraseDal() {Id = 4, LanguageId = 1, Text = "Phrase 4", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[3] } },
                new PhraseDal() {Id = 5, LanguageId = 1, Text = "Phrase 5", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[4] } }
            };

            CreatePhrasesMockDbSet(phrases);

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new WriteTranslationTaskService(phrasesService, languagesService, Db);
            var settings = new WriteTranslationTaskSettingModel()
            {
                LanguageId = 1,
                CountOfWordsUsed = WriteTranslationTaskService.MaxCountOfWordsUsed
            };
            var actual = service.RunNewTask(settings);

            /* Assert */
            AssertWriteTranslationTaskRunModelContract(actual);
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
            var records = new List<WriteTranslationTaskJournalRecordDal>()
            {
                new WriteTranslationTaskJournalRecordDal() {Id = 1, PhraseId = 1, LastRepetitonTime = date, CorrectWrongAnswersDelta = 2},
                new WriteTranslationTaskJournalRecordDal() {Id = 2, PhraseId = 2, LastRepetitonTime = date, CorrectWrongAnswersDelta = 1},
                new WriteTranslationTaskJournalRecordDal() {Id = 3, PhraseId = 3, LastRepetitonTime = date, CorrectWrongAnswersDelta = -1},
                new WriteTranslationTaskJournalRecordDal() {Id = 5, PhraseId = 5, LastRepetitonTime = date, CorrectWrongAnswersDelta = -2}
            };

            CreateWriteTranslationTaskJournalRecordsMockDbSet(records);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {Id = 1, LanguageId = 1, Text = "Phrase 1", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[0] } },
                new PhraseDal() {Id = 2, LanguageId = 1, Text = "Phrase 2", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[1] } },
                new PhraseDal() {Id = 3, LanguageId = 1, Text = "Phrase 3", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[2] } },
                new PhraseDal() {Id = 4, LanguageId = 1, Text = "Phrase 4", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() },
                new PhraseDal() {Id = 5, LanguageId = 1, Text = "Phrase 5", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[3] } }
            };

            CreatePhrasesMockDbSet(phrases);

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new WriteTranslationTaskService(phrasesService, languagesService, Db);
            var settings = new WriteTranslationTaskSettingModel()
            {
                LanguageId = 1,
                CountOfWordsUsed = WriteTranslationTaskService.MaxCountOfWordsUsed
            };
            var actual = service.RunNewTask(settings);

            /* Assert */
            AssertWriteTranslationTaskRunModelContract(actual);
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
            var records = new List<WriteTranslationTaskJournalRecordDal>()
            {
                new WriteTranslationTaskJournalRecordDal() {Id = 1, PhraseId = 1, LastRepetitonTime = date, CorrectWrongAnswersDelta = 2},
                new WriteTranslationTaskJournalRecordDal() {Id = 2, PhraseId = 2, LastRepetitonTime = date, CorrectWrongAnswersDelta = 1},
                new WriteTranslationTaskJournalRecordDal() {Id = 3, PhraseId = 3, LastRepetitonTime = date, CorrectWrongAnswersDelta = 0},
                new WriteTranslationTaskJournalRecordDal() {Id = 4, PhraseId = 4, LastRepetitonTime = date, CorrectWrongAnswersDelta = -1},
                new WriteTranslationTaskJournalRecordDal() {Id = 5, PhraseId = 5, LastRepetitonTime = date, CorrectWrongAnswersDelta = -2}
            };

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {Id = 1, LanguageId = 1, Text = "Phrase 1", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[0] } },
                new PhraseDal() {Id = 2, LanguageId = 1, Text = "Phrase 2", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[1] } },
                new PhraseDal() {Id = 3, LanguageId = 2, Text = "Phrase 3", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[2] } },
                new PhraseDal() {Id = 4, LanguageId = 2, Text = "Phrase 4", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[3] }},
                new PhraseDal() {Id = 5, LanguageId = 2, Text = "Phrase 5", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[4] } }
            };

            CreatePhrasesMockDbSet(phrases);

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new WriteTranslationTaskService(phrasesService, languagesService, Db);
            var settings = new WriteTranslationTaskSettingModel()
            {
                LanguageId = 2,
                CountOfWordsUsed = WriteTranslationTaskService.MaxCountOfWordsUsed
            };
            var actual = service.RunNewTask(settings);

            /* Assert */
            AssertWriteTranslationTaskRunModelContract(actual);
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
            var records = new List<WriteTranslationTaskJournalRecordDal>()
            {
                new WriteTranslationTaskJournalRecordDal() {Id = 1, PhraseId = 1, LastRepetitonTime = date, CorrectWrongAnswersDelta = 2},
                new WriteTranslationTaskJournalRecordDal() {Id = 2, PhraseId = 2, LastRepetitonTime = date, CorrectWrongAnswersDelta = 1},
                new WriteTranslationTaskJournalRecordDal() {Id = 3, PhraseId = 3, LastRepetitonTime = date, CorrectWrongAnswersDelta = 0},
                new WriteTranslationTaskJournalRecordDal() {Id = 4, PhraseId = 4, LastRepetitonTime = date, CorrectWrongAnswersDelta = -1},
                new WriteTranslationTaskJournalRecordDal() {Id = 5, PhraseId = 5, LastRepetitonTime = date, CorrectWrongAnswersDelta = -2}
            };

            CreateWriteTranslationTaskJournalRecordsMockDbSet(records);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {Id = 1, LanguageId = 1, Text = "Phrase 1", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[0] } },
                new PhraseDal() {Id = 2, LanguageId = 1, Text = "Phrase 2", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[1] } },
                new PhraseDal() {Id = 3, LanguageId = 1, Text = "Phrase 3", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[2] } },
                new PhraseDal() {Id = 4, LanguageId = 1, Text = "Phrase 4", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[3] } },
                new PhraseDal() {Id = 5, LanguageId = 1, Text = "Phrase 5", WriteTranslationTaskJournalRecords = new List<WriteTranslationTaskJournalRecordDal>() { records[4] } }
            };

            CreatePhrasesMockDbSet(phrases);

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new WriteTranslationTaskService(phrasesService, languagesService, Db);
            var settings = new WriteTranslationTaskSettingModel()
            {
                LanguageId = 1,
                CountOfWordsUsed = 3
            };
            var actual = service.RunNewTask(settings);

            /* Assert */
            AssertWriteTranslationTaskRunModelContract(actual);
            Assert.AreEqual(3, actual.Phrases.Count);

            Assert.AreEqual(phrases[2].Id, actual.Phrases[2].Id);
            Assert.AreEqual(phrases[3].Id, actual.Phrases[1].Id);
            Assert.AreEqual(phrases[4].Id, actual.Phrases[0].Id);
        }

        [TestMethod]
        public void RunNewTask_ShouldReturnEmptyListIfNoPhrases()
        {
            /* Arrange */
            var records = new List<WriteTranslationTaskJournalRecordDal>();
            CreateWriteTranslationTaskJournalRecordsMockDbSet(records);

            var phrases = new List<PhraseDal>();
            CreatePhrasesMockDbSet(phrases);

            var phrasesService = GetStubObject<IPhrasesService>();

            var languageServiceMock = GetLanguageServiceStub();
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = new WriteTranslationTaskService(phrasesService, languagesService, Db);
            var settings = new WriteTranslationTaskSettingModel()
            {
                LanguageId = 1,
                CountOfWordsUsed = WriteTranslationTaskService.MaxCountOfWordsUsed
            };
            var actual = service.RunNewTask(settings);

            /* Assert */
            AssertWriteTranslationTaskRunModelContract(actual);
            Assert.AreEqual(0, actual.Phrases.Count);
        }

        // ReSharper disable once UnusedParameter.Local
        private void AssertWriteTranslationTaskRunModelContract(WriteTranslationTaskRunModel actual)
        {
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Phrases);
        }
    }
}
