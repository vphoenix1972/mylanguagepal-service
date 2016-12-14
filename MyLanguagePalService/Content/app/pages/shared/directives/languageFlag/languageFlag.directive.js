(function () {
    'use strict';

    function LanguageFlagDirectiveController($element, $scope) {
        var self = this;

        self._$element = $element;
        self._$scope = $scope;

        /* Init */



        // Bind $destroy event
        self._$scope.$on('$destroy', self.onDestroy);
    }

    LanguageFlagDirectiveController.prototype.onDestroy = function () {

    }


    /* Private */


    LanguageFlagDirectiveController.$inject = ['$element', '$scope'];

    angular
        .module('app')
        .directive('languageFlag', function () {
            return {
                restrict: 'E',
                scope: {
                    languageId: '<'
                },
                controller: LanguageFlagDirectiveController,
                controllerAs: 'vm',
                bindToController: true,
                templateUrl: '/Content/app/pages/shared/directives/languageFlag/languageFlag.tpl.html'
            }
        });

    // Hack
    // Try to preload the directive template to prevent blinking of the directive
    // during the first load of page
    angular
        .module('app')
        .run([
            '$http', '$templateCache', function ($http, $templateCache) {
                $http.get('/Content/app/pages/shared/directives/languageFlag/languageFlag.tpl.html').then(function (response) {
                    $templateCache.put('/Content/app/pages/shared/directives/languageFlag/languageFlag.tpl.html', response.data);
                });
            }
        ]);
})();
