(function () {
    'use strict';

    angular
        .module('app.cryptocurrency')
        .factory('CryptocurrencyService', CryptocurrencyService);

    /** @ngInject */
    function CryptocurrencyService(SVCS, $http, $q) {
        var server = SVCS.Server;

        var service = {
            All: All,
            Get: Get,
            Create: Create,
            Update: Update,
        };

        return service;

        function Get(id) {
            var deferer = $q.defer();

            $http.get(server + '/api/cryptocurrencies/' + id).then(function (response) {
                deferer.resolve(response.data);
            })

            return deferer.promise;
        }

        function All() {
            var deferer = $q.defer();

            $http.get(server + '/api/cryptocurrencies/all').then(function (response) {
                deferer.resolve(response.data);
            })

            return deferer.promise;
        }

        function Create(crypto) {
            var deferer = $q.defer();

            $http.post(server + '/api/cryptocurrencies', crypto).then(function (response) {
                deferer.resolve(response.data);
            })

            return deferer.promise;
        }

        function Update(crypto) {
            var deferer = $q.defer();

            $http.put(server + '/api/cryptocurrencies', crypto).then(function (response) {
                deferer.resolve(response.data);
            })

            return deferer.promise;
        }
    }
})();