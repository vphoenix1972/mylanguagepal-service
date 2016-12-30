using System.Collections.Generic;
using JetBrains.Annotations;
using MyLanguagePalService.BLL.Phrases;

namespace MyLanguagePalService.BLL.Tasks.Quiz
{
    public class QuizTaskRunModel
    {
        [NotNull]
        public IList<PhraseWithTranslations> Phrases { get; set; } = new List<PhraseWithTranslations>();
    }
}