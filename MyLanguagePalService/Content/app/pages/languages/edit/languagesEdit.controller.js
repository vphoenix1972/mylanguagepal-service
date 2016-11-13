(function () {
    function LanguagesEditController($scope, $routeParams, $location, errorReportingService, progressBarService, languagesService) {
        PageController.call(this, $scope, errorReportingService, progressBarService);

        var self = this;
        self._languagesService = languagesService;
        self._$routeParams = $routeParams;
        self._$location = $location;

        /* View model */
        self.isNew = true;
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
        if (angular.isDefined(self._$routeParams.languageId)) {
            self.asyncRequest({
                request: function () { return self._languagesService.getLanguage(self._$routeParams.languageId); },
                success: function (result) {
                    self.isLoading = false;

                    self.isNew = false;
                    self.languageName = result.data.name;
                },
                error: function () {
                    self._$location.path('/languages');
                }
            });
        } else {
            self.isLoading = false;

            self.isNew = true;
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

                    self._$location.path('/languages');
                }
            });
        } else {
            alert('edit');
        }
    }

    angular
        .module('app')
        .controller('languagesEditController', [
            '$scope',
            '$routeParams',
            '$location',
            'errorReportingService',
            'progressBarService',
            'languagesService',
            LanguagesEditController
        ]);
})();
