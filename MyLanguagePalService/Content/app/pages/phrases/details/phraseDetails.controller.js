(function () {
    function PhraseDetailsController($scope, $routeParams, $location, errorReportingService, progressBarService, phrasesService) {
        PageController.call(this, $scope, errorReportingService, progressBarService);

        var self = this;

        self.asyncRequest({
            request: function () { return phrasesService.getPhraseDetails($routeParams.phraseId); },
            success: function (result) {
                self.isLoading = false;

                self.phrase = result;
                self.phrase.translations = self.phrase.translations.orderBy(phrasesService.translationsComparerByPrevalence);

                self.phrase.translations.forEach(function (translation) {
                    translation.synonims = translation.synonims.orderBy(phrasesService.translationsComparerByPrevalence);
                });
            },
            error: function () {
                $location.path('/phrases');
            }
        });
    }

    PhraseDetailsController.prototype = Object.create(PageController.prototype);
    PhraseDetailsController.prototype.constructor = PhraseDetailsController;

    PhraseDetailsController.$inject = ['$scope', '$routeParams', '$location', 'errorReportingService', 'progressBarService', 'phrasesService'];

    angular
        .module('app')
        .controller('phraseDetailsController', PhraseDetailsController);
})();
