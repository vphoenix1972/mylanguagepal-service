﻿using System;
using System.Linq;
using System.Web.Http;
using MyLanguagePalService.Areas.App.Models.Controller.TasksApi;
using MyLanguagePalService.BLL.Tasks;
using MyLanguagePalService.Core;

namespace MyLanguagePalService.Areas.App.Controllers
{
    [RoutePrefix("api/tasks")]
    public class TasksApiController : AppApiControllerBase
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetTasks()
        {
            return Ok(ServiceManager.Tasks.Select(task => new { name = task.Name }));
        }

        [Route("{name}/settings")]
        [HttpGet]
        public IHttpActionResult GetSettings(string name)
        {
            var task = FindTask(name);
            if (task == null)
                return TaskNotFound(name);

            return Ok((task.GetSettings()));
        }

        [Route("{name}/settings")]
        [HttpPost]
        public IHttpActionResult SetSettings(string name, [FromBody] object settings)
        {
            var task = FindTask(name);
            if (task == null)
                return TaskNotFound(name);

            object result;
            try
            {
                result = task.SetSettings(settings);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (ValidationFailedException vfe)
            {
                return UnprocessableEntity(vfe);
            }

            ServiceManager.Save();

            return Ok(result);
        }

        [Route("{name}/run")]
        [HttpPost]
        public IHttpActionResult RunTask(string name, [FromBody] object settings)
        {
            var task = FindTask(name);
            if (task == null)
                return TaskNotFound(name);

            object result;
            try
            {
                result = task.RunNewTask(settings);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (ValidationFailedException vfe)
            {
                return UnprocessableEntity(vfe);
            }

            return Ok(result);
        }

        [Route("{name}/finish")]
        [HttpPost]
        public IHttpActionResult FinishTask(string name, [FromBody] FinishTaskModel model)
        {
            var task = FindTask(name);
            if (task == null)
                return TaskNotFound(name);

            object result;
            try
            {
                // ReSharper disable AssignNullToNotNullAttribute
                result = task.FinishTask(model?.Settings, model?.AnswersModel);
                // ReSharper restore AssignNullToNotNullAttribute
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (ValidationFailedException vfe)
            {
                return UnprocessableEntity(vfe);
            }

            ServiceManager.Save();

            return Ok(result);
        }

        private ITaskService FindTask(string name)
        {
            return ServiceManager.Tasks.FirstOrDefault(task => task.Name == name);
        }

        private IHttpActionResult TaskNotFound(string name)
        {
            return NotFound($"Task '{name}' does not exist");
        }
    }
}