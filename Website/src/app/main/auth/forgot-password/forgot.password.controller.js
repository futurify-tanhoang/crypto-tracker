(function () {
    'use-strict';
    angular
        .module('app.auth')
        .controller('ForgotPasswordController', ForgotPasswordController);

    /** @ngInject **/
    function ForgotPasswordController(AuthService, $scope) {
        var vm = this;
        vm.userName = null;

        vm.requestResetPassword = function () {
            if ($scope.forgotPasswordForm.$invalid) {
                return;
            }

            AuthService.RequestResetPassword(vm.userName).then(function () {
                $state.go('auth.login');
            }, function (error) {
                alert(error.Message);
            });
        };
    };
})();