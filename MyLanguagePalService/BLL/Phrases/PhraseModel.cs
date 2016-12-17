using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Phrases
{
    public class PhraseModel
    {
        public int Id { get; set; }

        public int LanguageId { get; set; }

        [NotNull]
        public string Text { get; set; } = string.Empty;

        [NotNull]
        public IList<TranslationModel> Translations { get; set; } = new List<TranslationModel>();

        [NotNull]
        public static PhraseModel MapFrom([NotNull] PhraseDal dal)
        {
            if (dal == null)
                throw new ArgumentNullException(nameof(dal));

            return new PhraseModel()
            {
                Id = dal.Id,
                LanguageId = dal.LanguageId,
                Text = dal.Text
            };
        }
    }
}