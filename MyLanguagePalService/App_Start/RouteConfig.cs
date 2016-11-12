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
            var route = routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );
            // This allows default redirection to Area Controller
            // See http://stackoverflow.com/questions/2140208/how-to-set-a-default-route-to-an-area-in-mvc for details
            route.DataTokens = new RouteValueDictionary(new { area = "App" });
        }
    }
}
