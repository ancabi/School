angular.module("school").controller("equiposCtrl", ["$scope", "$http", "$window", "notify", "sweetAlert", equiposCtrl]);


function equiposCtrl($scope, $http, $window, notify, sweetAlert) {
    vm = this;
    vm.session = $window.sessionStorage;

    vm.jugadores = [];
    vm.jugadores_disp = [];
    vm.loading1 = true;


    if (vm.session.usuario == null) {
        $window.location.href = webroot + "Account/Login";
    }

    ///////INIT
    getJugadores();

    function getJugadores() {

        vm.loading = true;
        $http.post(webroot + "Equipos/getJugadores", {idEquipo:vm.session.idEquipo})
            .then(function (response) {
                if (response.data.cod == "OK") {
                    vm.jugadores = response.data.d.jugadores;
                    vm.jugadores_disp = [].concat(response.data.d.jugadores);
                } else {
                    notify({ message: response.data.msj, classes: 'alert-danger' });
                }
                vm.loading = false;
            });

    }
    

}

