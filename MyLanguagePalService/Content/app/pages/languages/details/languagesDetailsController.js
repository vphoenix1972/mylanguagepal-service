(function () {

    function LanguagesDetailsController($scope, $routeParams, languagesService) {
        $scope.language = {};

        languagesService.getLanguage($routeParams.languageId).then(function (language) {
            $scope.language = language;
        });
    }

    angular.module('app').controller('languagesDetailsController', [
        '$scope',
        '$routeParams',
        'languagesService',
        LanguagesDetailsController
    ]);
}());