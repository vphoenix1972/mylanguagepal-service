using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.Quiz;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.Tests.BLL.Tasks.WriteTranslation
{
    [TestClass]
    public class WriteTranslationTaskServiceFinishTaskTest : WriteTranslationTaskTestBase
    {
        [TestMethod]
        public void FinishTask_ShouldCheckLanguageId()
        {
            ShouldCheckLanguageId((service, settings) => service.FinishTask(settings, new QuizTaskAnswersModel()));
        }


        [TestMethod]
        public void FinishTask_ShouldCheckCountOfWordsUsed()
        {
            ShouldCheckCountOfWordsUsed((service, settings) => service.FinishTask(settings, new QuizTaskAnswersModel()),
                WriteTranslationTaskService.MinCountOfWordsUsed,
                WriteTranslationTaskService.MaxCountOfWordsUsed);
        }

        [TestMethod]
        public void FinishTask_ShouldAssertAnswersContract()
        {
            /* Arrange */
            var correctSettings = GetCorrectSettings();

            /* Act */
            var service = CreateService();

            /* Assert */

            // Should check null reference
            // ReSharper disable once AssignNullToNotNullAttribute
            AssertIsArgumentExceptionThrown(() => service.FinishTask(correctSettings, null));

            // Should check answers type
            AssertIsArgumentExceptionThrown(() => service.FinishTask(correctSettings, correctSettings));

            // Should check that answers is not null
            // ReSharper disable once AssignNullToNotNullAttribute
            AssertIsArgumentNullExceptionThrown(() => service.FinishTask(correctSettings, new QuizTaskAnswersModel() { Answers = null }),
                nameof(QuizTaskAnswersModel.Answers));

            // Should check that each answer is not null
            AssertIsArgumentNullExceptionThrown(() => service.FinishTask(correctSettings, new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>() { null }
            }),
            $"{nameof(QuizTaskAnswersModel.Answers)}[0]");
        }

        [TestMethod]
        public void FinishTask_ShouldCheckThatPhrasesListInAnswerIsNotNull()
        {
            /* Arrange */
            var correctSettings = GetCorrectSettings();

            var phrasesServiceMock = CreatePhrasesServiceStub();
            phrasesServiceMock
                .Setup(s => s.GetPhrase(It.Is<int>(i => i == 1)))
                .Returns(new PhraseDal() { Id = 1, LanguageId = 1, Text = "Phrase 1" });

            var answers = new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>()
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    new QuizTaskAnswer() { PhraseId = 1, Answers = null }
                }
            };

            /* Act */
            var service = CreateService(phrasesService: phrasesServiceMock.Object);

            /* Assert */
            AssertIsArgumentNullExceptionThrown(() => service.FinishTask(correctSettings, answers),
                $"{nameof(QuizTaskAnswersModel.Answers)}[0]{nameof(QuizTaskAnswer.Answers)}");
        }

        [TestMethod]
        public void FinishTask_ShouldCheckThatPhraseExists()
        {
            /* Arrange */
            var correctSettings = GetCorrectSettings();

            var phrasesServiceMock = CreatePhrasesServiceStub();
            phrasesServiceMock
                .Setup(s => s.GetPhrase(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    if (id == 2)
                        return null;
                    return new PhraseDal() { Id = id, LanguageId = 1, Text = $"Phrase {id}" };
                });

            var answers = new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>()
                {
                    new QuizTaskAnswer() { PhraseId = 1 },
                    new QuizTaskAnswer() { PhraseId = 2 },
                    new QuizTaskAnswer() { PhraseId = 3 }
                }
            };

            /* Act */
            var service = CreateService(phrasesService: phrasesServiceMock.Object);

            /* Assert */

            AssertIsArgumentExceptionThrown(() => service.FinishTask(correctSettings, answers),
                expectedParamName: $"{nameof(QuizTaskAnswersModel.Answers)}[1]",
                expectedMessage: $"Phrase does not exist.{Environment.NewLine}Parameter name: Answers[1]");
        }

        [TestMethod]
        public void FinishTask_ShouldCheckThatPhraseHasTheSameLanguageAsSettingsPassed()
        {
            /* Arrange */
            var correctSettings = GetCorrectSettings();
            correctSettings.LanguageId = 2;

            var phrasesServiceMock = CreatePhrasesServiceStub();
            phrasesServiceMock
                .Setup(s => s.GetPhrase(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    if (id == 2)
                        return new PhraseDal() { Id = id, LanguageId = 1, Text = $"Phrase {id}" };
                    return new PhraseDal() { Id = id, LanguageId = 2, Text = $"Phrase {id}" };
                });

            var answers = new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>()
                {
                    new QuizTaskAnswer() { PhraseId = 1 },
                    new QuizTaskAnswer() { PhraseId = 2 },
                    new QuizTaskAnswer() { PhraseId = 3 }
                }
            };

            /* Act */
            var service = CreateService(phrasesService: phrasesServiceMock.Object);

            /* Assert */

            AssertIsArgumentExceptionThrown(() => service.FinishTask(correctSettings, answers),
                expectedParamName: $"{nameof(QuizTaskAnswersModel.Answers)}[1]",
                expectedMessage: $"Phrase has different language from language in settings.{Environment.NewLine}Parameter name: Answers[1]");
        }

        [TestMethod]
        public void FinishTask_AnswerCorrect_ShouldAddStatsInDb()
        {
            /* Arrange */
            var currentTime = new DateTime(2017, 1, 9);
            double expectedLevel = 24 * 60 * 60;

            var framework = CreateFrameworkStub();
            framework
                .Setup(m => m.UtcNow)
                .Returns(currentTime);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {Id = 1, LanguageId = 1, Text = "Hello"},
                new PhraseDal() {Id = 2, LanguageId = 2, Text = "Привет"}
            };
            var translations = new List<TranslationModelBbl>()
            {
                new TranslationModelBbl()
                {
                    Phrase = phrases[1],
                    Prevalence = PhrasesService.PrevalenceMax
                }
            };


            var correctSettings = GetCorrectSettings();

            var phrasesServiceMock = CreatePhrasesServiceStub();
            phrasesServiceMock
                .Setup(m => m.GetPhrase(1))
                .Returns(phrases[0]);
            phrasesServiceMock
                .Setup(m => m.GetTranslations(phrases[0]))
                .Returns(translations);

            var answers = new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>()
                    {
                        new QuizTaskAnswer() { PhraseId = 1, Answers = new List<string>()
                        {
                            "Привет"
                        }
                    }
                }
            };

            var knowledgeLevelsMockDbSet = CreateKnowledgeLevelsMockDbSet();

            /* Act */
            var service = CreateService(
                framework: framework.Object,
                phrasesService: phrasesServiceMock.Object
                );

            var summaryObject = service.FinishTask(correctSettings, answers);

            /* Assert */
            phrasesServiceMock.Verify(m => m.GetPhrase(1), Times.Once);
            phrasesServiceMock.Verify(m => m.GetTranslations(phrases[0]), Times.Once);

            knowledgeLevelsMockDbSet.Verify(
                m => m.Add(It.Is<KnowledgeLevelDal>(
                    dal => dal.TaskId == WriteTranslationTaskService.WriteTranslationTaskId &&
                           dal.PhraseId == phrases[0].Id &&
                           dal.LastRepetitonTime == currentTime &&
                           Math.Abs(dal.CurrentLevel - expectedLevel) < Tolerance &&
                           dal.PreviousLevel == null
                )), Times.Once);

            var summary = AssertSummaryContract(summaryObject);
            Assert.AreEqual(1, summary.Results.Count);
            Assert.AreEqual(phrases[0].Id, summary.Results[0].Entity.Id);
            Assert.AreEqual(expectedLevel, summary.Results[0].Improvement);
            Assert.AreEqual(true, summary.Results[0].IsCorrect);
        }

        [TestMethod]
        public void FinishTask_AnswerCorrect_ShouldUpdateStatsInDb()
        {
            /* Arrange */
            var lastRepetitionTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);
            var currentLevel = OneDayLevel();

            var framework = CreateFrameworkStub();
            framework
                .Setup(m => m.UtcNow)
                .Returns(currentTime);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {LanguageId = 1, Text = "Hello"},
                new PhraseDal() {LanguageId = 2, Text = "Привет"}
            };
            SetIds(phrases);
            var translations = new List<TranslationModelBbl>()
            {
                new TranslationModelBbl()
                {
                    Phrase = phrases[1],
                    Prevalence = PhrasesService.PrevalenceMax
                }
            };


            var correctSettings = GetCorrectSettings();

            var phrasesServiceMock = CreatePhrasesServiceStub();
            phrasesServiceMock
                .Setup(m => m.GetPhrase(1))
                .Returns(phrases[0]);
            phrasesServiceMock
                .Setup(m => m.GetTranslations(phrases[0]))
                .Returns(translations);

            var answers = new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>()
                    {
                        new QuizTaskAnswer() { PhraseId = 1, Answers = new List<string>()
                        {
                            "Привет"
                        }
                    }
                }
            };

            var knoledgeLevels = new List<KnowledgeLevelDal>()
            {
                new KnowledgeLevelDal()
                {
                    TaskId = WriteTranslationTaskService.WriteTranslationTaskId,
                    PhraseId = phrases[0].Id,
                    LastRepetitonTime = lastRepetitionTime,
                    CurrentLevel = currentLevel,
                    PreviousLevel = null
                }
            };
            SetIds(knoledgeLevels);
            var knowledgeLevelsMockDbSet = CreateKnowledgeLevelsMockDbSet(knoledgeLevels);

            /* Act */
            var service = CreateService(
                framework: framework.Object,
                phrasesService: phrasesServiceMock.Object
                );

            var summaryObject = service.FinishTask(correctSettings, answers);

            /* Assert */
            phrasesServiceMock.Verify(m => m.GetPhrase(1), Times.Once);
            phrasesServiceMock.Verify(m => m.GetTranslations(phrases[0]), Times.Once);

            AssertThatNoRecordsWasAdded(knowledgeLevelsMockDbSet);
            AssertThatOnlyOneRecordWasMarkedAsModified<KnowledgeLevelDal>(dal => dal == knoledgeLevels[0]);

            // Assert that the target record was changed
            Assert.AreEqual(WriteTranslationTaskService.WriteTranslationTaskId, knoledgeLevels[0].TaskId);
            Assert.AreEqual(phrases[0].Id, knoledgeLevels[0].PhraseId);
            Assert.AreEqual(currentTime, knoledgeLevels[0].LastRepetitonTime);
            Assert.AreEqual(2 * currentLevel, knoledgeLevels[0].CurrentLevel);
            Assert.IsNull(knoledgeLevels[0].PreviousLevel);

            // Assert summary
            var summary = AssertSummaryContract(summaryObject);
            Assert.AreEqual(1, summary.Results.Count);
            Assert.AreEqual(phrases[0].Id, summary.Results[0].Entity.Id);
            Assert.AreEqual(currentLevel, summary.Results[0].Improvement);
            Assert.AreEqual(true, summary.Results[0].IsCorrect);
        }

        [TestMethod]
        public void FinishTask_AnswerCorrect_ShouldRespectMaxKnowledgeLevel()
        {
            /* Arrange */
            var lastRepetitionTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);
            var currentLevel = CreateLevel(days: 29);

            var framework = CreateFrameworkStub();
            framework
                .Setup(m => m.UtcNow)
                .Returns(currentTime);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {LanguageId = 1, Text = "Hello"},
                new PhraseDal() {LanguageId = 2, Text = "Привет"}
            };
            SetIds(phrases);
            var translations = new List<TranslationModelBbl>()
            {
                new TranslationModelBbl()
                {
                    Phrase = phrases[1],
                    Prevalence = PhrasesService.PrevalenceMax
                }
            };


            var correctSettings = GetCorrectSettings();

            var phrasesServiceMock = CreatePhrasesServiceStub();
            phrasesServiceMock
                .Setup(m => m.GetPhrase(1))
                .Returns(phrases[0]);
            phrasesServiceMock
                .Setup(m => m.GetTranslations(phrases[0]))
                .Returns(translations);

            var answers = new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>()
                    {
                        new QuizTaskAnswer() { PhraseId = 1, Answers = new List<string>()
                        {
                            "Привет"
                        }
                    }
                }
            };

            var knoledgeLevels = new List<KnowledgeLevelDal>()
            {
                new KnowledgeLevelDal()
                {
                    TaskId = WriteTranslationTaskService.WriteTranslationTaskId,
                    PhraseId = phrases[0].Id,
                    LastRepetitonTime = lastRepetitionTime,
                    CurrentLevel = currentLevel,
                    PreviousLevel = null
                }
            };
            SetIds(knoledgeLevels);
            var knowledgeLevelsMockDbSet = CreateKnowledgeLevelsMockDbSet(knoledgeLevels);

            /* Act */
            var service = CreateService(
                framework: framework.Object,
                phrasesService: phrasesServiceMock.Object
                );

            var summaryObject = service.FinishTask(correctSettings, answers);

            /* Assert */
            phrasesServiceMock.Verify(m => m.GetPhrase(1), Times.Once);
            phrasesServiceMock.Verify(m => m.GetTranslations(phrases[0]), Times.Once);

            AssertThatNoRecordsWasAdded(knowledgeLevelsMockDbSet);
            AssertThatOnlyOneRecordWasMarkedAsModified<KnowledgeLevelDal>(dal => dal == knoledgeLevels[0]);

            // Assert that the target record was changed
            Assert.AreEqual(WriteTranslationTaskService.WriteTranslationTaskId, knoledgeLevels[0].TaskId);
            Assert.AreEqual(phrases[0].Id, knoledgeLevels[0].PhraseId);
            Assert.AreEqual(currentTime, knoledgeLevels[0].LastRepetitonTime);
            Assert.AreEqual(CreateLevel(days: 30), knoledgeLevels[0].CurrentLevel);
            Assert.IsNull(knoledgeLevels[0].PreviousLevel);

            // Assert summary
            var summary = AssertSummaryContract(summaryObject);
            Assert.AreEqual(1, summary.Results.Count);
            Assert.AreEqual(phrases[0].Id, summary.Results[0].Entity.Id);
            Assert.AreEqual(CreateLevel(days: 1), summary.Results[0].Improvement);
            Assert.AreEqual(true, summary.Results[0].IsCorrect);
        }

        [TestMethod]
        public void FinishTask_AnswerInCorrect_ShouldDropKnowledgeLevelAndRecordPreviousLevel()
        {
            /* Arrange */
            var lastRepetitionTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);
            var currentLevel = CreateLevel(days: 4);

            var framework = CreateFrameworkStub();
            framework
                .Setup(m => m.UtcNow)
                .Returns(currentTime);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {LanguageId = 1, Text = "Hello"},
                new PhraseDal() {LanguageId = 2, Text = "Привет"}
            };
            SetIds(phrases);
            var translations = new List<TranslationModelBbl>()
            {
                new TranslationModelBbl()
                {
                    Phrase = phrases[1],
                    Prevalence = PhrasesService.PrevalenceMax
                }
            };


            var correctSettings = GetCorrectSettings();

            var phrasesServiceMock = CreatePhrasesServiceStub();
            phrasesServiceMock
                .Setup(m => m.GetPhrase(1))
                .Returns(phrases[0]);
            phrasesServiceMock
                .Setup(m => m.GetTranslations(phrases[0]))
                .Returns(translations);

            var answers = new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>()
                    {
                        new QuizTaskAnswer() { PhraseId = 1, Answers = new List<string>()
                        {
                            "До свидания"
                        }
                    }
                }
            };

            var knoledgeLevels = new List<KnowledgeLevelDal>()
            {
                new KnowledgeLevelDal()
                {
                    TaskId = WriteTranslationTaskService.WriteTranslationTaskId,
                    PhraseId = phrases[0].Id,
                    LastRepetitonTime = lastRepetitionTime,
                    CurrentLevel = currentLevel,
                    PreviousLevel = null
                }
            };
            SetIds(knoledgeLevels);
            var knowledgeLevelsMockDbSet = CreateKnowledgeLevelsMockDbSet(knoledgeLevels);

            /* Act */
            var service = CreateService(
                framework: framework.Object,
                phrasesService: phrasesServiceMock.Object
                );

            var summaryObject = service.FinishTask(correctSettings, answers);

            /* Assert */
            phrasesServiceMock.Verify(m => m.GetPhrase(1), Times.Once);
            phrasesServiceMock.Verify(m => m.GetTranslations(phrases[0]), Times.Once);

            AssertThatNoRecordsWasAdded(knowledgeLevelsMockDbSet);
            AssertThatOnlyOneRecordWasMarkedAsModified<KnowledgeLevelDal>(dal => dal == knoledgeLevels[0]);

            // Assert that the target record was changed
            Assert.AreEqual(WriteTranslationTaskService.WriteTranslationTaskId, knoledgeLevels[0].TaskId);
            Assert.AreEqual(phrases[0].Id, knoledgeLevels[0].PhraseId);
            Assert.AreEqual(currentTime, knoledgeLevels[0].LastRepetitonTime);
            Assert.AreEqual(0, knoledgeLevels[0].CurrentLevel);
            Assert.AreEqual(currentLevel, knoledgeLevels[0].PreviousLevel);

            // Assert summary
            var summary = AssertSummaryContract(summaryObject);
            Assert.AreEqual(1, summary.Results.Count);
            Assert.AreEqual(phrases[0].Id, summary.Results[0].Entity.Id);
            Assert.AreEqual(-currentLevel, summary.Results[0].Improvement);
            Assert.IsFalse(summary.Results[0].IsCorrect);
        }

        [TestMethod]
        public void FinishTask_AnswerCorrectAfterIncorrectAnswer_ShouldKeepPreviousLevelAndIncreaseCurrentLevelByOneDay()
        {
            /* Arrange */
            var lastRepetitionTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);
            var currentLevel = 0;
            var previousLevel = CreateLevel(days: 7);

            var framework = CreateFrameworkStub();
            framework
                .Setup(m => m.UtcNow)
                .Returns(currentTime);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {LanguageId = 1, Text = "Hello"},
                new PhraseDal() {LanguageId = 2, Text = "Привет"}
            };
            SetIds(phrases);
            var translations = new List<TranslationModelBbl>()
            {
                new TranslationModelBbl()
                {
                    Phrase = phrases[1],
                    Prevalence = PhrasesService.PrevalenceMax
                }
            };


            var correctSettings = GetCorrectSettings();

            var phrasesServiceMock = CreatePhrasesServiceStub();
            phrasesServiceMock
                .Setup(m => m.GetPhrase(1))
                .Returns(phrases[0]);
            phrasesServiceMock
                .Setup(m => m.GetTranslations(phrases[0]))
                .Returns(translations);

            var answers = new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>()
                    {
                        new QuizTaskAnswer() { PhraseId = 1, Answers = new List<string>()
                        {
                            "Привет"
                        }
                    }
                }
            };

            var knoledgeLevels = new List<KnowledgeLevelDal>()
            {
                new KnowledgeLevelDal()
                {
                    TaskId = WriteTranslationTaskService.WriteTranslationTaskId,
                    PhraseId = phrases[0].Id,
                    LastRepetitonTime = lastRepetitionTime,
                    CurrentLevel = currentLevel,
                    PreviousLevel = previousLevel
                }
            };
            SetIds(knoledgeLevels);
            var knowledgeLevelsMockDbSet = CreateKnowledgeLevelsMockDbSet(knoledgeLevels);

            /* Act */
            var service = CreateService(
                framework: framework.Object,
                phrasesService: phrasesServiceMock.Object
                );

            var summaryObject = service.FinishTask(correctSettings, answers);

            /* Assert */
            phrasesServiceMock.Verify(m => m.GetPhrase(1), Times.Once);
            phrasesServiceMock.Verify(m => m.GetTranslations(phrases[0]), Times.Once);

            AssertThatNoRecordsWasAdded(knowledgeLevelsMockDbSet);
            AssertThatOnlyOneRecordWasMarkedAsModified<KnowledgeLevelDal>(dal => dal == knoledgeLevels[0]);

            // Assert that the target record was changed
            Assert.AreEqual(WriteTranslationTaskService.WriteTranslationTaskId, knoledgeLevels[0].TaskId);
            Assert.AreEqual(phrases[0].Id, knoledgeLevels[0].PhraseId);
            Assert.AreEqual(currentTime, knoledgeLevels[0].LastRepetitonTime);
            Assert.AreEqual(CreateLevel(days: 1), knoledgeLevels[0].CurrentLevel);
            Assert.AreEqual(previousLevel, knoledgeLevels[0].PreviousLevel);

            // Assert summary
            var summary = AssertSummaryContract(summaryObject);
            Assert.AreEqual(1, summary.Results.Count);
            Assert.AreEqual(phrases[0].Id, summary.Results[0].Entity.Id);
            Assert.AreEqual(CreateLevel(days: 1), summary.Results[0].Improvement);
            Assert.IsTrue(summary.Results[0].IsCorrect);
        }

        [TestMethod]
        public void FinishTask_TwoAnswersCorrectAfterIncorrectAnswerPreviousLevelBetween4and14days_ShouldKeepPreviousLevelAndRespectPreviousLevel()
        {
            /* Arrange */
            var lastRepetitionTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);
            var currentLevel = CreateLevel(days: 1);
            var previousLevel = CreateLevel(days: 7);

            var framework = CreateFrameworkStub();
            framework
                .Setup(m => m.UtcNow)
                .Returns(currentTime);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {LanguageId = 1, Text = "Hello"},
                new PhraseDal() {LanguageId = 2, Text = "Привет"}
            };
            SetIds(phrases);
            var translations = new List<TranslationModelBbl>()
            {
                new TranslationModelBbl()
                {
                    Phrase = phrases[1],
                    Prevalence = PhrasesService.PrevalenceMax
                }
            };


            var correctSettings = GetCorrectSettings();

            var phrasesServiceMock = CreatePhrasesServiceStub();
            phrasesServiceMock
                .Setup(m => m.GetPhrase(1))
                .Returns(phrases[0]);
            phrasesServiceMock
                .Setup(m => m.GetTranslations(phrases[0]))
                .Returns(translations);

            var answers = new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>()
                    {
                        new QuizTaskAnswer() { PhraseId = 1, Answers = new List<string>()
                        {
                            "Привет"
                        }
                    }
                }
            };

            var knoledgeLevels = new List<KnowledgeLevelDal>()
            {
                new KnowledgeLevelDal()
                {
                    TaskId = WriteTranslationTaskService.WriteTranslationTaskId,
                    PhraseId = phrases[0].Id,
                    LastRepetitonTime = lastRepetitionTime,
                    CurrentLevel = currentLevel,
                    PreviousLevel = previousLevel
                }
            };
            SetIds(knoledgeLevels);
            var knowledgeLevelsMockDbSet = CreateKnowledgeLevelsMockDbSet(knoledgeLevels);

            /* Act */
            var service = CreateService(
                framework: framework.Object,
                phrasesService: phrasesServiceMock.Object
                );

            var summaryObject = service.FinishTask(correctSettings, answers);

            /* Assert */
            phrasesServiceMock.Verify(m => m.GetPhrase(1), Times.Once);
            phrasesServiceMock.Verify(m => m.GetTranslations(phrases[0]), Times.Once);

            AssertThatNoRecordsWasAdded(knowledgeLevelsMockDbSet);
            AssertThatOnlyOneRecordWasMarkedAsModified<KnowledgeLevelDal>(dal => dal == knoledgeLevels[0]);

            // Assert that the target record was changed
            Assert.AreEqual(WriteTranslationTaskService.WriteTranslationTaskId, knoledgeLevels[0].TaskId);
            Assert.AreEqual(phrases[0].Id, knoledgeLevels[0].PhraseId);
            Assert.AreEqual(currentTime, knoledgeLevels[0].LastRepetitonTime);
            Assert.AreEqual(CreateLevel(days: 3), knoledgeLevels[0].CurrentLevel);
            Assert.AreEqual(previousLevel, knoledgeLevels[0].PreviousLevel);

            // Assert summary
            var summary = AssertSummaryContract(summaryObject);
            Assert.AreEqual(1, summary.Results.Count);
            Assert.AreEqual(phrases[0].Id, summary.Results[0].Entity.Id);
            Assert.AreEqual(CreateLevel(days: 2), summary.Results[0].Improvement);
            Assert.IsTrue(summary.Results[0].IsCorrect);
        }

        [TestMethod]
        public void FinishTask_TwoAnswersCorrectAfterIncorrectAnswerPreviousLevelMoreOrEqual14days_ShouldKeepPreviousLevelAndRespectPreviousLevel()
        {
            /* Arrange */
            var lastRepetitionTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);
            var currentLevel = CreateLevel(days: 1);
            var previousLevel = CreateLevel(days: 14);

            var framework = CreateFrameworkStub();
            framework
                .Setup(m => m.UtcNow)
                .Returns(currentTime);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {LanguageId = 1, Text = "Hello"},
                new PhraseDal() {LanguageId = 2, Text = "Привет"}
            };
            SetIds(phrases);
            var translations = new List<TranslationModelBbl>()
            {
                new TranslationModelBbl()
                {
                    Phrase = phrases[1],
                    Prevalence = PhrasesService.PrevalenceMax
                }
            };


            var correctSettings = GetCorrectSettings();

            var phrasesServiceMock = CreatePhrasesServiceStub();
            phrasesServiceMock
                .Setup(m => m.GetPhrase(1))
                .Returns(phrases[0]);
            phrasesServiceMock
                .Setup(m => m.GetTranslations(phrases[0]))
                .Returns(translations);

            var answers = new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>()
                    {
                        new QuizTaskAnswer() { PhraseId = 1, Answers = new List<string>()
                        {
                            "Привет"
                        }
                    }
                }
            };

            var knoledgeLevels = new List<KnowledgeLevelDal>()
            {
                new KnowledgeLevelDal()
                {
                    TaskId = WriteTranslationTaskService.WriteTranslationTaskId,
                    PhraseId = phrases[0].Id,
                    LastRepetitonTime = lastRepetitionTime,
                    CurrentLevel = currentLevel,
                    PreviousLevel = previousLevel
                }
            };
            SetIds(knoledgeLevels);
            var knowledgeLevelsMockDbSet = CreateKnowledgeLevelsMockDbSet(knoledgeLevels);

            /* Act */
            var service = CreateService(
                framework: framework.Object,
                phrasesService: phrasesServiceMock.Object
                );

            var summaryObject = service.FinishTask(correctSettings, answers);

            /* Assert */
            phrasesServiceMock.Verify(m => m.GetPhrase(1), Times.Once);
            phrasesServiceMock.Verify(m => m.GetTranslations(phrases[0]), Times.Once);

            AssertThatNoRecordsWasAdded(knowledgeLevelsMockDbSet);
            AssertThatOnlyOneRecordWasMarkedAsModified<KnowledgeLevelDal>(dal => dal == knoledgeLevels[0]);

            // Assert that the target record was changed
            Assert.AreEqual(WriteTranslationTaskService.WriteTranslationTaskId, knoledgeLevels[0].TaskId);
            Assert.AreEqual(phrases[0].Id, knoledgeLevels[0].PhraseId);
            Assert.AreEqual(currentTime, knoledgeLevels[0].LastRepetitonTime);
            Assert.AreEqual(CreateLevel(days: 4), knoledgeLevels[0].CurrentLevel);
            Assert.AreEqual(previousLevel, knoledgeLevels[0].PreviousLevel);

            // Assert summary
            var summary = AssertSummaryContract(summaryObject);
            Assert.AreEqual(1, summary.Results.Count);
            Assert.AreEqual(phrases[0].Id, summary.Results[0].Entity.Id);
            Assert.AreEqual(CreateLevel(days: 3), summary.Results[0].Improvement);
            Assert.IsTrue(summary.Results[0].IsCorrect);
        }

        [TestMethod]
        public void FinishTask_TwoAnswersCorrect_ShouldUpdateStatsInDbAsOneAnswerCorrect()
        {
            /* Arrange */
            var lastRepetitionTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);
            var currentLevel = OneDayLevel();

            var framework = CreateFrameworkStub();
            framework
                .Setup(m => m.UtcNow)
                .Returns(currentTime);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {LanguageId = 1, Text = "Hello"},
                new PhraseDal() {LanguageId = 2, Text = "Привет"},
                new PhraseDal() {LanguageId = 2, Text = "Привет дружище"}
            };
            SetIds(phrases);
            var translations = new List<TranslationModelBbl>()
            {
                new TranslationModelBbl()
                {
                    Phrase = phrases[1],
                    Prevalence = PhrasesService.PrevalenceMax
                },
                new TranslationModelBbl()
                {
                    Phrase = phrases[2],
                    Prevalence = PhrasesService.PrevalenceMax
                }
            };


            var correctSettings = GetCorrectSettings();

            var phrasesServiceMock = CreatePhrasesServiceStub();
            phrasesServiceMock
                .Setup(m => m.GetPhrase(1))
                .Returns(phrases[0]);
            phrasesServiceMock
                .Setup(m => m.GetTranslations(phrases[0]))
                .Returns(translations);

            var answers = new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>()
                    {
                        new QuizTaskAnswer() { PhraseId = 1, Answers = new List<string>()
                        {
                            "Привет",
                            "Привет дружище"
                        }
                    }
                }
            };

            var knoledgeLevels = new List<KnowledgeLevelDal>()
            {
                new KnowledgeLevelDal()
                {
                    TaskId = WriteTranslationTaskService.WriteTranslationTaskId,
                    PhraseId = phrases[0].Id,
                    LastRepetitonTime = lastRepetitionTime,
                    CurrentLevel = currentLevel,
                    PreviousLevel = null
                }
            };
            SetIds(knoledgeLevels);
            var knowledgeLevelsMockDbSet = CreateKnowledgeLevelsMockDbSet(knoledgeLevels);

            /* Act */
            var service = CreateService(
                framework: framework.Object,
                phrasesService: phrasesServiceMock.Object
                );

            var summaryObject = service.FinishTask(correctSettings, answers);

            /* Assert */
            phrasesServiceMock.Verify(m => m.GetPhrase(1), Times.Once);
            phrasesServiceMock.Verify(m => m.GetTranslations(phrases[0]), Times.Once);

            AssertThatNoRecordsWasAdded(knowledgeLevelsMockDbSet);
            AssertThatOnlyOneRecordWasMarkedAsModified<KnowledgeLevelDal>(dal => dal == knoledgeLevels[0]);

            // Assert that the target record was changed
            Assert.AreEqual(WriteTranslationTaskService.WriteTranslationTaskId, knoledgeLevels[0].TaskId);
            Assert.AreEqual(phrases[0].Id, knoledgeLevels[0].PhraseId);
            Assert.AreEqual(currentTime, knoledgeLevels[0].LastRepetitonTime);
            Assert.AreEqual(2 * currentLevel, knoledgeLevels[0].CurrentLevel);
            Assert.IsNull(knoledgeLevels[0].PreviousLevel);

            // Assert summary
            var summary = AssertSummaryContract(summaryObject);
            Assert.AreEqual(1, summary.Results.Count);
            Assert.AreEqual(phrases[0].Id, summary.Results[0].Entity.Id);
            Assert.AreEqual(currentLevel, summary.Results[0].Improvement);
            Assert.AreEqual(true, summary.Results[0].IsCorrect);
        }

        [TestMethod]
        public void FinishTask_OneAnswerCorrectAndOneIncorrect_ShouldUpdateStatsInDbAsOneAnswerInCorrect()
        {
            /* Arrange */
            var lastRepetitionTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);
            var currentLevel = OneDayLevel();

            var framework = CreateFrameworkStub();
            framework
                .Setup(m => m.UtcNow)
                .Returns(currentTime);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {LanguageId = 1, Text = "Hello"},
                new PhraseDal() {LanguageId = 2, Text = "Привет"},
                new PhraseDal() {LanguageId = 2, Text = "Привет дружище"}
            };
            SetIds(phrases);
            var translations = new List<TranslationModelBbl>()
            {
                new TranslationModelBbl()
                {
                    Phrase = phrases[1],
                    Prevalence = PhrasesService.PrevalenceMax
                },
                new TranslationModelBbl()
                {
                    Phrase = phrases[2],
                    Prevalence = PhrasesService.PrevalenceMax
                }
            };


            var correctSettings = GetCorrectSettings();

            var phrasesServiceMock = CreatePhrasesServiceStub();
            phrasesServiceMock
                .Setup(m => m.GetPhrase(1))
                .Returns(phrases[0]);
            phrasesServiceMock
                .Setup(m => m.GetTranslations(phrases[0]))
                .Returns(translations);

            var answers = new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>()
                    {
                        new QuizTaskAnswer() { PhraseId = 1, Answers = new List<string>()
                        {
                            "Привет",
                            "До свидания"
                        }
                    }
                }
            };

            var knoledgeLevels = new List<KnowledgeLevelDal>()
            {
                new KnowledgeLevelDal()
                {
                    TaskId = WriteTranslationTaskService.WriteTranslationTaskId,
                    PhraseId = phrases[0].Id,
                    LastRepetitonTime = lastRepetitionTime,
                    CurrentLevel = currentLevel,
                    PreviousLevel = null
                }
            };
            SetIds(knoledgeLevels);
            var knowledgeLevelsMockDbSet = CreateKnowledgeLevelsMockDbSet(knoledgeLevels);

            /* Act */
            var service = CreateService(
                framework: framework.Object,
                phrasesService: phrasesServiceMock.Object
                );

            var summaryObject = service.FinishTask(correctSettings, answers);

            /* Assert */
            phrasesServiceMock.Verify(m => m.GetPhrase(1), Times.Once);
            phrasesServiceMock.Verify(m => m.GetTranslations(phrases[0]), Times.Once);

            AssertThatNoRecordsWasAdded(knowledgeLevelsMockDbSet);
            AssertThatOnlyOneRecordWasMarkedAsModified<KnowledgeLevelDal>(dal => dal == knoledgeLevels[0]);

            // Assert that the target record was changed
            Assert.AreEqual(WriteTranslationTaskService.WriteTranslationTaskId, knoledgeLevels[0].TaskId);
            Assert.AreEqual(phrases[0].Id, knoledgeLevels[0].PhraseId);
            Assert.AreEqual(currentTime, knoledgeLevels[0].LastRepetitonTime);
            Assert.AreEqual(0, knoledgeLevels[0].CurrentLevel);
            Assert.AreEqual(currentLevel, knoledgeLevels[0].PreviousLevel);

            // Assert summary
            var summary = AssertSummaryContract(summaryObject);
            Assert.AreEqual(1, summary.Results.Count);
            Assert.AreEqual(phrases[0].Id, summary.Results[0].Entity.Id);
            Assert.AreEqual(-currentLevel, summary.Results[0].Improvement);
            Assert.IsFalse(summary.Results[0].IsCorrect);
        }

        [TestMethod]
        public void FinishTask_TwoWordsUsedAnswerCorrect_ShouldAddStatsInDb()
        {
            /* Arrange */
            var currentTime = new DateTime(2017, 1, 9);
            double expectedLevel = 24 * 60 * 60;

            var framework = CreateFrameworkStub();
            framework
                .Setup(m => m.UtcNow)
                .Returns(currentTime);

            var phrases = new List<PhraseDal>()
            {
                new PhraseDal() {LanguageId = 1, Text = "Hello"},
                new PhraseDal() {LanguageId = 2, Text = "Привет"},
                new PhraseDal() {LanguageId = 1, Text = "Goodbye"},
                new PhraseDal() {LanguageId = 2, Text = "До свидания"}
            };
            SetIds(phrases);
            var translations0 = new List<TranslationModelBbl>()
            {
                new TranslationModelBbl()
                {
                    Phrase = phrases[1],
                    Prevalence = PhrasesService.PrevalenceMax
                }
            };
            var translations2 = new List<TranslationModelBbl>()
            {
                new TranslationModelBbl()
                {
                    Phrase = phrases[3],
                    Prevalence = PhrasesService.PrevalenceMax
                }
            };


            var correctSettings = GetCorrectSettings();

            var phrasesServiceMock = CreatePhrasesServiceStub();
            phrasesServiceMock
                .Setup(m => m.GetPhrase(1))
                .Returns(phrases[0]);
            phrasesServiceMock
                .Setup(m => m.GetPhrase(3))
                .Returns(phrases[2]);
            phrasesServiceMock
                .Setup(m => m.GetTranslations(phrases[0]))
                .Returns(translations0);
            phrasesServiceMock
                .Setup(m => m.GetTranslations(phrases[2]))
                .Returns(translations2);

            var answers = new QuizTaskAnswersModel()
            {
                Answers = new List<QuizTaskAnswer>()
                    {
                        new QuizTaskAnswer() {
                            PhraseId = 1, Answers = new List<string>()
                            {
                                "Привет"
                            },
                        },
                        new QuizTaskAnswer() {
                            PhraseId = 3,
                            Answers = new List<string>()
                            {
                                "До свидания"
                            }
                        }
                    }
            };

            var knowledgeLevelsMockDbSet = CreateKnowledgeLevelsMockDbSet();

            /* Act */
            var service = CreateService(
                framework: framework.Object,
                phrasesService: phrasesServiceMock.Object
                );

            var summaryObject = service.FinishTask(correctSettings, answers);

            /* Assert */
            phrasesServiceMock.Verify(m => m.GetPhrase(1), Times.Once);
            phrasesServiceMock.Verify(m => m.GetTranslations(phrases[0]), Times.Once);
            phrasesServiceMock.Verify(m => m.GetPhrase(3), Times.Once);
            phrasesServiceMock.Verify(m => m.GetTranslations(phrases[2]), Times.Once);

            knowledgeLevelsMockDbSet.Verify(
                m => m.Add(It.Is<KnowledgeLevelDal>(
                    dal => dal.TaskId == WriteTranslationTaskService.WriteTranslationTaskId &&
                           dal.PhraseId == phrases[0].Id &&
                           dal.LastRepetitonTime == currentTime &&
                           Math.Abs(dal.CurrentLevel - expectedLevel) < Tolerance &&
                           dal.PreviousLevel == null
                )), Times.Once);
            knowledgeLevelsMockDbSet.Verify(
                m => m.Add(It.Is<KnowledgeLevelDal>(
                    dal => dal.TaskId == WriteTranslationTaskService.WriteTranslationTaskId &&
                           dal.PhraseId == phrases[2].Id &&
                           dal.LastRepetitonTime == currentTime &&
                           Math.Abs(dal.CurrentLevel - expectedLevel) < Tolerance &&
                           dal.PreviousLevel == null
                )), Times.Once);

            var summary = AssertSummaryContract(summaryObject);
            Assert.AreEqual(2, summary.Results.Count);

            Assert.AreEqual(phrases[0].Id, summary.Results[0].Entity.Id);
            Assert.AreEqual(expectedLevel, summary.Results[0].Improvement);
            Assert.AreEqual(true, summary.Results[0].IsCorrect);

            Assert.AreEqual(phrases[2].Id, summary.Results[1].Entity.Id);
            Assert.AreEqual(expectedLevel, summary.Results[1].Improvement);
            Assert.AreEqual(true, summary.Results[1].IsCorrect);
        }

        private QuizTaskSummary AssertSummaryContract(object summary)
        {
            var typedSummary = summary as QuizTaskSummary;

            Assert.IsNotNull(typedSummary);
            Assert.IsNotNull(typedSummary.Results);
            AssertItemsNotNull(typedSummary.Results,
                (item, index) =>
                {
                    if (item.Entity == null)
                        Assert.Fail($"Summary item at '{index}' is null");
                });

            return typedSummary;
        }

        private QuizTaskSettings GetCorrectSettings()
        {
            return new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = WriteTranslationTaskService.MinCountOfWordsUsed
            };
        }
    }
}