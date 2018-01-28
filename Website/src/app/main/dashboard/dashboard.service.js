(function () {
    'use strict';

    angular
        .module('app.dashboard')
        .factory('DashboardService', DashboardService);

    /** @ngInject */
    function DashboardService(SVCS, $http, $q) {
        var server = SVCS.Server;

        var service = {
        };
        return service;
    }
})();