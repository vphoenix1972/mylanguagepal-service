(function () {

    function LanguagesDeleteController($scope) {
        $scope.language = { id: 1, name: 'english' };
    }

    angular.module('app').controller('languagesDeleteController', [
        '$scope',
        LanguagesDeleteController
    ]);
}());