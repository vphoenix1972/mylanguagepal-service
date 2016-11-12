(function () {
    angular
        .module('app')
        .controller('languagesIndexController', [
            '$scope',
            'PageControllerType',
            'errorReportingService',
            'progressBarService',
            'languagesService',
            function ($scope, PageControllerType, errorReportingService, progressBarService, languagesService) {
                function LanguagesIndexController() {
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

                LanguagesIndexController.prototype = Object.create(PageControllerType.prototype);
                LanguagesIndexController.prototype.constructor = LanguagesIndexController;

                return LanguagesIndexController;
            }]);
})();
