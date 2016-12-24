using System.Collections.Generic;
using JetBrains.Annotations;

namespace MyLanguagePalService.BLL.Tasks.WriteTranslation
{
    public class WriteTranslationTaskFinishedSummaryModel
    {
        [NotNull]
        public IList<WriteTranslationTaskJournalRecord> Results { get; set; } = new List<WriteTranslationTaskJournalRecord>();
    }
}