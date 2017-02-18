using System.Collections.Generic;
using JetBrains.Annotations;

namespace MyLanguagePalService.BLL.Rules
{
    public interface IRulesService
    {
        [NotNull]
        IList<Rule> GetRules();

        [CanBeNull]
        Rule GetRule(int id);
    }
}