(function () {
    'use strict';

    function SprintTaskService(connector) {
        var self = this;

        self._connector = connector;
    }

    SprintTaskService.prototype.getSettings = function () {
        var self = this;
        return self._connector.getSprintTaskSettings();
    }

    SprintTaskService.prototype.setSettings = function (settings) {
        var self = this;
        return self._connector.setSprintTaskSettings('/api/tasks/sprint/settings', settings);
    }

    /* Private */


    SprintTaskService.$inject = ['connectorService'];

    angular
        .module('app')
        .service('sprintTaskService', SprintTaskService);

})();