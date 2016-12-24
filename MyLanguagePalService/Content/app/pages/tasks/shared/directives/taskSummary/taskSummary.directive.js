(function () {
    'use strict';

    /* Private */


    angular
        .module('app')
        .directive('taskSummary', function () {
            return {
                restrict: 'E',
                templateUrl: '/Content/app/pages/tasks/shared/directives/taskSummary/taskSummary.tpl.html'
            }
        });

    // Hack
    // Try to preload the directive template to prevent blinking of the directive
    // during the first load of page
    angular
        .module('app')
        .run([
            '$http', '$templateCache', function ($http, $templateCache) {
                $http.get('/Content/app/pages/tasks/shared/directives/taskSummary/taskSummary.tpl.html').then(function (response) {
                    $templateCache.put('/Content/app/pages/tasks/shared/directives/taskSummary/taskSummary.tpl.html', response.data);
                });
            }
        ]);
})();
