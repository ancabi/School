angular.module("school").controller("partidosCtrl", partidosCtrl);

partidosCtrl.$inject = ["$scope", "$http", "$filter", "$modal", "$document", "notify", "sweetAlert", "$window", "PANEL_HCLASES"];


function partidosCtrl($scope, $http, $filter, $modal, $document, notify, sweetAlert, $window, PANEL_HCLASES) {

   

    vm = this;

    vm.rows = [];

    vm.getPartidos = getPartidos;
    vm.showModalPartido = showModalPartido;

    vm.editPartido = editPartido;
    vm.chActive = chActive;
    vm.deletePartido = deletePartido;

    vm.showModalIncidenciasCtrl = showModalIncidenciasCtrl;
    vm.showModalResultado = showModalResultado;
    vm.showModalActa = showModalActa;

    getPartidos();

    function getPartidos() {
        var n = 0;
        $http.post(webroot + "Partidos/getPartidos")
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.rows = response.data.d.partidos;
              } else {
                  notify({ message: 'No se ha podido mostrar el historico.', classes: 'alert-danger' });
              }
          });
    }
    function editPartido(id) {
        alert(id);
        $("#modalEditPartido").modal('show');
    }
    function chActive(id) {
        $http.post(webroot + "Partidos/changeActive", {id:id})
          .then(function (response) {
              if (response.data.cod === "OK") {
                  notify({ message: 'Estado cambiado.', classes: 'alert-success' });
              } else {
                  notify({ message: 'No se ha cambiar el estado.', classes: 'alert-danger' });
              }
          });
    }

    function deletePartido(id) {
    }

    function showModalPartido() {
        var modalPartido = $modal.open({
            templateUrl: 'modalPartido.html',
            size: "md",
            controller: modalPartidoCtrl,
            controllerAs: 'vm',
            backdrop: 'static',
            resolve: { function () { 
                    getPartidos();
                }
            }
        });

    }

    function showModalIncidenciasCtrl(id) {
        var modalIncidencias = $modal.open({
            templateUrl: 'modalIncidencias.html',
            size: "md",
            controller: modalIncidenciasCtrl,
            controllerAs: 'vm',
            backdrop: 'static',
            resolve: {
                function () { return; }
            }
        });

    }

    function showModalResultado(id) {
        var modalResultado = $modal.open({
            templateUrl: 'modalResultado.html',
            size: "md",
            controller: modalResultadoCtrl,
            controllerAs: 'vm',
            backdrop: 'static',
            resolve: {
                function () { return; }
            }
        });

    }

    function showModalActa(id) {
        var modalActa = $modal.open({
            templateUrl: 'modalActa.html',
            size: "md",
            controller: modalActaCtrl,
            controllerAs: 'vm',
            backdrop: 'static',
            resolve: {
                function () { return; }
            }
        });

    }

}

modalPartidoCtrl.$inject = ['$scope', '$modalInstance', '$http', 'notify'];

function modalPartidoCtrl($scope, $modalInstance, $http, notify) {
    var vm = this;

    vm.ddEquipos = "";
    vm.equipo_contrario = "";
    vm.liga = "";
    vm.ddLugar = "";
    vm.direccion = "";
    vm.fechaPartido = "";
    vm.hora_inicio = "";

    vm.insertPartido = insertPartido;
    vm.closeModal = closeModal;

    function insertPartido() {
        var n = 0;
        var params = {
            equipo: vm.ddEquipos,
            equipo_contrario: vm.equipo_contrario,
            liga: vm.liga,
            lugar: vm.ddLugar,
            direccion: vm.direccion,
            fecha: vm.fechaPartido,
            hora: vm.hora_inicio
        }
        $http.post(webroot + "Partidos/newPartido", { partido: params })
          .then(function (response) {
              if (response.data.cod === "OK") {
                  closeModal();
              } else {
                  notify({ message: 'No se ha podido mostrar el historico.', classes: 'alert-danger' });
              }
          });
    }

    function closeModal() {
        $modalInstance.close();
    }
}

modalResultadoCtrl.$inject = ['$scope', '$modalInstance', '$http', 'notify'];

function modalResultadoCtrl($scope, $modalInstance, $http, notify) {
    var vm = this;

    vm.closeModal = closeModal;
    function closeModal() {
        $modalInstance.close();
    }
}

modalIncidenciasCtrl.$inject = ['$scope', '$modalInstance', '$http', 'notify'];

function modalIncidenciasCtrl($scope, $modalInstance, $http, notify) {
    var vm = this;

    vm.closeModal = closeModal;


    function closeModal() {
        $modalInstance.close();
    }
}

modalActaCtrl.$inject = ['$scope', '$modalInstance', '$http', 'notify'];

function modalActaCtrl($scope, $modalInstance, $http, notify) {
    var vm = this;

    vm.insertPartido = insertPartido;
    vm.closeModal = closeModal;

    function insertPartido() {
        var n = 0;

        var params = {
            equipo: vm.ddEquipos,
            equipo_contrario: vm.equipo_contrario,
            equipo_contrario: vm.equipo_contrario,
            liga: vm.liga,
            lugar: vm.ddLugar,
            direccion: vm.direccion,
            fechaPartido: vm.fechaPartido,
            hora_inicio: vm.hora_inicio
        }
        $http.post(webroot + "Partidos/getPartidos", { partido: params })
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.rows = response.data.d.partidos;
              } else {
                  notify({ message: 'No se ha podido mostrar el historico.', classes: 'alert-danger' });
              }
          });
    }

    function closeModal() {
        $modalInstance.close();
    }
}
