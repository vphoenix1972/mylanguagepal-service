DirectiveWithMethods = (function () {
    'use strict';

    function DirectiveWithMethods($scope) {
        var self = this;

        self.$scope = $scope;
    }

    DirectiveWithMethods.prototype.$onInit = function () {
        var self = this;

        // Bind methods property
        self.$scope.$watch(function () {
            return self.methods;
        }, function () {
            if (!angular.isObject(self.methods))
                return;

            self.onMethodsPropertyChanged();
        });
    }


    DirectiveWithMethods.prototype.onMethodsPropertyChanged = function () {
        // Override this function to add methods for the directive
    }


    /* Private */


    DirectiveWithMethods.$inject = ['$scope'];

    return DirectiveWithMethods;
})();
