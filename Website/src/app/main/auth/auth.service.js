(function () {
    'use strict';

    angular
        .module('app.auth')
        .factory('AuthService', AuthService);

    /** @ngInject **/
    function AuthService(SVCS, $http, $q, $cookies, $rootScope) {

        // permissions of current user
        var userPermissions = [];

        var service = {
            IsLoggedIn: false,
            GetUserPermissionsAsync: getUserPermissionsAsync,
            HasPermissions: hasPermissions,
            Login: login,
            Logout: logout,
            SetRedirectAfterLogin: setRedirectAfterLogin,
            ChangePassword: changePassword,
            RequestResetPassword: requestResetPassword
        }

        // first check authentication
        if ($cookies.get('access_token')) {
            service.IsLoggedIn = true;

            getUserPermissionsAsync();
        }

        // private state to tracking redirect uri after login
        var redirectAfterAuth = {
            state: null,
            params: null
        };

        return service;

        // store next redirect to state
        function setRedirectAfterLogin(state, params) {
            redirectAfterAuth.state = state;
            redirectAfterAuth.params = params;
        }

        function login(model) {
            var deferer = $q.defer();

            $http.post(SVCS.Server + '/api/auth/login', model)
                .then(function (response) {
                    var data = response.data;

                    var cookieOptions = {
                        path: '/'
                    };

                    if (model.remember) {
                        cookieOptions.expires = new Date(data.Expires);
                    }

                    $cookies.put('access_token', data.AccessToken, cookieOptions);

                    service.IsLoggedIn = true;

                    $rootScope.$emit('AUTH_SIGNED_IN');

                    var redirectTo = angular.copy(redirectAfterAuth);

                    redirectAfterAuth = {
                        state: null,
                        params: null
                    };

                    getUserPermissionsAsync().then(function () {
                        deferer.resolve(angular.copy(redirectTo));
                    }, function () {
                        deferer.resolve(angular.copy(redirectTo));
                    })

                }, function (responseError) {
                    deferer.reject(responseError.data);
                })

            return deferer.promise;
        }

        function logout() {
            var deferer = $q.defer();

            $http.post(SVCS.Server + '/api/auth/logout')
                .then(function () {

                    userPermissions = [];
                    service.account = null;
                    service.isLoggedIn = false;

                    $cookies.remove('access_token');
                    $rootScope.$emit('AUTH_SIGNED_OUT');

                    deferer.resolve();

                }, function (responseError) {
                    deferer.reject(responseError.data);
                })

            return deferer.promise;
        }

        function hasPermissions(permissions) {
            if (!service.isLoggedIn) {
                return false;
            } else if (permissions) {
                if (permissions.constructor !== Array) {
                    return userPermissions.indexOf(permissions) > -1;
                } else if (permissions.length) {
                    if (!userPermissions.length) {
                        return false;
                    }
                    for (var i = 0; i < permissions.length; i++) {
                        if (userPermissions.indexOf(permissions[i]) != -1) {
                            return true;
                        }
                    }
                    return false;
                } else {
                    return true;
                }
            } else {
                return true;
            }
        }

        function getUserPermissionsAsync() {
            var deferer = $q.defer();

            if (!service.isLoggedIn) {
                deferer.reject();
            } else if (userPermissions && userPermissions.length) {
                deferer.resolve(angular.copy(userPermissions));
            } else {
                $http.get(SVCS.Server + '/api/auth/permissions').then(function (permissions) {

                    userPermissions = permissions.data;

                    $rootScope.$emit('AUTH_PERMISSIONS_LOADED');

                    deferer.resolve(angular.copy(userPermissions));

                }, function () {

                    deferer.reject();

                })
            }

            return deferer.promise;
        }

        function changePassword(model) {
            var deferer = $q.defer();

            $http.put(SVCS.Server + '/api/account/change-password', model).then(function (response) {
                deferer.resolve(response.data);
            })

            return deferer.promise;
        }

        function requestResetPassword(userName) {
            var deferer = $q.defer();

            $http.get(SVCS.Server + '/api/account/request-reset-password?userName=' + userName).then(function (res) {
                deferer.resolve();
            },
                function (responseError) {
                    deferer.reject(responseError.data);
                });

            return deferer.promise;
        }
    }

})();