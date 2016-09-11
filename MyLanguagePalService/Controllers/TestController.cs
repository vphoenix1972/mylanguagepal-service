using System;

namespace MyLanguagePalService.Controllers
{
    public class TestController : SecuredSiteControllerBase
    {
        // GET: Crash
        public string Crash()
        {
            throw new Exception();
        }
    }
}