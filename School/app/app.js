/**
 * school - Responsive Admin Theme
 *
 */
(function () {
    angular.module('school', [
        'ui.router',                    // Routing
        //'oc.lazyLoad',                  // ocLazyLoad
        'ui.bootstrap',                 // Ui Bootstrap
        //'pascalprecht.translate',       // Angular Translate
        //'ngIdle',                       // Idle timer
        'ngSanitize',                    // ngSanitize
        'chart.js',                   // Chart.js
        'xeditable',                // Angular-xeditable
        'smart-table',
        'ngDragDrop',
        'angular.filter',           // Angular Filters
        'ui.bootstrap.datetimepicker',  // Angular bootstrap date & time picker
        'isteven-multi-select',
        'ngAnimate',
        'toastr',
        'angular-flexslider'        // Angular Flex-slider Directive
    ])
})();

// Other libraries are loaded dynamically in the config.js file using the library ocLazyLoad