(function () {
    'use strict';

    angular
        .module('app')
        .config(function ($routeProvider) {
            $routeProvider
                .when('/tasks/writeTranslation/settings', {
                    templateUrl: '/Content/app/pages/tasks/writeTranslation/settings/writeTranslationTaskSettings.tpl.html',
                    controller: 'writeTranslationTaskSettingsController as vm'
                })
                .when('/tasks/writeTranslation/task', {
                    templateUrl: '/Content/app/pages/tasks/writeTranslation/task/writeTranslationTask.tpl.html',
                    controller: 'writeTranslationTaskController as vm'
                });
        });
})();