using System;
using JetBrains.Annotations;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;

namespace MyLanguagePalService.BLL
{
    public interface IServiceManager : IDisposable
    {
        [NotNull]
        ILanguagesService LanguagesService { get; }

        [NotNull]
        IPhrasesService PhrasesService { get; }

        [NotNull]
        ISprintTaskService SprintTaskService { get; }


        [NotNull]
        IWriteTranslationTaskService WriteTranslationTaskService { get; }

        void Save();
    }
}