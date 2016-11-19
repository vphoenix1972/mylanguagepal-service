(function () {
    'use strict';

    angular
        .module('app')
        .config(function ($routeProvider) {
            $routeProvider
            .when('/languages', {
                templateUrl: '/Content/app/pages/languages/index/languagesIndex.tpl.html',
                controller: 'languagesIndexController as vm'
            })
            .when('/languages/details/:languageId', {
                templateUrl: '/Content/app/pages/languages/details/languagesDetails.tpl.html',
                controller: 'languagesDetailsController as vm'
            })
            .when('/languages/create', {
                templateUrl: '/Content/app/pages/languages/edit/languagesEdit.tpl.html',
                controller: 'languagesEditController as vm'
            })
            .when('/languages/edit/:languageId', {
                templateUrl: '/Content/app/pages/languages/edit/languagesEdit.tpl.html',
                controller: 'languagesEditController as vm'
            })
            .when('/languages/delete/:languageId', {
                templateUrl: '/Content/app/pages/languages/delete/languagesDelete.tpl.html',
                controller: 'languagesDeleteController as vm'
            });
        });
})();