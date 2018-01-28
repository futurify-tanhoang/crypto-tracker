(function ()
{
    'use strict';

    /**
     * Main module of the Fuse
     */
    angular
        .module('crypto', [

            // Core
            'app.core',

            // Navigation
            'app.navigation',

            // Toolbar
            'app.toolbar',

            // my modules
            'app.dashboard',
            'app.auth'
        ]);
})();