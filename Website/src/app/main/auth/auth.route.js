(function () {
    'use strict';

    angular
        .module('app.auth')
        .config(config);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider) {
        // State
        $stateProvider
            .state('app.auth_login', {
                url: '/login',
                views: {
                    'main@': {
                        templateUrl: 'app/core/layouts/content-only.html',
                        controller: 'MainController as vm'
                    },
                    'content@app.auth_login': {
                        templateUrl: 'app/main/auth/login/login.html',
                        controller: 'LoginController as vm'
                    }
                },
                bodyClass: 'login'
            })
            .state('app.auth_forgot-password', {
                url: '/forgot-password',
                views: {
                    'main@': {
                        templateUrl: 'app/core/layouts/content-only.html',
                        controller: 'MainController as vm'
                    },
                    'content@app.auth_forgot-password': {
                        templateUrl: 'app/main/auth/forgot-password/forgot.password.html',
                        controller: 'ForgotPasswordController as vm'
                    }
                },
                bodyClass: 'forgot-password'
            });

        // Translation
        $translatePartialLoaderProvider.addPart('app/main/auth');

    }

})();
