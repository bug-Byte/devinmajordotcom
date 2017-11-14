using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace devinmajordotcom.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.validate.unobtrusive.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/cover.css",
                      "~/Content/bootsnack.css",
                      "~/Content/font-awesome.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap-iconpicker.min.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/mediacss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/cover2.css",
                      "~/Content/font-awesome.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/portfoliocss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/font-awesome.css",
                      "~/Content/reset.css",
                      "~/Content/style.css",
                      "~/Content/templatemo-style.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/customScripts").Include(
                "~/Scripts/IndexScripts.js",
                "~/Scripts/jquery.bootsnack.js",
                "~/Scripts/jquery.signalR-2.2.2.js",
                "~/Scripts/angular.js",
                "~/Scripts/bootstrap-iconpicker-iconset-all.min.js",
                "~/Scripts/bootstrap-iconpicker.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/portfolioScripts").Include(
                "~/Scripts/smoothscroll.js",
                "~/Scripts/jquery.nav.js",
                "~/Scripts/isotope.js",
                "~/Scripts/custom.js",
                "~/Scripts/index.js",
                "~/Scripts/imagesloaded.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/mediaScripts").Include(
                "~/Scripts/smoothscroll.js",
                "~/Scripts/jquery.nav.js",
                "~/Scripts/isotope.js",
                "~/Scripts/mediaScripts.js"
                      ));

        }
    }
}