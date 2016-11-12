(function () {
    angular
        .module('app')
        .controller('languagesEditController', [
            '$scope',
            'PageControllerType',
            'errorReportingService',
            'progressBarService',
            'languagesService',
            function ($scope, PageControllerType, errorReportingService, progressBarService, languagesService) {
                function LanguagesEditController() {
                    PageControllerType.call(this, $scope, errorReportingService, progressBarService);

                    $scope.language = { id: 1, name: 'english' };
                }

                return LanguagesEditController;
            }]);
})();
