using System.Collections.Generic;
using System.Web.Http;

namespace MyLanguagePalService.Areas.App.Controllers
{
    public class TestApiController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        public string Get(int id)
        {
            return "value";
        }
    }
}
