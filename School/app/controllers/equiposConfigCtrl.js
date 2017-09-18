angular.module("school").controller("equiposConfigCtrl", ["$scope", "$http", "$window", "toastr", "$modal","dragulaService", equiposConfigCtrl]);


function equiposConfigCtrl($scope, $http, $window, toastr, $modal, dragulaService) {
    var vm = this;
    vm.session = $window.sessionStorage;

    vm.equipos = [];
    vm.equipoSelected = [];
    vm.jugadores = [];
    vm.jugadores_disp = [];
    vm.entrenamiento = null;
    vm.loading = true;


    if (vm.session.usuario == null) {
        $window.location.href = webroot + "Account/Login";
    }

    vm.getJugadores = getJugadores;

    ///////INIT
    getEquipos();

    function getEquipos() {

        vm.loading = true;
        $http.post(webroot + "Manage/LoadEquipos", {})
            .then(function (response) {
                if (response.data.cod == "OK") {
                    vm.equipos = response.data.d.equipos;
                } else {
                    toastr.warning({ title: "Title example", body: "This is example of Toastr notification box." });
                }
                vm.loading = false;
            });

    }

    function getJugadores() {

        
        $http.post(webroot + "Manage/LoadJugadores", {idEquipo:vm.equipoSelected[0].id})
            .then(function (response) {
                if (response.data.cod == "OK") {
                    vm.jugadores = response.data.d.libres;
                    vm.jugadoresEquipo = response.data.d.jugadoresEquipo;
                } else {
                    toastr.warning({ title: "Title example", body: "This is example of Toastr notification box." });
                }
                vm.loading = false;
            });

    }

    
    vm.console = function() {
        console.log(vm.jugadores);
        console.log(vm.jugadoresEquipo);
    }

}

