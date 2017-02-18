using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Rules;
using MyLanguagePalService.BLL.Tasks;

namespace MyLanguagePalService.BLL
{
    public interface IServiceManager : IDisposable
    {
        [NotNull]
        ILanguagesService LanguagesService { get; }

        [NotNull]
        IPhrasesService PhrasesService { get; }


        [NotNull]
        IRulesService RulesService { get; }

        IList<ITaskService> Tasks { get; }

        void Save();
    }
}