using System.Collections.Generic;
using JetBrains.Annotations;
using MyLanguagePalService.DAL.Dto;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.DAL
{
    public interface IApplicationDbManager
    {
        [NotNull]
        IList<PhraseDal> GetPhrases();

        [CanBeNull]
        PhraseDal GetPhrase(int id);

        [NotNull]
        IList<TranslationDto> GetTranslations([NotNull] PhraseDal phraseDal);

        void CreatePhrase([NotNull] string text, int languageId, [CanBeNull] IList<string> translations);

        void UpdatePhrase([NotNull] PhraseDal phrase, [NotNull] string text, [CanBeNull] IList<string> translations);

        void DeletePhrase([NotNull] PhraseDal phraseDal);
    }
}