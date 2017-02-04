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

        self.selectedTags = [];
        self.tags = [
            {
                id: 1,
                name: 'noun'
            },
            {
                id: 2,
                name: 'adjective'
            },
            {
                id: 3,
                name: 'verb'
            },
            {
                id: 4,
                name: 'adverb'
            },
            {
                id: 5,
                name: 'phrase'
            },
            {
                id: 6,
                name: 'preposition'
            },
            {
                id: 7,
                name: 'formal'
            }
        ];
    }

    DashboardController.prototype = Object.create(PageController.prototype);
    DashboardController.prototype.constructor = DashboardController;

    DashboardController.prototype.onChange = function (event, $item, $model) {
        var self = this;

        console.log(event + ' ' + $item);
    }

    /* Private */


    DashboardController.$inject = ['$injector', '$scope', 'tasksService'];

    angular
        .module('app')
        .controller('dashboardController', DashboardController);

})();