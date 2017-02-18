using System.Collections.Generic;
using System.Linq;
using MyLanguagePal.Shared.Exceptions;
using MyLanguagePalService.DAL;

namespace MyLanguagePalService.BLL.Rules
{
    public class RulesService : ServiceBase,
                                IRulesService
    {
        private readonly IApplicationDbContext _db;
        private readonly IList<Rule> _rules;

        public RulesService(IApplicationDbContext db)
        {
            _db = db;

            _rules = new List<Rule>()
            {
                new Rule()
                {
                    Id = 1,
                    Name = "Rule 1",
                    Content = "1234"
                },
                new Rule()
                {
                    Id = 2,
                    Name = "Rule 2",
                    Content = "4321"
                }
            };
        }

        public IList<Rule> GetRules()
        {
            return _rules;
        }

        public Rule GetRule(int id)
        {
            if (id == 4)
                throw new MlpException(3001);

            return _rules.FirstOrDefault((e) => e.Id == id);
        }
    }
}