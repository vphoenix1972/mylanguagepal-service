using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MyLanguagePalService.Areas.App.Models.Controller.PhrasesApi;
using MyLanguagePalService.BLL;
using MyLanguagePalService.BLL.Models;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;

namespace MyLanguagePalService.Areas.App.Controllers
{
    [RoutePrefix("api/phrases")]
    public class PhrasesApiController : WebApiControllerBase
    {
        private readonly IApplicationDbContext _db;
        private readonly IPhrasesService _phrasesService;

        public PhrasesApiController()
        {
            _db = new ApplicationDbContext();
            _phrasesService = new PhrasesService(_db);
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<PhrasesApiGetAllAm> GetAllPhrases()
        {
            return _phrasesService.GetPhrases().Select(dal => new PhrasesApiGetAllAm()
            {
                Id = dal.Id,
                Text = dal.Text
            });
        }


        [Route("{id:int}")]
        [HttpGet]
        public IHttpActionResult GetPhrase(int id)
        {
            var phraseDal = _phrasesService.GetPhrase(id);
            if (phraseDal == null)
            {
                return NotFound();
            }

            return Ok(new PhrasesApiDetailsAm()
            {
                Id = phraseDal.Id,
                Text = phraseDal.Text,
                Translations = _phrasesService.GetTranslations(phraseDal).Select(t => new TranslationAm()
                {
                    Id = t.Phrase.Id,
                    Text = t.Phrase.Text,
                    Prevalence = t.Prevalence
                }).ToList()
            });
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult CreatePhrase(PhrasesApiCreateIm inputModel)
        {
            // *** Request validation ***
            if (inputModel == null)
            {
                ModelState.AddModelError("Phrase", "Phrase cannot be null");
                return BadRequest(ModelState);
            }

            if (inputModel.LanguageId == null)
            {
                // User cannot send request without language id using UI
                // Unsupported request
                ModelState.AddModelError(nameof(PhrasesApiCreateIm.LanguageId), "Language id cannot be null");
                return BadRequest(ModelState);
            }

            // *** Phrase creation ***
            try
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                _phrasesService.CreatePhrase(inputModel.Text,
                    inputModel.LanguageId.Value,
                    inputModel.Translations.Select(ToTranslationImBll).ToList());
            }
            catch (ValidationFailedException vfe)
            {
                return UnprocessableEntity(vfe);
            }

            return Ok();
        }


        [Route("{id:int}")]
        [HttpPut]
        public IHttpActionResult UpdatePhrase(int id, PhrasesApiCreateIm inputModel)
        {
            // *** Request validation ***

            // Find phrase in database
            var phraseDal = _db.Phrases.Find(id);
            if (phraseDal == null)
            {
                return NotFound();
            }

            // *** Phrase modification ***
            try
            {
                _phrasesService.UpdatePhrase(phraseDal,
                    inputModel.Text,
                    inputModel.Translations.Select(ToTranslationImBll).ToList());
            }
            catch (ValidationFailedException vfe)
            {
                return UnprocessableEntity(vfe);
            }

            return Ok();
        }

        [Route("{id:int}")]
        [HttpDelete]
        public IHttpActionResult DeletePhrase(int id)
        {
            /* Validation */
            var dal = _phrasesService.GetPhrase(id);
            if (dal == null)
            {
                return NotFound();
            }

            /* Delete the phrase */
            _phrasesService.DeletePhrase(dal);

            return Ok();
        }

        private TranslationImBll ToTranslationImBll(TranslationIm im)
        {
            if (im == null)
                return null;

            return new TranslationImBll()
            {
                Text = im.Text,
                Prevalence = im.Prevalence
            };
        }
    }
}
