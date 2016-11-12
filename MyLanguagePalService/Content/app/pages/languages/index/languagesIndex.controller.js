(function () {
    function LanguagesIndexController($scope, PageControllerType, errorReportingService, progressBarService, languagesService) {
        this.prototype = PageControllerType.prototype;
        PageControllerType.call(this, $scope, errorReportingService, progressBarService);

        $scope.languages = [];

        this.asyncRequest({
            request: function () { return languagesService.getLanguages(); },
            success: function (languages) {
                $scope.isLoading = false;
                $scope.languages = languages;
            }
        });
    }

    angular
        .module('app')
        .controller('languagesIndexController', [
            '$scope',
            'PageControllerType',
            'errorReportingService',
            'progressBarService',
            'languagesService',
            LanguagesIndexController
        ]);
})();
