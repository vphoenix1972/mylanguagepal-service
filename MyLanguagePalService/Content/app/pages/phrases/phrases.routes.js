(function () {
    'use strict';

    angular
        .module('app')
        .config(function ($routeProvider) {
            $routeProvider
            .when('/phrases', {
                templateUrl: '/Content/app/pages/phrases/index/phrasesIndex.tpl.html',
                controller: 'phrasesIndexController as vm'
            })
            .when('/phrases/details/:phraseId', {
                templateUrl: '/Content/app/pages/phrases/details/phraseDetails.tpl.html',
                controller: 'phraseDetailsController as vm'
            })
            .when('/phrases/create', {
                templateUrl: '/Content/app/pages/phrases/edit/phrasesEdit.tpl.html',
                controller: 'phrasesEditController as vm'
            })
            .when('/phrases/edit/:phraseId', {
                templateUrl: '/Content/app/pages/phrases/edit/phrasesEdit.tpl.html',
                controller: 'phrasesEditController as vm'
            })
            .when('/phrases/delete/:phraseId', {
                templateUrl: '/Content/app/pages/phrases/delete/phrasesDelete.tpl.html',
                controller: 'phrasesDeleteController as vm'
            });
        });
})();