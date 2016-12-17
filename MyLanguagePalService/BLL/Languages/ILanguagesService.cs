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
        Language GetDefaultLanguage();

        /// <summary>
        /// Returns a list of languages.
        /// </summary>        
        [NotNull]
        IList<Language> GetLanguages();

        /// <summary>
        /// Returns a language with id, or null if language was not found.
        /// </summary>        
        [CanBeNull]
        Language GetLanguage(int id);

        /// <summary>
        /// Checks that language with id exists.
        /// </summary>
        bool CheckIfLanguageExists(int id);
    }
}