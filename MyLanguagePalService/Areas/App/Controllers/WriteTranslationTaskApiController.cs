using System.Web.Http;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;
using MyLanguagePalService.Core;

namespace MyLanguagePalService.Areas.App.Controllers
{
    [RoutePrefix("api/tasks/writeTranslation")]
    public class WriteTranslationTaskApiController : AppApiControllerBase
    {
        private readonly IWriteTranslationTaskService _writeTranslationTaskService;

        public WriteTranslationTaskApiController()
        {
            _writeTranslationTaskService = ServiceManager.WriteTranslationTaskService;
        }

        [Route("settings")]
        [HttpGet]
        public IHttpActionResult GetSettings()
        {
            var settings = _writeTranslationTaskService.GetSettings();
            return Ok(settings);
        }

        [Route("settings")]
        [HttpPost]
        public IHttpActionResult SetSettings(WriteTranslationTaskSettingModel settings)
        {
            // *** Request validation ***
            if (settings == null)
            {
                ModelState.AddModelError("Settings", "Settings cannot be null");
                return BadRequest(ModelState);
            }

            try
            {
                _writeTranslationTaskService.SetSettings(settings);
                ServiceManager.Save();
            }
            catch (ValidationFailedException vfe)
            {
                return UnprocessableEntity(vfe);
            }

            return Ok();
        }

        [Route("run")]
        [HttpPost]
        public IHttpActionResult RunNewTask(WriteTranslationTaskSettingModel settings)
        {
            // *** Request validation ***
            if (settings == null)
            {
                ModelState.AddModelError("Settings", "Settings cannot be null");
                return BadRequest(ModelState);
            }

            WriteTranslationTaskRunModel result;
            try
            {
                result = _writeTranslationTaskService.RunNewTask(settings);
            }
            catch (ValidationFailedException vfe)
            {
                return UnprocessableEntity(vfe);
            }

            return Ok(result);
        }

        [Route("finish")]
        [HttpPost]
        public IHttpActionResult FinishTask(WriteTranslationTaskFinishedSummaryModel summary)
        {
            // *** Request validation ***
            if (summary == null)
            {
                ModelState.AddModelError(nameof(summary), "Summary cannot be null");
                return BadRequest(ModelState);
            }

            try
            {
                _writeTranslationTaskService.FinishTask(summary);
                ServiceManager.Save();
            }
            catch (ValidationFailedException vfe)
            {
                return UnprocessableEntity(vfe);
            }

            return Ok();
        }
    }
}