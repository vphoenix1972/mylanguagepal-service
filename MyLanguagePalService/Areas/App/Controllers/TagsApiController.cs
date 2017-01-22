using System;
using System.Collections.Generic;
using System.Web.Http;
using MyLanguagePalService.BLL.Tags;
using MyLanguagePalService.Core;

namespace MyLanguagePalService.Areas.App.Controllers
{
    [RoutePrefix("api/tags")]
    public class TagsApiController : AppApiControllerBase
    {
        [Route("")]
        [HttpGet]
        public IList<Tag> GetAll()
        {
            return ServiceManager.TagsService.GetTags();
        }

        [Route("{id:int}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var tag = ServiceManager.TagsService.GetTag(id);
            if (tag == null)
                return NotFound();

            return Ok(tag);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult Create(Tag tag)
        {
            try
            {
                var result = ServiceManager.TagsService.CreateTag(tag);
                return Ok(result);
            }
            catch (ValidationFailedException vfe)
            {
                return UnprocessableEntity(vfe);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Tag cannot be null");
            }
        }

        [Route("{id:int}")]
        [HttpPut]
        public IHttpActionResult Update(int id, Tag tag)
        {
            try
            {
                var result = ServiceManager.TagsService.UpdateTag(id, tag);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (ValidationFailedException vfe)
            {
                return UnprocessableEntity(vfe);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Tag cannot be null");
            }
        }

        [Route("{id:int}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var result = ServiceManager.TagsService.DeleteTag(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}