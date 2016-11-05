(function () {

    function LanguagesIndexController($scope) {
        $scope.languages = [
            { id: 1, name: 'english' },
            { id: 2, name: 'russian' }
        ];
    }

    angular.module('app').controller('languagesIndexController', [
        '$scope',        
        LanguagesIndexController
    ]);
}());