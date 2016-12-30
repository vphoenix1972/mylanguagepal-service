using System.Collections.Generic;
using JetBrains.Annotations;
using MyLanguagePalService.BLL.Phrases;

namespace MyLanguagePalService.BLL.Tasks.Quiz
{
    public class QuizTaskSummary
    {
        [NotNull]
        public IList<QuizTaskResult<Phrase>> Results { get; set; } = new List<QuizTaskResult<Phrase>>();
    }
}