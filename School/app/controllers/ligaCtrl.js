angular.module("school").controller("ligaCtrl", ["$scope", "$http", "$window", "notify", "$modal", ligaCtrl]);

function ligaCtrl($scope, $http, $window, notify, $modal) {
    vm = this;

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

    getLiga();
    
    function getLiga() {
        $http.post(webroot + "Liga/getLiga")
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.liga = response.data.d.liga;
                  vm.nombreliga = vm.liga[0].nombre;
                  vm.idliga = vm.liga[0].id;
                  getLigaEquipos(vm.liga[0].id);
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

}

