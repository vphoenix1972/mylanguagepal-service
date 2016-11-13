(function () {
    function LanguagesDeleteController($scope, $routeParams, $location, errorReportingService, progressBarService, languagesService) {
        PageController.call(this, $scope, errorReportingService, progressBarService);

        var self = this;
        self._languagesService = languagesService;
        self._languageId = $routeParams.languageId;
        self._$location = $location;

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
                self._$location.path('/languages');
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
                self._$location.path('/languages');
            }
        });
    }

    angular
        .module('app')
        .controller('languagesDeleteController', [
            '$scope',
            '$routeParams',
            '$location',
            'errorReportingService',
            'progressBarService',
            'languagesService',
            LanguagesDeleteController
        ]);
})();
