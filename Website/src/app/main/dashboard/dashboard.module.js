(function ()
{
    'use strict';

    angular
        .module('app.dashboard', [])
        .config(config);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider)
    {
        // State
        $stateProvider
            .state('app.dashboard', {
                url: '/',
                views  : {
                    'content@app': {
                        templateUrl: 'app/main/dashboard/dashboard.html',
                        controller: 'DashboardController as vm'
                    }
                },
                protect: true
            });

        // Translation
        $translatePartialLoaderProvider.addPart('app/main/dashboard');
    }
})();