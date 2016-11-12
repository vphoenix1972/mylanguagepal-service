(function () {
    angular
        .module('app')
        .controller('languagesIndexController', [
            '$scope',
            '$routeParams',
            'PageControllerType',
            'errorReportingService',
            'progressBarService',
            'languagesService',
            function ($scope, $routeParams, PageControllerType, errorReportingService, progressBarService, languagesService) {
                function LanguagesDetailsController() {
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

                LanguagesDetailsController.prototype = Object.create(PageControllerType.prototype);
                LanguagesDetailsController.prototype.constructor = LanguagesDetailsController;

                return LanguagesDetailsController;
            }]);
})();
