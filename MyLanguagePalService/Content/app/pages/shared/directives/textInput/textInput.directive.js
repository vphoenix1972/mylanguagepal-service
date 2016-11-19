(function () {
    'use strict';

    function TextInputDirectiveController() {
    }

    angular
        .module('app')
        .directive('textInput', function () {
            return {
                scope: {
                    text: '=',
                    validationMessage: '@'
                },
                controller: TextInputDirectiveController,
                controllerAs: 'vm',
                bindToController: true,
                templateUrl: '/Content/app/pages/shared/directives/textInput/textInput.directive.html'
            };
        });
})();