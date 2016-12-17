(function() {
    'use strict';

    function SprintTaskController() {
        var self = this;

        self.timerDirective = {};
    }

    SprintTaskController.prototype.onTimerDirectiveElapsed = function () {
        var self = this;

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


    SprintTaskController.$inject = [];

    angular
        .module('app')
        .controller('sprintTaskController', SprintTaskController);

})();