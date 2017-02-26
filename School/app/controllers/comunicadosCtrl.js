angular.module("school").controller("comunicadosCtrl", comunicadosCtrl);

comunicadosCtrl.$inject = ["$scope", "$http", "$filter", "$modal", "$document", "$window","toastr"];


function comunicadosCtrl($scope, $http, $filter, $modal, $document, $window,toastr) {
    var vm = this;

    vm.comunicados = [];
    vm.equipos = [];
    vm.jornadas = [];
    vm.loading = true;

    vm.getComunicados = getComunicados;
    vm.enviarComunicado = enviarComunicado;

    
    loadEquipos();
    

    function loadEquipos() {
        $http.post(webroot + "Main/getEquipos").then(function(response) {
            vm.equipos = response.data.d.equipos;
            getComunicados();
            });
    }

    function getComunicados() {
        $http.post(webroot + "Comunicados/getComunicados")
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.comunicados = response.data.d.comunicados;
                  
                  angular.forEach(vm.comunicados,
                      function(c) {
                          var ids = c.idequipos.split(";");
                          c.equipos = "";

                          for(var y = 0;y<ids.length-1;y++){

                                  for (var x = 0; x < vm.equipos.length; x++) {

                                      if (ids[y] == vm.equipos[x].id) {
                                          c.equipos += vm.equipos[x].nombre+",";
                                          break;
                                      }

                                  }
                          }
                          c.equipos = c.equipos.substring(0, c.equipos.length - 2);
                          vm.loading = false;
                      });


              } else {
                  toastr.warning({ title: "Title example", body: "This is example of Toastr notification box." });
                  vm.loading = false;
              }
          });
    }

    

    function enviarComunicado() {

        var modalComunicado = $modal.open({
            templateUrl: 'modalComunicado.html',
            size: "lg",
            controller: modalComunicadoCtrl,
            controllerAs: 'vm',
            backdrop: 'static',
            resolve: {
                function () { return; }
            }
        });

    }

}

modalComunicadoCtrl.$inject = ['$scope', '$modalInstance', '$http'];

function modalComunicadoCtrl($scope, $modalInstance, $http) {
    var vm = this;
    vm.titulo = "";
    vm.descripcion = "";
    vm.fecha = moment().format("DD-MM-YYYY");
    vm.equipos = [];
    vm.equipoSelected = [];

    loadEquipos();

    vm.closeModal = closeModal;
    vm.send = send;

    function loadEquipos() {
        $http.post(webroot + "Main/getEquipos").then(function(response) {
            vm.equipos = response.data.d.equipos;
        });
    }

    function send() {

        var idEquipos = [];

        angular.forEach(vm.equipoSelected,
            function(e) {
                idEquipos.push(e.id);
            });

        if (idEquipos.length == 0) {
            angular.element("#inputEquipos").addClass("has-error");
        } else {


            $http.post(webroot + "Comunicados/Enviar",
                    { titulo: vm.titulo, descripcion: vm.descripcion, fecha: vm.fecha, equipos:idEquipos })
                .then(function(response) {
                    if (response.data.cod == "OK") {
                        closeModal();
                    } else {
                        notify({ message: "Error al enviar el comunicado", classes: 'alert-danger' });
                    }

                });
        }
    }

    function closeModal() {
        $modalInstance.close();
    }
}
