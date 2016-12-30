(function () {
    'use strict';

    function SprintTaskController($injector, $scope, utils, tasksService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self._utils = utils;

        self._tasksService = tasksService;

        self._taskName = 'sprint';

        /* Init */
        self.startingTimerDirective = {};
        self.runningTimerDirective = {};
        self.currentPhrase = {};
        self.currentTranslation = {};
        self.score = 0;
        self.phrases = [];
        self.summary = {};

        self.doAsync(self._tasksService.getSettings(self._taskName))
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

    SprintTaskController.prototype.onRunTaskAgainButtonClicked = function () {
        var self = this;

        self.gotoUrlForce('/' + self._tasksService.getTaskProperties(self._taskName).urls.task);
    }

    /* Private */

    SprintTaskController.prototype._gotoState = function (state) {
        var self = this;

        if (state === 'starting') {
            self.doAsync(self._tasksService.runTask(self._taskName, self.settings))
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
                        phrase.delta = function () {
                            return this.correctAnswersCount - this.wrongAnswersCount;
                        }
                        phrase.usedCount = 0;
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
            // Send only used phrases
            var results = self.phrases
                .filter(function (phrase) { return phrase.usedCount > 0; })
                .orderBy(function (p1, p2) { return p1.delta() - p2.delta(); })
                .map(function (phrase) {
                    return {
                        phraseId: phrase.id,
                        delta: phrase.delta()
                    }
                });

            self.doAsync(self._tasksService.finishTask(self._taskName, {
                settings: self.settings,
                answersModel: results
            })).then(function (summary) {
                self.summary = summary;
                self.state = state;
            }).catch(function () {
                self.returnToDashboard();
            });

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

        self.currentPhrase.usedCount++;

        self._nextPhrase();
    }

    SprintTaskController.prototype._nextPhrase = function () {
        var self = this;

        var translationPhrase;
        var randomIndex;

        // Select current phrase        
        var notUsedPhrases = self.phrases.filter(function (phrase) { return phrase.usedCount < 1; });
        if (notUsedPhrases.length > 0) {
            if (notUsedPhrases.length === 1) {
                // Only one phrase left unused
                // Use this phrase
                self.currentPhrase = notUsedPhrases[0];
            } else {
                // Use unused phrases first
                randomIndex = self._utils.getRandomInt(notUsedPhrases.length);
                self.currentPhrase = notUsedPhrases[randomIndex];
            }
        } else {
            // No unused phrases left - just select random phrase
            randomIndex = self._utils.getRandomInt(self.phrases.length);
            self.currentPhrase = self.phrases[randomIndex];
        }

        // Select if translation will be correct
        var isTranslationCorrect = self._utils.getRandomBool();
        if (isTranslationCorrect) {
            translationPhrase = self.currentPhrase;
        } else {
            var currentPhraseIndex = self.phrases.indexOf(self.currentPhrase);
            var translationPhraseIndex = self._utils.getRandomIntExcluding(self.phrases.length, [currentPhraseIndex]);
            translationPhrase = self.phrases[translationPhraseIndex];
        }

        // Select a translation
        var translationIndex = self._utils.getRandomInt(translationPhrase.translations.length);
        var translation = translationPhrase.translations[translationIndex].phrase;

        self.currentTranslation = translation;
        self.currentTranslation.isCorrect = isTranslationCorrect;
    }


    SprintTaskController.$inject = ['$injector', '$scope', 'utils', 'tasksService'];

    angular
        .module('app')
        .controller('sprintTaskController', SprintTaskController);

})();