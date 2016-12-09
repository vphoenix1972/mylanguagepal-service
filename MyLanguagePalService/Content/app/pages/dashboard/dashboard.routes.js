(function () {
    'use strict';

    angular
        .module('app')
        .config(function ($routeProvider) {
            $routeProvider
                .when('/dashboard', {
                    templateUrl: '/Content/app/pages/dashboard/dashboard.tpl.html',
                    controller: 'dashboardController as vm'
                });
        });
})();