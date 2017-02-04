(function () {
    'use strict';

    function MlpTagsInputTypeDirectiveController($attrs, $parse, $scope) {
        var self = this;

        self._$attrs = $attrs;
        self._$parse = $parse;
        self._$scope = $scope;

        self._defaultOptions = [];
    }


    MlpTagsInputTypeDirectiveController.prototype.$onInit = function () {
        var self = this;

        // ReSharper disable once PossiblyUnassignedProperty
        var property = self._$attrs.mlpTagsInputType;
        if (!property) {
            self._type = function () { return undefined; }
            return;
        }

        var invoker = self._$parse(property);
        self._type = function () { return invoker(self._$scope); };

        // Watch type property to set provider
        self._$scope.$watch(function () {
            return self._type();
        }, function (newType) {
            if (newType === 'tags')
                self._provider = ;

            self._provider = undefined;
        });
    }

    MlpTagsInputTypeDirectiveController.prototype.type = function () {
        var self = this;

        return self._type();
    }

    MlpTagsInputTypeDirectiveController.prototype.options = function () {
        var self = this;

        return self._defaultOptions;
    }

    /* Private */


    MlpTagsInputTypeDirectiveController.$inject = ['$attrs', '$parse', '$scope'];

    angular
        .module('app')
        .directive('mlpTagsInputType',
            function () {
                return {
                    restrict: 'A',
                    controller: MlpTagsInputTypeDirectiveController
                }
            });
})();
