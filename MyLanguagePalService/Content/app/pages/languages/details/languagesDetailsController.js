function LanguagesDetailsController($scope, $location, $routeParams, utils, languagesService) {
    PageController.call(this, $scope, $location);

    $scope.language = {};

    utils.asyncTryCatch(
        function () { return languagesService.getLanguage($routeParams.languageId) },
        function (language) {
            $scope.isLoading = false;
            $scope.language = language;
        },
        function () {
            $location.path('/languages');
        }
    );
}

LanguagesDetailsController.prototype = Object.create(PageController.prototype);
LanguagesDetailsController.prototype.constructor = LanguagesDetailsController;

angular.module('app').controller('languagesDetailsController', [
    '$scope',
    '$location',
    '$routeParams',
    'utils',
    'languagesService',
    LanguagesDetailsController
]);