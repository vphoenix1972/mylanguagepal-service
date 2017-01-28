(function () {
    'use strict';

    angular
        .module('mlp.shared')
        .directive('mlpEnter', function () {
            return {
                restrict: 'A',
                link: function ($scope, element, $attributes) {
                    // Create jquery event id
                    var eventId = 'keydown keypress.' + 'mlpEnter';

                    // Get jquery element
                    var $element = angular.element(element);

                    var handler = function (event) {
                        if (event.which === 13) {
                            $scope.$apply(function () {
                                $scope.$eval($attributes.mlpEnter, { $event: event });
                            });
                            event.preventDefault();
                        }
                    }

                    // Bind the event
                    $element.off(eventId, handler)
                        .on(eventId, handler);

                    // Unbind the handler from the event when the directive is destroyed
                    $scope.$on('$destroy', function () {
                        $element.off(eventId, handler);
                    });
                }
            }
        });
})();