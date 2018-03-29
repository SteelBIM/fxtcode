using System.Web;
using System.Web.Optimization;

namespace FXT.DataCenter.WebUI
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/commonJS").Include(
                      "~/Content/assets/js/jquery-1.8.3.min.js",
                      "~/Content/assets/bootstrap/js/bootstrap.min.js",
                      "~/Content/assets/bootstrap-datepicker/js/bootstrap-datepicker.js",
                      "~/Content/assets/jquery-ui/jquery-ui-1.10.1.custom.min.js",
                      "~/Content/assets/select2/select2.min.js",
                      "~/content/scripts/jquery.validate.min.js",
                      "~/content/scripts/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/layouteditJS").Include(
                      "~/content/scripts/admin.edit.js"));

            bundles.Add(new ScriptBundle("~/bundles/layoutJS").Include(
                        "~/Content/assets/breakpoints/breakpoints.js",
                        "~/Content/assets/js/jquery.blockui.js",
                        "~/Content/assets/js/jquery.cookie.js",
                        "~/Content/assets/uniform/jquery.uniform.min.js",
                        "~/Content/assets/data-tables/jquery.dataTables.js",
                        "~/Content/assets/data-tables/DT_bootstrap.js",
                        "~/Content/assets/js/app.js",
                        "~/content/scripts/jquery.thickbox.js",
                        "~/content/scripts/admin.main.js"
                        ));

         
            bundles.Add(new ScriptBundle("~/bundles/fixIE").Include(
                        "~/Content/assets/js/excanvas.js",
                        "~/Content/assets/js/respond.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/content/scripts/modernizr-2.6.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                       "~/content/scripts/admin.main.js"));

            bundles.Add(new StyleBundle("~/content/commonCSS").Include(
                     "~/Content/assets/bootstrap/css/bootstrap.min.css",
                     "~/Content/assets/css/metro.css",
                     "~/Content/assets/font-awesome/css/font-awesome.css",
                     "~/Content/assets/css/style.css",
                     "~/Content/assets/jquery-ui/jquery-ui-1.10.1.custom.min.css",
                     "~/content/styles/admin.main.css",
                     "~/content/Assets/select2/select2.css",
                     "~/Content/assets/bootstrap-datepicker/css/datepicker.css"));


            bundles.Add(new StyleBundle("~/content/layoutCSS").Include(
                        "~/Content/assets/bootstrap/css/bootstrap-responsive.min.css",
                        "~/Content/assets/css/style_responsive.css",
                        "~/Content/assets/css/style_light.css",
                        "~/Content/assets/uniform/css/uniform.default.css",
                        "~/Content/assets/data-tables/DT_bootstrap.css",
                        "~/content/styles/jquery.thickbox.css"));
        }

    }
}