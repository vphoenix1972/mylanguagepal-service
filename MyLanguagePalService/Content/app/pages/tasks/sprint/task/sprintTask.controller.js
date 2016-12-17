(function() {
    'use strict';

    function SprintTaskController($scope) {
        var self = this;

        self.timerDirective = {};
        self.elapsedCount = 0;

        $scope.$watch(function () {
            console.log('self.elapsedCount watch');
            return self.elapsedCount;
        }, function() {});
    }

    SprintTaskController.prototype.onTimerDirectiveElapsed = function () {
        var self = this;
        self.elapsedCount++;
        console.log('SprintTaskController.prototype.onTimerDirectiveElapsed');
    }

    SprintTaskController.prototype.start = function () {
        var self = this;

        self.timerDirective.start();
    }

    SprintTaskController.prototype.stop = function () {
        var self = this;

        self.timerDirective.stop();
    }

    /* Private */


    SprintTaskController.$inject = ['$scope'];

    angular
        .module('app')
        .controller('sprintTaskController', SprintTaskController);

})();