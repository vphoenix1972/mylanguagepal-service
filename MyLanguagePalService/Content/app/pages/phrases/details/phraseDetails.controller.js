(function () {
    function PhraseDetailsController($scope, $routeParams, errorReportingService, progressBarService, phrasesService) {
        PageController.call(this, $scope, errorReportingService, progressBarService);

        var self = this;

        self.id = $routeParams.phraseId;
        
        self.asyncRequest({
            request: function () { return phrasesService.getPhrase($routeParams.phraseId); },
            success: function (result) {
                self.isLoading = false;

                self.id = $routeParams.phraseId;
                self.phrase = {
                    languageId: 1,
                    text: result.data.text
                }                
                self.translations = result.data.translations.orderBy(function (ts1, ts2) { return ts2.prevalence - ts1.prevalence; });
            },
            error: function () {
                $location.path('/phrases');
            }
        });
    }

    PhraseDetailsController.prototype = Object.create(PageController.prototype);
    PhraseDetailsController.prototype.constructor = PhraseDetailsController;

    angular
        .module('app')
        .controller('phraseDetailsController', [
            '$scope',
            '$routeParams',
            'errorReportingService',
            'progressBarService',
            'phrasesService',
            PhraseDetailsController
        ]);
})();
