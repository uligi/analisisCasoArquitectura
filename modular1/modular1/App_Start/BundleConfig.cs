﻿using System.Web;
using System.Web.Optimization;

namespace modular1
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new Bundle("~/bundles/Complementos").Include(
            "~/Scripts/scripts.js",
             "~/Scripts/DataTables/jquery.dataTables.js",
             "~/Scripts/DataTables/dataTables.responsive.js",
              "~/Scripts/sweetalert.min.js",
            "~/Scripts/fontawesome/all.min.js"));


            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(                   
                      "~/Content/site.css",
                       "~/Content/DataTables/css/jquery.dataTables.css",
                       "~/Content/DataTables/css/responsive.dataTables.css",
                       "~/Content/sweetalert.css"
                       ));
        }
    }
}
