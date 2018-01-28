(function ()
{
    'use strict';

    angular
        .module('crypto')
        .config(config);

    /** @ngInject */
    function config($translateProvider, $translatePartialLoaderProvider)
    {
        // Put your common app configurations here

        // angular-translate configuration
        $translateProvider.useLoader('$translatePartialLoader', {
            urlTemplate: '{part}/lang/{lang}.json'
        });
        $translateProvider.preferredLanguage('en');
        $translateProvider.useSanitizeValueStrategy('sanitize');

        // Translation
        $translatePartialLoaderProvider.addPart('app');
    }

})();