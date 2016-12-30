using System.Web.Http;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.Core;

namespace MyLanguagePalService.Areas.App.Controllers
{
    //[RoutePrefix("api/tasks/sprint")]
    //public class SprintTaskApiController : AppApiControllerBase
    //{
    //    private readonly ISprintTaskService _sprintTaskService;

    //    public SprintTaskApiController()
    //    {
    //        _sprintTaskService = ServiceManager.SprintTaskService;
    //    }

    //    [Route("settings")]
    //    [HttpGet]
    //    public IHttpActionResult GetSettings()
    //    {
    //        var settings = _sprintTaskService.GetSettings();
    //        return Ok(settings);
    //    }

    //    [Route("settings")]
    //    [HttpPost]
    //    public IHttpActionResult SetSettings(SprintTaskSettingModel settings)
    //    {
    //        // *** Request validation ***
    //        if (settings == null)
    //        {
    //            ModelState.AddModelError("Settings", "Settings cannot be null");
    //            return BadRequest(ModelState);
    //        }

    //        try
    //        {
    //            _sprintTaskService.SetSettings(settings);
    //            ServiceManager.Save();
    //        }
    //        catch (ValidationFailedException vfe)
    //        {
    //            return UnprocessableEntity(vfe);
    //        }

    //        return Ok();
    //    }

    //    [Route("run")]
    //    [HttpPost]
    //    public IHttpActionResult RunNewTask(SprintTaskSettingModel settings)
    //    {
    //        // *** Request validation ***
    //        if (settings == null)
    //        {
    //            ModelState.AddModelError("Settings", "Settings cannot be null");
    //            return BadRequest(ModelState);
    //        }

    //        SprintTaskRunModel result;
    //        try
    //        {
    //            result = _sprintTaskService.RunNewTask(settings);
    //        }
    //        catch (ValidationFailedException vfe)
    //        {
    //            return UnprocessableEntity(vfe);
    //        }

    //        return Ok(result);
    //    }

    //    [Route("finish")]
    //    [HttpPost]
    //    public IHttpActionResult FinishTask(SprintTaskFinishedSummaryModel summary)
    //    {
    //        // *** Request validation ***
    //        if (summary == null)
    //        {
    //            ModelState.AddModelError(nameof(summary), "Summary cannot be null");
    //            return BadRequest(ModelState);
    //        }

    //        try
    //        {
    //            _sprintTaskService.FinishTask(summary);
    //            ServiceManager.Save();
    //        }
    //        catch (ValidationFailedException vfe)
    //        {
    //            return UnprocessableEntity(vfe);
    //        }

    //        return Ok();
    //    }
    //}
}