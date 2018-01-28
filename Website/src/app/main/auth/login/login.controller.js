(function () {
    'use strict';

    angular
        .module('app.auth')
        .controller('LoginController', LoginController);

    /** @ngInject **/
    function LoginController(AuthService, $translate, $mdDialog, $state, $timeout) {
        var vm = this;

        vm.login = {
            username: null,
            password: null,
            remember: false
        }

        vm.error = null,

        vm.loginfunction = function () {
            AuthService.Login(vm.login).then(function (redirect) {
                if (redirect && redirect.toState) {
                    $state.go(redirect.state, redirect.params);
                } else {
                    $state.go('app.dashboard');
                }
            }, function (error) {
                $timeout(function () {
                    if (error && error.Code) {
                        switch (error.Code) {
                            case 'INCORRECT_LOGIN':
                                vm.error = { incorrect: true }
                                break;
                            default:
                                vm.error = { busy: true }
                                break;
                        }
                    } else {
                        vm.error = { busy: true }
                    }
                }, 300)
            });
        }

    }

})();