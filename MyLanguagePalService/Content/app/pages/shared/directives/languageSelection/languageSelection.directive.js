(function() {
    'use strict';

    function LanguageSelectionDirectiveController($element, $scope) {
        var self = this;

        self._$element = $element;
        self._$scope = $scope;

        /* Init */

        

        // Bind $destroy event
        self._$scope.$on('$destroy', self.onDestroy);
    }

    LanguageSelectionDirectiveController.prototype.onDestroy = function() {

    }


    /* Private */


    LanguageSelectionDirectiveController.$inject = ['$element', '$scope'];

    angular
        .module('app')
        .directive('languageSelection', function() {
            return {
                restrict: 'E',
                scope: {
                    languages: '<',
                    selectedLanguageId: '='
                },
                controller: LanguageSelectionDirectiveController,
                controllerAs: 'vm',
                bindToController: true,
                templateUrl: '/Content/app/pages/shared/directives/languageSelection/languageSelection.tpl.html'
            }
        });

    // Hack
    // Try to preload the directive template to prevent blinking of the directive
    // during the first load of page
    angular
        .module('app')
        .run([
            '$http', '$templateCache', function($http, $templateCache) {
                $http.get('/Content/app/pages/shared/directives/languageSelection/languageSelection.tpl.html').then(function(response) {
                    $templateCache.put('/Content/app/pages/shared/directives/languageSelection/languageSelection.tpl.html', response.data);
                });
            }
        ]);
})();
