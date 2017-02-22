angular.module("school").controller("entrenamientosCtrl", entrenamientosCtrl);

entrenamientosCtrl.$inject = ["$scope", "$http", "$filter", "$modal", "$document", "notify", "$window"];


function entrenamientosCtrl($scope, $http, $filter, $modal, $document, notify, $window) {
    vm = this;
    vm.session = $window.sessionStorage;

    vm.entrenamientos = [];
    vm.entrenamientos_disp = [];

    //FUNCTIONS
    vm.addEntrenamiento = addEntrenamiento;

    vm.editEntrenamiento = editEntrenamiento;
    
    vm.deleteEntrenamiento = deleteEntrenamiento;

    //INIT
    getEntrenamientos();

    function getEntrenamientos() {

        $http.post(webroot + "Entrenamientos/getEntrenamientos")
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.entrenamientos = response.data.d.entrenamientos;
                  vm.entrenamientos_disp = [].concat(response.data.d.entrenamientos);
              } else {
                  notify({ message: 'No se ha podido mostrar el historico.', classes: 'alert-danger' });
              }
          });
    }

    function editEntrenamiento(row) {
        var modalEntrenamiento = $modal.open({
            templateUrl: 'modalEntrenamiento.html',
            size: "lg",
            controller: modalCtrl,
            controllerAs: 'vm',
            backdrop: 'static',
            windowClass: 'hmodal-default',
            resolve: {
                item: function () { return row; }
            }
        });
    }

    function deleteEntrenamiento(row) {
        $http.post(webroot + "Entrenamientos/deleteEntrenamiento", {id:row.id})
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.entrenamientos.splice(vm.entrenamientos.indexOf(row),1);
              } else {
                  notify({ message: 'Error al borrar el entrenamiento', classes: 'alert-danger' });
              }
          });
    }

    function addEntrenamiento() {

        var modalEntrenamiento = $modal.open({
            templateUrl: 'modalEntrenamiento.html',
            size: "lg",
            controller: modalCtrl,
            controllerAs: 'vm',
            backdrop: 'static',
            windowClass:'hmodal-success',
            resolve: {
                item: function () { return null; }
            }
        }).result.then(function(result) {
            if (result) {
                getEntrenamientos();
            }
        });

    }

}
modalCtrl.$inject = ['$scope', '$modalInstance', '$http', 'notify','item'];

function modalCtrl($scope, $modalInstance, $http, notify,item) {
    var vm = this;

    if (item == null) {
        vm.edit = false;
        vm.nombre = "";
        vm.tipo = null;
        vm.descripcion = "";
        vm.duracion = "";
        
    } else {
        vm.edit = true;
        vm.nombre = item.nombre;
        vm.tipo = item.tipo;
        vm.descripcion = item.descripcion;
        vm.duracion = item.duracion;
        
    }

    vm.closeModal = closeModal;
    vm.addEntrenamiento = addEntrenamiento;
    vm.aceptar = aceptar;

    function closeModal(res) {
        $modalInstance.close(res);
    }

    function aceptar() {
        if (vm.edit) {
            saveEntrenamiento();
        } else {
            addEntrenamiento();
        }
    }

    function addEntrenamiento() {
        vm.saving = true;
        if (validarCampos()) {
            $http.post(webroot + "Entrenamientos/addEntrenamiento", {
                nombre: vm.nombre,
                tipo: vm.tipo,
                descripcion: vm.descripcion,
                duracion: vm.duracion
            }).then(function (response) {
                if (response.data.cod == "OK") {
                    closeModal(true);
                } else {
                    notify({ message: response.data.msg, classes: 'alert-danger' });
                }
                vm.saving = false;
                
            });
            
        } else {
            vm.saving = false;
        }
    }

    function saveEntrenamiento() {
        vm.saving = true;
        if (validarCampos()) {
            $http.post(webroot + "Entrenamientos/saveEntrenamiento", {
                id: item.id,
                nombre: vm.nombre,
                tipo: vm.tipo,
                descripcion: vm.descripcion,
                duracion: vm.duracion
            }).then(function (response) {
                if (response.data.cod == "OK") {
                    item.nombre = vm.nombre;
                    item.tipo = vm.tipo;
                    item.descripcion = vm.descripcion;
                    item.duracion = vm.duracion;
                    closeModal();
                } else {
                    notify({ message: response.data.msg, classes: 'alert-danger' });
                }
                vm.saving = false;
                
            });
            
        } else {
            vm.saving = false;
        }
    }

    function validarCampos() {
        var valido = true;

        if (vm.nombre == "") {
            vm.errorNombre = true;
            valido = false;
        } else {
            vm.errorNombre = false;
        }

        if (vm.tipo == null) {
            vm.errorTipo = true;
            valido = false;
        } else {
            vm.errorTipo = false;
        }

        if (vm.descripcion == "") {
            vm.errorDescripcion = true;
            valido = false;
        } else {
            vm.errorDescripcion = false;
        }

        if (vm.duracion == "") {
            vm.errorDuracion = true;
            valido = false;
        } else {
            vm.errorDuracion = false;
        }

        return valido;
    }
}