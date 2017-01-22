/**
 * HOMER - Responsive Admin Theme
 * version 1.8
 *
 */
(function () {
    angular.module('school', [
        'ui.router',                // Angular flexible routing
        'ngSanitize',               // Angular-sanitize
        'ui.bootstrap',             // AngularJS native directives for Bootstrap
        'angular-flot',             // Flot chart
        'angles',                   // Chart.js
        'angular-peity',            // Peity (small) charts
        'cgNotify',                 // Angular notify
        'hSweetAlert',              // Angular Sweet Alert
        'angles',                   // Angular ChartJS
        'ngAnimate',                // Angular animations
        'ui.map',                   // Ui Map for Google maps
        'ui.calendar',              // UI Calendar        
        'summernote',               // Summernote plugin
        'ngGrid',                   // Angular ng Grid
        //'ui.tree',                  // Angular ui Tree
        //'bm.bsTour',                // Angular bootstrap tour
        'datatables',               // Angular datatables plugin
        'datatables.buttons',       // Angular datatables buttons
        'xeditable',                // Angular-xeditable
        'ui.select',                // AngularJS ui-select
        'ui.sortable',              // AngularJS ui-sortable
        //'ui.footable',              // FooTable
        //'angular-chartist',         // Chartist
        'angular.filter',           // Angular Filters
        'angular-ladda',            // Ladda - loading buttons
        'ui.bootstrap.datetimepicker',  // Angular bootstrap date & time picker
        'angular-flexslider'        // Angular Flex-slider Directive
    ]).service('UtilService', function () {
        this.sum = function sum(items, prop) {
            return items.reduce(function (a, b) {
                return a + b[prop];
            }, 0);
        };        
    }).service('fileUpload', ['$http', function ($http) {
        this.uploadFileToUrl = function (file, uploadUrl) {
            var fd = new FormData();
            fd.append('file', file);
            $http.post(uploadUrl, fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .success(function () {
            })
            .error(function () {
            });
        }
    }]).constant("PANEL_HCLASES", ["hblue", "hgreen", "hnavyblue", "horange", "hred", "hreddeep", "hviolet", "hyellow"]
    ).run(['DTDefaultOptions', function (DTDefaultOptions) {
        DTDefaultOptions.setLanguageSource(webroot + 'Plugins/datatables/media/Spanish.json');
    }]);
})();

