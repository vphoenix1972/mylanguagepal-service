using System.Collections.Generic;
using JetBrains.Annotations;
using MyLanguagePalService.BLL.Phrases;

namespace MyLanguagePalService.BLL.Tasks.Quiz
{
    public class QuizTaskRunModel
    {
        [NotNull]
        public IList<Phrase> Phrases { get; set; } = new List<Phrase>();
    }
}