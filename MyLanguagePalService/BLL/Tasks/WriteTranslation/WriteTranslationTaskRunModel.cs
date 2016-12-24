using System.Collections.Generic;
using JetBrains.Annotations;
using MyLanguagePalService.BLL.Phrases;

namespace MyLanguagePalService.BLL.Tasks.WriteTranslation
{
    public class WriteTranslationTaskRunModel
    {
        [NotNull]
        public IList<PhraseWithTranslations> Phrases { get; set; } = new List<PhraseWithTranslations>();
    }
}