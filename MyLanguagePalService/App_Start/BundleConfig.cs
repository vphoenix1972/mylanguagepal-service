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

            // *** Other bundles ***
            bundles.Add(new StyleBundle("~/other/other-css").Include(
                "~/Content/shared/shared.css",
                "~/Content/other/other.css"));
        }
    }
}
