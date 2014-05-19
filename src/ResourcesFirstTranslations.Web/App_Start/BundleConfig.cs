using System.Web;
using System.Web.Optimization;

namespace ResourcesFirstTranslations.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

           

            bundles.Add(new ScriptBundle("~/bundles/vendorscripts").Include(
                     "~/Scripts/jquery-2.0.3.js",
                     "~/Scripts/angular.js",
                     "~/Scripts/angular-animate.js",
                     "~/Scripts/angular-route.js",
                     "~/Scripts/angular-sanitize.js",
                     "~/Scripts/bootstrap.js",
                     "~/Scripts/toastr.js",
                     "~/Scripts/moment.js",
                     "~/Scripts/q.js",
                     "~/Scripts/breeze.debug.js",
                     "~/Scripts/breeze.directives.js",
                     "~/Scripts/ui-bootstrap-tpls-0.10.0.js",
                     "~/Scripts/spin.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminappservices").Include(
                    "~/Areas/Administration/app/services/datacontext.js",
                    "~/Areas/Administration/app/services/directives.js"));

            bundles.Add(new ScriptBundle("~/bundles/admincommonmodules").Include(
                      "~/Areas/Administration/app/common/common.js",
                      "~/Areas/Administration/app/common/logger.js",
                      "~/Areas/Administration/app/common/spinner.js",
                      "~/Areas/Administration/app/common/bootstrap/bootstrap.dialog.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminbootstrapping").Include(
                      "~/Areas/Administration/app/app.js",
                      "~/Areas/Administration/app/config.js",
                      "~/Areas/Administration/app/config.exceptionHandler.js",
                      "~/Areas/Administration/app/config.route.js"));

            bundles.Add(new ScriptBundle("~/bundles/administrator").Include(
                      "~/Areas/Administration/app/branches/branches.js",
                      "~/Areas/Administration/app/branches/newbranch.js",
                      "~/Areas/Administration/app/branches/editbranch.js",
                      "~/Areas/Administration/app/branchesresourcefiles/branchesfiles.js",
                      "~/Areas/Administration/app/branchesresourcefiles/editbranchfile.js",
                      "~/Areas/Administration/app/branchesresourcefiles/newbranchfile.js",
                      "~/Areas/Administration/app/dashboard/dashboard.js",
                      "~/Areas/Administration/app/languages/languages.js",
                      "~/Areas/Administration/app/languages/newlanguage.js",
                      "~/Areas/Administration/app/resourcefiles/editresourcefile.js",
                      "~/Areas/Administration/app/resourcefiles/newresourcefile.js",
                      "~/Areas/Administration/app/resourcefiles/resourcefiles.js",
                      "~/Areas/Administration/app/sendmail/sendmail.js",
                      "~/Areas/Administration/app/translator/resetpassword.js",
                      "~/Areas/Administration/app/translator/edittranslator.js",
                      "~/Areas/Administration/app/translator/newtranslator.js",
                      "~/Areas/Administration/app/translator/translators.js",
                      "~/Areas/Administration/app/layout/shell.js",
                      "~/Areas/Administration/app/layout/sidebar.js"));

            bundles.Add(new ScriptBundle("~/bundles/translatorappservices").Include(
                  "~/app/Translation/services/datacontext.js",
                  "~/app/Translation/services/directives.js"));

            bundles.Add(new ScriptBundle("~/bundles/translatorcommonmodules").Include(
                     "~/app/Translation/common/common.js",
                     "~/app/Translation/common/logger.js",
                     "~/app/Translation/common/spinner.js",
                     "~/app/Translation/common/bootstrap/bootstrap.dialog.js"));

            bundles.Add(new ScriptBundle("~/bundles/translatorbootstrapping").Include(
                      "~/app/Translation/app.js",
                      "~/app/Translation/config.js",
                      "~/app/Translation/config.exceptionHandler.js",
                      "~/app/Translation/config.route.js"));

            bundles.Add(new ScriptBundle("~/bundles/translator").Include(
                      "~/app/Translation/translations/translation.js",
                      "~/app/Translation/dashboard/dashboard.js",
                      "~/app/Translation/layout/shell.js",
                      "~/app/Translation/layout/sidebar.js"));

            bundles.Add(new StyleBundle("~/Content/admincss").Include(
                      "~/Content/ie10mobile.css",
                      "~/Content/bootstrap.css",
                      "~/Content/breeze.directives.css",
                      "~/Content/font-awesome.css",
                      "~/Content/toastr.css",
                      "~/Content/customtheme.css",
                      "~/Content/styles.css"));

            bundles.Add(new StyleBundle("~/Content/translatorcss").Include(
                     "~/Content/ie10mobile.css",
                     "~/Content/bootstrap.css",
                     "~/Content/breeze.directives.css",
                     "~/Content/font-awesome.css",
                     "~/Content/toastr.css",
                     "~/Content/customthemeTranslator.css",
                     "~/Content/styles.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
