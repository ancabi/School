using System.Web;
using System.Web.Optimization;

namespace School
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Global style
            bundles.Add(new StyleBundle("~/bundles/style/css").Include(
                      "~/Content/style.css", new CssRewriteUrlTransform()).Include(
                      "~/Content/style_custom.css", new CssRewriteUrlTransform()));

            // school style
            bundles.Add(new StyleBundle("~/bundles/school/css").Include(
                      "~/Content/style_school.css", new CssRewriteUrlTransform()));

            // Bootstrap style
            bundles.Add(new StyleBundle("~/bundles/bootstrap/css").Include(
                      "~/Plugins/bootstrap/dist/css/bootstrap.min.css", new CssRewriteUrlTransform()));

            // Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap/js").Include(
                      "~/Plugins/bootstrap/dist/js/bootstrap.min.js"));

            // jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery/js").Include(
                      "~/Plugins/jquery/dist/jquery.min.js"));

            // jQuery UI
            bundles.Add(new ScriptBundle("~/bundles/jqueryui/js").Include(
                      "~/Plugins/jquery-ui/jquery-ui.min.js"));

            // AngularJS

            bundles.Add(new ScriptBundle("~/bundles/angular/js").Include(
                    "~/Plugins/angular/angular.js"));

            // Plugins CSS
            bundles.Add(new StyleBundle("~/bundles/plugins/css").Include(
                "~/plugins/angular-notify/dist/angular-notify.min.css",
                "~/plugins/metisMenu/dist/metisMenu.min.css",
                "~/plugins/animate.css/animate.css",
                "~/plugins/sweetalert/lib/sweet-alert.min.css",
                "~/plugins/fullcalendar/dist/fullcalendar.min.css",
                "~/plugins/ng-grid/ng-grid.min.css",
                "~/plugins/angular-ui-tree/dist/angular-ui-tree.min.css",
                "~/plugins/datatables.net-bs/css/dataTables.bootstrap.min.css",
                "~/plugins/angular-xeditable/dist/css/xeditable.css",
                "~/plugins/ui-select/dist/select.min.css",
                "~/plugins/bootstrap-touchspin/dist/jquery.bootstrap-touchspin.min.css",
                "~/plugins/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css",
                "~/plugins/blueimp-gallery/css/blueimp-gallery.min.css",
                "~/plugins/chartist/custom/chartist.min.css",
                "~/plugins/ladda/dist/ladda-themeless.min.css",
                "~/plugins/calendar/calendar.min.css",
                "~/plugins/angular-bootstrap-datetimepicker/src/css/datetimepicker.css"));

            // Plugins JS
            bundles.Add(new ScriptBundle("~/bundles/plugins/js").Include(
                "~/Plugins/moment/moment.js",
                "~/Plugins/moment/locale/es.js",
                "~/Plugins/jszip/jszip.min.js",
                "~/Plugins/slimScroll/jquery.slimscroll.min.js",
                "~/Plugins/angular-sanitize/angular-sanitize.min.js",
                "~/Plugins/angular-animate/angular-animate.min.js",
                "~/Plugins/angular-ui-router/release/angular-ui-router.min.js",
                "~/Plugins/angular-bootstrap/ui-bootstrap-tpls.min.js",
                "~/Plugins/angular/angular-locale_es-es.min.js",
                "~/Plugins/jquery-flot/jquery.flot.js",
                "~/Plugins/jquery-flot/jquery.flot.resize.js",
                "~/Plugins/jquery-flot/jquery.flot.pie.js",
                "~/Plugins/flot.curvedlines/curvedLines.js",
                "~/Plugins/jquery.flot.spline/index.js",
                "~/Plugins/angular-flot/angular-flot.js",
                "~/Plugins/metisMenu/dist/metisMenu.min.js",
                "~/Plugins/sweetalert/lib/sweet-alert.min.js",
                "~/Plugins/angular-sweetalert/ngSweetAlert.min.js",
                "~/Plugins/iCheck/icheck.js",
                "~/Plugins/sparkline/index.js",
                "~/Plugins/chartjs/Chart.js",
                "~/Plugins/angles/angles.js",
                "~/Plugins/peity/jquery.peity.js",
                "~/Plugins/angular-peity/angular-peity.js",
                "~/Plugins/angular-notify/dist/angular-notify.min.js",
                "~/Plugins/angular-ui-utils/ui-utils.js",
                "~/Plugins/angular-ui-map/ui-map.js",
                "~/Plugins/fullcalendar/dist/fullcalendar.js",
                "~/Plugins/angular-ui-calendar/src/calendar.js",
                "~/Plugins/summernote/dist/summernote.js",
                "~/Plugins/angular-summernote/dist/angular-summernote.js",
                "~/Plugins/ng-grid/build/ng-grid.js",
                "~/Plugins/datatables/media/js/jquery.dataTables.min.js",
                "~/Plugins/datatables.net-bs/js/dataTables.bootstrap.min.js",
                "~/Plugins/pdfmake/build/pdfmake.min.js",
                "~/Plugins/pdfmake/build/vfs_fonts.js",
                "~/Plugins/datatables.net-buttons/js/buttons.html5.min.js",
                "~/Plugins/datatables.net-buttons/js/buttons.print.min.js",
                "~/Plugins/datatables.net-buttons/js/dataTables.buttons.min.js",
                "~/Plugins/datatables.net-buttons-bs/js/buttons.bootstrap.min.js",
                "~/Plugins/angular-datatables/dist/plugins/buttons/angular-datatables.buttons.min.js",
                "~/Plugins/angular-datatables/dist/angular-datatables.min.js",
                "~/Plugins/angular-xeditable/dist/js/xeditable.min.js",
                "~/Plugins/ui-select/dist/select.js",
                "~/Plugins/bootstrap-touchspin/dist/jquery.bootstrap-touchspin.js",
                "~/Plugins/angular-ui-sortable/sortable.js",
                "~/Plugins/angular-chartist.js/dist/angular-chartist.min.js",
                "~/Plugins/angular-filter/dist/angular-filter.min.js",
                "~/Plugins/ladda/js/spin.js",
                "~/Plugins/ladda/js/ladda.js",
                "~/Plugins/ladda/js/ladda.jquery.js",
                "~/Plugins/angular-ladda/dist/angular-ladda.js",
                "~/plugins/angular-bootstrap-datetimepicker/src/js/datetimepicker.js",
                "~/plugins/angular-bootstrap-datetimepicker/src/js/datetimepicker.templates.js",
                "~/plugins/flexslider/jquery.flexslider-min.js",
                "~/plugins/calendar/calendar.min.js",
                "~/plugins/smart-table/smart-table.min.js",
                "~/plugins/angular-flexslider/angular-flexslider.js"));

            // school script
            bundles.Add(new ScriptBundle("~/bundles/school/js").Include(
                      "~/app/school.js",
                      "~/app/app.js",
                      "~/app/config.js",
                      "~/app/filters/props.js",
                      "~/app/directives/directives.js",
                      "~/app/directives/touchSpin.js",
                      "~/app/factories/http-interceptor.js",
                      "~/app/factories/sweet-alert.js"));

            // Controllers JS Templates
            bundles.Add(new ScriptBundle("~/bundles/templates/js").Include());

            // Controllers JS
            bundles.Add(new ScriptBundle("~/bundles/login/js").Include(
                      "~/app/controllers/loginCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/main/js").Include(
                      "~/app/controllers/mainCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/eventos/js").Include(
                      "~/app/controllers/eventosCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/equipos/js").Include(
                      "~/app/controllers/equiposCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/planificacion/js").Include(
                      "~/app/controllers/planificacionCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/calendario/js").Include(
                      "~/app/controllers/calendarioCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/partidos/js").Include(
                      "~/app/controllers/partidosCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/entrenamientos/js").Include(
                      "~/app/controllers/entrenamientosCtrl.js"));


            BundleTable.EnableOptimizations = false;
        }
    }
}
