(function () {
    'use strict';

    angular
        .module('app')
        .config(function ($routeProvider) {
            $routeProvider
                .when('/about', {
                    templateUrl: '/Content/app/pages/about/about.tpl.html'
                });
        });
})();