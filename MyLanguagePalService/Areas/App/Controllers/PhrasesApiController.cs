using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MyLanguagePalService.Areas.App.Models.Controller.PhrasesApi;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;

namespace MyLanguagePalService.Areas.App.Controllers
{
    [RoutePrefix("api/phrases")]
    public class PhrasesApiController : WebApiControllerBase
    {
        private readonly IApplicationDbContext _db;
        private readonly IApplicationDbManager _dbManager;

        public PhrasesApiController()
        {
            _db = new ApplicationDbContext();
            _dbManager = new ApplicationDbManager(_db);
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<PhrasesApiGetAllAm> GetAllPhrases()
        {
            return _dbManager.GetPhrases().Select(dal => new PhrasesApiGetAllAm()
            {
                Id = dal.Id,
                Text = dal.Text
            });
        }


        [Route("{id:int}")]
        [HttpGet]
        public IHttpActionResult GetPhrase(int id)
        {
            var phraseDal = _dbManager.GetPhrase(id);
            if (phraseDal == null)
            {
                return NotFound();
            }

            return Ok(new PhrasesApiDetailsAm()
            {
                Id = phraseDal.Id,
                Text = phraseDal.Text,
                Translations = _dbManager.GetTranslations(phraseDal).Select(t => new TranslationAm()
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
            PreparePhraseText(inputModel);
            PreparePhraseTranslations(inputModel);

            try
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                _dbManager.CreatePhrase(inputModel.Text, inputModel.LanguageId.Value, inputModel.Translations);
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
            PreparePhraseText(inputModel);
            PreparePhraseTranslations(inputModel);

            try
            {
                _dbManager.UpdatePhrase(phraseDal, inputModel.Text, inputModel.Translations);
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
            var dal = _dbManager.GetPhrase(id);
            if (dal == null)
            {
                return NotFound();
            }

            /* Delete the phrase */
            _dbManager.DeletePhrase(dal);

            return Ok();
        }

        private void PreparePhraseTranslations(PhrasesApiCreateIm inputModel)
        {
            if (inputModel.Translations != null)
            {
                inputModel.Translations = inputModel.Translations
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Trim())
                    .ToList();
            }
        }

        private void PreparePhraseText(PhrasesApiCreateIm inputModel)
        {
            if (inputModel.Text != null)
            {
                inputModel.Text = inputModel.Text.Trim();
            }
        }
    }
}
