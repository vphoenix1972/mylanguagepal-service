﻿(function () {
    'use strict';

    function WriteTranslationTaskController($injector, $scope, tasksService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self._tasksService = tasksService;

        self._taskName = 'writeTranslation';

        /* Init */
        self.title = 'Write translation task settings';

        self.doAsync(self._tasksService.getSettings(self._taskName))
            .then(function (result) {
                self.isLoading = false;
                self.settings = result;
            })
            .catch(function () {
                self.returnToDashboard();
            });
    }

    WriteTranslationTaskController.prototype = Object.create(PageController.prototype);
    WriteTranslationTaskController.prototype.constructor = WriteTranslationTaskController;

    WriteTranslationTaskController.prototype.onReturnToDashboardButtonClicked = function () {
        var self = this;

        self.returnToDashboard();
    }

    WriteTranslationTaskController.prototype.onSaveAndReturnToDashboardButtonClicked = function () {
        var self = this;

        self._save().then(function () {
            self.returnToDashboard();
        });
    }

    WriteTranslationTaskController.prototype.onSaveAndRunButtonClicked = function () {
        var self = this;

        self._save().then(function () {
            self.$location.path(self._tasksService.getTaskProperties(self._taskName).urls.task);
        });
    }

    /* Private */

    WriteTranslationTaskController.prototype._save = function () {
        var self = this;

        return self.doAsync(self._tasksService.setSettings(self._taskName, self.settings))
            .then(function (result) {
                if (self.validationFailed(result))
                    return self.$q.reject();

                return result;
            });
    }

    WriteTranslationTaskController.$inject = ['$injector', '$scope', 'tasksService'];

    angular
        .module('app')
        .controller('writeTranslationTaskSettingsController', WriteTranslationTaskController);

})();