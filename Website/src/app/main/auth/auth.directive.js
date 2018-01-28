(function () {
    'use strict';

    angular
        .module('app.auth')
        .directive('showRoute', ['AuthService', '$rootScope', '$state', function (_authService, $rootScope, $state) {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    handler();

                    var evt_canceler = $rootScope.$on('AUTH_PERMISSIONS_LOADED', function () {
                        handler();
                    })

                    scope.$on('$destroy', function () {
                        evt_canceler();
                    })

                    function handler() {
                        if (!attrs.uiSref && !attrs.showForRoute) {
                            element.hide();
                        } else {
                            var permissions = [];
                            if (attrs.uiSref) {
                                var uiSrefState = $state.get(attrs.uiSref);
                                if (uiSrefState) {
                                    if (uiSrefState.permissions) {
                                        permissions = uiSrefState.permissions;
                                    }
                                }
                            }
                            if (attrs.showForRoute) {
                                var state = $state.get(attrs.showForRoute);
                                if (state) {
                                    if (state.permissions) {
                                        permissions = permissions.concat(state.permissions);
                                    }
                                }
                            }

                            if (permissions.length) {
                                if (!_authService.HasPermissions(permissions)) {
                                    element.hide();
                                } else {
                                    element.show();
                                }
                            } else {
                                element.show();
                            }
                        }
                    }
                }
            }
        }])
        .directive('showPermissions', ['AuthService', '$rootScope', '$state', function (_authService, $rootScope, $state) {
            return {
                restrict: 'A',
                scope: {
                    $$permissions: '=showPermissions'
                },
                link: function (scope, element, attrs) {
                    handler();

                    var evt_canceler = $rootScope.$on('AUTH_PERMISSIONS_LOADED', function () {
                        handler();
                    })
                    
                    scope.$on('$destroy', function () {
                        evt_canceler();
                    })

                    function handler() {
                        if (scope.$$permissions.length) {
                            if (!_authService.HasPermissions(scope.$$permissions)) {
                                element.hide();
                            } else {
                                element.show();
                            }
                        } else {
                            element.show();
                        }
                    }
                }
            }
        }]);
})();