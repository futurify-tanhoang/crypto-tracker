(function ()
{
    'use strict';

    angular
        .module('app.dashboard', [])
        .config(config);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider)
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

        // Navigation
        //msNavigationServiceProvider.saveItem('dashboard', {
        //    title: 'Dashboard',
        //    group: true,
        //    weight: 1
        //});

        msNavigationServiceProvider.saveItem('dashboard', {
            title: 'Dashboard',
            icon: 'icon-view-dashboard',
            state: 'app.dashboard',
            weight: 1
        });

        // Translation
        $translatePartialLoaderProvider.addPart('app/main/dashboard');
    }
})();