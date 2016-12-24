namespace MyLanguagePalService.BLL.Tasks.WriteTranslation
{
    public class WriteTranslationTaskJournalRecord
    {
        public int PhraseId { get; set; }

        public int CorrectAnswersCount { get; set; }

        public int WrongAnswersCount { get; set; }
    }
}