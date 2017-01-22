angular.module("school").controller("eventosCtrl", eventosCtrl);

eventosCtrl.$inject = ["$scope", "$http", "$filter", "$modal", "$document", "notify", "sweetAlert", "$window", "PANEL_HCLASES"];


function eventosCtrl($scope, $http, $filter, $modal, $document, notify, sweetAlert, $window, PANEL_HCLASES) {
    vm = this;

    vm.rows = [];

    vm.getEventos = getEventos;
    vm.showModalEvento = showModalEvento;

    vm.editEvento = editEvento;
    vm.chActive = chActive;
    vm.deleteEvento = deleteEvento;

    getEventos();

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

    function editEvento(id) {
        alert(id);
        $("#modalEditEvento").modal('show');
    }

    function chActive(id) {
        $http.post(webroot + "Eventos/changeActive", {id:id})
          .then(function (response) {
              if (response.data.cod === "OK") {
                  notify({ message: 'Estado cambiado.', classes: 'alert-success' });
              } else {
                  notify({ message: 'No se ha cambiar el estado.', classes: 'alert-danger' });
              }
          });
    }

    function deleteEvento(id) {
    }

    function showModalEvento() {

        var modalEvento = $modal.open({
            templateUrl: 'modalEvento.html',
            size: "md",
            controller: modalEventoCtrl,
            controllerAs: 'vm',
            backdrop: 'static',
            resolve: {
                function () { return; }
            }
        });

    }

}

//angular.module("school").controller("modalEventoCtrl", modalEventoCtrl);

modalEventoCtrl.$inject = ['$scope', '$modalInstance', '$http', 'notify'];

function modalEventoCtrl($scope, $modalInstance, $http, notify) {
    var vm = this;

    vm.closeModal = closeModal;

    function closeModal() {
        $modalInstance.close();
    }
}
