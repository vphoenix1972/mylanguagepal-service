(function () {
    'use strict';

    function DashboardRoutes($routeProvider) {
        $routeProvider
            .when('/dashboard', {
                templateUrl: '/Content/app/pages/dashboard/dashboard.tpl.html',
                controller: 'dashboardController as vm'
            });
    }

    DashboardRoutes.$inject = ['$routeProvider'];

    angular
        .module('app')
        .config(DashboardRoutes);
})();