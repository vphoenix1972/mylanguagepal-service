(function () {
    'use strict';

    function WriteAnswerQuizDirectiveController($injector, $scope, $log) {
        $injector.invoke(DirectiveWithMethods, this, { $scope: $scope });

        var self = this;

        self._$log = $log;
    }

    WriteAnswerQuizDirectiveController.prototype = Object.create(DirectiveWithMethods.prototype);
    WriteAnswerQuizDirectiveController.prototype.constructor = WriteAnswerQuizDirectiveController;


    WriteAnswerQuizDirectiveController.prototype.$onInit = function () {
        DirectiveWithMethods.prototype.$onInit.call(this);

        var self = this;

        self.isQuizStarted = false;
        self.currentQuestion = {};
        self.currentQuestionIndex = -1;
    }

    WriteAnswerQuizDirectiveController.prototype.$postLink = function () {

    }

    WriteAnswerQuizDirectiveController.prototype.$onDestroy = function () {

    }

    WriteAnswerQuizDirectiveController.prototype.onMethodsPropertyChanged = function () {
        var self = this;

        self.methods.startQuiz = function (questions) { self.startQuiz(questions); }
    }

    WriteAnswerQuizDirectiveController.prototype.startQuiz = function (questions) {
        var self = this;

        if (self.isQuizStarted) {
            self._$log.error('Cannot start quiz - quiz is already started.');
            return;
        }

        if (!angular.isArray(questions)) {
            self._$log.error('Cannot start quiz - questions passed is not an array.');
            return;
        }

        self._questions = questions;

        // Start quiz
        self.isQuizStarted = true;
        self.currentIndex = -1;

        self.nextQuestion();
    }

    WriteAnswerQuizDirectiveController.prototype.nextQuestion = function () {
        var self = this;

        self.currentIndex++;

        if (self.currentIndex >= self._questions.length) {
            // Finish quiz
            self.isQuizStarted = false;
            self.currentIndex = -1;

            // Parse answers' texts
            self._questions.forEach(function (question) {
                var answerText = question.answerText;
                if (!angular.isString(answerText)) {
                    question.answers = [];
                    return;
                }

                question.answers = answerText.splitAndTrim(',');
            });

            self.onQuizFinished({ questions: self._questions });

            return;
        }

        self.currentQuestion = self._questions[self.currentIndex];
    }


    /* Private */


    WriteAnswerQuizDirectiveController.$inject = ['$injector', '$scope', '$log'];

    angular
        .module('app')
        .directive('writeAnswerQuiz', function () {
            return {
                restrict: 'E',
                scope: {
                    methods: '<',
                    onQuizFinished: '&'
                },
                controller: WriteAnswerQuizDirectiveController,
                controllerAs: 'vm',
                bindToController: true,
                templateUrl: '/Content/app/pages/tasks/shared/directives/writeAnswerQuiz/writeAnswerQuiz.tpl.html'
            }
        });

    // Hack
    // Try to preload the directive template to prevent blinking of the directive
    // during the first load of page
    angular
        .module('app')
        .run([
            '$http', '$templateCache', function ($http, $templateCache) {
                $http.get('/Content/app/pages/tasks/shared/directives/writeAnswerQuiz/writeAnswerQuiz.tpl.html').then(function (response) {
                    $templateCache.put('/Content/app/pages/tasks/shared/directives/writeAnswerQuiz/writeAnswerQuiz.tpl.html', response.data);
                });
            }
        ]);
})();
