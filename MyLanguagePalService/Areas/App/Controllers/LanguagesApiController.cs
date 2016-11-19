using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using MyLanguagePalService.Areas.App.Models.Controller.LanguagesApi;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.Areas.App.Controllers
{
    [RoutePrefix("api/languages")]
    public class LanguagesApiController : WebApiControllerBase
    {
        private readonly IApplicationDbContext _db;

        public LanguagesApiController()
        {
            _db = new ApplicationDbContext();
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<LanguagesApiAm> GetAllLanguages()
        {
            return _db.Languages.Select(ToAm).ToList();
        }

        [Route("{id:int}")]
        [HttpGet]
        public IHttpActionResult GetLanguage(int id)
        {
            var languageDal = _db.Languages.Find(id);
            if (languageDal == null)
            {
                return NotFound();
            }
            return Ok(ToAm(languageDal));
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult CreateLanguage(LanguagesApiCreateIm im)
        {
            /* Validation */
            IHttpActionResult actionResult;
            if (ValidateLanguageIm(im, out actionResult))
                return actionResult;

            /* Create a language */
            _db.Languages.Add(new LanguageDal()
            {
                Name = im.Name
            });
            _db.SaveChanges();

            return Ok();
        }

        [Route("{id:int}")]
        [HttpPut]
        public IHttpActionResult UpdateLanguage(int id, LanguagesApiCreateIm im)
        {
            /* Validation */
            var languageDal = _db.Languages.Find(id);
            if (languageDal == null)
            {
                return NotFound();
            }

            IHttpActionResult actionResult;
            if (ValidateLanguageIm(im, out actionResult))
                return actionResult;

            /* Update the language */
            languageDal.Name = im.Name;

            _db.Entry(languageDal).State = EntityState.Modified;
            _db.SaveChanges();

            return Ok();
        }

        [Route("{id:int}")]
        [HttpDelete]
        public IHttpActionResult DeleteLanguage(int id)
        {
            /* Validation */
            var languageDal = _db.Languages.Find(id);
            if (languageDal == null)
            {
                return NotFound();
            }

            /* Delete the language */
            _db.Languages.Remove(languageDal);
            _db.SaveChanges();

            return Ok();
        }

        private bool ValidateLanguageIm(LanguagesApiCreateIm im, out IHttpActionResult actionResult)
        {
            if (im == null)
            {
                ModelState.AddModelError("Language", "Language cannot be null");

                actionResult = BadRequest(ModelState);
                return true;
            }

            if (string.IsNullOrWhiteSpace(im.Name))
            {
                ModelState.AddModelError(nameof(LanguagesApiCreateIm.Name), "Language name cannot be empty");

                actionResult = UnprocessableEntity(ModelState);
                return true;
            }

            actionResult = null;
            return false;
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
