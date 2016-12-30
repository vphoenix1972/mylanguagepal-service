(function () {
    'use strict';

    function TasksService(connector) {
        var self = this;

        self._connector = connector;

        self._taskProperties = {};

        self._taskProperties['writeTranslation'] = {
            urls: {
                settings: 'tasks/writeTranslation/settings',
                task: 'tasks/writeTranslation/task'
            }
        }

        self._taskProperties['sprint'] = {
            urls: {
                settings: 'tasks/sprint/settings',
                task: 'tasks/sprint/task'
            }
        }
    }

    TasksService.prototype.getTaskProperties = function (taskName) {
        var self = this;

        return self._taskProperties[taskName];
    }

    TasksService.prototype.getTasks = function () {
        var self = this;

        return self._connector.get('/api/tasks')
            .then(function (result) {
                var tasks = result.data;
                return tasks;
            });
    }

    TasksService.prototype.getSettings = function (taskName) {
        var self = this;
        return self._connector.get('/api/tasks/' + taskName + '/settings')
            .then(function (result) {
                return result.data;
            });
    }

    TasksService.prototype.setSettings = function (taskName, settings) {
        var self = this;
        return self._connector.postAndHandle422('/api/tasks/' + taskName + '/settings', settings);
    }

    /* Private */

    TasksService.$inject = ['connectorService'];

    angular
        .module('app')
        .service('tasksService', TasksService);

})();