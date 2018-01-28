(function () {
    'use strict';

    angular
        .module('app.core.elementLoading', [])
        .directive('elementLoading', function () {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    if (attrs.elementLoading) {
                        element.append('<div class="element-loading"><div class="loader"></div></div>');

                        var loader = element.find('>.element-loading');

                        element.click(function (e) {
                            if (element.hasClass('loading')) {
                                e.preventDefault();
                                e.stopPropagation();
                            }
                        })

                        function correctContainerSize(count) {
                            loader.width(element.outerWidth());
                            loader.height(element.outerHeight());
                            if (count < 5) {
                                setTimeout(function () {
                                    correctContainerSize(count + 1)
                                }, 200);
                            }
                        }

                        if (scope.$eval(attrs.elementLoading)) {
                            loader.width(element.outerWidth());
                            loader.height(element.outerHeight());
                            loader.addClass('loading');
                            element.addClass('loading');
                            correctContainerSize(0);
                        }

                        scope.$watch(function () {
                            return scope.$eval(attrs.elementLoading);
                        }, function (val, old) {
                            if (val != old) {
                                if (val) {
                                    correctContainerSize(0);
                                    loader.addClass('loading');
                                    element.addClass('loading');
                                } else {
                                    loader.removeClass('loading');
                                    element.removeClass('loading');
                                }
                            }
                        })
                    }
                }
            }
        })
})();
