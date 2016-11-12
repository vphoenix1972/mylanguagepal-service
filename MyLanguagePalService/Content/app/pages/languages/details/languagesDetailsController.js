function LanguagesDetailsController($scope, $location, $routeParams, errorReportingService, progressBarService, languagesService) {
    PageController.call(this, $scope, errorReportingService, progressBarService);

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

LanguagesDetailsController.prototype = Object.create(PageController.prototype);
LanguagesDetailsController.prototype.constructor = LanguagesDetailsController;