using System.Web;
using System.Web.Optimization;

namespace EXP.Website
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/account").Include(
                        "~/Scripts/jquery-1.7.2.min.js",
                        "~/Scripts/jquery-1.8.3.min.js",
                        "~/Scripts/jquery.backstretch.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.form.js",
                        "~/Scripts/jquery-ui-1.8.11.js",
                        "~/Scripts/jquery.selectbox-0.6.1.js",
                        "~/Scripts/glDatePicker.min.js",
                        "~/Scripts/registrationBuildingOwner.js",
                        "~/Scripts/Index.js",
                        "~/Scripts/jquery.validate.my-additional.js",
                        "~/Scripts/jquery.cookie.js",
                        "~/Scripts/contactUs.js",
                        "~/Scripts/jquery.inputmask.custom.extensions.js",
                        "~/Scripts/jquery.inputmask.date.extensions.js",
                        "~/Scripts/jquery.inputmask.extensions.js",
                        "~/Scripts/jquery.inputmask.js",
                        "~/Scripts/jquery.inputmask.numeric.extensions.js",
                        "~/Scripts/jquery.maskMoney.js",
                        "~/Scripts/common.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/dashboardCommon").Include(
                        "~/Scripts/jquery-1.7.2.min.js",
                        "~/Scripts/jquery.backstretch.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.form.js",
                        "~/Scripts/jquery-ui-1.8.11.js",
                        "~/Scripts/jquery.selectbox-0.6.1.js",
                        "~/Scripts/common.js",
                        "~/Scripts/alerts.js",
                        "~/Scripts/contactUs.js",
                        "~/Scripts/jquery.inputmask.custom.extensions.js",
                        "~/Scripts/jquery.inputmask.date.extensions.js",
                        "~/Scripts/jquery.inputmask.extensions.js",
                        "~/Scripts/jquery.inputmask.js",
                        "~/Scripts/jquery.inputmask.numeric.extensions.js",
                        "~/Scripts/jquery.validate.my-additional.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/dashboardOwner").Include(
                        "~/Scripts/personaldata.js",
                        "~/Scripts/phones.js",
                        "~/Scripts/ownerReports.js",
                        "~/Scripts/glDatePicker.min.js",
                        "~/Scripts/dashboardOwner.js",
                        "~/Scripts/jquery.inputmask.custom.extensions.js",
                        "~/Scripts/jquery.inputmask.date.extensions.js",
                        "~/Scripts/jquery.inputmask.extensions.js",
                        "~/Scripts/jquery.inputmask.js",
                        "~/Scripts/jquery.inputmask.numeric.extensions.js",
                        "~/Scripts/common.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                        "~/Scripts/personaldata.js",
                        "~/Scripts/vehicles.js",
                        "~/Scripts/phones.js",
                        "~/Scripts/payments.js",
                        "~/Scripts/glDatePicker.min.js",
                        "~/Scripts/dashboard.js",
                        "~/Scripts/jquery.inputmask.custom.extensions.js",
                        "~/Scripts/jquery.inputmask.date.extensions.js",
                        "~/Scripts/jquery.inputmask.extensions.js",
                        "~/Scripts/jquery.inputmask.js",
                        "~/Scripts/jquery.inputmask.numeric.extensions.js",
                        "~/Scripts/common.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Styles/dashboardStyles").Include(
                "~/Styles/reset.css",
                "~/Styles/dashboard.css",
                "~/Styles/jquery.selectbox.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css"));
            
            bundles.Add(new StyleBundle("~/bundles/webfonts/").Include(
                "~/Styles/webfonts/"));
        }
    }
}