(function () {
    'use strict';

    function SprintTaskController($injector, $scope, sprintTaskService, languagesService, $timeout) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self._sprintTaskService = sprintTaskService;
        self._languagesService = languagesService;

        /* Init */
        self.doAsync(self._languagesService.getLanguages())
            .then(function (result) {
                self.languages = result[0].data;
                return self.doAsync(self._sprintTaskService.getSettings());
            })
            .then(function (result) {
                self.isLoading = false;
                self.settings = result[0].data;
            })
            .catch(function () {
                self.$location.path('/dashboard');
            });
    }

    SprintTaskController.prototype = Object.create(PageController.prototype);
    SprintTaskController.prototype.constructor = SprintTaskController;

    /* Private */


    SprintTaskController.$inject = ['$injector', '$scope', 'sprintTaskService', 'languagesService', '$timeout'];

    angular
        .module('app')
        .controller('sprintTaskController', SprintTaskController);

})();