using System.Collections.Generic;
using System.Linq;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Languages
{
    public class LanguagesService : ServiceBase,
                                    ILanguagesService
    {
        private readonly IApplicationDbContext _db;

        public LanguagesService(IApplicationDbContext db)
        {
            _db = db;
        }

        public LanguageModel GetDefaultLanguage()
        {
            return GetLanguages().First();
        }

        public IList<LanguageModel> GetLanguages()
        {
            return _db.Languages.Select(ToModel).ToList();
        }

        public LanguageModel GetLanguage(int id)
        {
            var languageDal = _db.Languages.Find(id);
            if (languageDal == null)
                return null;

            return ToModel(languageDal);
        }

        public bool CheckIfLanguageExists(int id)
        {
            return _db.Languages.Find(id) != null;
        }

        private LanguageModel ToModel(LanguageDal entity)
        {
            return new LanguageModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}