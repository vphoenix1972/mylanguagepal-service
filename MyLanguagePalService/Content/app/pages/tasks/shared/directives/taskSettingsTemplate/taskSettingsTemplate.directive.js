(function () {
    'use strict';

    angular
        .module('app')
        .directive('taskSettingsTemplate', function () {
            return {
                restrict: 'E',
                transclude: true,
                templateUrl: '/Content/app/pages/tasks/shared/directives/taskSettingsTemplate/taskSettingsTemplate.tpl.html'
            }
        });

    // Hack
    // Try to preload the directive template to prevent blinking of the directive
    // during the first load of page
    angular
        .module('app')
        .run([
            '$http', '$templateCache', function ($http, $templateCache) {
                $http.get('/Content/app/pages/tasks/shared/directives/taskSettingsTemplate/taskSettingsTemplate.tpl.html').then(function (response) {
                    $templateCache.put('/Content/app/pages/tasks/shared/directives/taskSettingsTemplate/taskSettingsTemplate.tpl.html', response.data);
                });
            }
        ]);
})();
