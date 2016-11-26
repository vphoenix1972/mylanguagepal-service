(function () {
    function PhrasesEditController($scope, $routeParams, $location, errorReportingService, progressBarService, phrasesService) {
        PageController.call(this, $scope, errorReportingService, progressBarService);

        var self = this;
        self._phrasesService = phrasesService;
        self._phraseId = $routeParams.phraseId;
        self._$location = $location;

        /* View model defaults */
        self.isNew = !angular.isDefined(self._phraseId);
        self.languageId = 1; // ToDo: Use English by default right now
        self.text = '';
        self.translations = '';
        self.save = self._save;

        /* Initialize */
        self._init();
    }

    PhrasesEditController.prototype = Object.create(PageController.prototype);
    PhrasesEditController.prototype.constructor = PhrasesEditController;

    PhrasesEditController.prototype._init = function () {
        var self = this;

        // If edit requested ...
        if (self.isNew) {
            self.isLoading = false;
        } else {
            self.asyncRequest({
                request: function () { return self._phrasesService.getPhrase(self._phraseId); },
                success: function (result) {
                    self.isLoading = false;

                    self._fromPhraseDto(result.data);
                },
                error: function () {
                    self._redirectToIndex();
                }
            });
        }
    }

    PhrasesEditController.prototype._save = function () {
        var self = this;

        if (self.isNew) {
            self.asyncRequest({
                request: function () {
                    return self._phrasesService.createPhrase(self._toPhraseDto());
                },
                success: self._onSaveSuccess
            });
        } else {
            self.asyncRequest({
                request: function () {
                    return self._phrasesService.updatePhrase(self._phraseId, self._toPhraseDto());
                },
                success: self._onSaveSuccess
            });
        }
    }

    PhrasesEditController.prototype._onSaveSuccess = function (result) {
        var self = this;

        if (result instanceof ValidationConnectorResult) {

            angular.forEach(self, function (value, key) {
                if (key.endsWith('ValidationError'))
                    self[key] = undefined;
            });

            angular.forEach(result.validationState, function (value, key) {
                self[key + 'ValidationError'] = value.join();
            });

            return;
        }

        self._redirectToIndex();
    }

    PhrasesEditController.prototype._fromPhraseDto = function (dto) {
        var self = this;

        self.languageId = dto.languageId;
        self.text = dto.text;
        self.translations = dto.translations
            .map(function (t) { return t.text; })
            .join(', ');;
    }

    PhrasesEditController.prototype._toPhraseDto = function () {
        var self = this;

        // Split and trim the translations
        var translations = self.translations
            .split(',')
            .filter(function (s) { return s.trim(); })
            .map(function (ts) {
                return {
                    text: ts,
                    prevalence: 40
                }
            });

        return {
            languageId: self.languageId,
            text: self.text,
            translations: translations
        }
    }

    PhrasesEditController.prototype._redirectToIndex = function () {
        var self = this;
        self._$location.path('/phrases');
    }

    angular
        .module('app')
        .controller('phrasesEditController', [
            '$scope',
            '$routeParams',
            '$location',
            'errorReportingService',
            'progressBarService',
            'phrasesService',
            PhrasesEditController
        ]);
})();
