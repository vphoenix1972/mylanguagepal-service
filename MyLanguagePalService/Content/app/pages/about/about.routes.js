(function () {
    'use strict';

    function AboutRoutes($routeProvider) {
        $routeProvider
            .when('/about', {
                templateUrl: '/Content/app/pages/about/about.tpl.html'
            });
    }

    AboutRoutes.$inject = ['$routeProvider'];

    angular
        .module('app')
        .config(AboutRoutes);
})();