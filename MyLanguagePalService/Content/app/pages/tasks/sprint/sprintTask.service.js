(function () {
    'use strict';

    function SprintTaskService(connector) {
        var self = this;

        self._connector = connector;
    }

    SprintTaskService.prototype.getSettings = function () {
        var self = this;
        return self._connector.get('/api/tasks/sprint/settings')
            .then(function (result) {
                return result.data;
            });
    }

    SprintTaskService.prototype.setSettings = function (settings) {
        var self = this;
        return self._connector.postAndHandle422('/api/tasks/sprint/settings', settings);
    }

    SprintTaskService.prototype.runNewTask = function (settings) {
        var self = this;

        return self._connector.post('/api/tasks/sprint/run', settings)
            .then(function (result) {
                return result.data;
            });
    }

    /* Private */


    SprintTaskService.$inject = ['connectorService'];

    angular
        .module('app')
        .service('sprintTaskService', SprintTaskService);

})();