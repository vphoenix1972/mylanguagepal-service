(function () {

    function LanguagesIndexController($scope, languagesService) {
        $scope.languages = [];

        languagesService.getLanguages().then(function (languages) {
            $scope.languages = languages;
        });
    }

    angular.module('app').controller('languagesIndexController', [
        '$scope',
        'languagesService',
        LanguagesIndexController
    ]);
}());