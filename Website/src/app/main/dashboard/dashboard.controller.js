(function ()
{
    'use strict';

    angular
        .module('app.dashboard')
        .controller('DashboardController', DashboardController);

    /** @ngInject */
    function DashboardController(SVCS, DashboardService, $scope, $translate) {
        var vm = this;

        vm.deposit = function (type) {

        }

        vm.withdraw = function (type) {

        }
    }
})();
