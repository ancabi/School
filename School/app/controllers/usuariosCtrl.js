angular.module('school').controller('usuariosCtrl', ['$scope', '$http', '$window', '$modal','$filter', usuariosCtrl]);

function usuariosCtrl($scope, $http, $window, $modal,$filter) {
    var vm = this;

    //Variables
    vm.loading = false;
    vm.onlyPagados = false;
    //////////////////////////////

    //Funciones
    vm.loadHijos = loadHijos;
    vm.editUsuario = editUsuario;
    vm.showPagados = showPagados;
    vm.refreshPagados = refreshPagados;
    //////////////////////////////

    //INIT
    loadUsuarios();
    //////////////////////////////

    function loadUsuarios() {
        vm.loading = true;
        $http.post(webroot + "Manage/LoadUsuarios", {})
            .then(function (response) {
                if (response.data.cod === "OK") {
                    vm.usuariosSafe = response.data.d.usuarios;
                    vm.usuarios = response.data.d.usuarios;
                    vm.usuarios_disp = [].concat(response.data.d.usuarios);
                    vm.usuarioSelected = vm.usuarios[0];
                    vm.loading = false;
                } else {
                    vm.loading = false;
                    swal({ title: 'Oops...', text: response.data.msg, type: 'error' });
                }
            });
    }

    function loadHijos() {
        vm.loading = true;
        $http.post(webroot + "Manage/LoadHijos", {idPadre:vm.usuarioSelected.id})
            .then(function (response) {
                if (response.data.cod === "OK") {
                    vm.usuarioSelected.hijos = response.data.d.hijos;
                    vm.loading = false;
                } else {
                    vm.loading = false;
                    //swal({ title: 'Oops...', text: response.data.msg, type: 'error' });
                }
            });
    }

    function refreshPagados() {
        vm.loading = true;
        $http.post(webroot + "Manage/RefreshPagados", {})
            .then(function (response) {
                if (response.data.cod === "OK") {
                    loadUsuarios();
                    vm.loading = false;
                } else {
                    vm.loading = false;
                    //swal({ title: 'Oops...', text: response.data.msg, type: 'error' });
                }
            });
    }

    function editUsuario(item) {
        $modal.open({
            templateUrl: 'formUsuario.html',
            controller: ModalInstanceCtrl,
            controllerAs: 'vm',
            windowClass: "usuario-modal",
            resolve: {
                item: function () { return item; }
            }
        }).result.then(function (result) {

            loadUsuarios();

        });
    }

    function showPagados() {
        if (vm.onlyPagados) {
            vm.usuarios = $filter("filter")(vm.usuariosSafe, { pagado: 1 });
            vm.usuarios_disp = [].concat(vm.usuarios);
            vm.usuarioSelected = vm.usuarios[0];
        } else {
            vm.usuarios = vm.usuariosSafe;
            vm.usuarios_disp = [].concat(vm.usuarios);
        }
    }

}

ModalInstanceCtrl.$inject = ['$scope', '$modalInstance', '$http', '$window', 'item'];
function ModalInstanceCtrl($scope, $modalInstance, $http, $window,  item) {

    //Variables
    var vm = this;
    vm.edit = false;
    vm.session = $window.sessionStorage;
    vm.tipoSelected = [];

    if (item == null) {
        vm.dni = "";
        vm.nombre = "";
        vm.apellidos = "";
        vm.email = "";
        vm.telefono = "";
        vm.telefonoAlt = "";
        vm.usuario = "";
        vm.password = "";
        vm.autorizacion = 0;
        vm.pagado = 0;

        vm.edit = false;
    } else {
        vm.dni = item.dni;
        vm.nombre = item.nombre;
        vm.apellidos = item.apellidos;
        vm.email = item.email;
        vm.telefono = item.telefono;
        vm.telefonoAlt = item.telefonoalt;
        vm.usuario = item.usuario;
        vm.password = "";
        vm.autorizacion = item.autorizacion;
        vm.pagado = item.pagado;

        vm.edit = true;
    }

    //FUNCIONES
    vm.aceptar = aceptar;
    vm.cancelar = cancelar;
    ///////////////////////////
    //INIT
    loadTiposUsuarios();

    ////////////////////////////

    

    function loadTiposUsuarios() {
        $http.post(webroot + "Manage/LoadTiposUsuarios").then(function (response) {
            vm.tipos = response.data.d.tipos;

            if (vm.edit) {
                angular.forEach(vm.tipos,
                function (e) {
                    if (e.id == item.tipo) {
                        vm.tipoSelected.push(e);
                        e.ticked = true;
                    }
                });
            }

        });
    }

    function aceptar() {

        var tipo = -1;
        
            if (vm.tipoSelected.length != 0) {
                tipo = vm.tipoSelected[0].id;
            }

            if (!vm.edit) {
                $http.post(webroot + "Configuracion/addUsuario",
                {
                    dni: vm.dni,
                    nombre: vm.nombre,
                    apellidos: vm.apellidos,
                    telefono: vm.telefono,
                    telefonoAlt: vm.telefonoAlt,
                    email: vm.email,
                    usuario: vm.usuario,
                    password: vm.password,
                    tipo: tipo,
                    autorizacion:vm.autorizacion
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
                $http.post(webroot + "Manage/saveUsuario",
                {
                    id: item.id,
                    dni: vm.dni,
                    nombre: vm.nombre,
                    apellidos: vm.apellidos,
                    telefono: vm.telefono,
                    telefonoAlt: vm.telefonoAlt,
                    email: vm.email,
                    usuario: vm.usuario,
                    password: vm.password,
                    autorizacion: vm.autorizacion,
                    tipo: tipo,
                    pagado:vm.pagado
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