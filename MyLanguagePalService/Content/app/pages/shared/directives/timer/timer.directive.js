(function () {
    'use strict';

    function TimerDirectiveController($element, $scope, $log) {
        var self = this;

        self._$element = $element;
        self._$scope = $scope;
        self._$log = $log;

        self._currentCount = 0;

        self._intervalId = null;
    }

    TimerDirectiveController.prototype.$onInit = function () {

    }

    TimerDirectiveController.prototype.$postLink = function () {
        var self = this;

        // Cache DOM elements
        self._$span = self._$element.find('.js-timer-directive-span');

        // Bind methods property
        self._$scope.$watch(function () {
            return self.methods;
        }, function () {
            self.onMethodsPropertyChanged();
        });

        // Bind count property
        self._$scope.$watch(function () {
            return self.count;
        }, function () {
            self.onCountPropertyChanged();
        });
    }

    TimerDirectiveController.prototype.$onDestroy = function () {
        var self = this;

        self._clearInterval();
    }

    TimerDirectiveController.prototype.onMethodsPropertyChanged = function () {
        var self = this;

        if (!angular.isObject(self.methods))
            return;

        // Put functions to an external object so directive's methods can be called outside
        self.methods.start = function () { return self.start(); }
        self.methods.stop = function () { return self.stop(); }
    }

    TimerDirectiveController.prototype.onCountPropertyChanged = function () {
        var self = this;

        if (self._isStarted()) {
            // Timer is started, control of current count is handled by setInterval
            // New count will be used on next start call
            return;
        }

        // Timer is not started, just render the count property
        self._currentCount = self.count;
        self._render();
    }

    TimerDirectiveController.prototype.start = function () {
        var self = this;

        if (!angular.isInteger(self.count)) {
            self._$log.warn('Cannot start timer directive: count property is not an integer, count = ' + self.count);
            return;
        }

        if (self._isStarted()) {
            self._clearInterval();
        }

        self._currentCount = self.count;
        self._render();
        self._startInterval();
    }

    TimerDirectiveController.prototype.stop = function () {
        var self = this;

        if (!self._isStarted()) {
            return;
        }

        self._clearInterval();
        self._currentCount = self.count;
        self._render();
    }

    /* Private */

    TimerDirectiveController.prototype._onTimeout = function () {
        var self = this;

        self._currentCount--;

        if (self._currentCount < 1) {
            clearInterval(self._intervalId);
            self._currentCount = self.count;
            self._render();

            self.onElapsed();
            self._$scope.$apply();
        } else {
            self._render();
        }
    }

    TimerDirectiveController.prototype._render = function () {
        var self = this;

        self._$span.html(self._currentCount);
    }

    TimerDirectiveController.prototype._isStarted = function () {
        var self = this;

        return self._intervalId != null;
    }

    TimerDirectiveController.prototype._startInterval = function () {
        var self = this;

        self._intervalId = setInterval(function () { self._onTimeout(); }, 1000);
    }

    TimerDirectiveController.prototype._clearInterval = function () {
        var self = this;

        clearInterval(self._intervalId);
        self._intervalId = null;
    }


    TimerDirectiveController.$inject = ['$element', '$scope', '$log'];

    angular
        .module('app')
        .directive('timer', function () {
            return {
                restrict: 'E',
                scope: {
                    methods: '<',
                    count: '<',
                    onElapsed: '&'
                },
                controller: TimerDirectiveController,
                controllerAs: 'vm',
                bindToController: true,
                templateUrl: '/Content/app/pages/shared/directives/timer/timer.tpl.html'
            }
        });

    // Hack
    // Try to preload the directive template to prevent blinking of the directive
    // during the first load of page
    angular
        .module('app')
        .run([
            '$http', '$templateCache', function ($http, $templateCache) {
                $http.get('/Content/app/pages/shared/directives/timer/timer.tpl.html').then(function (response) {
                    $templateCache.put('/Content/app/pages/shared/directives/timer/timer.tpl.html', response.data);
                });
            }
        ]);
})();