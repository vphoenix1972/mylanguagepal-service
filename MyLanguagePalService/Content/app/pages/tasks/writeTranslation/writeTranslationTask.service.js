(function() {
    'use strict';

    function WriteTranslationTaskService(connector) {
        var self = this;

        self._connector = connector;
    }

    WriteTranslationTaskService.prototype.getSettings = function () {
        var self = this;
        return self._connector.get('/api/tasks/writeTranslation/settings')
            .then(function (result) {
                return result.data;
            });
    }

    WriteTranslationTaskService.prototype.setSettings = function (settings) {
        var self = this;
        return self._connector.postAndHandle422('/api/tasks/writeTranslation/settings', settings);
    }

    WriteTranslationTaskService.prototype.runNewTask = function (settings) {
        var self = this;

        return self._connector.post('/api/tasks/writeTranslation/run', settings)
            .then(function (result) {
                return result.data;
            });
    }

    WriteTranslationTaskService.prototype.finishTask = function (summary) {
        var self = this;

        return self._connector.post('/api/tasks/writeTranslation/finish', summary);
    }

    /* Private */


    WriteTranslationTaskService.$inject = ['connectorService'];

    angular
        .module('app')
        .service('writeTranslationTaskService', WriteTranslationTaskService);

})();