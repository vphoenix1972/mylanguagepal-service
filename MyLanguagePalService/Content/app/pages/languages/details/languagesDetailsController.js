(function () {

    function LanguagesDetailsController($scope) {
        $scope.language = { id: 1, name: 'english' };
    }

    angular.module('app').controller('languagesDetailsController', [
        '$scope',        
        LanguagesDetailsController
    ]);
}());