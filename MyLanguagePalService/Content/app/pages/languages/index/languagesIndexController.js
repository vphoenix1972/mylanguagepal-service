(function () {
    function LanguagesIndexController($scope, errorReportingService, progressBarService, languagesService) {
        PageController.call(this, $scope, errorReportingService, progressBarService);

        $scope.languages = [];

        this.asyncRequest({
            request: function () { return languagesService.getLanguages(); },
            success: function (languages) {
                $scope.isLoading = false;
                $scope.languages = languages;
            }
        });
    }

    LanguagesIndexController.prototype = Object.create(PageController.prototype);
    LanguagesIndexController.prototype.constructor = LanguagesIndexController;

    angular.module('app').controller('languagesIndexController', [
        '$scope',
        'errorReportingService',
        'progressBarService',
        'languagesService',
        LanguagesIndexController
    ]);
}());
