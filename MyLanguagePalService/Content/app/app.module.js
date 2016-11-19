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
    app.config(function ($routeProvider) {
        $routeProvider
        .when('/', {
            templateUrl: '/Content/app/pages/about/about.html'
        })
        .when('/about', {
            templateUrl: '/Content/app/pages/about/about.html'
        });
    });
})();