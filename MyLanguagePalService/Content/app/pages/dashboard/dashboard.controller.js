(function () {
    'use strict';

    function DashboardController($injector, $scope, tasksService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;
        self._tasksService = tasksService;

        /* Init */
        self.tasks = [];

        self.doAsync(self._tasksService.getTasks())
            .then(function (result) {
                self.tasks = result;
                self.tasks.forEach(function (task) {
                    angular.extend(task, self._tasksService.getTaskProperties(task.name));
                });
            });
    }

    DashboardController.prototype = Object.create(PageController.prototype);
    DashboardController.prototype.constructor = DashboardController;

    /* Private */


    DashboardController.$inject = ['$injector', '$scope', 'tasksService'];

    angular
        .module('app')
        .controller('dashboardController', DashboardController);

})();