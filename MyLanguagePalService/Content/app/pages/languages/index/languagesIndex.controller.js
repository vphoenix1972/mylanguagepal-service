(function () {
    function LanguagesIndexController($scope, errorReportingService, progressBarService, languagesService) {
        PageController.call(this, $scope, errorReportingService, progressBarService);

        var self = this;

        self.languages = [];

        this.asyncRequest({
            request: function () { return languagesService.getLanguages(); },
            success: function (result) {
                self.isLoading = false;
                self.languages = result.data;
            }
        });
    }

    LanguagesIndexController.prototype = Object.create(PageController.prototype);
    LanguagesIndexController.prototype.constructor = LanguagesIndexController;

    angular
        .module('app')
        .controller('languagesIndexController', [
            '$scope',            
            'errorReportingService',
            'progressBarService',
            'languagesService',
            LanguagesIndexController
        ]);
})();
