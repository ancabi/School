angular.module("school").controller("comunicadosCtrl", comunicadosCtrl);

comunicadosCtrl.$inject = ["$scope", "$http", "$filter", "$modal", "$document", "notify", "$window"];


function comunicadosCtrl($scope, $http, $filter, $modal, $document, notify, $window) {
    vm = this;

    vm.comunicados = [];

    vm.getComunicados = getComunicados;
    vm.enviarComunicado = enviarComunicado;

    getComunicados();

    function getComunicados() {
        $http.post(webroot + "Comunicados/getComunicados")
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.comunicados = response.data.d.comunicados;
              } else {
                  notify({ message: 'No se ha podido mostrar el historico.', classes: 'alert-danger' });
              }
          });
    }

    function enviarComunicado() {

        var modalComunicado = $modal.open({
            templateUrl: 'modalComunicado.html',
            size: "md",
            controller: modalComunicadoCtrl,
            controllerAs: 'vm',
            backdrop: 'static',
            resolve: {
                function () { return; }
            }
        });

    }

}

modalComunicadoCtrl.$inject = ['$scope', '$modalInstance', '$http', 'notify'];

function modalComunicadoCtrl($scope, $modalInstance, $http, notify) {
    var vm = this;
    vm.titulo = "";
    vm.descripcion = "";
    vm.fecha = moment().format("DD-MM-YYYY");
    vm.equipos = [];
    vm.equipoSelected = [];

    vm.closeModal = closeModal;

    function closeModal() {
        $modalInstance.close();
    }
}
