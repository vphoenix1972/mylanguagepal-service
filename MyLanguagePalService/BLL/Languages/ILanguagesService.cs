using System.Collections.Generic;
using JetBrains.Annotations;

namespace MyLanguagePalService.BLL.Languages
{
    public interface ILanguagesService
    {
        /// <summary>
        /// Returns the language to use by default.
        /// </summary>        
        [NotNull]
        LanguageModel GetDefaultLanguage();

        /// <summary>
        /// Returns a list of languages.
        /// </summary>        
        [NotNull]
        IList<LanguageModel> GetLanguages();

        /// <summary>
        /// Returns a language with id, or null if language was not found.
        /// </summary>        
        [CanBeNull]
        LanguageModel GetLanguage(int id);

        /// <summary>
        /// Checks that language with id exists.
        /// </summary>
        bool CheckIfLanguageExists(int id);
    }
}