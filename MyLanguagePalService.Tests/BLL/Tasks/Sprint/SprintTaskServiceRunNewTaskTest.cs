using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLanguagePalService.BLL.Tasks.Sprint;

namespace MyLanguagePalService.Tests.BLL.Tasks.Sprint
{
    [TestClass]
    public class SprintTaskServiceRunNewTaskTest : SprintTaskTestBase
    {
        [TestMethod]
        public void RunNewTask_ShouldCheckLanguageId()
        {
            ShouldCheckLanguageId((service, settings) => service.RunNewTask(settings));
        }

        [TestMethod]
        public void RunNewTask_ShouldCheckCountOfWordsUsed()
        {
            ShouldCheckCountOfWordsUsed((service, settings) => service.RunNewTask(settings),
                SprintTaskService.MinCountOfWordsUsed,
                SprintTaskService.MaxCountOfWordsUsed);
        }


        [TestMethod]
        public void RunNewTask_AllWordsAreNew_ShouldReturnWords_own()
        {
            RunNewTask_AllWordsAreNew_ShouldReturnWords();
        }

        [TestMethod]
        public void RunNewTask_AllWordsAreNew_ShouldReturnWordsAndRespectLanguageId_own()
        {
            RunNewTask_AllWordsAreNew_ShouldReturnWordsAndRespectLanguageId();
        }

        [TestMethod]
        public void RunNewTask_AllWordsAreNew_ShouldReturnWordsAndRespectCountOfWordsUsed_own()
        {
            RunNewTask_AllWordsAreNew_ShouldReturnWordsAndRespectCountOfWordsUsed();
        }

        [TestMethod]
        public void RunNewTask_AllWordsAreNew_ShouldReturnWordsAndRespectCountOfWordsUsedAndRespectLanguageId_own()
        {
            RunNewTask_AllWordsAreNew_ShouldReturnWordsAndRespectCountOfWordsUsedAndRespectLanguageId();
        }

        [TestMethod]
        public void RunNewTask_AllWordsAreNeededToRepeat_ShouldReturnWords_own()
        {
            RunNewTask_AllWordsAreNeededToRepeat_ShouldReturnWords();
        }

        [TestMethod]
        public void RunNewTask_AllWordsAreNeededToRepeat_ShouldReturnWordsAndRespectLanguageId_own()
        {
            RunNewTask_AllWordsAreNeededToRepeat_ShouldReturnWordsAndRespectLanguageId();
        }

        [TestMethod]
        public void RunNewTask_AllWordsAreNeededToRepeat_ShouldReturnWordsAndRespectCountOfWordsUsed_own()
        {
            RunNewTask_AllWordsAreNeededToRepeat_ShouldReturnWordsAndRespectCountOfWordsUsed();
        }

        [TestMethod]
        public void RunNewTask_AllWordsAreNeededToRepeat_ShouldReturnWordsAndRespectCountOfWordsUsedAndRespectLanguageId_own()
        {
            RunNewTask_AllWordsAreNeededToRepeat_ShouldReturnWordsAndRespectCountOfWordsUsedAndRespectLanguageId();
        }

        [TestMethod]
        public void RunNewTask_NewAndRepeatedWords_ShouldReturnNewWords_own()
        {
            RunNewTask_NewAndRepeatedWords_ShouldReturnNewWords();
        }

        [TestMethod]
        public void RunNewTask_NewAndRepeatedWords_ShouldReturnNewWordsAndRespectLanguageId_own()
        {
            RunNewTask_NewAndRepeatedWords_ShouldReturnNewWordsAndRespectLanguageId();
        }

        [TestMethod]
        public void RunNewTask_NewAndRepeatedWords_ShouldReturnNewWordsAndRespectCountOfWordsUsed_own()
        {
            RunNewTask_NewAndRepeatedWords_ShouldReturnNewWordsAndRespectCountOfWordsUsed();
        }

        [TestMethod]
        public void RunNewTask_NewAndRepeatedWords_ShouldReturnNewWordsAndRespectCountOfWordsUsedAndRespectLanguageId_own()
        {
            RunNewTask_NewAndRepeatedWords_ShouldReturnNewWordsAndRespectCountOfWordsUsedAndRespectLanguageId();
        }

        [TestMethod]
        public void RunNewTask_WordsToRepeat_ShouldRespectCurrentLevel_own()
        {
            RunNewTask_WordsToRepeat_ShouldRespectCurrentLevel();
        }

        [TestMethod]
        public void RunNewTask_WordsToRepeat_ShouldRespectCurrentLevelAndRespectCountOfWordsUsed_own()
        {
            RunNewTask_WordsToRepeat_ShouldRespectCurrentLevelAndRespectCountOfWordsUsed();
        }

        [TestMethod]
        public void RunNewTask_WordsToRepeat_ShouldReturnEmptyListIfAllWordsAreRepeated_own()
        {
            RunNewTask_WordsToRepeat_ShouldReturnEmptyListIfAllWordsAreRepeated();
        }
    }
}
