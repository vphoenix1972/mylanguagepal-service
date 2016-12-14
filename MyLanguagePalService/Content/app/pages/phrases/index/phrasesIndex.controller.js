(function () {
    function PhrasesIndexController($injector, $scope, phrasesService) {
        $injector.invoke(PageController, this, { $scope: $scope });

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

    PhrasesIndexController.$inject = ['$injector', '$scope', 'phrasesService'];

    angular
        .module('app')
        .controller('phrasesIndexController', PhrasesIndexController);
})();
