using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/homecss").Include(
                "~/plugins/font-awesome-4.7.0/css/font-awesome.min.css",
                "~/plugins/OwlCarousel2-2.2.1/owl.carousel.css",
                "~/plugins/OwlCarousel2-2.2.1/owl.theme.default.css",
                "~/plugins/OwlCarousel2-2.2.1/animate.css",
                "~/plugins/themify-icons/themify-icons.css",
                "~/plugins/jquery-ui-1.12.1.custom/jquery-ui.css",
                "~/styles/main_styles.css",
                "~/styles/responsive.css"));
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
        }
    }
}
