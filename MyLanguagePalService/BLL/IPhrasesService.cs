using System.Collections.Generic;
using JetBrains.Annotations;
using MyLanguagePalService.BLL.Models;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL
{
    public interface IPhrasesService
    {
        [NotNull]
        IList<PhraseDal> GetPhrases();

        [CanBeNull]
        PhraseDal GetPhrase(int id);

        [NotNull]
        IList<TranslationBll> GetTranslations([NotNull] PhraseDal phraseDal);

        void CreatePhrase([NotNull] string text, int languageId, [CanBeNull] IList<TranslationImBll> translations);

        void UpdatePhrase([NotNull] PhraseDal phrase, [NotNull] string text, [CanBeNull] IList<TranslationImBll> translations);

        void DeletePhrase([NotNull] PhraseDal phraseDal);
    }
}