(function () {
    'use strict';

    function LanguageSelectionDirectiveController($element, $q, languagesService) {
        var self = this;

        self._$element = $element;
        self._$q = $q;
        self._languagesService = languagesService;
    }

    LanguageSelectionDirectiveController.prototype.$onInit = function () {
        var self = this;

        self.languages = [];

        self._$q.when(self._languagesService.getLanguages()).then(function (result) {
            self.languages = result.data;
        });
    }

    LanguageSelectionDirectiveController.prototype.$onDestroy = function () {

    }


    /* Private */


    LanguageSelectionDirectiveController.$inject = ['$element', '$q', 'languagesService'];

    angular
        .module('app')
        .directive('languageSelection', function () {
            return {
                restrict: 'E',
                scope: {
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
            '$http', '$templateCache', function ($http, $templateCache) {
                $http.get('/Content/app/pages/shared/directives/languageSelection/languageSelection.tpl.html').then(function (response) {
                    $templateCache.put('/Content/app/pages/shared/directives/languageSelection/languageSelection.tpl.html', response.data);
                });
            }
        ]);
})();