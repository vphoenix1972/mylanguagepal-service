(function () {
    function PhrasesIndexController($scope, errorReportingService, progressBarService, phrasesService) {
        PageController.call(this, $scope, errorReportingService, progressBarService);

        var self = this;

        self.phrases = [];

        this.asyncRequest({
            request: function () { return phrasesService.getPhrases(); },
            success: function (result) {
                self.isLoading = false;
                self.phrases = result.data;
            }
        });
    }

    PhrasesIndexController.prototype = Object.create(PageController.prototype);
    PhrasesIndexController.prototype.constructor = PhrasesIndexController;

    PhrasesIndexController.$inject = [
        '$scope',
        'errorReportingService',
        'progressBarService',
        'phrasesService'
    ];

    angular
        .module('app')
        .controller('phrasesIndexController', PhrasesIndexController);
})();
