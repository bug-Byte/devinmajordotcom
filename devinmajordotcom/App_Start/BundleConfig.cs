﻿using System;
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
                "~/Scripts/umd/popper.min.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootsnack.css",
                      "~/Content/font-awesome.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap-iconpicker.min.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/bootstrap-colorpicker.css",
                      "~/Content/cover.css"
            ));

            bundles.Add(new StyleBundle("~/Content/mediacss").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/cover2.css",
                "~/Content/font-awesome.css"
            ));

            bundles.Add(new StyleBundle("~/Content/homecss").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/themes/jquery-ui.css",
                "~/Content/magnific-popup.css",
                "~/Content/font-awesome.css",
                "~/Content/bootsnack.css",
                "~/Content/templatemo-style2.css"
            ));

            bundles.Add(new StyleBundle("~/Content/portfoliocss").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/font-awesome.css",
                "~/Content/reset.css",
                "~/Content/style.css",
                "~/Content/templatemo-style.css"
            ));

            bundles.Add(new StyleBundle("~/Content/FineUploader").Include(
                "~/Content/FineUploader/fine-uploader-new.css"));

            bundles.Add(new ScriptBundle("~/bundles/customScripts").Include(
                "~/Scripts/jquery.bootsnack.js",
                "~/Scripts/angular.js",
                "~/Scripts/bootstrap-iconpicker-iconset-all.min.js",
                "~/Scripts/bootstrap-iconpicker.min.js",
                "~/Scripts/jquery.signalR-2.2.3.min.js",
                "~/Scripts/jquery.easypiechart.min.js",
                "~/Scripts/handlebars.min.js",
                "~/Scripts/moment.js",
                "~/Scripts/bootstrap-datetimepicker.js",
                "~/Scripts/bootstrap-colorpicker.js",
                "~/Scripts/IndexScripts.js"
            ));

            bundles.Add(new ScriptBundle("~/scriptBundles/tinymce").Include(
                "~/Scripts/tinymce/tinymce.min.js",
                "~/Scripts/tinymce/jquery.tinymce.min.js",
                "~/Scripts/tinymce/themes/inlite/theme.js",
                "~/Scripts/tinymce/themes/mobile/theme.js",
                "~/Scripts/tinymce/themes/modern/theme.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/portfolioScripts").Include(
                "~/Scripts/smoothscroll.js",
                "~/Scripts/jquery.nav.js",
                "~/Scripts/isotope.js",
                "~/Scripts/imagesloaded.min.js",
                "~/Scripts/moment.js",
                "~/Scripts/custom.js",
                "~/Scripts/index.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/mediaScripts").Include(
                "~/Scripts/smoothscroll.js",
                "~/Scripts/jquery.nav.js",
                "~/Scripts/isotope.js",
                "~/Scripts/mediaScripts.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/homeScripts").Include(
                "~/Scripts/jquery.stellar.min.js",
                "~/Scripts/jquery.magnific-popup.min.js",
                "~/Scripts/smoothscroll.js",
                "~/Scripts/moment.js",
                "~/Scripts/jquery.bootsnack.js",
                "~/Scripts/jquery.ui.touch-punch.min.js",
                "~/Scripts/homeScripts.js"
            ));

            bundles.Add(new ScriptBundle("~/scriptBundles/FineUploader").Include(
                "~/Scripts/fine-uploader.js"));

        }
    }
}