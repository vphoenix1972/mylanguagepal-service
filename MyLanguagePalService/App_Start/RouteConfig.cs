using System.Web.Mvc;
using System.Web.Routing;

namespace MyLanguagePalService
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Predefined routes
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = "About|Error|Languages" }
            );

            // Show http 404 for any other routes
            routes.MapRoute(
                "catchall",
                "{*url}",
                new { controller = "Error", action = "Http404" }
            );
        }
    }
}
