using MyLanguagePal.Shared.Models;

namespace MyLanguagePalService.BLL.Rules
{
    public class Rule : Entity
    {
        public string Name { get; set; }

        public string Content { get; set; }
    }
}