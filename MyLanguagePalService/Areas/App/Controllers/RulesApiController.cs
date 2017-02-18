using System;
using System.Collections.Generic;
using System.Web.Http;
using MyLanguagePal.Shared.Exceptions;
using MyLanguagePalService.BLL;
using MyLanguagePalService.BLL.Rules;
using MyLanguagePalService.Core.Extensions;

namespace MyLanguagePalService.Areas.App.Controllers
{
    [RoutePrefix("api/rules")]
    public class RulesApiController : AppApiControllerBase
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetRules()
        {
            return HandleRequest(() => ServiceManager.RulesService.GetRules());
        }

        [Route("{id:int}")]
        [HttpGet]
        public IHttpActionResult GetRule(int id)
        {
            Rule result;

            try
            {
                result = ServiceManager.RulesService.GetRule(id);
            }
            catch (MlpException e)
            {
                return BadRequest(e);
            }

            if (result == null)
                return RuleNotFound(id);

            return Ok(result);
        }

        private IHttpActionResult RuleNotFound(int id)
        {
            return NotFound(
            new
            {
                Code = MlpServiceErrorCodes.RuleNotFound,
                Id = id
            });
        }

        private IHttpActionResult Ok<T>(Func<T> request)
        {
            return Ok(HandleRequest(request));
        }
        private IHttpActionResult HandleRequest<T>(Func<T> request)
        {
            T result;

            try
            {
                result = request();
            }
            catch (MlpNotFoundException e)
            {
                return NotFound(e);
            }
            catch (MlpException e)
            {
                return BadRequest(e);
            }

            return Ok(result);
        }
    }
}