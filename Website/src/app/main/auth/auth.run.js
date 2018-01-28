(function () {
    'use strict';

    angular
        .module('app.auth')
        .run(['AuthService', '$rootScope', '$state', '$stateParams', function (_authService, $rootScope, $state, $stateParams) {

            var permissionsLoaded = false;

            $rootScope.$on('PERMISSIONS_LOADED', function () {
                permissionsLoaded = true;
            })

            $rootScope.$on("$stateChangeStart", function (event, toState, toParams, fromState, fromParams) {
                if (toState.protect === true) {
                    if (!_authService.IsLoggedIn) {

                        event.preventDefault();

                        _authService.SetRedirectAfterLogin(toState.name, angular.copy(toParams));

                        $state.go('app.auth_login');

                    } else if (toState.permissions && toState.permissions.length) {
                        if (!_authService.HasPermissions(toState.permissions)) {
                            if (permissionsLoaded) {

                                event.preventDefault();

                                _authService.Logout();

                                _authService.SetRedirectAfterLogin(toState.name, angular.copy(toParams));
                                $state.go('app.auth_login');

                            } else {

                                event.preventDefault();

                                var evtCanceler = $rootScope.$on('PERMISSIONS_LOADED', function () {

                                    evtCanceler();

                                    if (!_authService.HasPermissions(toState.permissions)) {
                                        _authService.Logout();
                                        _authService.SetRedirectAfterLogin(toState.name, angular.copy(toParams));
                                        $state.go('app.auth_login');
                                    } else {
                                        var params = angular.copy(toParams);
                                        $state.go(toState.name, params);
                                    }

                                });

                            }
                        }
                    }
                }
            });

            $rootScope.$on('AUTH_TOKEN_EXPIRED', function () {

                alert('Your logged in session has been expired, please sign in again!');

                _authService.Logout();

                _authService.SetRedirectAfterLogin($state.current.name, angular.copy($stateParams));
                $state.go('app.auth_login');

            })

        }]);
})();