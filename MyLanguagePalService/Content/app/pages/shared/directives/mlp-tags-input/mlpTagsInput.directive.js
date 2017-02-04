(function () {
    'use strict';

    function MlpTagsInputDirectiveController($injector) {
        $injector.invoke(MlpElement, this);
    }

    MlpTagsInputDirectiveController.prototype = Object.create(MlpElement.prototype);
    MlpTagsInputDirectiveController.prototype.constructor = MlpTagsInputDirectiveController;

    MlpTagsInputDirectiveController.prototype.$onInit = function () {

    }

    MlpTagsInputDirectiveController.prototype.$postLink = function () {

    }

    MlpTagsInputDirectiveController.prototype.$onDestroy = function () {

    }

    MlpTagsInputDirectiveController.prototype.classesArray = function () {
        var self = this;

        var result = MlpElement.prototype.classesArray.call(self);

        // Add additional classes

        return result;
    }

    MlpTagsInputDirectiveController.prototype.options = function () {
        var self = this;

        if (!angular.isObject(self.mlpTagsInputType))
            return undefined;

        return self.mlpTagsInputType.options();
    }

    /* Private */


    MlpTagsInputDirectiveController.$inject = ['$injector'];

    angular
        .module('app')
        .directive('mlpTagsInput',
            function () {
                return {
                    restrict: 'E',
                    require: {
                        mlpDisabled: '?^mlpDisabled',
                        mlpTagsInputType: '?^mlpTagsInputType'
                    },
                    scope: {
                        classes: '<?',
                        value: '=?'
                    },
                    controller: MlpTagsInputDirectiveController,
                    controllerAs: 'vm',
                    bindToController: true,
                    templateUrl: '/Content/app/pages/shared/directives/mlp-tags-input/mlpTagsInput.tpl.html'
                }
            });

    // Hack
    // Try to preload the directive template to prevent blinking of the directive
    // during the first load of page
    angular
        .module('app')
        .run([
            '$http', '$templateCache', function ($http, $templateCache) {
                $http.get('/Content/app/pages/shared/directives/mlp-tags-input/mlpTagsInput.tpl.html').then(function (response) {
                    $templateCache.put('/Content/app/pages/shared/directives/mlp-tags-input/mlpTagsInput.tpl.html', response.data);
                });
            }
        ]);
})();
