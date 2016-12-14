(function () {
    function PhraseDetailsController($injector, $scope, phrasesService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self.asyncRequest({
            // ReSharper disable once PossiblyUnassignedProperty
            request: function () { return phrasesService.getPhraseDetails(self.$routeParams.phraseId); },
            success: function (result) {
                self.isLoading = false;

                self.phrase = result;
                self.phrase.translations = self.phrase.translations.orderBy(phrasesService.translationsComparerByPrevalence);

                self.phrase.translations.forEach(function (translation) {
                    translation.synonims = translation.synonims.orderBy(phrasesService.translationsComparerByPrevalence);
                });
            },
            error: function () {
                self.$location.path('/phrases');
            }
        });
    }

    PhraseDetailsController.prototype = Object.create(PageController.prototype);
    PhraseDetailsController.prototype.constructor = PhraseDetailsController;

    PhraseDetailsController.$inject = ['$injector', '$scope', 'phrasesService'];

    angular
        .module('app')
        .controller('phraseDetailsController', PhraseDetailsController);
})();
