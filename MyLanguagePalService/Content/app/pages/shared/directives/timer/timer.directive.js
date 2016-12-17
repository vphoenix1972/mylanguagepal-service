(function () {
    'use strict';

    angular
        .module('app')
        .directive('timer', function () {
            return {
                restrict: 'E',
                scope: {
                    methods: '<',
                    count: '<',
                    onElapsed: '&'
                },
                controller: 'timerController',
                controllerAs: 'vm',
                bindToController: true,
                templateUrl: '/Content/app/pages/shared/directives/timer/timer.tpl.html'
            }
        });

    // Hack
    // Try to preload the directive template to prevent blinking of the directive
    // during the first load of page
    angular
        .module('app')
        .run([
            '$http', '$templateCache', function ($http, $templateCache) {
                $http.get('/Content/app/pages/shared/directives/timer/timer.tpl.html').then(function (response) {
                    $templateCache.put('/Content/app/pages/shared/directives/timer/timer.tpl.html', response.data);
                });
            }
        ]);
})();