angular.module("school").controller("ligaCtrl", ["$scope", "$http", "$window", "notify", "$modal", ligaCtrl]);

function ligaCtrl($scope, $http, $window, notify, $modal) {
    var vm = this;
    vm.session = $window.sessionStorage;

    vm.liga = [];
    vm.ligaequipos = [];
    vm.ligaequipos_disp = [];
    vm.ligaresultados = [];
    vm.ligaresultados_disp = [];

    vm.nombreliga = "";
    vm.idliga = 0;

    vm.getLiga = getLiga;
    vm.getLigaEquipos = getLigaEquipos;
    vm.getLigaResultados = getLigaResultados;
    vm.addJornada = addJornada;

    getLiga();
    
    
    function getLiga() {
        $http.post(webroot + "Liga/getLiga", {idEquipo:vm.session.idEquipo})
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.liga = response.data.d.liga;
                  vm.nombreliga = vm.liga[0].nombre;
                  vm.idliga = vm.liga[0].id;
                  getLigaEquipos(vm.liga[0].id);
                  loadJornadas();
              } else {
                  notify({ message: 'No se ha podido mostrar el historico.', classes: 'alert-danger' });
              }
          });
    }
    function getLigaEquipos(idliga) {
        $http.post(webroot + "Liga/getLigaEquipos", { idLiga: idliga })
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.ligaequipos = response.data.d.ligaequipos;
                  vm.ligaequipos_disp = response.data.d.ligaequipos;
              } else {
                  notify({ message: 'No se ha podido mostrar el historico.', classes: 'alert-danger' });
              }
          });
    }
    function getLigaResultados(idliga, jornada) {
        $http.post(webroot + "Liga/getLigaResultados", { idLiga: idliga, jornada: jornada })
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.ligaresultados = response.data.d.ligaresultados;
              } else {
                  notify({ message: 'No se ha podido mostrar el historico.', classes: 'alert-danger' });
              }
          });
    }

    function loadJornadas() {
        $http.post(webroot + "Liga/getJornadas", { idLiga: vm.idliga })
            .then(function (response) {
                vm.jornadas = response.data.d.jornadas;
                vm.jornada = vm.jornadas[0].jornada;
            });
    }

    function addJornada() {

        var modalEntrenamiento = $modal.open({
            templateUrl: 'modalJornada.html',
            size: "lg",
            controller: modalCtrl,
            controllerAs: 'vm',
            backdrop: 'static',
            windowClass: 'hmodal-success',
            resolve: {
                item: function () { return vm.idliga; }
            }
        }).result.then(function (result) {
            if (result) {
                loadJornadas();
            }
        });

    }

}

modalCtrl.$inject = ['$scope', '$modalInstance', '$http', 'notify', 'item'];

function modalCtrl($scope, $modalInstance, $http, notify, item) {
    var vm = this;

    
    $scope.list2 = {};
    

    if (item == null) {
        vm.edit = false;
        vm.local = [];
        vm.visitante = [];
    } else {
        vm.edit = true;
        vm.local = [];
        vm.visitante = [];
    }

    vm.closeModal = closeModal;
    vm.addJornada = addJornada;
    vm.aceptar = aceptar;
    vm.getNumeroPartidos=getNumeroPartidos;

    getLigaEquipos();

    function getLigaEquipos() {
        $http.post(webroot + "Liga/getLigaEquipos", { idLiga: item })
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.ligaequipos = response.data.d.ligaequipos;
              } else {
                  notify({ message: 'No se ha podido mostrar el historico.', classes: 'alert-danger' });
              }
          });
    }

    function getNumeroPartidos() {
        return new Array(vm.ligaequipos.length /2);
    }

    function closeModal(res) {
        $modalInstance.close(res);
    }

    function aceptar() {
        //if (vm.edit) {
        //    saveEntrenamiento();
        //} else {
            addJornada();
        //}
    }

    function addJornada() {
        vm.saving = true;
        //if (validarCampos()) {
            $http.post(webroot + "Liga/addJornada", {
                locales: vm.local,
                visitantes: vm.visitante
            }).then(function (response) {
                if (response.data.cod == "OK") {
                    closeModal(true);
                } else {
                    notify({ message: response.data.msg, classes: 'alert-danger' });
                }
                vm.saving = false;

            });

        //} else {
            //vm.saving = false;
        //}
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

        if (vm.ligaequipos.length != 0) {
            swal("Cuidado", "Debe asignar un partido a todos los equipos", "warning");
            valido = false;
        } 

        return valido;
    }
}
