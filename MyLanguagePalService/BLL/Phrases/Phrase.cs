using System;
using JetBrains.Annotations;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Phrases
{
    public class Phrase
    {
        public Phrase()
        {

        }

        public Phrase([NotNull] Phrase other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Id = other.Id;
            LanguageId = other.LanguageId;
            Text = other.Text;
        }

        public int Id { get; set; }

        public int LanguageId { get; set; }

        [NotNull]
        public string Text { get; set; } = string.Empty;

        [NotNull]
        public static Phrase MapFrom([NotNull] PhraseDal dal)
        {
            if (dal == null)
                throw new ArgumentNullException(nameof(dal));

            return new Phrase()
            {
                Id = dal.Id,
                LanguageId = dal.LanguageId,
                Text = dal.Text
            };
        }
    }
}