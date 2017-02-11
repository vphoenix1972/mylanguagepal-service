(function () {
    function PhrasesEditController($injector, $scope, phrasesService, languagesService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;
        self._phrasesService = phrasesService;
        self._languagesService = languagesService;
        // ReSharper disable once PossiblyUnassignedProperty
        self._phraseId = self.$routeParams.phraseId;

        /* View model defaults */
        self.isNew = !angular.isDefined(self._phraseId);
        self.languageId = 1; // ToDo: Use English by default right now
        self.text = '';
        self.definition = '';
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

    PhrasesEditController.prototype.onSaveAndGotoListButtonClicked = function () {
        var self = this;

        self.save();
    }

    PhrasesEditController.prototype.onSaveAndGotoDetailsButtonClicked = function () {
        var self = this;

        self.save('gotoDetails');
    }

    /* Private */

    PhrasesEditController.prototype._init = function () {
        var self = this;

        // If edit requested ...
        if (self.isNew) {
            self.isLoading = false;

            // Watch user input to automatically detect the language
            self.$scope.$watch(function () {
                return self.text;
            }, function (userInput) {
                self.languageId = self._languagesService.detectLanguage(userInput);
            });
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

    PhrasesEditController.prototype._save = function (isRedirectToDetails) {
        var self = this;

        var showValidationErrors = function (result) {
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
        }

        var leavePage = function () {
            if (isRedirectToDetails) {
                self._redirectToDetails();
            } else {
                self._redirectToIndex();
            }
        }


        if (self.isNew) {
            self.asyncRequest({
                request: function () {
                    return self._phrasesService.createPhrase(self._toPhraseDto());
                },
                success: function (result) {
                    if (result instanceof ValidationConnectorResult) {
                        showValidationErrors(result);
                        return;
                    }

                    self._phraseId = result.data.id;

                    leavePage();
                }
            });
        } else {
            self.asyncRequest({
                request: function () {
                    return self._phrasesService.updatePhrase(self._phraseId, self._toPhraseDto());
                },
                success: function (result) {
                    if (result instanceof ValidationConnectorResult) {
                        showValidationErrors(result);
                        return;
                    }

                    leavePage();
                }
            });
        }
    }

    PhrasesEditController.prototype._fromPhraseDto = function (dto) {
        var self = this;

        self.languageId = dto.languageId;
        self.text = dto.text;
        self.definition = dto.definition;
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
            definition: self.definition,
            translations: translations
        }
    }

    PhrasesEditController.prototype._redirectToDetails = function () {
        var self = this;
        self.$location.path('/phrases/details/' + self._phraseId);
    }

    PhrasesEditController.prototype._redirectToIndex = function () {
        var self = this;
        self.$location.path('/phrases');
    }

    PhrasesEditController.$inject = ['$injector', '$scope', 'phrasesService', 'languagesService'];

    angular.module('app')
        .controller('phrasesEditController', PhrasesEditController);
})();
