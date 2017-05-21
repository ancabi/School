var app = angular.module('school').controller('registerCtrl', ['$scope', '$http', '$window', '$modal', registerCtrl]);

function registerCtrl($scope, $http, $window, $modal) {
    var vm = this;

    //Variables
    vm.dni = "";
    vm.nombre = "";
    vm.apellidos = "";
    vm.email = "";
    vm.user = "";
    vm.pass = "";
    vm.confirmPass = "";
    vm.talla = "";
    vm.numero = "";
    vm.categoria = "";
    vm.pack = false;
    vm.autorizacion = false;
    vm.paso = 1;
    vm.loading = false;
    //////////////////////////////

    //Funciones
    vm.registrar = registrar;
    vm.siguientePaso = siguientePaso;
    vm.anteriorPaso = anteriorPaso;
    vm.openAutorizacion = openAutorizacion;
    //////////////////////////////

    //INIT
    $window.sessionStorage.clear();
    //////////////////////////////

    function registrar() {
        if (vm.user == undefined) {
            vm.userExist = true;
        } else {

            vm.userExist = false;

            if (vm.pass == vm.confirmPass) {
                vm.loading = true;
                $http.post(webroot + "Account/Registrar",
                    {
                        dni: vm.dni,
                        nombre: vm.nombre,
                        apellidos: vm.apellidos,
                        email: vm.email,
                        user: vm.user,
                        pass: vm.pass,
                        talla: vm.talla,
                        numero: vm.numero,
                        categoria: vm.categoria,
                        pack: vm.pack,
                        autorizacion: vm.autorizacion
                    })
                    .then(function(response) {
                        if (response.data.cod === "OK") {
                            swal({
                                title: 'Regitrado!',
                                text:
                                    "Para finalizar debe hacer un ingreso o transferencia a ES38 2103 0147 3100 3002 2620 y en el concepto debe indicar el DNI/NIE de su hijo/a",
                                type: 'success'
                            });
                            vm.dni = "";
                            vm.nombre = "";
                            vm.apellidos = "";
                            vm.email = "";
                            vm.user = "";
                            vm.pass = "";
                            vm.confirmPass = "";
                            vm.talla = "";
                            vm.numero = "";
                            vm.categoria = "";
                            vm.pack = false;
                            vm.autorizacion = false;
                            vm.paso = 1;
                        } else {
                            vm.loading = false;
                            swal({ title: 'Oops...', text: response.data.msg, type: 'error' });
                        }
                    });
            } else {
                swal({ title: 'Oops...', text: "Las contraseñas no coinciden", type: 'error' });
            }
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

app.factory('isUserExist', ['$q', '$http', function ($q, $http) {
    return function (usuario, touched) {
        var deferred = $q.defer();

        //if (touched) {
            $http.post(webroot + "Account/ExisteUsuario", { Usuario: usuario })
                .then(function (response) {

                    if (response.data.cod == "OK") {
                        deferred.reject();
                    } else {
                        deferred.resolve();
                    }

                });
        //} else {
        //    deferred.resolve();
        //}
        return deferred.promise;
    }
}]);

app.directive("userunique", ['isUserExist', function (isUserExist) {
    return {
        restrict: "A",
        
        require: "ngModel",

        link: function (scope, element, attributes, ngModel) {
            ngModel.$asyncValidators.userunique = function (usuario) {
                return isUserExist(usuario, scope.registro.usuario.$touched);
            }
        }
    };
}]);
