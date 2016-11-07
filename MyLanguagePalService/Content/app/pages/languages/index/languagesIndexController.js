function LanguagesIndexController($scope, $location, languagesService) {
    PageController.call(this, $scope, $location);

    $scope.languages = [];

    languagesService.getLanguages().then(function (languages) {
        $scope.isLoading = false;
        $scope.languages = languages;
    });
}

LanguagesIndexController.prototype = Object.create(PageController.prototype);
LanguagesIndexController.prototype.constructor = LanguagesIndexController;

angular.module('app').controller('languagesIndexController', [
    '$scope',
    '$location',
    'languagesService',
    LanguagesIndexController
]);
