using System.Collections.Generic;
using JetBrains.Annotations;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Phrases
{
    public interface IPhrasesService
    {
        [NotNull]
        IList<PhraseDal> GetPhrasesDals();

        [NotNull]
        IList<Phrase> GetPhrases();

        [NotNull]
        IList<Translation> GetTranslations(int phraseId);

        [NotNull]
        IList<TranslationModelBbl> GetTranslations([NotNull] PhraseDal phraseDal);

        [CanBeNull]
        PhraseDal GetPhrase(int id);

        int CreatePhrase([NotNull] string text, int languageId, [CanBeNull] IList<TranslationImBll> translations);

        void UpdatePhrase([NotNull] PhraseDal phrase, [NotNull] string text, [CanBeNull] IList<TranslationImBll> translations);

        void DeletePhrase([NotNull] PhraseDal phraseDal);

        bool CheckIfPhraseExists(int id);
    }
}