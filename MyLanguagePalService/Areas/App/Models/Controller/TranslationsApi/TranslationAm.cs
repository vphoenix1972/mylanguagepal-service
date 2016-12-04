using System;
using JetBrains.Annotations;
using MyLanguagePalService.BLL.Models;

namespace MyLanguagePalService.Areas.App.Models.Controller.TranslationsApi
{
    public class TranslationAm
    {
        public int Id { get; set; }

        public int LanguageId { get; set; }

        public string Text { get; set; }

        public int Prevalence { get; set; }


        public static TranslationAm MapFrom([NotNull] TranslationBll translation)
        {
            if (translation == null)
                throw new ArgumentNullException(nameof(translation));

            return new TranslationAm()
            {
                Id = translation.Phrase.Id,
                LanguageId = translation.Phrase.LanguageId,
                Text = translation.Phrase.Text,
                Prevalence = translation.Prevalence
            };
        }
    }
}