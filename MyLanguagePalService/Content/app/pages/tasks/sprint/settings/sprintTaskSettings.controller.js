(function () {
    'use strict';

    function SprintTaskSettingsController($injector, $scope, sprintTaskService, languagesService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self._sprintTaskService = sprintTaskService;
        self._languagesService = languagesService;

        /* Init */
        self.doAsync(self._languagesService.getLanguages())
            .then(function (result) {
                self.languages = result.data;
                return self.doAsync(self._sprintTaskService.getSettings());
            })
            .then(function (result) {
                self.isLoading = false;
                self.settings = result.data;
            })
            .catch(function () {
                self._returnToDashboard();
            });
    }

    SprintTaskSettingsController.prototype = Object.create(PageController.prototype);
    SprintTaskSettingsController.prototype.constructor = SprintTaskSettingsController;

    SprintTaskSettingsController.prototype.onReturnToDashboardButtonClicked = function () {
        var self = this;

        self._returnToDashboard();
    }

    SprintTaskSettingsController.prototype.onSaveAndReturnToDashboardButtonClicked = function () {
        var self = this;

        self._save().then(function () {
            self._returnToDashboard();
        });
    }

    SprintTaskSettingsController.prototype.onSaveAndRunButtonClicked = function () {
        var self = this;

        self._save().then(function () {
            self.$location.path('/tasks/sprint/task');
        });
    }

    /* Private */

    SprintTaskSettingsController.prototype._save = function () {
        var self = this;

        return self.doAsync(self._sprintTaskService.setSettings(self.settings))
            .then(function (result) {
                if (self.validationFailed(result))
                    return self.$q.reject();

                return result;
            });
    }

    SprintTaskSettingsController.prototype._returnToDashboard = function () {
        var self = this;

        self.$location.path('/dashboard');
    }

    SprintTaskSettingsController.$inject = ['$injector', '$scope', 'sprintTaskService', 'languagesService'];

    angular
        .module('app')
        .controller('sprintTaskSettingsController', SprintTaskSettingsController);

})();