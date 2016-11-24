(function () {
    function PhrasesDetailsController($scope, $routeParams, errorReportingService, progressBarService, phrasesService) {
        PageController.call(this, $scope, errorReportingService, progressBarService);

        var self = this;

        self.id = $routeParams.phraseId;
        self.text = '';
        self.translations = '';

        self.asyncRequest({
            request: function () { return phrasesService.getPhrase($routeParams.phraseId); },
            success: function (result) {
                self.isLoading = false;

                self.id = $routeParams.phraseId;
                self.text = result.data.text;
                self.translations = result.data.translations
                .map(function (t) { return t.text; })
                .join(' ');
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
