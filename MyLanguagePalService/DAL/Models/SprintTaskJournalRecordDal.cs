using System;

namespace MyLanguagePalService.DAL.Models
{
    public class SprintTaskJournalRecordDal
    {
        public int Id { get; set; }

        public int PhraseId { get; set; }

        public DateTime CreationTime { get; set; }

        public int CorrectAnswersCount { get; set; }

        public int WrongAnswersCount { get; set; }

        public PhraseDal Phrase { get; set; }
    }
}