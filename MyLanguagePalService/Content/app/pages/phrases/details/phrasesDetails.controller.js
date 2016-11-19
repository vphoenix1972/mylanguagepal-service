(function () {
    function PhrasesDetailsController($scope, $routeParams, errorReportingService, progressBarService, phrasesService) {
        PageController.call(this, $scope, errorReportingService, progressBarService);

        var self = this;

        self.phrase = {
            id: $routeParams.phraseId,
            text: '',
            translations: ''
        };

        self.asyncRequest({
            request: function () { return phrasesService.getPhrase($routeParams.phraseId); },
            success: function (result) {
                self.isLoading = false;
                self.phrase = {
                    id: $routeParams.phraseId,
                    text: result.data.text,
                    translations: result.data.translations.join(' ')
                }
            },
            error: function () {
                $location.path('/phrases');
            }
        });
    }

    PhrasesDetailsController.prototype = Object.create(PageController.prototype);
    PhrasesDetailsController.prototype.constructor = PhrasesDetailsController;

    angular
        .module('app')
        .controller('phrasesDetailsController', [
            '$scope',
            '$routeParams',
            'errorReportingService',
            'progressBarService',
            'phrasesService',
            PhrasesDetailsController
        ]);
})();
