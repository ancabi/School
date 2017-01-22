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
            resolve: {
                function () { return; }
            }
        });

    }

}

modalPartidoCtrl.$inject = ['$scope', '$modalInstance', '$http', 'notify'];

function modalPartidoCtrl($scope, $modalInstance, $http, notify) {
    var vm = this;

    vm.closeModal = closeModal;

    function closeModal() {
        $modalInstance.close();
    }
}
