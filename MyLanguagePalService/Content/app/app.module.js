(function () {
    /* Setup module and deps */
    var app = angular.module('app', [
        // Angular modules
        'ngRoute',
        'ngAnimate',
        'ngProgress',

        // Third-party modules
        'ui.bootstrap',

        // Application modules
        'app.core'
    ]);

    /* Setup default routes */
    function DefaultRoutes($routeProvider) {
        $routeProvider
        .when('/', {
            redirectTo: '/dashboard'
        })
        .otherwise({
            redirectTo: '/dashboard'
        });
    }

    DefaultRoutes.$inject = ['$routeProvider'];

    app.config(DefaultRoutes);
})();