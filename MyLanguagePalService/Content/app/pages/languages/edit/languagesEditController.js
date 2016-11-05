(function () {

    function LanguagesEditController($scope) {
        $scope.language = { id: 1, name: 'english' };
    }

    angular.module('app').controller('languagesEditController', [
        '$scope',
        LanguagesEditController
    ]);
}());