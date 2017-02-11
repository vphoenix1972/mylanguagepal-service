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
            Definition = other.Definition;
        }

        public Phrase([NotNull] PhraseDal dal)
        {
            if (dal == null)
                throw new ArgumentNullException(nameof(dal));

            Id = dal.Id;
            LanguageId = dal.LanguageId;
            Text = dal.Text;
            Definition = dal.Definition;
        }

        public int Id { get; set; }

        public int LanguageId { get; set; }

        [NotNull]
        public string Text { get; set; } = string.Empty;

        [CanBeNull]
        public string Definition { get; set; }

        [NotNull]
        public static Phrase MapFrom([NotNull] PhraseDal dal)
        {
            return new Phrase(dal);
        }

        [NotNull]
        public static PhraseDal MapFrom([NotNull] Phrase phrase, [NotNull] PhraseDal dal)
        {
            if (phrase == null)
                throw new ArgumentNullException(nameof(phrase));
            if (dal == null)
                throw new ArgumentNullException(nameof(dal));

            dal.Id = phrase.Id;
            dal.Text = phrase.Text;
            dal.Definition = phrase.Definition;
            dal.LanguageId = phrase.LanguageId;

            return dal;
        }
    }
}