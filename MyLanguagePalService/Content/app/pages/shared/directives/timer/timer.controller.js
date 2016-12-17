(function () {
    'use strict';

    function TimerController($element, $scope, $log) {
        var self = this;

        self._$element = $element;
        self._$scope = $scope;
        self._$log = $log;

        self._currentCount = 0;

        self._intervalId = null;
    }

    TimerController.prototype.$onInit = function () {
        var self = this;

        // Bind methods property
        self._$scope.$watch(function () {
            return self.methods;
        }, function () {
            self._onMethodsPropertyChanged();
        });

        // Bind count property
        self._$scope.$watch(function () {
            return self.count;
        }, function () {
            self._onCountPropertyChanged();
        });
    }

    TimerController.prototype.$postLink = function () {
    }

    TimerController.prototype.$onDestroy = function () {
        var self = this;

        self._clearInterval();
    }

    TimerController.prototype.isStarted = function () {
        var self = this;

        return self._intervalId != null;
    }

    TimerController.prototype.currentCount = function () {
        var self = this;

        return self._currentCount;
    }

    TimerController.prototype.start = function () {
        var self = this;

        if (!angular.isInteger(self.count)) {
            self._$log.warn('Cannot start timer directive: count property is not an integer, count = ' + self.count);
            return;
        }

        if (self.isStarted()) {
            self._clearInterval();
        }

        self._setCurrentCountToCountProperty();
        self._startInterval();
    }

    TimerController.prototype.stop = function () {
        var self = this;

        if (!self.isStarted()) {
            return;
        }

        self._clearInterval();
        self._setCurrentCountToCountProperty();
    }

    /* Private */

    TimerController.prototype._onMethodsPropertyChanged = function () {
        var self = this;

        if (!angular.isObject(self.methods))
            return;

        // Put functions to an external object so directive's methods can be called outside
        self.methods.start = function () { return self.start(); }
        self.methods.stop = function () { return self.stop(); }
    }

    TimerController.prototype._onCountPropertyChanged = function () {
        var self = this;

        if (self.isStarted()) {
            // Timer is started, control of current count is handled by setInterval
            // New count will be used on next start call
            return;
        }

        // Timer is not started, just render the count property
        self._setCurrentCountToCountProperty();;
    }

    TimerController.prototype._onTimeout = function () {
        var self = this;

        self._currentCount--;

        if (self._currentCount < 1) {
            self._clearInterval();
            self._setCurrentCountToCountProperty();

            self.onElapsed();
            self._$scope.$apply();
        } else {
            self._$scope.$digest();
        }
    }

    TimerController.prototype._startInterval = function () {
        var self = this;

        self._intervalId = setInterval(function () { self._onTimeout(); }, 1000);
    }

    TimerController.prototype._clearInterval = function () {
        var self = this;

        clearInterval(self._intervalId);
        self._intervalId = null;
    }

    TimerController.prototype._setCurrentCountToCountProperty = function () {
        var self = this;

        if (!angular.isInteger(self.count)) {
            self._$log.warn('Cannot set current count: count property is not an integer, count = ' + self.count);
            self._currentCount = 0;
            return;
        }

        self._currentCount = self.count;
    }

    TimerController.$inject = ['$element', '$scope', '$log'];

    angular
        .module('app')
        .controller('timerController', TimerController);

})();