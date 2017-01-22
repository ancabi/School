/**
 * HOMER - Responsive Admin Theme
 * Copyright 2015 Webapplayers.com
 *
 */

angular
    .module('school')
    .directive('fileModel', ['$parse', fileModel])
    .directive('ngRandomClass', ngRandomClass)
    .directive('errSrc', ['$rootScope', errSrc])
    .directive('pageTitle', ['$rootScope', '$timeout', pageTitle])
    .directive('sideNavigation', ['$timeout', sideNavigation])
    .directive('minimalizaMenu', ['$rootScope', minimalizaMenu])
    .directive('sparkline', sparkline)
    .directive('icheck', ['$timeout', icheck])
    .directive('panelTools', ['$timeout', panelTools])
    .directive('panelToolsFullscreen', ['$timeout', panelToolsFullscreen])
    .directive('smallHeader', smallHeader)
    .directive('animatePanel', ['$timeout', '$state', animatePanel])
    .directive('landingScrollspy', landingScrollspy)
    //.directive('flexSlider', ['$parse', '$timeout', flexSlider])

/*
 * fileModel - Directiva para la subida de ficheros
 */
function fileModel($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}

/*
 * ngRandomClass - Directiva para asignar una clase Random de un array por parámetros
 */
function ngRandomClass() {
    return {
        restrict: 'EA',
        replace: false,
        scope: {
            ngClasses: "=ngRandomClass"
        },
        link: function (scope, elem, attr) {
            //Add random background class to selected element
            elem.addClass(scope.ngClasses[Math.floor(Math.random() * (scope.ngClasses.length))]);
        }
    }
};

/*
 * errSrc - Directiva para sustituir las imágenes que no existan
 */
function errSrc($rootScope) {
    return {
        link: function (scope, element, attrs) {
            element.bind('error', function () {
                if (attrs.src != attrs.errSrc) {
                    attrs.$set('src', attrs.errSrc);
                }
            });
        }
    }
};

/**
 * pageTitle - Directive for set Page title - mata title
 */
function pageTitle($rootScope, $timeout) {
    return {
        link: function (scope, element) {
            var listener = function (event, toState, toParams, fromState, fromParams) {
                // Default title
                var title = 'school | AngularJS Responsive WebApp';
                // Create your own title pattern
                if (toState.data && toState.data.pageTitle) title = 'school | ' + toState.data.pageTitle;
                $timeout(function () {
                    element.text(title);
                });
            };
            $rootScope.$on('$stateChangeStart', listener);
        }
    }
};

/**
 * sideNavigation - Directive for run metsiMenu on sidebar navigation
 */
function sideNavigation($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element) {
            // Call the metsiMenu plugin and plug it to sidebar navigation
            element.metisMenu();

            // Colapse menu in mobile mode after click on element
            var menuElement = $('#side-menu a:not([href$="\\#"])');
            menuElement.click(function () {

                if ($(window).width() < 769) {
                    $("body").toggleClass("show-sidebar");
                }
            });


        }
    };
};

/**
 * minimalizaSidebar - Directive for minimalize sidebar
 */
function minimalizaMenu($rootScope) {
    return {
        restrict: 'EA',
        template: '<div class="header-link hide-menu" ng-click="minimalize()"><i class="fa fa-bars"></i></div>',
        controller: ['$scope', '$element', function ($scope, $element) {

            $scope.minimalize = function () {
                if ($(window).width() < 769) {
                    $("body").toggleClass("show-sidebar");
                } else {
                    $("body").toggleClass("hide-sidebar");
                }
            }
        }]
    };
};

/**
 * sparkline - Directive for Sparkline chart
 */
function sparkline() {
    return {
        restrict: 'A',
        scope: {
            sparkData: '=',
            sparkOptions: '=',
        },
        link: function (scope, element, attrs) {
            scope.$watch(scope.sparkData, function () {
                render();
            });
            scope.$watch(scope.sparkOptions, function () {
                render();
            });
            var render = function () {
                $(element).sparkline(scope.sparkData, scope.sparkOptions);
            };
        }
    }
};

/**
 * icheck - Directive for custom checkbox icheck
 */
function icheck($timeout) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function ($scope, element, $attrs, ngModel) {
            return $timeout(function () {
                var value;
                value = $attrs['value'];

                $scope.$watch($attrs['ngModel'], function (newValue) {
                    $(element).iCheck('update');
                })

                return $(element).iCheck({
                    checkboxClass: 'icheckbox_square-green',
                    radioClass: 'iradio_square-green'

                }).on('ifChanged', function (event) {
                    if ($(element).attr('type') === 'checkbox' && $attrs['ngModel']) {
                        $scope.$apply(function () {
                            return ngModel.$setViewValue(event.target.checked);
                        });
                    }
                    if ($(element).attr('type') === 'radio' && $attrs['ngModel']) {
                        return $scope.$apply(function () {
                            return ngModel.$setViewValue(value);
                        });
                    }
                });
            });
        }
    };
}

/**
 * panelTools - Directive for panel tools elements in right corner of panel
 */
function panelTools($timeout) {
    return {
        restrict: 'A',
        scope: true,
        templateUrl: webroot + 'app/templates/panel_tools.html',
        controller: ['$scope', '$element', function ($scope, $element) {
            // Function for collapse ibox
            $scope.showhide = function () {
                var hpanel = $element.closest('div.hpanel');
                var icon = $element.find('i:first');
                var body = hpanel.find('div.panel-body');
                var footer = hpanel.find('div.panel-footer');
                body.slideToggle(300);
                footer.slideToggle(200);
                // Toggle icon from up to down
                icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
                hpanel.toggleClass('').toggleClass('panel-collapse');
                $timeout(function () {
                    hpanel.resize();
                    hpanel.find('[id^=map-]').resize();
                }, 50);
            },

            // Function for close ibox
            $scope.closebox = function () {
                var hpanel = $element.closest('div.hpanel');
                hpanel.remove();
            }

        }]
    };
};

/**
 * panelToolsFullscreen - Directive for panel tools elements in right corner of panel with fullscreen option
 */
function panelToolsFullscreen($timeout) {
    return {
        restrict: 'A',
        scope: true,
        templateUrl: webroot + 'app/templates/panel_tools_fullscreen.html',
        controller: ['$scope', '$element', function ($scope, $element) {
            // Function for collapse ibox
            $scope.showhide = function () {
                var hpanel = $element.closest('div.hpanel');
                var icon = $element.find('i:first');
                var body = hpanel.find('div.panel-body');
                var footer = hpanel.find('div.panel-footer');
                body.slideToggle(300);
                footer.slideToggle(200);
                // Toggle icon from up to down
                icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
                hpanel.toggleClass('').toggleClass('panel-collapse');
                $timeout(function () {
                    hpanel.resize();
                    hpanel.find('[id^=map-]').resize();
                }, 50);
            };

            // Function for close ibox
            $scope.closebox = function () {
                var hpanel = $element.closest('div.hpanel');
                hpanel.remove();
            }

            // Function for fullscreen
            $scope.fullscreen = function () {
                var hpanel = $element.closest('div.hpanel');
                var icon = $element.find('i:first');
                $('body').toggleClass('fullscreen-panel-mode');
                icon.toggleClass('fa-expand').toggleClass('fa-compress');
                hpanel.toggleClass('fullscreen');
                setTimeout(function () {
                    $(window).trigger('resize');
                }, 100);
            }

        }]
    };
};

/**
 * smallHeader - Directive for page title panel
 */
function smallHeader() {
    return {
        restrict: 'A',
        scope: true,
        controller: ['$scope', '$element', function ($scope, $element) {
            $scope.small = function () {
                var icon = $element.find('i:first');
                var breadcrumb = $element.find('#hbreadcrumb');
                $element.toggleClass('small-header');
                breadcrumb.toggleClass('m-t-lg');
                icon.toggleClass('fa-arrow-up').toggleClass('fa-arrow-down');
            }
        }]
    }
}

function animatePanel($timeout, $state) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {

            //Set defaul values for start animation and delay
            var startAnimation = 0;
            var delay = 0.06;   // secunds
            var start = Math.abs(delay) + startAnimation;

            // Store current state where directive was start
            var currentState = $state.current.name;

            // Set default values for attrs
            if (!attrs.effect) { attrs.effect = 'zoomIn' };
            if (attrs.delay) { delay = attrs.delay / 10 } else { delay = 0.06 };
            if (!attrs.child) { attrs.child = '.row > div' } else { attrs.child = "." + attrs.child };

            // Get all visible element and set opactiy to 0
            var panel = element.find(attrs.child);
            panel.addClass('opacity-0');

            // Count render time
            var renderTime = panel.length * delay * 1000 + 700;

            // Wrap to $timeout to execute after ng-repeat
            $timeout(function () {

                // Get all elements and add effect class
                panel = element.find(attrs.child);
                panel.addClass('stagger').addClass('animated-panel').addClass(attrs.effect);

                var panelsCount = panel.length + 10;
                var animateTime = (panelsCount * delay * 10000) / 10;

                // Add delay for each child elements
                panel.each(function (i, elm) {
                    start += delay;
                    var rounded = Math.round(start * 10) / 10;
                    $(elm).css('animation-delay', rounded + 's');
                    // Remove opacity 0 after finish
                    $(elm).removeClass('opacity-0');
                });

                // Clear animation after finish
                $timeout(function () {
                    $('.stagger').css('animation', '');
                    $('.stagger').removeClass(attrs.effect).removeClass('animated-panel').removeClass('stagger');
                }, animateTime)

            });



        }
    }
}

function landingScrollspy() {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.scrollspy({
                target: '.navbar-fixed-top',
                offset: 80
            });
        }
    }
}

/**
 * FlexSlider - Slider multiple items
 */
//function flexSlider($parse, $timeout) {
//    return {
//        restrict: 'AE',
//        scope: false,
//        replace: true,
//        transclude: true,
//        template: '<div class="flexslider-container"></div>',
//        compile: function (element, attr, linker) {
//            return function ($scope, $element) {
//                var addSlide, collectionString, flexsliderDiv, getTrackFromItem, indexString, match, removeSlide, slidesItems, trackBy;
//                match = (attr.slide || attr.flexSlide).match(/^\s*(.+)\s+in\s+(.*?)(?:\s+track\s+by\s+(.+?))?\s*$/);
//                indexString = match[1];
//                collectionString = match[2];
//                trackBy = angular.isDefined(match[3]) ? $parse(match[3]) : $parse("" + indexString);
//                flexsliderDiv = null;
//                slidesItems = {};
//                getTrackFromItem = function (collectionItem, index) {
//                    var locals;
//                    locals = {};
//                    locals[indexString] = collectionItem;
//                    locals['$index'] = index;
//                    return trackBy($scope, locals);
//                };
//                addSlide = function (collectionItem, index, callback) {
//                    var childScope, track;
//                    track = getTrackFromItem(collectionItem, index);
//                    if (slidesItems[track] != null) {
//                        throw "Duplicates in a repeater are not allowed. Use 'track by' expression to specify unique keys.";
//                    }
//                    childScope = $scope.$new();
//                    childScope[indexString] = collectionItem;
//                    childScope['$index'] = index;
//                    return linker(childScope, function (clone) {
//                        var slideItem;
//                        slideItem = {
//                            collectionItem: collectionItem,
//                            childScope: childScope,
//                            element: clone
//                        };
//                        slidesItems[track] = slideItem;
//                        return typeof callback === "function" ? callback(slideItem) : void 0;
//                    });
//                };
//                removeSlide = function (collectionItem, index) {
//                    var slideItem, track;
//                    track = getTrackFromItem(collectionItem, index);
//                    slideItem = slidesItems[track];
//                    if (slideItem == null) {
//                        return;
//                    }
//                    delete slidesItems[track];
//                    slideItem.childScope.$destroy();
//                    return slideItem;
//                };
//                return $scope.$watchCollection(collectionString, function (collection, oldCollection) {
//                    var attrKey, attrVal, c, currentSlidesLength, e, i, idx, n, options, slider, slides, t, toAdd, toRemove, trackCollection, _i, _j, _k, _l, _len, _len1, _len2, _len3;
//                    if (!(collection != null ? collection.length : void 0) && !(oldCollection != null ? oldCollection.length : void 0)) {
//                        return;
//                    }
//                    if (flexsliderDiv != null) {
//                        slider = flexsliderDiv.data('flexslider');
//                        currentSlidesLength = Object.keys(slidesItems).length;
//                        if (collection == null) {
//                            collection = [];
//                        }
//                        trackCollection = {};
//                        for (i = _i = 0, _len = collection.length; _i < _len; i = ++_i) {
//                            c = collection[i];
//                            trackCollection[getTrackFromItem(c, i)] = c;
//                        }
//                        toAdd = (function () {
//                            var _j, _len1, _results;
//                            _results = [];
//                            for (i = _j = 0, _len1 = collection.length; _j < _len1; i = ++_j) {
//                                c = collection[i];
//                                if (slidesItems[getTrackFromItem(c, i)] == null) {
//                                    _results.push({
//                                        value: c,
//                                        index: i
//                                    });
//                                }
//                            }
//                            return _results;
//                        })();
//                        toRemove = (function () {
//                            var _results;
//                            _results = [];
//                            for (t in slidesItems) {
//                                i = slidesItems[t];
//                                if (trackCollection[t] == null) {
//                                    _results.push(i.collectionItem);
//                                }
//                            }
//                            return _results;
//                        })();
//                        if ((toAdd.length === 1 && toRemove.length === 0) || toAdd.length === 0) {
//                            for (_j = 0, _len1 = toRemove.length; _j < _len1; _j++) {
//                                e = toRemove[_j];
//                                e = removeSlide(e, collection.indexOf(e));
//                                if (e)
//                                    slider.removeSlide(e.element);
//                            }
//                            for (_k = 0, _len2 = toAdd.length; _k < _len2; _k++) {
//                                e = toAdd[_k];
//                                idx = e.index;
//                                addSlide(e.value, idx, function (item) {
//                                    if (idx === currentSlidesLength) {
//                                        idx = void 0;
//                                    }
//                                    return $scope.$evalAsync(function () {
//                                        return slider.addSlide(item.element, idx);
//                                    });
//                                });
//                            }
//                            return;
//                        }
//                    }
//                    slidesItems = {};
//                    if (flexsliderDiv != null) {
//                        flexsliderDiv.remove();
//                    }
//                    slides = angular.element('<ul class="slides"></ul>');
//                    flexsliderDiv = angular.element('<div class="flexslider"></div>');
//                    flexsliderDiv.append(slides);
//                    $element.append(flexsliderDiv);
//                    for (i = _l = 0, _len3 = collection.length; _l < _len3; i = ++_l) {
//                        c = collection[i];
//                        addSlide(c, i, function (item) {
//                            return slides.append(item.element);
//                        });
//                    }
//                    options = {};
//                    for (attrKey in attr) {
//                        attrVal = attr[attrKey];
//                        if (attrKey.indexOf('$') === 0) {
//                            continue;
//                        }
//                        if (!isNaN(n = parseInt(attrVal))) {
//                            options[attrKey] = n;
//                            continue;
//                        }
//                        if (attrVal === 'false' || attrVal === 'true') {
//                            options[attrKey] = attrVal === 'true';
//                            continue;
//                        }
//                        if (attrKey === 'start' || attrKey === 'before' || attrKey === 'after' || attrKey === 'end' || attrKey === 'added' || attrKey === 'removed') {
//                            options[attrKey] = (function (attrVal) {
//                                var f;
//                                f = $parse(attrVal);
//                                return function (slider) {
//                                    return $scope.$apply(function () {
//                                        return f($scope, {
//                                            '$slider': {
//                                                element: slider
//                                            }
//                                        });
//                                    });
//                                };
//                            })(attrVal);
//                            continue;
//                        }
//                        if (attrKey === 'startAt') {
//                            options[attrKey] = $parse(attrVal)($scope);
//                            continue;
//                        }
//                        options[attrKey] = attrVal;
//                    }
//                    if (!options.sliderId && attr.id) {
//                        options.sliderId = "" + attr.id + "-slider";
//                    }
//                    if (options.sliderId) {
//                        flexsliderDiv.attr('id', options.sliderId);
//                    }
//                    return $timeout((function () {
//                        return flexsliderDiv.flexslider(options);
//                    }), 0);
//                });
//            };
//        }
//    };
//}