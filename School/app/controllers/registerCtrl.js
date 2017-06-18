var app = angular.module('school').controller('registerCtrl', ['$scope', '$http', '$window', '$modal', registerCtrl]);

function registerCtrl($scope, $http, $window, $modal) {
    var vm = this;

    //Variables
    vm.dni = "";
    vm.nombre = "";
    vm.apellidos = "";
    vm.email = "";
    vm.telefono = "";
    vm.telefonoAlt = "";
    vm.user = "";
    vm.pass = "";
    vm.confirmPass = "";
    vm.pack = false;
    vm.autorizacion = false;
    vm.paso = 1;
    vm.loading = false;
    vm.hijos =[];
    vm.numHijos = "";
    vm.deportes = [{id:1,nombre:"Fútbol"}, {id:2,nombre:"Baloncesto"}];
    vm.deporteSelected = [];
    //////////////////////////////
    
    //Funciones
    vm.registrar = registrar;
    vm.siguientePaso = siguientePaso;
    vm.anteriorPaso = anteriorPaso;
    vm.openAutorizacion = openAutorizacion;
    vm.getNumHijos = getNumHijos;
    vm.inicializarHijos = inicializarHijos;
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
                angular.forEach(vm.hijos,
                    function (d) {
                        d.sports = [];
                        angular.forEach(d.deporteSelected,
                            function (s) {
                                d.sports.push(s.id);
                            });
                    });
                $http.post(webroot + "Account/Registrar",
                    {
                        dni: vm.dni,
                        nombre: vm.nombre,
                        apellidos: vm.apellidos,
                        email: vm.email,
                        telefono: vm.telefono,
                        telefonoAlt: vm.telefonoAlt,
                        user: vm.user,
                        pass: vm.pass,
                        autorizacion: vm.autorizacion,
                        hijos: vm.hijos
                    })
                    .then(function(response) {
                        if (response.data.cod === "OK") {
                            swal({
                                title: 'Registrado!',
                                text:
                                    "Al pulsar ACEPTAR accederá a la Tienda del Club Deportivo Atabal, donde deberá indicar su E-MAIL y CONTRASEÑA para acceder. Una vez dentro, haga clic en el carrito de la compra (arriba a la derecha) y realice el pago para terminar el proceso de inscripción.",
                                type: 'success',
                                showCancelButton: false,
                                confirmButtonColor: "#1BB394",
                                confirmButtonText: "Aceptar",
                                closeOnConfirm: true
                            },
                            function () {
                                window.location.href = "http://elatabaltienda.dged.es/es/iniciar-sesion";
                            });
                            vm.dni = "";
                            vm.nombre = "";
                            vm.apellidos = "";
                            vm.email = "";
                            vm.telefono = "";
                            vm.telefonoAlt = "";
                            vm.user = "";
                            vm.pass = "";
                            vm.confirmPass = "";
                            vm.autorizacion = 0;
                            vm.paso = 1;
                            vm.numHijos = 0;
                            vm.hijos = [];
                            vm.numHijos = "";
                            vm.loading = false;
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

    function getNumHijos() {
        return new Array(parseInt(vm.numHijos));
    }

    function inicializarHijos() {
        for (var x = 0; x < vm.numHijos; x++) {
            vm.hijos[x] = {};
            vm.hijos[x].deportes = JSON.parse(JSON.stringify(vm.deportes));
            vm.hijos[x].deporteSelected = [];
        }
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
