angular.module('school').controller('registerCtrl', ['$scope', '$http', '$window', '$modal', registerCtrl]);

function registerCtrl($scope, $http, $window, $modal) {
    var vm = this;

    //Variables
    vm.usuario = "";
    vm.pass = "";
    vm.paso = 1;
    vm.loading = false;
    //////////////////////////////

    //Funciones
    vm.login = login;
    vm.siguientePaso = siguientePaso;
    vm.anteriorPaso = anteriorPaso;
    vm.openAutorizacion = openAutorizacion;
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
                        $window.sessionStorage.cif = response.data.d.cif;
                        $window.sessionStorage.ip_cliente = response.data.d.ip_cliente;
                        $window.sessionStorage.nombre = response.data.d.nombre;
                        $window.location.href = response.data.d.url;
                    } else {
                        vm.loading = false;
                        swal({ title: 'Oops...', text: response.data.msg, type: 'error' });
                    }                    
                });
        }
    }

    function siguientePaso() {
        vm.paso++;
    }

    function anteriorPaso() {
        vm.paso--;
    }

    function openAutorizacion() {

        var modal = $modal.open({
            templateUrl: 'modalAutorizacion.html',
            size: "lg",
            controller: modalCtrl,
            controllerAs: 'vm',
            backdrop: 'static',
            windowClass: 'hmodal-success',
            resolve: {
                item: function() { return null; }
            }
        });

    }
}

modalCtrl.$inject = ['$scope', '$modalInstance', '$http', 'toastr', 'item'];

function modalCtrl($scope, $modalInstance, $http, toastr, item) {
    var vm = this;

    vm.closeModal = closeModal;


    function closeModal(res) {
        $modalInstance.close(res);
    }

    
}