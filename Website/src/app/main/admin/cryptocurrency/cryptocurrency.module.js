(function ()
{
    'use strict';

    angular
        .module('app.cryptocurrency', ['datatables'])
        .config(config);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider)
    {
        // State
        $stateProvider
            .state('app.cryptocurrency', {
                url: '/cryptocurrency',
                abstract: true,
                protect: true
            })
            .state('app.cryptocurrency.list', {
                url: '/',
                views: {
                    'content@app': {
                        templateUrl: 'app/main/admin/cryptocurrency/cryptocurrency.html',
                        controller: 'CryptocurrencyController as vm'
                    }
                },
                resolve: {
                    DatatablesLanguage: function (msApi) {
                        return msApi.resolve('datatables.language@get');
                    }
                },
                protect: true
            });

        // Translation
        $translatePartialLoaderProvider.addPart('app/main/admin/cryptocurrency');

        // Navigation
        msNavigationServiceProvider.saveItem('admin', {
            title : 'Admin',
            group : true,
            weight: 2
        });

        msNavigationServiceProvider.saveItem('admin.currency', {
            title    : 'Currency',
            icon     : 'icon-format-list-bulleted',
            state    : 'app.cryptocurrency.list',
            weight   : 1
        });

        // Api
        msApiProvider.register('datatables.language', ['app/data/datatables/language.json']);
    }
})();