angular.module('school').controller('loginCtrl', ['$scope', '$http', '$window', loginCtrl]);

function loginCtrl($scope, $http, $window) {
    var vm = this;

    //Variables
    vm.usuario = "";
    vm.pass = "";
    vm.loading = false;
    //////////////////////////////

    //Funciones
    vm.login = login;
    //////////////////////////////

    //INIT
    $window.sessionStorage.clear();
    //////////////////////////////

    function login() {
        if (vm.usuario && vm.pass) {
            vm.loading = true;
            $http.post(webroot + "Account/LoginUser", { usuario: vm.usuario, pass: vm.pass })
                .then(function (response) {
                    if (response.data.cod === "OK") {
                        $window.sessionStorage.webroot = webroot;
                        $window.sessionStorage.usuario = response.data.d.usuario;
                        $window.sessionStorage.tipo = response.data.d.tipo;
                        $window.sessionStorage.nomtipo = response.data.d.nomtipo;
                        $window.sessionStorage.dni = response.data.d.dni;
                        $window.sessionStorage.ip_cliente = response.data.d.ip_cliente;
                        $window.sessionStorage.nombre = response.data.d.nombre;
                        $window.sessionStorage.apellidos = response.data.d.apellidos;
                        $window.location.href = response.data.d.url;
                    } else {
                        vm.loading = false;
                        swal({ title: 'Oops...', text: "Usuario o contraseña no validas", type: 'error' });
                    }                    
                });
        }
    }
}