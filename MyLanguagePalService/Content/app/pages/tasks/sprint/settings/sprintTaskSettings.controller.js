(function () {
    'use strict';

    function SprintTaskSettingsController($injector, $scope, tasksService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self._tasksService = tasksService;

        self._taskName = 'sprint';

        /* Init */
        self.title = 'Sprint task settings';

        self.doAsync(self._tasksService.getSettings(self._taskName))
            .then(function (result) {
                self.isLoading = false;
                self.settings = result;
            })
            .catch(function () {
                self.returnToDashboard();
            });
    }

    SprintTaskSettingsController.prototype = Object.create(PageController.prototype);
    SprintTaskSettingsController.prototype.constructor = SprintTaskSettingsController;

    SprintTaskSettingsController.prototype.onReturnToDashboardButtonClicked = function () {
        var self = this;

        self.returnToDashboard();
    }

    SprintTaskSettingsController.prototype.onSaveAndReturnToDashboardButtonClicked = function () {
        var self = this;

        self._save().then(function () {
            self.returnToDashboard();
        });
    }

    SprintTaskSettingsController.prototype.onSaveAndRunButtonClicked = function () {
        var self = this;

        self._save().then(function () {
            self.$location.path(self._tasksService.getTaskProperties(self._taskName).urls.task);
        });
    }

    /* Private */

    SprintTaskSettingsController.prototype._save = function () {
        var self = this;

        return self.doAsync(self._tasksService.setSettings(self._taskName, self.settings))
            .then(function (result) {
                if (self.validationFailed(result))
                    return self.$q.reject();

                return result;
            });
    }

    SprintTaskSettingsController.$inject = ['$injector', '$scope', 'tasksService'];

    angular
        .module('app')
        .controller('sprintTaskSettingsController', SprintTaskSettingsController);

})();