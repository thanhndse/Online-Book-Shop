using System.Web;
using System.Web.Optimization;

namespace BookShopWithAuthen.Web
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
            bundles.Add(new StyleBundle("~/Content/FrontEndRef/css").Include(
                "~/Content/FrontEndRef/lib/bootstrap/css/bootstrap.min.css",
                "~/Content/FrontEndRef/lib/font-awesome/css/font-awesome.min.css",
                "~/Content/FrontEndRef/lib/animate/animate.min.css",
                "~/Content/FrontEndRef/lib/ionicons/css/ionicons.min.css",
                "~/Content/FrontEndRef/lib/owlcarousel/assets/owl.carousel.min.css",
                "~/Content/FrontEndRef/lib/owlcarousel/assets/owl.theme.default.min.css",
                "~/Content/FrontEndRef/lib/lightbox/css/lightbox.min.css",
                "~/Content/FrontEndRef/css/style.css",
                "~/Content/FrontEndRef/css/main.css"
                ));
            bundles.Add(new ScriptBundle("~/Content/FrontEndRef/script").Include(
                 "~/Content/FrontEndRef/lib/jquery/jquery.min.js",
                 "~/Content/FrontEndRef/lib/jquery/jquery-migrate.min.js",
                 "~/Content/FrontEndRef/lib/bootstrap/js/bootstrap.bundle.min.js",
                 "~/Content/FrontEndRef/lib/easing/easing.min.js",
                 "~/Content/FrontEndRef/lib/superfish/hoverIntent.js",
                 "~/Content/FrontEndRef/lib/superfish/superfish.min.js",
                 "~/Content/FrontEndRef/lib/wow/wow.min.js",
                 "~/Content/FrontEndRef/lib/waypoints/waypoints.min.js",
                 "~/Content/FrontEndRef/lib/counterup/counterup.min.js",
                 "~/Content/FrontEndRef/lib/owlcarousel/owl.carousel.min.js",
                 "~/Content/FrontEndRef/lib/isotope/isotope.pkgd.min.js",
                 "~/Content/FrontEndRef/lib/lightbox/js/lightbox.min.js",
                 "~/Content/FrontEndRef/lib/touchSwipe/jquery.touchSwipe.min.js",
                 "~/Content/FrontEndRef/contactform/contactform.js",
                 "~/Content/FrontEndRef/js/main.js",
                 "~/Content/FrontEndRef/js/carousel.js"
                 ));
        }
    }
}
