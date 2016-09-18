﻿namespace MyLanguagePalService.DAL.Models
{
    public class PhraseDal
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int LanguageId { get; set; }

        public virtual LanguageDal Language { get; set; }
    }
}