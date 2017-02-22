angular.module('school').controller('mainCtrl', ["$scope", "$http", "$timeout", "$window", "$modal", "PANEL_HCLASES", mainCtrl]);

function mainCtrl($scope, $http, $timeout, $window, $modal, PANEL_HCLASES) {
    var vm = this;
    vm.session = $window.sessionStorage;
    vm.PANEL_HCLASES = PANEL_HCLASES;

    vm.rows = [];
    vm.equipo = {};

    vm.getEventos = getEventos;
    vm.changeEquipo = changeEquipo;

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
                  loadEquipo();
                  
              } else {
                  notify({ message: 'Error', classes: 'alert-danger' });
              }
          });
    }

    function changeEquipo() {
        $http.post(webroot + "Main/setEquipoSelected", { e: vm.equipo });
        vm.session.idEquipo = vm.equipo.id;
    }

    function loadEquipo() {
        $http.post(webroot + "Main/getEquipoSelected")
          .then(function (response) {
             
              vm.equipo = response.data;

                angular.forEach(vm.equipos,
                    function(e) {
                        if (e.id == vm.equipo.id) {
                            vm.equipo = e;
                            vm.session.idEquipo = e.id;
                        }
                    });

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

$(document).ready(function () {


        /* initialize the calendar
         -----------------------------------------------------------------*/
        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();
        var i = 0;
        var events = [];

        $.ajax(
        {
            url: "/Calendario/getCalendario",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (data) {
                var res = data.d.calendario;
                console.log(res);
                $.each(res, function () {
                    var fecha = moment(res[i].fecha).format("YYYY-MM-DD");
                    console.log(res[i]);
                    events.push({ title: res[i].titulo, start: fecha });
                    i++;
                });
            }
        });

        $('#calendar').fullCalendar({
            lang: 'es',
            //editable: true,
            //droppable: true, // this allows things to be dropped onto the calendar
            //drop: function () {
                // is the "remove after drop" checkbox checked?
            //    if ($('#drop-remove').is(':checked')) {
            //        // if so, remove the element from the "Draggable Events" list
            //        $(this).remove();
            //    }
            //},
            events: events
        });





});