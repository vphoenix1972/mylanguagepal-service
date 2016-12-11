using System.Web.Http;
using MyLanguagePalService.Areas.App.Models.Controller.SprintTaskApi;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.Core;

namespace MyLanguagePalService.Areas.App.Controllers
{
    [RoutePrefix("api/tasks/sprint")]
    public class SprintTaskApiController : AppApiControllerBase
    {
        private readonly ISprintTaskService _sprintTaskService;

        public SprintTaskApiController()
        {
            _sprintTaskService = UnitOfWork.SprintTaskService;
        }

        [Route("settings")]
        [HttpGet]
        public IHttpActionResult GetSettings()
        {
            var settings = _sprintTaskService.GetSettings();
            return Ok(ToAm(settings));
        }

        [Route("settings")]
        [HttpPost]
        public IHttpActionResult SetSettings(SprintTaskApiSettingsAm inputModel)
        {
            // *** Request validation ***
            if (inputModel == null)
            {
                ModelState.AddModelError("Settings", "Settings cannot be null");
                return BadRequest(ModelState);
            }

            try
            {
                _sprintTaskService.SetSettings(FromAm(inputModel));
                UnitOfWork.Save();
            }
            catch (ValidationFailedException vfe)
            {
                return UnprocessableEntity(vfe);
            }

            return Ok();
        }

        private SprintTaskApiSettingsAm ToAm(SprintTaskSettingModel settings)
        {
            return new SprintTaskApiSettingsAm()
            {
                TotalTimeForTask = settings.TotalTimeForTask,
                CountOfWordsUsed = settings.CountOfWordsUsed,
                LanguageId = settings.LanguageId
            };
        }

        private SprintTaskSettingModel FromAm(SprintTaskApiSettingsAm inputModel)
        {
            return new SprintTaskSettingModel()
            {
                TotalTimeForTask = inputModel.TotalTimeForTask,
                CountOfWordsUsed = inputModel.CountOfWordsUsed,
                LanguageId = inputModel.LanguageId
            };
        }
    }
}