(function () {
    'use strict';

    angular
        .module('app')
        .config(function ($routeProvider) {
            $routeProvider
            .when('/tasks/sprint/settings', {
                templateUrl: '/Content/app/pages/tasks/sprint/settings/sprintTaskSettings.tpl.html',
                controller: 'sprintTaskSettingsController as vm'
            });
        });
})();