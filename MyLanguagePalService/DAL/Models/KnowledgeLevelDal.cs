using System;

namespace MyLanguagePalService.DAL.Models
{
    public class KnowledgeLevelDal
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public int PhraseId { get; set; }

        public DateTime LastRepetitonTime { get; set; }

        public double CurrentLevel { get; set; }

        public double? PreviousLevel { get; set; }

        public PhraseDal Phrase { get; set; }
    }
}