angular.module("school").controller("equiposConfigCtrl", ["$scope", "$http", "$window", "toaster", "$modal", equiposConfigCtrl]);


function equiposConfigCtrl($scope, $http, $window, toaster, $modal) {
    var vm = this;
    vm.session = $window.sessionStorage;

    vm.equipos = [];
    vm.equipoSelected = [];
    vm.jugadoresEquipo = [];
    vm.jugadores = [];
    vm.entrenamiento = null;
    vm.loading = true;


    if (vm.session.usuario == null) {
        $window.location.href = webroot + "Account/Login";
    }

    vm.getEquipos = getEquipos;
    vm.getJugadores = getJugadores;
    vm.addHijoEquipo = addHijoEquipo;
    vm.removeHijoEquipo = removeHijoEquipo;
    vm.addEquipo = addEquipo;
    vm.editEquipo = editEquipo;

    ///////INIT
    getDeportes();
    getCategorias();

    function getEquipos() {
        vm.equipos = [];
        vm.jugadores = [];
        vm.jugadoresEquipo = [];
        
        $http.post(webroot + "Manage/LoadEquipos", {idDeporte:vm.deporteSelected[0].id,idCategoria:vm.categoriaSelected[0].id})
            .then(function (response) {
                if (response.data.cod == "OK") {
                    vm.equipos = response.data.d.equipos;
                } else {
                    toaster.pop({
                        type: 'error',
                        title: "Error",
                        body: "Test",
                        showCloseButton: true
                    });
                }
                vm.loading = false;
            });

    }

    function getDeportes() {

        vm.loading = true;
        $http.post(webroot + "Manage/LoadDeportes", {})
            .then(function (response) {
                if (response.data.cod == "OK") {
                    vm.deportes = response.data.d.deportes;
                } else {
                    toaster.warning({ title: "Title example", body: "This is example of toaster notification box." });
                }
                vm.loading = false;
            });

    }

    function getCategorias() {

        vm.loading = true;
        $http.post(webroot + "Manage/LoadCategorias", {})
            .then(function (response) {
                if (response.data.cod == "OK") {
                    vm.categorias = response.data.d.categorias;
                } else {
                    toaster.warning({ title: "Title example", body: "This is example of toaster notification box." });
                }
                vm.loading = false;
            });

    }

    function getJugadores() {

        vm.loading = true;
        $http.post(webroot + "Manage/LoadJugadores", {idEquipo:vm.equipoSelected[0].id})
            .then(function (response) {
                if (response.data.cod == "OK") {
                    if (response.data.d.libres != undefined) {
                        vm.jugadores = response.data.d.libres;
                    }
                    if (response.data.d.jugadoresEquipo != undefined) {
                        vm.jugadoresEquipo = response.data.d.jugadoresEquipo;
                    }
                } else {
                    toaster.warning({ title: "Title example", body: "This is example of toaster notification box." });
                }
                vm.loading = false;
            });

    }

    function addHijoEquipo(idHijo) {

        
        $http.post(webroot + "Manage/AddHijoEquipo", {idHijo:idHijo,idEquipo:vm.equipoSelected[0].id})
            .then(function (response) {
                if (response.data.cod == "OK") {
                    
                } else {
                    toaster.warning({ title: "Title example", body: "This is example of toaster notification box." });
                }
                vm.loading = false;
            });

    }

    function removeHijoEquipo(idHijo) {

        
        $http.post(webroot + "Manage/RemoveHijoEquipo", {idHijo:idHijo,idEquipo:vm.equipoSelected[0].id})
            .then(function (response) {
                if (response.data.cod == "OK") {
                    
                } else {
                    toaster.pop({
                        type: 'error',
                        title: "Error",
                        body: response.data.msg,
                        showCloseButton: true
                    });
                }
                vm.loading = false;
            });

    }

    
    function addEquipo() {

        var item = {};
        item.deportes = vm.deportes;
        item.categorias = vm.categorias;

        $modal.open({
            templateUrl: 'formEquipo.html',
            controller: ModalInstanceCtrl,
            controllerAs: 'vm',
            windowClass: "usuario-modal",
            resolve: {
                item: function () { return item; }
            }
        }).result.then(function (result) {

            

        });
    }

    function editEquipo(equipo) {

        var item = {};
        item.deportes = vm.deportes;
        item.categorias = vm.categorias;
        item.equipo = equipo;

        $modal.open({
            templateUrl: 'formEquipo.html',
            controller: ModalInstanceCtrl,
            controllerAs: 'vm',
            windowClass: "usuario-modal",
            resolve: {
                item: function () { return item; }
            }
        }).result.then(function (result) {

            

        });
    }

}

ModalInstanceCtrl.$inject = ['$scope', '$modalInstance', '$http', '$window', 'item'];
function ModalInstanceCtrl($scope, $modalInstance, $http, $window, item) {

    //Variables
    var vm = this;
    vm.edit = false;
    vm.session = $window.sessionStorage;
    vm.tipoSelected = [];
    vm.deportes = item.deportes;
    vm.deporteSelected = [];
    vm.categorias = item.categorias;
    vm.categoriaSelected = [];

    if (item.equipo == null) {
        
        vm.nombre = "";
        

        vm.edit = false;
    } else {
        
        vm.nombre = item.nombre;
        

        vm.edit = true;
    }

    //FUNCIONES
    vm.aceptar = aceptar;
    vm.cancelar = cancelar;
    ///////////////////////////
    //INIT
    
    ////////////////////////////



    function aceptar() {

        if (!vm.edit) {
            $http.post(webroot + "Manage/addEquipo",
                {
                    nombre: vm.nombre,
                    idDeporte: vm.deporteSelected[0].id,
                    idCategoria: vm.categoriaSelected[0].id
                }).then(function (response) {
                if (response.data.cod == "OK") {
                    //toaster.pop({
                    //    type: 'success',
                    //    title: "Correcto",
                    //    body: "Se ha añadido el usuario",
                    //    showCloseButton: true
                    //});
                    $modalInstance.close();
                } else {
                    //toaster.pop({
                    //    type: 'error',
                    //    title: "Error",
                    //    body: response.data.msg,
                    //    showCloseButton: true
                    //});
                }
            });
        } else {
            $http.post(webroot + "Manage/saveEquipo",
                {
                    id: item.id,
                    nombre: vm.nombre,
                    idDeporte: vm.deporteSelected[0].id,
                    idCategoria: vm.categoriaSelected[0].id
                }).then(function (response) {
                if (response.data.cod == "OK") {
                    //toaster.pop({
                    //    type: 'success',
                    //    title: "Correcto",
                    //    body: "Se ha guardado el usuario",
                    //    showCloseButton: true
                    //});

                    $modalInstance.close();
                } else {
                    //toaster.pop({
                    //    type: 'error',
                    //    title: "Error",
                    //    body: response.data.msg,
                    //    showCloseButton: true
                    //});
                }
            });
        }






    };

    function cancelar() {

        $modalInstance.dismiss('cancel');
    };



};