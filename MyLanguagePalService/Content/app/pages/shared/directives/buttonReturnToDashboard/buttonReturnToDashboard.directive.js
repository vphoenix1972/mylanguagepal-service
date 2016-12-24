(function () {
    'use strict';

    angular
        .module('app')
        .directive('buttonReturnToDashboard', function () {
            return {
                restrict: 'E',
                templateUrl: '/Content/app/pages/shared/directives/buttonReturnToDashboard/buttonReturnToDashboard.tpl.html'
            }
        });

    // Hack
    // Try to preload the directive template to prevent blinking of the directive
    // during the first load of page
    angular
        .module('app')
        .run([
            '$http', '$templateCache', function ($http, $templateCache) {
                $http.get('/Content/app/pages/shared/directives/buttonReturnToDashboard/buttonReturnToDashboard.tpl.html').then(function (response) {
                    $templateCache.put('/Content/app/pages/shared/directives/buttonReturnToDashboard/buttonReturnToDashboard.tpl.html', response.data);
                });
            }
        ]);
})();
