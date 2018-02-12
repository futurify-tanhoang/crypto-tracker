(function ()
{
    'use strict';

    angular
        .module('app.cryptocurrency')
        .controller('CryptocurrencyController', CryptocurrencyController);

    /** @ngInject */
    function CryptocurrencyController(SVCS, $scope, $compile, $filter, DatatablesLanguage, CryptocurrencyService, DTOptionsBuilder, DTColumnBuilder, $mdDialog)
    {
        var vm = this;

        vm.config = function () {
            // Data
            vm.cryptos = [];

            vm.dtOptions = DTOptionsBuilder
                .newOptions()
                .withDOM('<"top"f>rt<"bottom"<"flex"<"info"i><"pagination"p>>>')
                .withPaginationType('full_numbers')
                .withOption('processing', true) //for show progress bar
                .withOption('serverSide', true)
                .withOption("bLengthChange", false)
                .withOption("bPaginate", true)
                .withOption("searchDelay", 1000)
                .withOption('createdRow', function (row) {
                    // Recompiling so we can bind Angular directive to the DT
                    //using for the button are added
                    $compile(angular.element(row).contents())($scope);
                })
                .withOption('ajax', function (data, callback, settings) {
                    CryptocurrencyService.All().then(function (res) {
                        vm.cryptos = res;
                        callback({ data: vm.cryptos, recordsFiltered: vm.cryptos.length, recordsTotal: vm.cryptos.length });
                    });
                })
                .withDataProp('data')//set field that display data

            vm.dtInstanceCallback = function (inst) {
                vm.dtInstance = inst;
            };

            vm.dtColumns = [
                DTColumnBuilder.newColumn('Id').withTitle('Id').notVisible(),
                DTColumnBuilder.newColumn(null).withTitle('#').withOption('width', '5%').renderWith(function (data, type, full, meta) {
                    return meta.row + 1;
                }),
                DTColumnBuilder.newColumn('Name').withTitle('Name').withOption('width', '75%').notSortable(),
                DTColumnBuilder.newColumn(null).withTitle('Action').withOption('width', '20%').renderWith(function (data, type, full, meta) {
                    return '<md-button ng-click="vm.edit(' + data.Id + ', $event)" class="md-raised">Edit</md-button> <md-button ng-click="vm.delete(' + data.Id + ', $event)" class="md-warn md-raised">Remove</md-button>'
                })
            ];
        }

        // Methods
        vm.config();

        vm.showDialog = function (action, ev, crypto) {
            $mdDialog.show({
                locals: { crypto: crypto, action: action },
                controller: DialogController,
                templateUrl: 'app/main/admin/cryptocurrency/cryptocurrency.detail.tpl.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                fullscreen: $scope.customFullscreen // Only for -xs, -sm breakpoints.
            }).then(function () {
                vm.dtInstance.reloadData();
            }, function () {

            });
        }

        vm.add = function (ev) {
            vm.showDialog('new', ev);
        };

        vm.edit = function (id, ev) {
            var crypto = vm.cryptos.findItem('Id', id);
            if (!crypto) {
                return;
            }
            vm.showDialog('edit', ev, crypto);
        }
        //////////
    }

    function DialogController($scope, $mdDialog, CryptocurrencyService, crypto, action) {
        $scope.title = "Add new";

        if (action == 'edit') { $scope.title = 'Edit' }

        if (crypto)
            $scope.crypto = crypto;
        else
            $scope.crypto = {};

        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.save = function () {
            if (action == 'new') {
                CryptocurrencyService.Create($scope.crypto).then(function (res) {
                    $mdDialog.hide(res);
                })
            }
            else {
                CryptocurrencyService.Update($scope.crypto).then(function (res) {
                    $mdDialog.hide(res);
                })
            }
        };
    }
})();
