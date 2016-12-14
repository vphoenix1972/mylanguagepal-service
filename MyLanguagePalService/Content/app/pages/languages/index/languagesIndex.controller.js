(function () {
    function LanguagesIndexController($injector, $scope, languagesService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self.languages = [];

        this.asyncRequest({
            request: function () { return languagesService.getLanguages(); },
            success: function (result) {
                self.isLoading = false;
                self.languages = result.data;
            }
        });
    }

    LanguagesIndexController.prototype = Object.create(PageController.prototype);
    LanguagesIndexController.prototype.constructor = LanguagesIndexController;

    LanguagesIndexController.$inject = ['$injector', '$scope', 'languagesService'];

    angular
        .module('app')
        .controller('languagesIndexController', LanguagesIndexController);
})();
