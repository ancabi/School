angular.module("school").controller("planificacionCtrl", ["$scope", "$http", "notify", "sweetAlert", mercadoCtrl]);


function planificacionCtrl($scope, $http, notify, sweetAlert) {
    vm = this;

    vm.rows = [];
    vm.loading1 = true;
    vm.loading2 = true;
    vm.loading3 = true;
    vm.loading4 = true;
    vm.loading5 = true;
    vm.customStyle = {};
    vm.boldStyle1 = {};
    vm.boldStyle2 = {};
    vm.boldStyle3 = {};
    vm.boldStyle4 = {};
    vm.boldStyle5 = {};

    vm.anadirProductoListado = anadirProductoListado;
    vm.getComparativa1 = getComparativa1;
    vm.getComparativa2 = getComparativa2;
    vm.getComparativa3 = getComparativa3;
    vm.getComparativa4 = getComparativa4;
    vm.getComparativa5 = getComparativa5;
    vm.getProductosListado = getProductosListado;
    vm.insertProductoListado = insertProductoListado;
    vm.getHistorico = getHistorico;
    vm.getGrafica = getGrafica;
    vm.getAnalizarMercado = getAnalizarMercado;
    vm.deleteLinea = deleteLinea;
    vm.getGreenColor = getGreenColor;

    getProductosListado();

    function getAnalizarMercado() {
        var comparar1 = true;
        var comparar2 = true;
        var comparar3 = true;
        var comparar4 = true;
        var comparar5 = true;
        vm.comparar6 = [];
        var n = 0

        $http.post(webroot + "Comparativa/updateActualizacion").then(function (response) {
            if (response.data.cod === "OK") {
                angular.forEach(vm.rows, function () {

                    vm.rows[n].loading1 = true;
                    vm.rows[n].loading2 = true;
                    vm.rows[n].loading3 = true;
                    vm.rows[n].loading4 = true;
                    vm.rows[n].loading5 = true;

                    vm.rows[n].precio1 = "-";
                    vm.rows[n].precio2 = "-";
                    vm.rows[n].precio3 = "-";
                    vm.rows[n].precio4 = "-";
                    vm.rows[n].precio5 = "-";

                    getComparativa0(vm.rows[n]);
                    getComparativa1(vm.rows[n]);
                    getComparativa2(vm.rows[n]);
                    getComparativa3(vm.rows[n]);
                    getComparativa4(vm.rows[n]);
                    getComparativa5(vm.rows[n]);

                    if ((!vm.rows[n].precio1) || (vm.rows[n].precio0 < vm.rows[n].precio1)) {
                        comparar1 = true;
                    } else {
                        comparar1 = false;
                        vm.comparar6.push(vm.rows[n].precio1);
                    }
                    if ((!vm.rows[n].precio2) || (vm.rows[n].precio0 < vm.rows[n].precio2)) {
                        comparar2 = true;
                    } else {
                        comparar2 = false;
                        vm.comparar6.push(vm.rows[n].precio2);
                    }
                    if ((!vm.rows[n].precio3) || (vm.rows[n].precio0 < vm.rows[n].precio3)) {
                        comparar3 = true;
                    } else {
                        comparar3 = false;
                        vm.comparar6.push(vm.rows[n].precio3);
                    }
                    if ((!vm.rows[n].precio4) || (vm.rows[n].precio0 < vm.rows[n].precio4)) {
                        comparar4 = true;
                    } else {
                        comparar4 = false;
                        vm.comparar6.push(vm.rows[n].precio4);
                    }
                    if ((!vm.rows[n].precio5) || (vm.rows[n].precio0 < vm.rows[n].precio5)) {
                        comparar5 = true;
                    } else {
                        comparar5 = false;
                        vm.comparar6.push(vm.rows[n].precio5);
                    }

                    if ((comparar1 == true) && (comparar2 == true) && (comparar3 == true) && (comparar4 == true) && (comparar5 == true)) {
                        //vm.rows[n].customStyle.style = { "color": "green" };
                    } else {

                        Array.prototype.min = function () {
                            return Math.min.apply(null, this);
                        };

                        if (vm.comparar6.min() == vm.rows[n].precio1) {
                            vm.rows[n].boldStyle1.style = { "font-weight": "bold" }
                        }
                        if (vm.comparar6.min() == vm.rows[n].precio2) {
                            vm.rows[n].boldStyle2.style = { "font-weight": "bold" }
                        }
                        if (vm.comparar6.min() == vm.rows[n].precio3) {
                            vm.rows[n].boldStyle3.style = { "font-weight": "bold" }
                        }
                        if (vm.comparar6.min() == vm.rows[n].precio4) {
                            vm.rows[n].boldStyle4.style = { "font-weight": "bold" }
                        }
                        if (vm.comparar6.min() == vm.rows[n].precio5) {
                            vm.rows[n].boldStyle5.style = { "font-weight": "bold" }
                        }

                        //vm.rows[n].customStyle.style = { "color": "red" };
                    }

                    n++;
                });
                notify({ message: 'Comenzando estudio de mercado', classes: 'alert-success' });
            } else {
                notify({ message: 'No se ha podido añadir al listado.', classes: 'alert-danger' });
            }
        });

    }
    function getGrafica(ean) {
        //console.log('Entramos');
        vm.grafica = [];
        vm.labelsDatos = ["01-01-2016"];
        vm.datasetsschool = [0];
        vm.datasetsCarrefour = [0];
        vm.datasetsElcorteingles = [0];
        vm.datasetsFnac = [0];
        vm.datasetsWorten = [0];
        vm.datasetsMediamarkt = [0];
        var n = 0;

        $http.post(webroot + "Comparativa/getGrafica", { ean: ean })
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.mostrarHistorico = true;
                  vm.grafica = response.data.d.grafica;
                  //console.log(vm.grafica);
                  angular.forEach(vm.grafica, function () {
                      vm.labelsDatos.push(moment(vm.grafica[n].fechatot).format("DD-MM-YYYY"));
                      vm.datasetsschool.push(vm.grafica[n].pvpschool);
                      vm.datasetsCarrefour.push(vm.grafica[n].pvpcarrefour);
                      vm.datasetsElcorteingles.push(vm.grafica[n].pvpel_corte_ingles);
                      vm.datasetsFnac.push(vm.grafica[n].pvpfnac);
                      vm.datasetsWorten.push(vm.grafica[n].pvpworten);
                      vm.datasetsMediamarkt.push(vm.grafica[n].pvpmediamarkt);
                      n++;
                  });
                  vm.lineData = {
                      labels: vm.labelsDatos,
                      datasets: [
                          {
                              label: "school",
                              fillColor: "rgba(255,240,0,0)",
                              strokeColor: "rgba(255,240,0,1)",
                              pointColor: "rgba(255,240,0,1)",
                              pointStrokeColor: "#fff",
                              pointHighlightFill: "#fff",
                              pointHighlightStroke: "rgba(255,240,0,1)",
                              data: vm.datasetsschool
                          },
                           {
                               label: "Carrefour",
                               fillColor: "rgba(0,78,142,0)",
                               strokeColor: "rgba(0,78,142,1)",
                               pointColor: "rgba(0,78,142,1)",
                               pointStrokeColor: "#fff",
                               pointHighlightFill: "#fff",
                               pointHighlightStroke: "rgba(0,78,142,1)",
                               data: vm.datasetsCarrefour
                           },
                          {
                              label: "El Corte Inglés",
                              fillColor: "rgba(2,135,42,0)",
                              strokeColor: "rgba(2,135,42,1)",
                              pointColor: "rgba(2,135,42,1)",
                              pointStrokeColor: "#fff",
                              pointHighlightFill: "#fff",
                              pointHighlightStroke: "rgba(2,135,42,1)",
                              data: vm.datasetsElcorteingles
                          },
                          {
                              label: "Fnac",
                              fillColor: "rgba(231,173,15,0)",
                              strokeColor: "rgba(231,173,15,1)",
                              pointColor: "rgba(231,173,15,1)",
                              pointStrokeColor: "#fff",
                              pointHighlightFill: "#fff",
                              pointHighlightStroke: "rgba(231,173,15,1)",
                              data: vm.datasetsFnac
                          },
                          {
                              label: "Worten",
                              fillColor: "rgba(255, 106, 0,0)",
                              strokeColor: "rgba(255, 106, 0,1)",
                              pointColor: "rgba(255, 106, 0,1)",
                              pointStrokeColor: "#fff",
                              pointHighlightFill: "#fff",
                              pointHighlightStroke: "rgba(255, 106, 0,1)",
                              data: vm.datasetsWorten
                          },
                         {
                             label: "Mediamarkt",
                             fillColor: "rgba(236,27,35,0)",
                             strokeColor: "rgba(236,27,35,1)",
                             pointColor: "rgba(236,27,35,1)",
                             pointStrokeColor: "#fff",
                             pointHighlightFill: "#fff",
                             pointHighlightStroke: "rgba(236,27,35,1)",
                             data: vm.datasetsMediamarkt
                         }
                      ]
                  };
                  vm.lineOptions = {
                      scaleShowGridLines: true,
                      scaleGridLineColor: "rgba(0,0,0,.05)",
                      scaleGridLineWidth: 1,
                      bezierCurve: true,
                      bezierCurveTension: 0.4,
                      pointDot: true,
                      pointDotRadius: 4,
                      pointDotStrokeWidth: 1,
                      pointHitDetectionRadius: 20,
                      datasetStroke: true,
                      datasetStrokeWidth: 1,
                      datasetFill: true,
                      legend: { show: true },
                      legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].strokeColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>"
                  };
              } else {
                  notify({ message: 'No se ha podido mostrar el historico.', classes: 'alert-danger' });
              }
          });
    }
    function getHistorico(ean, nombre) {
        getGrafica(ean)
        vm.historico = [];
        vm.nombreProducto = "";
        var n = 0;

        $http.post(webroot + "Comparativa/getHistorico", { ean: ean })
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.mostrarHistorico = true;
                  vm.historico = response.data.d.historico;
                  vm.nombreProducto = nombre;
              } else {
                  notify({ message: 'No se ha podido mostrar el historico.', classes: 'alert-danger' });
              }
          });
    }
    function getProductosListado() {
        vm.productos = [];
        var n = 0;

        $http.post(webroot + "Comparativa/getProductosListado")
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.productos = response.data.d.listado;
                  console.log(vm.productos);
                  angular.forEach(vm.productos, function (obj) {
                      anadirProductoListado(obj.codigo, obj, "no");
                  });
                  //notify({ message: 'Añadido al listado correctamente', classes: 'alert-success' });
              } else {
                  notify({ message: 'No se ha podido añadir al listado.', classes: 'alert-danger' });
              }
          });
    }
    function anadirProductoListado(codArticulo, obj, guardar) {
        console.log(codigoEan);
        if (!codArticulo) {
            sweetAlert.swal("Oops...", "Debe introducir un codigo de Artículo!", "error");;
        } else {
            $http.post(webroot + "Comparativa/getEanFromCodArticulo", { cod_articulo: codArticulo })
            .then(function (response) {
                if (response.data.cod == "OK") {
                    var codigoEan = response.data.d.ean;

                    var comparar1 = true;
                    var comparar2 = true;
                    var comparar3 = true;
                    var comparar4 = true;
                    var comparar5 = true;
                    vm.comparar6 = [];

                    vm.rows.push({codigo: codArticulo, ean: codigoEan, nombre: "-", precio0: "-", precio1: "-", precio2: "-", precio3: "-", precio4: "-", precio5: "-", loading1: true, loading2: true, loading3: true, loading4: true, loading5: true, customStyle: { style: { "color": "green" } }, boldStyle1: { style: { "font-weight": "normal" } }, boldStyle2: { style: { "font-weight": "normal" } }, boldStyle3: { style: { "font-weight": "normal" } }, boldStyle4: { style: { "font-weight": "normal" } }, boldStyle5: { style: { "font-weight": "normal" } } });
                    var nR = vm.rows.length - 1;
                    if (guardar != "no") {
                        insertProductoListado(codigoEan, vm.rows[nR]);
                        getComparativa0(vm.rows[nR]);
                        getComparativa1(vm.rows[nR]);
                        getComparativa2(vm.rows[nR]);
                        getComparativa3(vm.rows[nR]);
                        getComparativa4(vm.rows[nR]);
                        getComparativa5(vm.rows[nR]);
                    } else {
                        vm.rows[nR].nombre = obj.nombre
                        vm.rows[nR].precio0 = obj.p_school
                        vm.rows[nR].precio1 = obj.redcoon
                        vm.rows[nR].loading1 = false;
                        vm.rows[nR].precio2 = obj.el_corte_ingles
                        vm.rows[nR].loading2 = false;
                        vm.rows[nR].precio3 = obj.fnac
                        vm.rows[nR].loading3 = false;
                        vm.rows[nR].precio4 = obj.worten
                        vm.rows[nR].loading4 = false;
                        vm.rows[nR].precio5 = obj.mediamarkt
                        vm.rows[nR].loading5 = false;
                    }

                    if ((!vm.rows[nR].precio1) || (vm.rows[nR].precio0 < vm.rows[nR].precio1)) {
                        comparar1 = true;
                    } else {
                        comparar1 = false;
                        vm.comparar6.push(vm.rows[nR].precio1);
                    }
                    if ((!vm.rows[nR].precio2) || (vm.rows[nR].precio0 < vm.rows[nR].precio2)) {
                        comparar2 = true;
                    } else {
                        comparar2 = false;
                        vm.comparar6.push(vm.rows[nR].precio2);
                    }
                    if ((!vm.rows[nR].precio3) || (vm.rows[nR].precio0 < vm.rows[nR].precio3)) {
                        comparar3 = true;
                    } else {
                        comparar3 = false;
                        vm.comparar6.push(vm.rows[nR].precio3);
                    }
                    if ((!vm.rows[nR].precio4) || (vm.rows[nR].precio0 < vm.rows[nR].precio4)) {
                        comparar4 = true;
                    } else {
                        comparar4 = false;
                        vm.comparar6.push(vm.rows[nR].precio4);
                    }
                    if ((!vm.rows[nR].precio5) || (vm.rows[nR].precio0 < vm.rows[nR].precio5)) {
                        comparar5 = true;
                    } else {
                        comparar5 = false;
                        vm.comparar6.push(vm.rows[nR].precio5);
                    }

                    if ((comparar1 == true) && (comparar2 == true) && (comparar3 == true) && (comparar4 == true) && (comparar5 == true)) {
                        vm.rows[nR].customStyle.style = { "color": "green" };
                    } else {

                        Array.prototype.min = function () {
                            return Math.min.apply(null, this);
                        };

                        if (vm.comparar6.min() == vm.rows[nR].precio1) {
                            vm.rows[nR].boldStyle1.style = { "font-weight": "bold" }
                        }
                        if (vm.comparar6.min() == vm.rows[nR].precio2) {
                            vm.rows[nR].boldStyle2.style = { "font-weight": "bold" }
                        }
                        if (vm.comparar6.min() == vm.rows[nR].precio3) {
                            vm.rows[nR].boldStyle3.style = { "font-weight": "bold" }
                        }
                        if (vm.comparar6.min() == vm.rows[nR].precio4) {
                            vm.rows[nR].boldStyle4.style = { "font-weight": "bold" }
                        }
                        if (vm.comparar6.min() == vm.rows[nR].precio5) {
                            vm.rows[nR].boldStyle5.style = { "font-weight": "bold" }
                        }

                        vm.rows[nR].customStyle.style = { "color": "red" };
                    }
                } else {
                    notify({ message: response.data.msg, classes: 'alert-danger' });
                }
            });
        }
    }
    function deleteLinea(codigoEan, cell) {
        $http.post(webroot + "Comparativa/deleteLinea", { ean: codigoEan })
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.rows.splice(vm.rows.indexOf(cell), 1);
                  notify({ message: 'Eliminado de la lista correctamente', classes: 'alert-success' });
              } else {
                  notify({ message: 'No se ha podido borrar del listado.', classes: 'alert-danger' });
              }
          });

    }
    function insertProductoListado(codigoEan, rows) {
        $http.post(webroot + "Comparativa/insertProductoListado", { ean: codigoEan })
          .then(function (response) {
              if (response.data.cod === "OK") {
                  vm.productos = response.data.d.articulo;
                  rows.nombre = vm.productos[0].nombre;
                  rows.precio0 = vm.productos[0].tarifa_pvp;
                  notify({ message: 'Añadido al listado correctamente', classes: 'alert-success' });
              } else {
                  notify({ message: response.data.msg, classes: 'alert-danger' });
              }
          });

    }
    function getComparativa0(rows) {
        var ean = rows.ean
        if (rows.ean) {
            $http.post(webroot + "Comparativa/getComparativa0", { ean: ean })
                .then(function (response) {
                    if (response.data.cod === "OK") {
                        rows.precio0 = response.data.d.pvpComparativa0;
                        //vm.precios.pvpComparativa1 = response.data.d.pvpComparativa1;
                    }
                    //rows.loading0 = false;
                });
        } else {
            //rows.loading1 = false;
        }
    }
    function getComparativa1(rows) {
        var ean = rows.ean
        if (rows.ean) {
            $http.post(webroot + "Comparativa/getComparativa1", { ean: ean })
                .then(function (response) {
                    if (response.data.cod === "OK") {
                        rows.precio1 = response.data.d.pvpComparativa1;
                        //vm.precios.pvpComparativa1 = response.data.d.pvpComparativa1;
                    }
                    rows.loading1 = false;
                });
        } else {
            rows.loading1 = false;
        }
    }
    function getComparativa2(rows) {
        var ean = rows.ean
        if (ean) {
            $http.post(webroot + "Comparativa/getComparativa2", { ean: ean })
                .then(function (response) {
                    if (response.data.cod === "OK") {
                        rows.precio2 = response.data.d.pvpComparativa2;
                        //vm.precios.pvpComparativa2 = response.data.d.pvpComparativa2;
                        getGreenColor(rows);
                    }
                    rows.loading2 = false;
                });
        } else {
            rows.loading2 = false;
        }
    }
    function getComparativa3(rows) {
        var ean = rows.ean
        if (ean) {
            $http.post(webroot + "Comparativa/getComparativa3", { ean: ean })
                .then(function (response) {
                    if (response.data.cod === "OK") {
                        rows.precio3 = response.data.d.pvpComparativa3;
                        //vm.precios.pvpComparativa3 = response.data.d.pvpComparativa3;
                        getGreenColor(rows);
                    }
                    rows.loading3 = false;
                });
        } else {
            rows.loading3 = false;
        }
    }
    function getComparativa4(rows) {
        var ean = rows.ean
        if (ean) {
            $http.post(webroot + "Comparativa/getComparativa4", { ean: ean })
                .then(function (response) {
                    if (response.data.cod === "OK") {
                        rows.precio4 = response.data.d.pvpComparativa4;
                        //vm.precios.pvpComparativa4 = response.data.d.pvpComparativa4;
                        getGreenColor(rows);
                    }
                    rows.loading4 = false;
                });
        } else {
            rows.loading4 = false;
        }
    }
    function getComparativa5(rows) {
        var ean = rows.ean
        if (ean) {
            $http.post(webroot + "Comparativa/getComparativa5", { ean: ean })
                .then(function (response) {
                    if (response.data.cod === "OK") {
                        rows.precio5 = response.data.d.pvpComparativa5;
                        //vm.precios.pvpComparativa5 = response.data.d.pvpComparativa5;
                        getGreenColor(rows);
                    }
                    rows.loading5 = false;
                });
        } else {
            rows.loading5 = false;
        }
    }
    function closeComparativaPVP() {
        $modalInstance.dismiss('cancel');
    }
    function getGreenColor(rows) {
        rows.customStyle.style = { "color": "gray" };
        var comparar1 = true;
        var comparar2 = true;
        var comparar3 = true;
        var comparar4 = true;
        var comparar5 = true;

        if ((!rows.precio1) || (rows.precio0 < rows.precio1) || (rows.precio1 == "-")) {
            comparar1 = true;
        } else {
            comparar1 = false;
        }
        if ((!rows.precio2) || (rows.precio0 < rows.precio2) || (rows.precio2 == "-")) {
            comparar2 = true;
        } else {
            comparar2 = false;
        }
        if ((!rows.precio3) || (rows.precio0 < rows.precio3) || (rows.precio3 == "-")) {
            comparar3 = true;
        } else {
            comparar3 = false;
        }
        if ((!rows.precio4) || (rows.precio0 < rows.precio4) || (rows.precio4 == "-")) {
            comparar4 = true;
        } else {
            comparar4 = false;
        }
        if ((!rows.precio5) || (rows.precio0 < rows.precio5) || (rows.precio5 == "-")) {
            comparar5 = true;
        } else {
            comparar5 = false;
        }
        console.log(rows);
        console.log(rows.precio1 + " - " + rows.precio2 + " - " + rows.precio3 + " - " + rows.precio4 + " - " + rows.precio5);
        console.log(comparar1 + " - " + comparar2 + " - " + comparar3 + " - " + comparar4 + " - " + comparar5);
        if ((comparar1 == true) && (comparar2 == true) && (comparar3 == true) && (comparar4 == true) && (comparar5 == true)) {
            rows.customStyle.style = { "color": "green" };
        } else {
            rows.customStyle.style = { "color": "red" };
        }
    }

}

