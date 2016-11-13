(function () {
    function LanguagesEditController($scope, errorReportingService, progressBarService, languagesService) {
        PageController.call(this, $scope, errorReportingService, progressBarService);

        $scope.language = { id: 1, name: 'english' };
    }

    LanguagesEditController.prototype = Object.create(PageController.prototype);
    LanguagesEditController.prototype.constructor = LanguagesEditController;

    angular
        .module('app')
        .controller('languagesEditController', [
            '$scope',            
            'errorReportingService',
            'progressBarService',
            'languagesService',
            LanguagesEditController
        ]);
})();
