(function () {
    'use strict';

    angular
        .module('app.auth')
        .factory('HttpBearerTokenAuthorizationInterceptor', ['$cookies', '$rootScope', '$q', function ($cookies, $rootScope, $q) {
            return {
                'request': function (config) {
                    var accessToken = $cookies.get('access_token');

                    if (config.params && config.params.isExternalRequest) {
                        delete (config.params.isExternalRequest)
                    } else if (accessToken) {
                        config.headers.Authorization = 'bearer ' + accessToken;
                    }

                    return config;
                },
                'responseError': function (responseError) {
                    if (responseError.status === 401) {
                        if ($cookies.get('access_token')) {
                            $rootScope.$emit('AUTH_TOKEN_EXPIRED');
                        }
                    }

                    return $q.reject(responseError);
                }
            }
        }])
        .config(['$httpProvider', function ($httpProvider) {
            $httpProvider.interceptors.push('HttpBearerTokenAuthorizationInterceptor');
        }]);
})();