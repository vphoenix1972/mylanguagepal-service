(function () {
    function LanguagesDeleteController($injector, $scope, languagesService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;
        self._languagesService = languagesService;
        self._languageId = self.$routeParams.languageId;

        /* View model */
        self.language = {};
        self.deleteLanguage = self._deleteLanguage;

        /* Initialize */
        self._init();
    }

    LanguagesDeleteController.prototype = Object.create(PageController.prototype);
    LanguagesDeleteController.prototype.constructor = LanguagesDeleteController;

    LanguagesDeleteController.prototype._init = function () {
        var self = this;

        self.asyncRequest({
            request: function () { return self._languagesService.getLanguage(self._languageId); },
            success: function (result) {
                self.isLoading = false;

                self.language = result.data;
            },
            error: function () {
                self.$location.path('/languages');
            }
        });
    }

    LanguagesDeleteController.prototype._deleteLanguage = function () {
        var self = this;

        self.asyncRequest({
            request: function () {
                return self._languagesService.deleteLanguage(self._languageId);
            },
            success: function () {
                self.$location.path('/languages');
            }
        });
    }

    LanguagesDeleteController.$inject = ['$injector', '$scope', 'languagesService'];

    angular
        .module('app')
        .controller('languagesDeleteController', LanguagesDeleteController);
})();
