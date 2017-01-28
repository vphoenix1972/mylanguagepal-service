(function () {
    'use strict';

    function MlpDisabledDirectiveController($attrs, $parse, $scope) {
        var self = this;

        self._$attrs = $attrs;
        self._$parse = $parse;
        self._$scope = $scope;
    }

    MlpDisabledDirectiveController.prototype.$onInit = function () {
        var self = this;

        // ReSharper disable once PossiblyUnassignedProperty
        var vaDisabledProperty = self._$attrs.mlpDisabled;
        if (!vaDisabledProperty) {
            self.value = function () { return true; }
            return;
        }

        var invoker = self._$parse(vaDisabledProperty);
        self.value = function () { return invoker(self._$scope); };
    }

    MlpDisabledDirectiveController.prototype.$postLink = function () {

    }

    MlpDisabledDirectiveController.prototype.$onDestroy = function () {

    }


    /* Private */


    MlpDisabledDirectiveController.$inject = ['$attrs', '$parse', '$scope'];

    angular
        .module('app')
        .directive('mlpDisabled',
            function () {
                return {
                    restrict: 'A',
                    controller: MlpDisabledDirectiveController,
                    bindToController: true
                }
            });
})();