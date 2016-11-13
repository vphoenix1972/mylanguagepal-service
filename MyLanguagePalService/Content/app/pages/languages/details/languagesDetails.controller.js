(function () {
    function LanguagesDetailsController($scope, $routeParams, errorReportingService, progressBarService, languagesService) {
        PageController.call(this, $scope, errorReportingService, progressBarService);

        var self = this;

        self.language = {};        

        self.asyncRequest({
            request: function () { return languagesService.getLanguage($routeParams.languageId); },
            success: function (result) {
                self.isLoading = false;
                self.language = result.data;
            },
            error: function () {
                $location.path('/languages');
            }
        });
    }

    LanguagesDetailsController.prototype = Object.create(PageController.prototype);
    LanguagesDetailsController.prototype.constructor = LanguagesDetailsController;

    angular
        .module('app')
        .controller('languagesDetailsController', [
            '$scope',
            '$routeParams',            
            'errorReportingService',
            'progressBarService',
            'languagesService',
            LanguagesDetailsController
        ]);
})();
