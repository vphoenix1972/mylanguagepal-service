using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.Tests.BLL.Tasks.Sprint
{
    [TestClass]
    public class SprintTaskServiceFinishTaskTest : SprintTaskTestBase
    {
        [TestMethod]
        public void FinishTask_ShouldThrowIfSummaryIsNull()
        {
            /* Arrange */


            /* Act */
            var service = new SprintTaskService(GetStubObject<IPhrasesService>(), GetLanguageServiceStubObject(), Db);

            /* Assert */

            // We are testing the contract
            // ReSharper disable once AssignNullToNotNullAttribute
            AssertIsArgumentNullExceptionThrown(() => service.FinishTask(null), "summary");
        }

        [TestMethod]
        public void FinishTask_ShouldThrowIfSummaryResultsIsNull()
        {
            /* Arrange */


            /* Act */
            var service = new SprintTaskService(GetStubObject<IPhrasesService>(), GetLanguageServiceStubObject(), Db);
            var summary = new SprintTaskFinishedSummaryModel()
            {
                // For testing ...
                // ReSharper disable once AssignNullToNotNullAttribute
                Results = null
            };

            /* Assert */

            // We are testing the contract
            // ReSharper disable once AssignNullToNotNullAttribute
            AssertIsArgumentNullExceptionThrown(() => service.FinishTask(summary), nameof(SprintTaskFinishedSummaryModel.Results));
        }

        [TestMethod]
        public void FinishTask_ShouldThrowIfOneOfSummaryResultsIsNull()
        {
            /* Arrange */


            /* Act */
            var service = new SprintTaskService(GetStubObject<IPhrasesService>(), GetLanguageServiceStubObject(), Db);
            var summary = new SprintTaskFinishedSummaryModel()
            {
                Results = new List<SprintTaskJournalRecord>() { null }
            };

            /* Assert */

            AssertIsArgumentNullExceptionThrown(() => service.FinishTask(summary), "Results[0]");
        }

        [TestMethod]
        public void FinishTask_ShouldCheckExistanceOfSummaryPhrases()
        {
            /* Arrange */
            var pharasesServiceMock = new Mock<IPhrasesService>();
            pharasesServiceMock.Setup(phrasesService => phrasesService.CheckIfPhraseExists(It.IsAny<int>()))
                .Returns(false);

            /* Act */
            var service = new SprintTaskService(pharasesServiceMock.Object, GetLanguageServiceStubObject(), Db);
            var summary = new SprintTaskFinishedSummaryModel()
            {
                Results = new List<SprintTaskJournalRecord>()
                {
                    new SprintTaskJournalRecord()
                    {
                        PhraseId = 1,
                        CorrectAnswersCount = 5,
                        WrongAnswersCount = 0
                    }
                }
            };

            /* Assert */
            AssertIsValidationFailedExceptionTrown(() => service.FinishTask(summary), "Results[0]");
        }

        [TestMethod]
        public void FinishTask_ShouldCheckThatCorrectAnswersCountIsPositiveOrZero()
        {
            /* Arrange */

            /* Act */
            var service = new SprintTaskService(GetPhrasesServiceStubObject(), GetLanguageServiceStubObject(), Db);
            var summary = new SprintTaskFinishedSummaryModel()
            {
                Results = new List<SprintTaskJournalRecord>()
                {
                    new SprintTaskJournalRecord()
                    {
                        PhraseId = 1,
                        CorrectAnswersCount = -1,
                        WrongAnswersCount = 0
                    }
                }
            };

            /* Assert */
            AssertIsValidationFailedExceptionTrown(() => service.FinishTask(summary), "Results[0].CorrectAnswersCount");
        }

        [TestMethod]
        public void FinishTask_ShouldCheckThatWrongAnswersCountIsPositiveOrZero()
        {
            /* Arrange */

            /* Act */
            var service = new SprintTaskService(GetPhrasesServiceStubObject(), GetLanguageServiceStubObject(), Db);
            var summary = new SprintTaskFinishedSummaryModel()
            {
                Results = new List<SprintTaskJournalRecord>()
                {
                    new SprintTaskJournalRecord()
                    {
                        PhraseId = 1,
                        CorrectAnswersCount = 0,
                        WrongAnswersCount = -1
                    }
                }
            };

            /* Assert */
            AssertIsValidationFailedExceptionTrown(() => service.FinishTask(summary), "Results[0].WrongAnswersCount");
        }

        [TestMethod]
        public void FinishTask_ShouldAddSummaryInDatabase()
        {
            /* Arrange */
            var date = DateTime.UtcNow;
            var records = new List<SprintTaskJournalRecordDal>()
            {
                new SprintTaskJournalRecordDal() {Id = 1, PhraseId = 1, LastRepetitonTime = date, CorrectWrongAnswersDelta = 2}
            };
            var sprintTaskJournalRecordsMockDbSet = CreateSprintTaskJournalRecordsMockDbSet(records);

            var phrasesServiceMock = GetPhrasesServiceStub();
            var phrasesService = phrasesServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, GetLanguageServiceStubObject(), Db);
            var summary = new SprintTaskFinishedSummaryModel()
            {
                Results = new List<SprintTaskJournalRecord>()
                {
                    new SprintTaskJournalRecord()
                    {
                        PhraseId = 2,
                        CorrectAnswersCount = 10,
                        WrongAnswersCount = 5
                    }
                }
            };
            service.FinishTask(summary);

            /* Assert */
            phrasesServiceMock.Verify(m => m.CheckIfPhraseExists(summary.Results[0].PhraseId), Times.Once);
            sprintTaskJournalRecordsMockDbSet.Verify(
                m => m.Add(It.Is<SprintTaskJournalRecordDal>(
                    dal => dal.PhraseId == summary.Results[0].PhraseId &&
                           dal.CorrectWrongAnswersDelta == summary.Results[0].CorrectAnswersCount - summary.Results[0].WrongAnswersCount &&
                           dal.LastRepetitonTime != default(DateTime)
                )), Times.Once);
        }

        [TestMethod]
        public void FinishTask_ShouldEditSummaryInDatabase()
        {
            /* Arrange */
            var date = DateTime.UtcNow;
            var records = new List<SprintTaskJournalRecordDal>()
            {
                new SprintTaskJournalRecordDal() {Id = 1, PhraseId = 1, LastRepetitonTime = date, CorrectWrongAnswersDelta = 2},
                new SprintTaskJournalRecordDal() {Id = 2, PhraseId = 2, LastRepetitonTime = date, CorrectWrongAnswersDelta = 5}
            };
            var sprintTaskJournalRecordsMockDbSet = CreateSprintTaskJournalRecordsMockDbSet(records);

            var phrasesServiceMock = GetPhrasesServiceStub();
            var phrasesService = phrasesServiceMock.Object;

            /* Act */
            var service = new SprintTaskService(phrasesService, GetLanguageServiceStubObject(), Db);
            var summary = new SprintTaskFinishedSummaryModel()
            {
                Results = new List<SprintTaskJournalRecord>()
                {
                    new SprintTaskJournalRecord()
                    {
                        PhraseId = 1,
                        CorrectAnswersCount = 5,
                        WrongAnswersCount = 10
                    }
                }
            };
            service.FinishTask(summary);

            /* Assert */
            phrasesServiceMock.Verify(m => m.CheckIfPhraseExists(summary.Results[0].PhraseId), Times.Once);

            AssertThatNoRecordsWasAdded(sprintTaskJournalRecordsMockDbSet);

            AssertThatOnlyOneRecordWasMarkedAsModified<SprintTaskJournalRecordDal>(
                dal => dal.PhraseId == summary.Results[0].PhraseId &&
                       dal.CorrectWrongAnswersDelta == 2 +
                            summary.Results[0].CorrectAnswersCount - summary.Results[0].WrongAnswersCount &&
                       dal.LastRepetitonTime != date
                );

            // Assert that the first record was changed
            Assert.AreEqual(1, records[0].Id);
            Assert.AreEqual(summary.Results[0].PhraseId, records[0].PhraseId);
            Assert.AreNotEqual(date, records[0].LastRepetitonTime);
            Assert.AreEqual(2 + summary.Results[0].CorrectAnswersCount - summary.Results[0].WrongAnswersCount,
                records[0].CorrectWrongAnswersDelta);

            // Assert that the other records was not changed
            Assert.AreEqual(2, records[1].Id);
            Assert.AreEqual(2, records[1].PhraseId);
            Assert.AreEqual(date, records[1].LastRepetitonTime);
            Assert.AreEqual(5, records[1].CorrectWrongAnswersDelta);
        }
    }
}