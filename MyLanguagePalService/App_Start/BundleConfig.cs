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
                /* Deps */
                "~/Content/deps/bootstrap/css/bootstrap.css",
                "~/Content/deps/angular/ngprogress.css",

                /* Shared site styles */
                "~/Content/shared/shared.css",

                /* Core */
                "~/Content/app/core/core.module.css",

                /* Pages */
                
                // Shared
                "~/Content/app/pages/shared/services/progressBar/progressBar.service.css",
                "~/Content/app/pages/shared/page/page.css",
                "~/Content/app/pages/shared/directives/textInput/textInput.directive.css"));

            bundles.Add(new ScriptBundle("~/app/app-js").Include(
                /* Deps */
                "~/Content/deps/jquery/jquery-3.1.0.js",
                "~/Content/deps/bootstrap/js/bootstrap.js",
                "~/Content/deps/respond.js",
                "~/Content/deps/angular/angular.js",
                "~/Content/deps/angular/angular-route.js",
                "~/Content/deps/angular/angular-animate.js",
                "~/Content/deps/angular/angular-touch.js",
                "~/Content/deps/angular/ui-bootstrap-tpls-2.2.0.js",
                "~/Content/deps/angular/ngprogress.js",

                /* Core */
                "~/Content/app/core/core.module.js",
                "~/Content/app/core/utils.service.js",
                "~/Content/app/core/promiseQueue.js",
                "~/Content/app/core/rest.js",
                "~/Content/app/core/customError.js",
                "~/Content/app/core/stringExtensions.js",

                /* Pages */
                "~/Content/app/app.module.js",

                // Shared
                "~/Content/app/pages/shared/services/config.service.js",
                "~/Content/app/pages/shared/services/connector/httpError.js",
                "~/Content/app/pages/shared/services/connector/networkError.js",
                "~/Content/app/pages/shared/services/connector/connectorResult.js",
                "~/Content/app/pages/shared/services/connector/validationResult.js",
                "~/Content/app/pages/shared/services/connector/connector.service.js",
                "~/Content/app/pages/shared/services/errorReporting/errorReporting.service.js",
                "~/Content/app/pages/shared/services/progressBar/progressBar.service.js",
                
                "~/Content/app/pages/shared/page/page.controller.js",
                "~/Content/app/pages/shared/directives/textInput/textInput.directive.js",

                // Languages
                "~/Content/app/pages/languages/languages.service.js",
                "~/Content/app/pages/languages/languages.routes.js",
                "~/Content/app/pages/languages/index/languagesIndex.controller.js",
                "~/Content/app/pages/languages/details/languagesDetails.controller.js",
                "~/Content/app/pages/languages/edit/languagesEdit.controller.js",
                "~/Content/app/pages/languages/delete/languagesDelete.controller.js",

                // Phrases
                "~/Content/app/pages/phrases/phrases.service.js",
                "~/Content/app/pages/phrases/phrases.routes.js",
                "~/Content/app/pages/phrases/index/phrasesIndex.controller.js",
                "~/Content/app/pages/phrases/details/phrasesDetails.controller.js",
                "~/Content/app/pages/phrases/edit/phrasesEdit.controller.js",
                "~/Content/app/pages/phrases/delete/phrasesDelete.controller.js"
                ));

            // *** Other bundles ***
            bundles.Add(new StyleBundle("~/other/other-css").Include(
                "~/Content/shared/shared.css",
                "~/Content/other/other.css"));
        }
    }
}
