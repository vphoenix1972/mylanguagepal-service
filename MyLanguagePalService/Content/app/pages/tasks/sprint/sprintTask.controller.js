(function () {
    'use strict';

    function SprintTaskController($injector, $scope, sprintTaskService, languagesService, $timeout) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self._sprintTaskService = sprintTaskService;
        self._languagesService = languagesService;

        //var onLoadError = function () {
        //    $location.path('/dashboard');
        //}
        
        //self.asyncRequest({
        //    request: function () { return self._sprintTaskService.getSettings(); },
        //    success: function (result) {
        //        self.asyncRequest({
        //            request: function () { return self._sprintTaskService.getSettings(); },
        //            success: function (result) {
        //                self.asyncRequest


        //            },
        //            error: onLoadError
        //        });


        //    },
        //    error: onLoadError
        //});
    }

    SprintTaskController.prototype = Object.create(PageController.prototype);
    SprintTaskController.prototype.constructor = SprintTaskController;

    /* Private */


    SprintTaskController.$inject = ['$injector', '$scope', 'sprintTaskService', 'languagesService', '$timeout'];

    angular
        .module('app')
        .controller('sprintTaskController', SprintTaskController);

})();