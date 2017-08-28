angular.module('school').controller('usuariosCtrl', ['$scope', '$http', '$window', '$modal', usuariosCtrl]);

function usuariosCtrl($scope, $http, $window, $modal) {
    var vm = this;

    //Variables
    vm.loading = false;
    //////////////////////////////

    //Funciones
    vm.loadHijos = loadHijos;
    vm.editUsuario = editUsuario;
    //////////////////////////////

    //INIT
    loadUsuarios();
    //////////////////////////////

    function loadUsuarios() {
        vm.loading = true;
        $http.post(webroot + "Manage/LoadUsuarios", {})
            .then(function (response) {
                if (response.data.cod === "OK") {
                    vm.usuarios = response.data.d.usuarios;
                    vm.usuarios_disp = [].concat(response.data.d.usuarios);
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
                    swal({ title: 'Oops...', text: response.data.msg, type: 'error' });
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

}

ModalInstanceCtrl.$inject = ['$scope', '$modalInstance', '$http', '$window', 'item'];
function ModalInstanceCtrl($scope, $modalInstance, $http, $window,  item) {

    //Variables
    var vm = this;
    vm.edit = false;
    vm.session = $window.sessionStorage;
    

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

        vm.edit = true;
    }

    //FUNCIONES
    vm.aceptar = aceptar;
    vm.cancelar = cancelar;
    ///////////////////////////
    //INIT
    //loadPerfiles();

    ////////////////////////////

    

    function loadPerfiles() {
        $http.post(webroot + "Configuracion/loadPerfiles").then(function (response) {
            vm.perfiles = response.data;

            if (vm.edit) {
                angular.forEach(vm.perfiles,
                function (e) {
                    if (e.id == item.idperfil) {
                        vm.perfilSelected.push(e);
                        e.ticked = true;
                    }
                });
            }

        });
    }

    function aceptar() {

        var tipo = -1;
        
            //if (vm.perfilSelected.length != 0) {
            //    tipo = vm.perfilSelected[0].id;
            //}

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
                    tipo: tipo,
                    autorizacion:vm.autorizacion
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