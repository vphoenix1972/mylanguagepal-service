using System.Web.Mvc;

namespace MyLanguagePalService.Areas.App
{
    public class AppAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "App";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "App_default",
                url: "App/{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}