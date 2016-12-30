using System.Collections.Generic;
using JetBrains.Annotations;

namespace MyLanguagePalService.BLL.Tasks.Quiz
{
    public class QuizTaskAnswersModel
    {
        [NotNull]
        public IList<QuizTaskAnswer> Answers { get; set; } = new List<QuizTaskAnswer>();
    }
}