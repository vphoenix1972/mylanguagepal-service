(function () {
    'use strict';

    function ButtonRunTaskDirectiveController() {

    }

    ButtonRunTaskDirectiveController.prototype.$onInit = function () {

    }

    ButtonRunTaskDirectiveController.prototype.$postLink = function () {

    }

    ButtonRunTaskDirectiveController.prototype.$onDestroy = function () {

    }


    /* Private */


    ButtonRunTaskDirectiveController.$inject = [];

    angular
        .module('app')
        .directive('buttonRunTask', function () {
            return {
                restrict: 'E',
                scope: {
                    text: '@'
                },
                controller: ButtonRunTaskDirectiveController,
                controllerAs: 'vm',
                bindToController: true,
                templateUrl: '/Content/app/pages/shared/directives/buttonRunTask/buttonRunTask.tpl.html'
            }
        });

    // Hack
    // Try to preload the directive template to prevent blinking of the directive
    // during the first load of page
    angular
        .module('app')
        .run([
            '$http', '$templateCache', function ($http, $templateCache) {
                $http.get('/Content/app/pages/shared/directives/buttonRunTask/buttonRunTask.tpl.html').then(function (response) {
                    $templateCache.put('/Content/app/pages/shared/directives/buttonRunTask/buttonRunTask.tpl.html', response.data);
                });
            }
        ]);
})();
