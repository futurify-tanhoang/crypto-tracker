(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('uploadControl', uploadControl);

    /** @ngInject */
    function uploadControl() {
        return {
            restrict: 'A',
            controller: [
                '$scope', function ($scope) {
                    this.$setControl = function (control) {
                        $scope.$control = function () {
                            return control;
                        }
                    }
                    this.$setFiles = function (files) {
                        $scope.$files = files;
                    }
                    this.$callback = function (callback) {
                        $scope.$evalAsync(callback);
                    }
                }
            ],
            controllerAs: '$uploader',
            link: function (scope, element, attrs, ctrl) {
                var $uploader = ctrl;

                var input = $(element.find('input')[0]);

                $uploader.$setControl(input);

                var clicked = false;
                element.click(function (event) {
                    if (!clicked) {
                        clicked = true;
                        input.click();
                        clicked = false;
                    }
                });

                input.change(function (event) {
                    $uploader.$setFiles(this.files);

                    if (attrs.onFileSelected) {
                        $uploader.$callback(attrs.onFileSelected);
                    }
                });
            }
        }
    }
})();