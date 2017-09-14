/**
 *
 * propsFilter
 *
 */

angular
    .module('school')
    .filter('propsFilter', propsFilter)
    .filter("jsonDate", ['$filter', jsonDate])
    .filter("trusted", ['$sce', trusted])
    .filter("sumByProp", sumByProp)
    .filter("random", random)
    .filter("formatExport", formatExport);

function propsFilter() {
    return function (input) {
        var out = "";

        if (angular.isArray(items)) {
            items.forEach(function (item) {
                var itemMatches = false;

                var keys = Object.keys(props);
                for (var i = 0; i < keys.length; i++) {
                    var prop = keys[i];
                    var text = props[prop].toLowerCase();
                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                        itemMatches = true;
                        break;
                    }
                }

                if (itemMatches) {
                    out.push(item);
                }
            });
        } else {
            // Let the output be the input untouched
            out = items;
        }

        return out;
    }
}

function jsonDate($filter) {
    return function (input, format) {
        return (input)
               ? $filter('date')(parseInt(input.substr(6)), format)
               : '';
    };
}

function trusted($sce) {
    return function (url) {
        return $sce.trustAsResourceUrl(url);
    };
}

function sumByProp() {
    return function sum(items, prop) {
        if (items) {
            return items.reduce(function (a, b) {
                return a + b[prop];
            }, 0);
        } else {
            return 0;
        }        
    };
}

function random() {
    return function (input, scope) {
        if (input != null && input != undefined && input > 1) {
            return Math.floor((Math.random() * input) + 1);
        }
    }
}

function formatExport() {
    return function (items, scope) {
        var outItems = [];
        if (angular.isArray(items)) {
            angular.forEach(items, function (item) {
                angular.forEach(item, function (value, key) {
                    if (value == null || value == '#EANF#' || value == 'NODATA') {
                        item[key] = '';
                    } else if (angular.isDate(value) || moment(value).isValid()) {
                        moment(value).format("DD-MM-YYYY");
                    }
                });
                outItems.push(item);
            });
        }
        return outItems;
    }
}