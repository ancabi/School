angular.module("school").controller("ligaCtrl", ["$scope", "$http", "$window", "$filter", "notify", "$modal", ligaCtrl]);

function ligaCtrl($scope, $http, $window, $filter, notify, $modal) {
    var vm = this;
    vm.session = $window.sessionStorage;

    vm.liga = [];
    vm.ligaequipos = [];
    vm.ligaequipos_disp = [];
    vm.ligaresultados = [];
    vm.ligaresultados_disp = [];
    vm.labelsPartidos = ["Ganados", "Empatados", "Perdidos"];
    vm.labelsGoles = ["A favor", "En contra"];
    vm.labelsPuntos = [];
    vm.dataPartidos = [];
    vm.dataGoles = [];
    vm.dataPuntos = [];
    vm.seriesPuntos = ["Puntos"];
    vm.colors = ["#356cbf", "#3775d3", "#4f8dea", "#6ba2f4"];

    vm.nombreliga = "";
    vm.idliga = 0;

    vm.getLiga = getLiga;
    vm.getLigaEquipos = getLigaEquipos;
    vm.getLigaResultados = getLigaResultados;
    vm.addJornada = addJornada;
    vm.addResultado = addResultado;

    getLiga();
    getClasificacion();
    
    
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
                vm.jornada = vm.jornadas[vm.jornadas.length-1].jornada;
            });
    }

    function getClasificacion() {

        vm.loading = true;
        $http.post(webroot + "Liga/getLigaClasificacion")
            .then(function (response) {
                if (response.data.cod == "OK") {
                    vm.clasificacion = response.data.d.clasificacion;

                    angular.forEach(vm.clasificacion,
                        function(c) {
                            if (c.id_equipo == vm.session.idEquipo) {
                                vm.dataPartidos.push(c.ganados);
                                vm.dataPartidos.push(c.empatados);
                                vm.dataPartidos.push(c.perdidos);

                                vm.dataGoles.push(c.goles_favor);
                                vm.dataGoles.push(c.goles_contra);
                            }

                            vm.labelsPuntos.push(c.nombre);
                            vm.dataPuntos.push(c.puntos);

                        });


                } else {
                    notify({ message: response.data.msj, classes: 'alert-danger' });
                }
                vm.loading = false;
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

    function addResultado() {

        var partidos = $filter('filter')(vm.jornadas, { 'jornada': vm.jornada });

        var modalEntrenamiento = $modal.open({
            templateUrl: 'modalResultados.html',
            size: "lg",
            controller: modalResultadoCtrl,
            controllerAs: 'vm',
            backdrop: 'static',
            windowClass: 'hmodal-success',
            resolve: {
                item: function () { return partidos; }
            }
        }).result.then(function (result) {
            if (result) {
                loadJornadas();
                getClasificacion();
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

            addJornada();
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
}


modalResultadoCtrl.$inject = ['$scope', '$modalInstance', '$http', 'notify', 'item'];

function modalResultadoCtrl($scope, $modalInstance, $http, notify, item) {
    var vm = this;

    vm.jornada = item;
    
    vm.edit = !(vm.jornada[0].resultado_local == undefined);
    

    vm.closeModal = closeModal;
    vm.addJornada = addJornada;
    vm.aceptar = aceptar;

    

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
        if (validarCampos()) {
            $http.post(webroot + "Liga/saveResultados", {
                jornada: vm.jornada,
                edit:vm.edit
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
            swal("Cuidado", "Debe completar todos los resultdos", "warning");
        }
    }

    function validarCampos() {
        var valido = true;

        angular.forEach(vm.jornada,
            function(j) {
                if (j.resultado_local == undefined || j.resultado_visitante == undefined) {
                    valido = false;
                }
            });

        return valido;
    }
}
