using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks;
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

        IList<ITaskService> Tasks { get; }

        void Save();
    }
}