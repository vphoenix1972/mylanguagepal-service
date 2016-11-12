using System.Web.Optimization;

namespace MyLanguagePalService
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // *** Deps bundles ***

            // Modernizr
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/deps/modernizr-js").Include(
                        "~/Content/deps/modernizr-*"));

            // *** Shared bundles ***

            // *** Site bundles ***
            bundles.Add(new StyleBundle("~/site/site-css").Include(
                "~/Content/deps/bootstrap/css/bootstrap.css",
                "~/Content/shared/shared.css",
                "~/Content/site/site.css"));

            bundles.Add(new ScriptBundle("~/site/site-js").Include(
                "~/Content/deps/jquery/jquery-3.1.0.js",
                "~/Content/deps/bootstrap/js/bootstrap.js",
                "~/Content/deps/respond.js"));

            // *** App bundles ***
            bundles.Add(new StyleBundle("~/app/app-css").Include(
                "~/Content/deps/bootstrap/css/bootstrap.css",
                "~/Content/deps/angular/ngprogress.css",
                "~/Content/shared/shared.css",
                "~/Content/app/shared/app.css",
                "~/Content/app/services/progressBar/progressBarService.css",
                "~/Content/app/pages/shared/page/page.css"));

            bundles.Add(new ScriptBundle("~/app/app-js").Include(
                // Deps
                "~/Content/deps/jquery/jquery-3.1.0.js",
                "~/Content/deps/bootstrap/js/bootstrap.js",
                "~/Content/deps/respond.js",
                "~/Content/deps/angular/angular.js",
                "~/Content/deps/angular/angular-route.js",
                "~/Content/deps/angular/angular-animate.js",
                "~/Content/deps/angular/angular-touch.js",
                "~/Content/deps/angular/ui-bootstrap-tpls-2.2.0.js",
                "~/Content/deps/angular/ngprogress.js",
                // App scripts
                "~/Content/app/shared/utils.js",
                "~/Content/app/shared/promiseQueueService.js",
                "~/Content/app/shared/restService.js",
                "~/Content/app/services/connector/connectorService.js",
                "~/Content/app/services/errorReporting/errorReportingService.js",
                "~/Content/app/services/progressBar/progressBarService.js",
                "~/Content/app/services/languagesService.js",
                "~/Content/app/pages/shared/page/pageController.js",
                "~/Content/app/pages/languages/index/languagesIndexController.js",
                "~/Content/app/pages/languages/details/languagesDetailsController.js",
                "~/Content/app/pages/languages/edit/languagesEditController.js",
                "~/Content/app/pages/languages/delete/languagesDeleteController.js",
                "~/Content/app/app.js"
                ));

            // *** Other bundles ***
            bundles.Add(new StyleBundle("~/other/other-css").Include(
                "~/Content/shared/shared.css",
                "~/Content/other/other.css"));
        }
    }
}
