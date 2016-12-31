using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks;
using MyLanguagePalService.BLL.Tasks.Quiz;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;
using MyLanguagePalService.Tests.TestsShared;
using Newtonsoft.Json;

namespace MyLanguagePalService.Tests.BLL.Tasks
{
    public abstract class TaskTestBase : TestBase
    {
        protected void ShouldCheckLanguageId(Func<ILanguagesService, ITaskService> createService, Action<ITaskService, QuizTaskSettings> method)
        {
            /* Arrange */
            var taskSettingsDbSetMock = CreateTaskSettingsMockDbSet();

            var languageServiceMock = GetLanguageServiceStub();
            languageServiceMock.Setup(m => m.CheckIfLanguageExists(It.IsAny<int>())).Returns(false);
            var languagesService = languageServiceMock.Object;

            /* Act */
            var service = createService(languagesService);
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

        protected void ShouldCheckCountOfWordsUsed(Func<ITaskService> createService, Action<ITaskService, QuizTaskSettings> method, int minCountOfWordsUsed, int maxCountOfWordsUsed)
        {
            /* Arrange */
            var taskSettingsDbSetMock = CreateTaskSettingsMockDbSet();

            /* Act */
            var service = createService();
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
    }
}