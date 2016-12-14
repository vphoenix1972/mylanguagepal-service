(function () {
    function LanguagesDetailsController($injector, $scope, languagesService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self.language = {};

        self.asyncRequest({
            request: function () { return languagesService.getLanguage(self.$routeParams.languageId); },
            success: function (result) {
                self.isLoading = false;
                self.language = result.data;
            },
            error: function () {
                self.$location.path('/languages');
            }
        });
    }

    LanguagesDetailsController.prototype = Object.create(PageController.prototype);
    LanguagesDetailsController.prototype.constructor = LanguagesDetailsController;

    LanguagesDetailsController.$inject = ['$injector', '$scope', 'languagesService'];

    angular
        .module('app')
        .controller('languagesDetailsController', LanguagesDetailsController);
})();
