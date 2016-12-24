(function () {
    'use strict';

    function WriteTranslationTaskController($injector, $scope, utils, taskService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self._utils = utils;
        self._taskService = taskService;

        /* Init */
        self.writeAnswerQuizDirective = {};
        self.isFinished = false;

        self.doAsync(self._taskService.getSettings())
           .then(function (result) {
               self.settings = result;

               return self.doAsync(self._taskService.runNewTask(self.settings))
                   .then(function (result) {
                       self.phrases = result.phrases;

                       self.isLoading = false;

                       self.phrases.forEach(function (phrase) {
                           phrase.correctAnswersCount = 0;
                           phrase.wrongAnswersCount = 0;
                           phrase.delta = 0;
                       });

                       self.writeAnswerQuizDirective.startQuiz(self.phrases.map(function (phrase) {
                           return {
                               phraseId: phrase.id,
                               text: phrase.text
                           }
                       }));
                   });
           })
           .catch(function () {
               self.returnToDashboard();
           });
    }

    WriteTranslationTaskController.prototype = Object.create(PageController.prototype);
    WriteTranslationTaskController.prototype.constructor = WriteTranslationTaskController;

    WriteTranslationTaskController.prototype.onQuizFinished = function (questions) {
        var self = this;

        // Check answers
        var score = 0;
        questions.forEach(function (question) {
            var phrase = self.phrases.find(function (p) { return p.id === question.phraseId; });

            if (question.answers.length < 1) {
                phrase.wrongAnswersCount++;
            } else {
                question.answers.forEach(function (answer) {
                    var isCorrect = phrase.translations.any(function (translation) { return translation.phrase.text === answer; });
                    if (isCorrect) {
                        phrase.correctAnswersCount++;
                    } else {
                        phrase.wrongAnswersCount++;
                    }
                });
            }

            phrase.delta = phrase.correctAnswersCount - phrase.wrongAnswersCount;
            score += phrase.delta;
        });

        // Show summary
        self.isFinished = true;
        self.summary = {
            score: score,
            phrases: self.phrases
        }

        // Send answers to server
        var summaryData = {
            results: self.summary.phrases.map(function (phrase) {
                return {
                    phraseId: phrase.id,
                    correctAnswersCount: phrase.correctAnswersCount,
                    wrongAnswersCount: phrase.wrongAnswersCount
                }
            })
        }

        self.doAsync(self._taskService.finishTask(summaryData));
    }

    /* Private */


    WriteTranslationTaskController.$inject = ['$injector', '$scope', 'utils', 'writeTranslationTaskService'];

    angular
        .module('app')
        .controller('writeTranslationTaskController', WriteTranslationTaskController);

})();