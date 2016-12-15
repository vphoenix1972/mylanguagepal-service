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
                self.$location.path('/dashboard');
            });
    }

    SprintTaskSettingsController.prototype = Object.create(PageController.prototype);
    SprintTaskSettingsController.prototype.constructor = SprintTaskSettingsController;

    /* Private */


    SprintTaskSettingsController.$inject = ['$injector', '$scope', 'sprintTaskService', 'languagesService'];

    angular
        .module('app')
        .controller('sprintTaskSettingsController', SprintTaskSettingsController);

})();