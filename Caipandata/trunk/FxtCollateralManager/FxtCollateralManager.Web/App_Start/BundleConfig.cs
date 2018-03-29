using System.Web;
using System.Web.Optimization;

namespace FxtCollateralManager.Web
{
    public class BundleConfig
    {
        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new System.ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
            ignoreList.Ignore("*.css", OptimizationMode.WhenDisabled);
        }

        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.IgnoreList.Clear();
            //AddDefaultIgnorePatterns(bundles.IgnoreList);
            //javascript
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            //"~/Scripts/jquery-{version}.js",
            //            "~/Scripts/jquery.extend.js"));


            //bundles.Add(new ScriptBundle("~/bundles/matrix").Include(
            //             "~/Content/matrix/js/jquery.js",
            //             "~/Content/matrix/js/jquery.ui.custom.js",
            //             "~/Content/matrix/js/bootstrap.js",
            //             "~/Content/matrix/js/jquery.uniform.js",
            //             "~/Content/matrix/js/select2.js",
            //             "~/Content/matrix/js/matrix.js"
            //             ));

            ////样式
            //bundles.Add(new StyleBundle("~/Content/matrix/css").Include(
            //            "~/Content/matrix/css/bootstrap.css",
            //            "~/Content/matrix/css/bootstrap-responsive.css",
            //            "~/Content/matrix/css/matrix-style.css",
            //            "~/Content/matrix/css/matrix-media.css",
            //            "~/Content/matrix/css/select2.css",
            //            "~/Content/matrix/font-awesome/css/font-awesome.css",
            //            "~/Content/matrix/css/fonts.googleapis.com_css_family_Open_Sans_400,700,800.css"
            //            ));

        }
    }
}