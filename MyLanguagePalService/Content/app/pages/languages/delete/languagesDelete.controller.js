(function () {
    angular
        .module('app')
        .controller('languagesDeleteController', [
            '$scope',
            'PageControllerType',
            'errorReportingService',
            'progressBarService',
            'languagesService',
            function ($scope, PageControllerType, errorReportingService, progressBarService, languagesService) {
                function LanguagesDeleteController() {
                    PageControllerType.call(this, $scope, errorReportingService, progressBarService);

                    $scope.language = { id: 1, name: 'english' };
                }

                return LanguagesDeleteController;
            }]);
})();
