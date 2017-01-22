angular.module('school').controller('mainCtrl', ["$scope", "$http", "$timeout", "$window", "$modal", "PANEL_HCLASES", mainCtrl]);

function mainCtrl($scope, $http, $timeout, $window, $modal, PANEL_HCLASES) {
    var vm = this;
    vm.session = $window.sessionStorage;
    vm.PANEL_HCLASES = PANEL_HCLASES;

    vm.rows = [];
    vm.equipo = {};

    vm.getEventos = getEventos;

    //INIT
    if (vm.session.usuario == null) {
        $window.location.href = webroot + "Account/Login";
    }

    getEventos();
    getEquipos();

    function getEventos() {
        var n = 0;
        $http.post(webroot + "Eventos/getEventos")
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.rows = response.data.d.eventos;
              } else {
                  notify({ message: 'No se ha podido mostrar el historico.', classes: 'alert-danger' });
              }
          });
    }

    function getEquipos() {
        $http.post(webroot + "Main/getEquipos")
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.equipos = response.data.d.equipos;
              } else {
                  notify({ message: 'Error', classes: 'alert-danger' });
              }
          });
    }

    // For iCheck purpose only
    $scope.checkOne = true;

    /**
     * Rating - used for rating in components view
     */
    $scope.rate = 7;
    $scope.max = 10;

    $scope.hoveringOver = function (value) {
        $scope.overStar = value;
        $scope.percent = 100 * (value / this.max);
    };

    $scope.oneAtATime = true;

    $scope.stanimation = 'bounceIn';
    $scope.runIt = true;
    $scope.runAnimation = function () {

        $scope.runIt = false;
        $timeout(function () {
            $scope.runIt = true;
        }, 100);

    };

}

var yy;
var calendarArray = [];
var monthOffset = [6, 7, 8, 9, 10, 11, 0, 1, 2, 3, 4, 5];
var monthArray = [["ENE", "enero"], ["FEB", "Febrero"], ["MAR", "Marzo"], ["ABR", "Abril"], ["MAY", "Mayo"], ["JUN", "Junio"], ["JUL", "Julio"], ["AGO", "Agosto"], ["SEP", "Septiembre"], ["OCT", "Octubre"], ["NOV", "Noviembre"], ["DIC", "Diciembre"]];
var letrasArray = ["L", "M", "X", "J", "V", "S", "D"];
var dayArray = ["7", "1", "2", "3", "4", "5", "6"];

$(document).ready(function () {
    $(document).on('click', '.calendar-day.have-events', schoolteDay);
    $(document).on('click', '.specific-day', schooltecalendar);
    $(document).on('click', '.calendar-month-view-arrow', offsetcalendar);
    $(window).resize(calendarScale);
    calendarSet();
    calendarScale();
    $('[data-toggle="tooltip"]').tooltip(); 
});