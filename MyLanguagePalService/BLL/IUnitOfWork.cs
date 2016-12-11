using System;
using JetBrains.Annotations;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Tasks.Sprint;

namespace MyLanguagePalService.BLL
{
    public interface IUnitOfWork : IDisposable
    {
        [NotNull]
        ILanguagesService LanguagesService { get; }

        [NotNull]
        IPhrasesService PhrasesService { get; }

        [NotNull]
        ISprintTaskService SprintTaskService { get; }

        void Save();
    }
}