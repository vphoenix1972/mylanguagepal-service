﻿(function () {
    'use strict';

    function WriteTranslationTaskController($injector, $scope, utils, tasksService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self._utils = utils;
        self._tasksService = tasksService;

        self._taskName = 'writeTranslation';

        /* Init */
        self.writeAnswerQuizDirective = {};
        self.isFinished = false;

        self.doAsync(self._tasksService.getSettings(self._taskName))
           .then(function (result) {
               self.settings = result;

               return self.doAsync(self._tasksService.runTask(self._taskName, self.settings))
                   .then(function (result) {
                       self.phrases = result.phrases;

                       self.isLoading = false;

                       self.phrases.forEach(function (phrase) {
                           phrase.answers = [];
                       });

                       self.writeAnswerQuizDirective.startQuiz(self.phrases.map(function (phrase) {
                           return {
                               phraseId: phrase.id,
                               text: phrase.text,
                               comment: phrase.definition
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

    WriteTranslationTaskController.prototype.onQuizFinished = function (answers) {
        var self = this;

        self.doAsync(self._tasksService.finishTask(self._taskName, {
            settings: self.settings,
            answersModel: {
                answers: answers
            }
        })).then(function (summary) {
            self.isFinished = true;
            self.summary = summary;
        });
    }

    WriteTranslationTaskController.prototype.onRunTaskAgainButtonClicked = function () {
        var self = this;

        self.gotoUrlForce('/' + self._tasksService.getTaskProperties(self._taskName).urls.task);
    }

    /* Private */


    WriteTranslationTaskController.$inject = ['$injector', '$scope', 'utils', 'tasksService'];

    angular
        .module('app')
        .controller('writeTranslationTaskController', WriteTranslationTaskController);

})();