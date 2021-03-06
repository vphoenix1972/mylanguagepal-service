﻿using System.Collections.Generic;

namespace MyLanguagePalService.DAL.Models
{
    public class LanguageDal
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<PhraseDal> Phrases { get; set; }
    }
}