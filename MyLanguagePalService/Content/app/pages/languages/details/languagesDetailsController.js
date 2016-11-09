function LanguagesDetailsController($scope, $location, $routeParams, progressBarService, utils, languagesService) {
    PageController.call(this, $scope, $location);

    $scope.language = {};

    //utils.asyncTryCatch(
    //    function () { return languagesService.getLanguage($routeParams.languageId) },
    //    function (language) {
    //        $scope.isLoading = false;
    //        $scope.language = language;
    //    },
    //    function () {
    //        $location.path('/languages');
    //    }
    //);
    var hasLeft = false;
    $scope.$on('$routeChangeStart', function (next, current) {
        hasLeft = true;
        progressBarService.reset();
    });

    progressBarService.start();
    languagesService.getLanguage($routeParams.languageId).then(function (language) {
        if (hasLeft)
            return;

        progressBarService.complete();

        $scope.isLoading = false;
        $scope.language = language;
    });
}

LanguagesDetailsController.prototype = Object.create(PageController.prototype);
LanguagesDetailsController.prototype.constructor = LanguagesDetailsController;

angular.module('app').controller('languagesDetailsController', [
    '$scope',
    '$location',
    '$routeParams',
    'progressBarService',
    'utils',
    'languagesService',
    LanguagesDetailsController
]);