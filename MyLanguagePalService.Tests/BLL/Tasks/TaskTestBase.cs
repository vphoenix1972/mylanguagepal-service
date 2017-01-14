using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyLanguagePal.Core.Framework;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks;
using MyLanguagePalService.BLL.Tasks.Quiz;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL.Models;
using MyLanguagePalService.Tests.TestsShared;

namespace MyLanguagePalService.Tests.BLL.Tasks
{
    public abstract class TaskTestBase : TestBase
    {
        protected abstract ITaskService CreateService(IFramework framework = null,
            IPhrasesService phrasesService = null,
            ILanguagesService languagesService = null);        

        protected void ShouldCheckLanguageId(Action<ITaskService, QuizTaskSettings> method)
        {
            /* Arrange */
            var taskSettingsDbSetMock = CreateTaskSettingsMockDbSet();

            var languageServiceMock = GetLanguageServiceStub();
            languageServiceMock.Setup(m => m.CheckIfLanguageExists(It.IsAny<int>())).Returns(false);
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = CreateService(languagesService: languagesService);
            var input = new QuizTaskSettings()
            {
                LanguageId = 3,
                CountOfWordsUsed = 40
            };

            ValidationFailedException vfeCaught = null;
            try
            {
                method(service, input);
            }
            catch (ValidationFailedException vfe)
            {
                vfeCaught = vfe;
            }

            /* Assert */
            languageServiceMock.Verify(m => m.CheckIfLanguageExists(input.LanguageId), Times.Once);

            taskSettingsDbSetMock.Verify(m => m.Add(It.IsAny<TaskSettingsDal>()), Times.Never);
            DbMock.Verify(db => db.MarkModified(It.IsAny<object>()), Times.Never);

            AssertValidationFailedException(vfeCaught, nameof(QuizTaskSettings.LanguageId));
        }

        protected void ShouldCheckCountOfWordsUsed(Action<ITaskService, QuizTaskSettings> method, int minCountOfWordsUsed, int maxCountOfWordsUsed)
        {
            /* Arrange */
            var taskSettingsDbSetMock = CreateTaskSettingsMockDbSet();

            /* Act */
            var service = CreateService();
            var input = new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = minCountOfWordsUsed - 1
            };

            ValidationFailedException vfeCaught = null;
            try
            {
                method(service, input);
            }
            catch (ValidationFailedException vfe)
            {
                vfeCaught = vfe;
            }

            /* Assert */
            taskSettingsDbSetMock.Verify(m => m.Add(It.IsAny<TaskSettingsDal>()), Times.Never);
            DbMock.Verify(db => db.MarkModified(It.IsAny<object>()), Times.Never);

            AssertValidationFailedException(vfeCaught, nameof(QuizTaskSettings.CountOfWordsUsed));

            /* Act */
            input = new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = maxCountOfWordsUsed + 1
            };

            vfeCaught = null;
            try
            {
                method(service, input);
            }
            catch (ValidationFailedException vfe)
            {
                vfeCaught = vfe;
            }

            /* Assert */
            taskSettingsDbSetMock.Verify(m => m.Add(It.IsAny<TaskSettingsDal>()), Times.Never);
            DbMock.Verify(db => db.MarkModified(It.IsAny<object>()), Times.Never);

            AssertValidationFailedException(vfeCaught, nameof(QuizTaskSettings.CountOfWordsUsed));
        }
        
        protected void RunNewTask_AllWordsAreNew_ShouldReturnWords()
        {
            var languages = new List<int>() { 1 };
            var expectedPhrasesIds = new List<int>() { 1, 2, 3, 4, 5 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = 10
            };

            var phrases = GeneratePhrases(5, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var knowledgeLevels = GenerateKnowledgeLevels(10, (i, l) =>
            {
                l.TaskId = 99;
                return l;
            });

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds
            );
        }
        
        protected void RunNewTask_AllWordsAreNew_ShouldReturnWordsAndRespectLanguageId()
        {
            var languages = new List<int>() { 1, 2 };
            var expectedPhrasesIds = new List<int>() { 2, 4, 6, 8, 10 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 2,
                CountOfWordsUsed = 10
            };

            var phrases = GeneratePhrases(10, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var knowledgeLevels = GenerateKnowledgeLevels(10, (i, l) =>
            {
                l.TaskId = 99;
                return l;
            });

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds
            );
        }
        
        protected void RunNewTask_AllWordsAreNew_ShouldReturnWordsAndRespectCountOfWordsUsed()
        {
            var languages = new List<int>() { 1 };
            var expectedPhrasesIds = new List<int>() { 1, 2, 3 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = 3
            };

            var phrases = GeneratePhrases(20, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var knowledgeLevels = GenerateKnowledgeLevels(10, (i, l) =>
            {
                l.TaskId = 99;
                return l;
            });

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds
            );
        }
        
        protected void RunNewTask_AllWordsAreNew_ShouldReturnWordsAndRespectCountOfWordsUsedAndRespectLanguageId()
        {
            var languages = new List<int>() { 1, 2 };
            var expectedPhrasesIds = new List<int>() { 2, 4, 6 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 2,
                CountOfWordsUsed = 3
            };

            var phrases = GeneratePhrases(20, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var knowledgeLevels = GenerateKnowledgeLevels(10, (i, l) =>
            {
                l.TaskId = 99;
                return l;
            });

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds
            );
        }
        
        protected void RunNewTask_AllWordsAreNeededToRepeat_ShouldReturnWords()
        {
            var languages = new List<int>() { 1 };
            var expectedPhrasesIds = new List<int>() { 1, 2, 3, 4, 5 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = 10
            };

            var phrases = GeneratePhrases(5, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var lastRepetitonTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);

            var knowledgeLevels = GenerateKnowledgeLevels(10, (i, l) =>
            {
                l.TaskId = (i < 5) ? WriteTranslationTaskService.WriteTranslationTaskId : 99;
                l.PhraseId = (i % 5) + 1;
                l.CurrentLevel = (i < 5) ? 24 * 60 * 60 : 4 * 24 * 60 * 60; // Repeat after 1 day
                l.LastRepetitonTime = lastRepetitonTime;
                return l;
            });

            var frameworkMock = GetFrameworkStub();
            frameworkMock
                .Setup(f => f.UtcNow)
                .Returns(currentTime);

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds,
                framework: frameworkMock.Object
            );
        }
        
        protected void RunNewTask_AllWordsAreNeededToRepeat_ShouldReturnWordsAndRespectLanguageId()
        {
            var languages = new List<int>() { 1, 2 };
            var expectedPhrasesIds = new List<int>() { 2, 4, 6, 8, 10 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 2,
                CountOfWordsUsed = 10
            };

            var phrases = GeneratePhrases(10, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var lastRepetitonTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);

            var knowledgeLevels = GenerateKnowledgeLevels(20, (i, l) =>
            {
                l.TaskId = (i < 10) ? WriteTranslationTaskService.WriteTranslationTaskId : 99;
                l.PhraseId = (i % 10) + 1;
                l.CurrentLevel = (i < 10) ?
                    i % languages.Count == 0 ? 3 * 24 * 60 * 60 : 24 * 60 * 60
                    : 4 * 24 * 60 * 60;
                l.LastRepetitonTime = lastRepetitonTime;
                return l;
            });

            var frameworkMock = GetFrameworkStub();
            frameworkMock
                .Setup(f => f.UtcNow)
                .Returns(currentTime);

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds,
                framework: frameworkMock.Object
            );
        }
        
        protected void RunNewTask_AllWordsAreNeededToRepeat_ShouldReturnWordsAndRespectCountOfWordsUsed()
        {
            var languages = new List<int>() { 1 };
            var expectedPhrasesIds = new List<int>() { 1, 2, 3 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = 3
            };

            var phrases = GeneratePhrases(5, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var lastRepetitonTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);

            var knowledgeLevels = GenerateKnowledgeLevels(10, (i, l) =>
            {
                l.TaskId = (i < 5) ? WriteTranslationTaskService.WriteTranslationTaskId : 99;
                l.PhraseId = (i % 5) + 1;
                l.CurrentLevel = (i < 5) ? 24 * 60 * 60 : 4 * 24 * 60 * 60; // Repeat after 1 day
                l.LastRepetitonTime = lastRepetitonTime;
                return l;
            });

            var frameworkMock = GetFrameworkStub();
            frameworkMock
                .Setup(f => f.UtcNow)
                .Returns(currentTime);

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds,
                framework: frameworkMock.Object
            );
        }
        
        protected void RunNewTask_AllWordsAreNeededToRepeat_ShouldReturnWordsAndRespectCountOfWordsUsedAndRespectLanguageId()
        {
            var languages = new List<int>() { 1, 2 };
            var expectedPhrasesIds = new List<int>() { 2, 4, 6 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 2,
                CountOfWordsUsed = 3
            };

            var phrases = GeneratePhrases(10, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var lastRepetitonTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);

            var knowledgeLevels = GenerateKnowledgeLevels(20, (i, l) =>
            {
                l.TaskId = (i < 10) ? WriteTranslationTaskService.WriteTranslationTaskId : 99;
                l.PhraseId = (i % 10) + 1;
                l.CurrentLevel = (i < 10) ?
                    i % languages.Count == 0 ? 3 * 24 * 60 * 60 : 24 * 60 * 60
                    : 4 * 24 * 60 * 60;
                l.LastRepetitonTime = lastRepetitonTime;
                return l;
            });

            var frameworkMock = GetFrameworkStub();
            frameworkMock
                .Setup(f => f.UtcNow)
                .Returns(currentTime);

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds,
                framework: frameworkMock.Object
            );
        }
        
        protected void RunNewTask_NewAndRepeatedWords_ShouldReturnNewWords()
        {
            var languages = new List<int>() { 1 };
            var expectedPhrasesIds = new List<int>() { 1, 2, 3 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = 10
            };

            var phrases = GeneratePhrases(5, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var lastRepetitonTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);

            var knowledgeLevels = new List<KnowledgeLevelDal>()
            {
                new KnowledgeLevelDal() { Id = 1, TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 4, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { Id = 2, TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 5, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { Id = 3, TaskId = 99, PhraseId = 1, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { Id = 4, TaskId = 99, PhraseId = 2, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { Id = 5, TaskId = 99, PhraseId = 3, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { Id = 6, TaskId = 99, PhraseId = 4, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { Id = 7, TaskId = 99, PhraseId = 5, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime }
            };

            var frameworkMock = GetFrameworkStub();
            frameworkMock
                .Setup(f => f.UtcNow)
                .Returns(currentTime);

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds,
                framework: frameworkMock.Object
            );
        }
        
        protected void RunNewTask_NewAndRepeatedWords_ShouldReturnNewWordsAndRespectLanguageId()
        {
            var languages = new List<int>() { 1, 2 };
            var expectedPhrasesIds = new List<int>() { 2, 4, 6 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 2,
                CountOfWordsUsed = 10
            };

            var phrases = GeneratePhrases(10, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var lastRepetitonTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);

            var knowledgeLevels = new List<KnowledgeLevelDal>()
            {
                // All russian phrases are repeated
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 8, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 10, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                
                // All english phrases have to be repeated for task
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 1, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 3, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 5, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 7, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 9, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },

                // All phrases in the other task have to be repeated
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 2, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 3, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 4, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 5, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 6, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 7, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 8, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 9, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 10, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime }
            };
            SetIds(knowledgeLevels);

            var frameworkMock = GetFrameworkStub();
            frameworkMock
                .Setup(f => f.UtcNow)
                .Returns(currentTime);

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds,
                framework: frameworkMock.Object
            );
        }
        
        protected void RunNewTask_NewAndRepeatedWords_ShouldReturnNewWordsAndRespectCountOfWordsUsed()
        {
            var languages = new List<int>() { 1 };
            var expectedPhrasesIds = new List<int>() { 1 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = 1
            };

            var phrases = GeneratePhrases(5, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var lastRepetitonTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);

            var knowledgeLevels = new List<KnowledgeLevelDal>()
            {
                new KnowledgeLevelDal() { Id = 1, TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 4, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { Id = 2, TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 5, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { Id = 3, TaskId = 99, PhraseId = 1, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { Id = 4, TaskId = 99, PhraseId = 2, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { Id = 5, TaskId = 99, PhraseId = 3, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { Id = 6, TaskId = 99, PhraseId = 4, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { Id = 7, TaskId = 99, PhraseId = 5, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime }
            };

            var frameworkMock = GetFrameworkStub();
            frameworkMock
                .Setup(f => f.UtcNow)
                .Returns(currentTime);

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds,
                framework: frameworkMock.Object
            );
        }
        
        protected void RunNewTask_NewAndRepeatedWords_ShouldReturnNewWordsAndRespectCountOfWordsUsedAndRespectLanguageId()
        {
            var languages = new List<int>() { 1, 2 };
            var expectedPhrasesIds = new List<int>() { 2 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 2,
                CountOfWordsUsed = 1
            };

            var phrases = GeneratePhrases(10, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var lastRepetitonTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);

            var knowledgeLevels = new List<KnowledgeLevelDal>()
            {
                // All russian phrases are repeated
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 8, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 10, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                
                // All english phrases have to be repeated for task
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 1, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 3, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 5, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 7, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 9, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },

                // All phrases in the other task have to be repeated
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 2, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 3, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 4, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 5, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 6, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 7, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 8, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 9, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 10, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime }
            };
            SetIds(knowledgeLevels);

            var frameworkMock = GetFrameworkStub();
            frameworkMock
                .Setup(f => f.UtcNow)
                .Returns(currentTime);

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds,
                framework: frameworkMock.Object
            );
        }
        
        protected void RunNewTask_WordsToRepeat_ShouldRespectCurrentLevel()
        {
            var languages = new List<int>() { 1 };
            var expectedPhrasesIds = new List<int>() { 1, 3, 4 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = 10
            };

            var phrases = GeneratePhrases(5, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var lastRepetitonTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);

            var knowledgeLevels = new List<KnowledgeLevelDal>()
            {
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 2, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 3, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 4, CurrentLevel = 2 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 5, CurrentLevel = 5 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
            };

            var frameworkMock = GetFrameworkStub();
            frameworkMock
                .Setup(f => f.UtcNow)
                .Returns(currentTime);

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds,
                framework: frameworkMock.Object
            );
        }
        
        protected void RunNewTask_WordsToRepeat_ShouldRespectCurrentLevelAndRespectCountOfWordsUsed()
        {
            var languages = new List<int>() { 1 };
            var expectedPhrasesIds = new List<int>() { 1 };
            var settings = new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = 1
            };

            var phrases = GeneratePhrases(5, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var lastRepetitonTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);

            var knowledgeLevels = new List<KnowledgeLevelDal>()
            {
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 2, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 3, CurrentLevel = 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 4, CurrentLevel = 2 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 5, CurrentLevel = 5 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
            };

            var frameworkMock = GetFrameworkStub();
            frameworkMock
                .Setup(f => f.UtcNow)
                .Returns(currentTime);

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds,
                framework: frameworkMock.Object
            );
        }
        
        protected void RunNewTask_WordsToRepeat_ShouldReturnEmptyListIfAllWordsAreRepeated()
        {
            var languages = new List<int>() { 1 };
            var expectedPhrasesIds = new List<int>();
            var settings = new QuizTaskSettings()
            {
                LanguageId = 1,
                CountOfWordsUsed = 10
            };

            var phrases = GeneratePhrases(5, (i, p) =>
            {
                p.LanguageId = languages[i % languages.Count];
                return p;
            });

            var lastRepetitonTime = new DateTime(2017, 1, 9);
            var currentTime = new DateTime(2017, 1, 11);

            var knowledgeLevels = new List<KnowledgeLevelDal>()
            {
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 1, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 2, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 3, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 4, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = WriteTranslationTaskService.WriteTranslationTaskId, PhraseId = 5, CurrentLevel = 4 * 24 * 60 * 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
                new KnowledgeLevelDal() { TaskId = 99, PhraseId = 1, CurrentLevel = 60, LastRepetitonTime = lastRepetitonTime },
            };

            var frameworkMock = GetFrameworkStub();
            frameworkMock
                .Setup(f => f.UtcNow)
                .Returns(currentTime);

            RunNewTask_Test(
                phrases: phrases,
                knowledgeLevels: knowledgeLevels,
                settings: settings,
                expectedPhrasesIds: expectedPhrasesIds,
                framework: frameworkMock.Object
            );
        }

        protected void RunNewTask_Test(IList<PhraseDal> phrases,
            IList<KnowledgeLevelDal> knowledgeLevels,
            object settings,
            IList<int> expectedPhrasesIds,
            IFramework framework = null)
        {
            /* Arrange */
            CreatePhrasesMockDbSet(phrases);
            CreateKnowledgeLevelsMockDbSet(knowledgeLevels);

            /* Act */
            var service = CreateService(framework);
            var actual = service.RunNewTask(settings) as QuizTaskRunModel;

            /* Assert */
            AssertWriteTranslationTaskRunModelContract(actual);

            // actual == null is checked in AssertWriteTranslationTaskRunModelContract
            // ReSharper disable PossibleNullReferenceException
            AssertPhrasesIds(expectedPhrasesIds, actual.Phrases);
        }

        // ReSharper disable once UnusedParameter.Local
        private void AssertWriteTranslationTaskRunModelContract(object actual)
        {
            var typedActual = actual as QuizTaskRunModel;
            Assert.IsNotNull(typedActual);
            Assert.IsNotNull(typedActual.Phrases);
        }
    }
}