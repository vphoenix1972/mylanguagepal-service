(function () {
    'use strict';

    angular
        .module('app')
        .config(function ($routeProvider) {
            $routeProvider
            .when('/tasks/sprint', {
                templateUrl: '/Content/app/pages/tasks/sprint/sprintTask.tpl.html',
                controller: 'sprintTaskController as vm'
            });
        });
})();