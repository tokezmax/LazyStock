using System.Web;
using System.Web.Optimization;

namespace LazyStock.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/css/bootstrap.min.css",
                "~/css/owl.carousel.css",
                "~/css/owl.theme.default.css",
                "~/css/magnific-popup.css",
                "~/css/font-awesome.min.css",
                "~/css/style.css",
                "~/css/toast.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/js/jquery.min.js",
                "~/js/bootstrap.min.js",
                "~/js/owl.carousel.min.js",
                "~/js/jquery.magnific-popup.js",
                "~/Scripts/jquery.cookie.js"
                , "~/js/Common.js"
                , "~/js/main.js"
            ));
        }
    }
}
