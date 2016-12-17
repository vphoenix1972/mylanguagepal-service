using System.Linq;
using System.Web.Http;
using MyLanguagePalService.Areas.App.Models.Controller.TranslationsApi;
using MyLanguagePalService.BLL;
using MyLanguagePalService.BLL.Phrases;

namespace MyLanguagePalService.Areas.App.Controllers
{
    [RoutePrefix("api/phrases/{phraseId:int}/translations")]
    public class TranslationsApiController : AppApiControllerBase
    {
        private readonly IPhrasesService _phrasesService;

        public TranslationsApiController()
        {
            _phrasesService = ServiceManager.PhrasesService;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult Get(int phraseId)
        {
            /* Validation */
            var phrase = _phrasesService.GetPhrase(phraseId);
            if (phrase == null)
            {
                return PhraseNotFound(phraseId);
            }

            /* Get data */

            var translations = _phrasesService.GetTranslations(phrase).Select(TranslationAm.MapFrom);

            return Ok(translations);
        }
    }
}