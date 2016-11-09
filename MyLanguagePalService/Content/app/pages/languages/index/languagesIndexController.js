(function () {
    function LanguagesIndexController($scope, $location, progressBarService, languagesService) {
        PageController.call(this, $scope, $location);

        $scope.languages = [];

        progressBarService.start();
        languagesService.getLanguages().then(function (languages) {
            progressBarService.complete();

            $scope.isLoading = false;
            $scope.languages = languages;
        });
    }

    LanguagesIndexController.prototype = Object.create(PageController.prototype);
    LanguagesIndexController.prototype.constructor = LanguagesIndexController;

    angular.module('app').controller('languagesIndexController', [
        '$scope',
        '$location',
        'progressBarService',
        'languagesService',
        LanguagesIndexController
    ]);
}());
