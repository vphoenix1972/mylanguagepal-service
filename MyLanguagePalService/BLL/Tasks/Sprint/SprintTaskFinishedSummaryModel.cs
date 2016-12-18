using System.Collections.Generic;
using JetBrains.Annotations;

namespace MyLanguagePalService.BLL.Tasks.Sprint
{
    public class SprintTaskFinishedSummaryModel
    {
        [NotNull]
        public IList<SprintTaskJournalRecord> Results { get; set; } = new List<SprintTaskJournalRecord>();
    }
}