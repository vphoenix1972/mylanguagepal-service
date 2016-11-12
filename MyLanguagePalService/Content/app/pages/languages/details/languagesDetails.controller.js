(function () {
    function LanguagesDetailsController($scope, $routeParams, PageControllerType, errorReportingService, progressBarService, languagesService) {
        PageControllerType.call(this, $scope, errorReportingService, progressBarService);

        $scope.language = {};

        this.asyncRequest({
            request: function () { return languagesService.getLanguage($routeParams.languageId); },
            success: function (language) {
                $scope.isLoading = false;
                $scope.language = language;
            },
            error: function () {
                $location.path('/languages');
            }
        });
    }

    angular
        .module('app')
        .controller('languagesDetailsController', [
            '$scope',
            '$routeParams',
            'PageControllerType',
            'errorReportingService',
            'progressBarService',
            'languagesService',
            LanguagesDetailsController
        ]);
})();
