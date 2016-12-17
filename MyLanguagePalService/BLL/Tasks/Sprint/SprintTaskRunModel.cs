using System.Collections.Generic;
using JetBrains.Annotations;
using MyLanguagePalService.BLL.Phrases;

namespace MyLanguagePalService.BLL.Tasks.Sprint
{
    public class SprintTaskRunModel
    {
        [NotNull]
        public IList<PhraseWithTranslations> Phrases { get; set; } = new List<PhraseWithTranslations>();
    }
}