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
        self.translations = [];
        self.levels = [20, 30, 40];
        self.save = self._save;

        /* Initialize */
        self._init();
    }

    PhrasesEditController.prototype = Object.create(PageController.prototype);
    PhrasesEditController.prototype.constructor = PhrasesEditController;

    /* Public */
    PhrasesEditController.prototype.onAddNewTranslationButtonClick = function () {
        var self = this;

        self.translations.add({
            text: '',
            prevalence: 40
        });
    }

    PhrasesEditController.prototype.onDeleteTranslationButtonClicked = function (translation) {
        var self = this;

        self.translations.remove(translation);
    }

    /* Private */

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

            // Get phrase text error
            self.textValidationError = undefined;
            if (angular.isDefined(result.validationState.text))
                self.textValidationError = result.validationState.text.join();

            // Get translations errors
            self.translations.forEach(function (ts) { ts.textValidationError = undefined; });
            angular.forEach(result.validationState, function (value, key) {
                if (!key.startsWith('translations'))
                    return;

                var indexValue = key.getBetween('[', ']');
                if (indexValue == null)
                    return;

                var index = parseInt(indexValue, 10);
                if (index === NaN || index < 0 || index > self.translations.length - 1)
                    return;

                self.translations[index].textValidationError = value.join();
            });

            return;
        }

        self._redirectToIndex();
    }

    PhrasesEditController.prototype._fromPhraseDto = function (dto) {
        var self = this;

        self.languageId = dto.languageId;
        self.text = dto.text;
        self.translations = dto.translations.orderBy(function (ts1, ts2) { return ts2.prevalence - ts1.prevalence; });
    }

    PhrasesEditController.prototype._toPhraseDto = function () {
        var self = this;

        var translations = self.translations
            .map(function (ts) {
                return {
                    text: ts.text.trim(),
                    prevalence: ts.prevalence
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
