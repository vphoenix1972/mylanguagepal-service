(function () {
    function LanguagesEditController($injector, $scope, languagesService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;
        self._languagesService = languagesService;
        self._languageId = self.$routeParams.languageId;
        
        /* View model */
        self.isNew = !angular.isDefined(self._languageId);
        self.languageName = '';
        self.languageNameValidationError = '';
        self.saveLanguage = self._saveLanguage;

        /* Initialize */
        self._init();
    }

    LanguagesEditController.prototype = Object.create(PageController.prototype);
    LanguagesEditController.prototype.constructor = LanguagesEditController;

    LanguagesEditController.prototype._init = function () {
        var self = this;

        // If edit requested ...
        if (self.isNew) {
            self.isLoading = false;
        } else {
            self.asyncRequest({
                request: function () { return self._languagesService.getLanguage(self._languageId); },
                success: function (result) {
                    self.isLoading = false;

                    self.languageName = result.data.name;
                },
                error: function () {
                    self.$location.path('/languages');
                }
            });
        }
    }

    LanguagesEditController.prototype._saveLanguage = function () {
        var self = this;

        if (self.isNew) {
            self.asyncRequest({
                request: function () {
                    return self._languagesService.createLanguage({
                        name: self.languageName
                    });
                },
                success: function (result) {
                    if (result instanceof ValidationConnectorResult) {
                        self.languageNameValidationError = result.validationState.name.join();
                        return;
                    }

                    self.$location.path('/languages');
                }
            });
        } else {
            self.asyncRequest({
                request: function () {
                    return self._languagesService.updateLanguage(self._languageId, {
                        name: self.languageName
                    });
                },
                success: function (result) {
                    if (result instanceof ValidationConnectorResult) {
                        self.languageNameValidationError = result.validationState.name.join();
                        return;
                    }

                    self.$location.path('/languages');
                }
            });
        }
    }

    LanguagesEditController.$inject = ['$injector', '$scope', 'languagesService'];

    angular
        .module('app')
        .controller('languagesEditController', LanguagesEditController);
})();
