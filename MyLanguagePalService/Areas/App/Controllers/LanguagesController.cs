using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MyLanguagePalService.Areas.App.Models.Controller.LanguagesApi;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.Areas.App.Controllers
{
    public class LanguagesController : WebApiControllerBase
    {
        private readonly IApplicationDbContext _db;

        public LanguagesController()
        {
            _db = new ApplicationDbContext();
        }

        public IEnumerable<LanguagesApiAm> GetAllLanguages()
        {
            return _db.Languages.Select(ToAm).ToList();
        }

        public IHttpActionResult GetLanguage(int id)
        {
            var languageDal = _db.Languages.Find(id);
            if (languageDal == null)
            {
                return NotFound();
            }
            return Ok(ToAm(languageDal));
        }

        [HttpPost]
        public IHttpActionResult CreateLanguage(LanguagesApiCreateIm im)
        {
            /* Validation */
            if (im == null)
            {
                ModelState.AddModelError("Language", "Language cannot be null");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(im.Name))
            {
                ModelState.AddModelError(nameof(LanguagesApiCreateIm.Name), "Language name cannot be empty");
                return UnprocessableEntity(ModelState);
            }

            /* Create a language */
            _db.Languages.Add(new LanguageDal()
            {
                Name = im.Name
            });
            _db.SaveChanges();

            return Ok();
        }

        private LanguagesApiAm ToAm(LanguageDal languageDal)
        {
            return new LanguagesApiAm()
            {
                Id = languageDal.Id,
                Name = languageDal.Name
            };
        }
    }
}
