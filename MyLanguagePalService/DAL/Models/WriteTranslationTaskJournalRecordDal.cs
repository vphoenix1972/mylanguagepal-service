using System;

namespace MyLanguagePalService.DAL.Models
{
    public class WriteTranslationTaskJournalRecordDal
    {
        public int Id { get; set; }

        public int PhraseId { get; set; }

        public DateTime LastRepetitonTime { get; set; }

        public int CorrectWrongAnswersDelta { get; set; }

        public PhraseDal Phrase { get; set; }
    }
}