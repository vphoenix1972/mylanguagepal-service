(function () {
    function LanguagesDeleteController($scope, errorReportingService, progressBarService, languagesService) {
        PageController.call(this, $scope, errorReportingService, progressBarService);

        var self = this;
        self.language = { id: 1, name: 'english' };
    }

    LanguagesDeleteController.prototype = Object.create(PageController.prototype);
    LanguagesDeleteController.prototype.constructor = LanguagesDeleteController;

    angular
        .module('app')
        .controller('languagesDeleteController', [
            '$scope',            
            'errorReportingService',
            'progressBarService',
            'languagesService',
            LanguagesDeleteController
        ]);
})();
