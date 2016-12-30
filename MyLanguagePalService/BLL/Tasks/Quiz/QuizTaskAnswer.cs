using System.Collections.Generic;
using JetBrains.Annotations;

namespace MyLanguagePalService.BLL.Tasks.Quiz
{
    public class QuizTaskAnswer
    {
        public int PhraseId { get; set; }

        [NotNull]
        public IList<string> Answers { get; set; } = new List<string>();
    }
}