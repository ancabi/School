angular.module("school").controller("equiposCtrl", ["$scope", "$http", "$window", "toastr", "$modal", equiposCtrl]);


function equiposCtrl($scope, $http, $window, toastr, $modal) {
    var vm = this;
    vm.session = $window.sessionStorage;

    vm.jugadores = [];
    vm.jugadores_disp = [];
    vm.entrenamiento = null;
    vm.loading = true;


    if (vm.session.usuario == null) {
        $window.location.href = webroot + "Account/Login";
    }

    vm.addEntrenamiento = addEntrenamiento;

    ///////INIT
    getJugadores();
    getNextEntrenamiento();
    getNextJornada();
    getClasificacion();

    function getJugadores() {

        vm.loading = true;
        $http.post(webroot + "Equipos/getJugadores", {idEquipo:vm.session.idEquipo})
            .then(function (response) {
                if (response.data.cod == "OK") {
                    vm.jugadores = response.data.d.jugadores;
                    vm.jugadores_disp = [].concat(response.data.d.jugadores);
                } else {
                    toastr.warning({ title: "Title example", body: "This is example of Toastr notification box." });
                }
                vm.loading = false;
            });

    }

    function getNextEntrenamiento() {

        vm.loading = true;
        $http.post(webroot + "Equipos/getNextEntrenamiento", {})
            .then(function (response) {
                if (response.data.cod == "OK") {
                    vm.entrenamiento = response.data.d.jugadores;
                } else {
                     toastr.warning({ title: "Title example", body: "This is example of Toastr notification box." });
                }
                vm.loading = false;
            });

    }

    function getNextJornada() {

        vm.loading = true;
        $http.post(webroot + "Equipos/getNextJornada", {})
            .then(function (response) {
                if (response.data.cod == "OK") {
                    vm.jornada = response.data.d.jornadas;
                } else {
                    //notify({ message: response.data.msj, classes: 'alert-danger' });
                }
                vm.loading = false;
            });

    }

    function getClasificacion() {

        vm.loading = true;
        $http.post(webroot + "Liga/getLigaClasificacion")
            .then(function (response) {
                if (response.data.cod == "OK") {
                    vm.clasificacion = response.data.d.clasificacion;
                } else {
                    toastr.warning({ title: "Title example", body: "This is example of Toastr notification box." });
                }
                vm.loading = false;
            });

    }

    function addEntrenamiento() {

        var modalEntrenamiento = $modal.open({
            templateUrl: 'modalEntrenamiento.html',
            size: "lg",
            controller: modalCtrl,
            controllerAs: 'vm',
            backdrop: 'static',
            windowClass: 'hmodal-success',
            resolve: {
                item: function () { return null; }
            }
        }).result.then(function (result) {
            if (result) {
                getNextEntrenamiento();
            }
        });

    }
    

}

modalCtrl.$inject = ['$scope', '$modalInstance', '$http', 'toastr', 'item'];

function modalCtrl($scope, $modalInstance, $http, toastr, item) {
    var vm = this;

    vm.entrenamientos = [];
    vm.entrenamiento = null;
    vm.fecha = "";

    vm.closeModal = closeModal;

    vm.aceptar = aceptar;

    //INIT
    getEntrenamientos();

    function getEntrenamientos() {

        $http.post(webroot + "Entrenamientos/getEntrenamientos")
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.entrenamientos = response.data.d.entrenamientos;
              } else {
                  notify({ message: 'No se ha podido mostrar el historico.', classes: 'alert-danger' });
              }
          });
    }

    function closeModal(res) {
        $modalInstance.close(res);
    }

    function aceptar() {
        vm.saving = true;
        if (validarCampos()) {
            $http.post(webroot + "Equipos/addEntrenamiento", {
                idEntrenamiento: vm.entrenamiento.id,
                fecha: vm.fecha
            }).then(function (response) {
                if (response.data.cod == "OK") {
                    closeModal(true);
                } else {
                    toastr.warning({ title: "Title example", body: "This is example of Toastr notification box." });
                }
                vm.saving = false;

            });

        } else {
            vm.saving = false;
        }
    }
    
    function validarCampos() {
        var valido = true;

        if (vm.entrenamiento == null) {
            vm.errorEntrenamiento = true;
            valido = false;
        } else {
            vm.errorEntrenamiento = false;
        }

        if (vm.fecha == "") {
            vm.errorFecha = true;
            valido = false;
        } else {
            vm.errorFecha = false;
        }

        return valido;
    }
}