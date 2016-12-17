(function () {
    'use strict';

    function SprintTaskController($injector, $scope, utils, sprintTaskService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self._utils = utils;
        self._sprintTaskService = sprintTaskService;

        /* Init */
        self.startingTimerDirective = {};
        self.runningTimerDirective = {};
        self.currentPhrase = {};
        self.currentTranslation = {};
        self.score = 0;
        self.phrases = [];

        self.doAsync(self._sprintTaskService.getSettings())
            .then(function (result) {
                self.settings = result;

                self._gotoState('starting');

                self.isLoading = false;
            })
            .catch(function () {
                self.returnToDashboard();
            });
    }

    SprintTaskController.prototype = Object.create(PageController.prototype);
    SprintTaskController.prototype.constructor = SprintTaskController;

    SprintTaskController.prototype.onTimerDirectiveElapsed = function () {
        var self = this;

        if (self.state === 'starting') {
            self._gotoState('running');
            return;
        }
        if (self.state === 'running') {
            self._gotoState('finished');
            return;
        }
    }

    SprintTaskController.prototype.onIsNotCorrectButtonClicked = function () {
        var self = this;

        self._recordAnswer(false);
    }

    SprintTaskController.prototype.onIsCorrectButtonClicked = function () {
        var self = this;

        self._recordAnswer(true);
    }

    /* Private */

    SprintTaskController.prototype._gotoState = function (state) {
        var self = this;

        if (state === 'starting') {
            self.doAsync(self._sprintTaskService.runNewTask(self.settings))
                .then(function (result) {
                    var phrases = result.phrases;

                    if (phrases.length < 2) {
                        self.errorReportingService.reportError('Cannot run sprint task when count of words less that 2');
                        self.returnToDashboard();
                        return;
                    }

                    phrases.forEach(function (phrase) {
                        phrase.correctAnswersCount = 0;
                        phrase.wrongAnswersCount = 0;
                    });

                    self.phrases = phrases;                    

                    self.startingTimerDirective.start();

                    self.state = state;
                })
                .catch(function () {
                    self.returnToDashboard();
                });

            return;
        }

        if (state === 'running') {
            self.runningTimerDirective.start();

            self.score = 0;
            self._nextPhrase();

            self.state = state;

            return;
        }

        if (state === 'finished') {

            self.state = state;

            return;
        }
    }

    SprintTaskController.prototype._recordAnswer = function (isUserAnsweredYes) {
        var self = this;

        if (self.currentTranslation.isCorrect === isUserAnsweredYes) {
            self.score++;

            self.currentPhrase.correctAnswersCount++;
        } else {
            self.currentPhrase.wrongAnswersCount++;
        }

        self._nextPhrase();
    }

    SprintTaskController.prototype._nextPhrase = function () {
        var self = this;

        var translationPhrase;

        // Select current phrase
        var phraseIndex = self._utils.getRandomInt(self.phrases.length);
        self.currentPhrase = self.phrases[phraseIndex];

        // Select if translation will be correct
        var isTranslationCorrect = self._utils.getRandomBool();
        if (isTranslationCorrect) {
            translationPhrase = self.currentPhrase;
        } else {
            var translationPhraseIndex = self._utils.getRandomIntExcluding(self.phrases.length, [phraseIndex]);
            translationPhrase = self.phrases[translationPhraseIndex];
        }

        // Select a translation
        var translationIndex = self._utils.getRandomInt(translationPhrase.translations.length);
        var translation = translationPhrase.translations[translationIndex].phrase;

        self.currentTranslation = translation;
        self.currentTranslation.isCorrect = isTranslationCorrect;
    }


    SprintTaskController.$inject = ['$injector', '$scope', 'utils', 'sprintTaskService'];

    angular
        .module('app')
        .controller('sprintTaskController', SprintTaskController);

})();