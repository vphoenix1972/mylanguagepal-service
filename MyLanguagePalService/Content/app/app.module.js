/* Setup application dependencies */
var app = angular.module('app', [
    'ui.bootstrap',
    'ngRoute',
    'ngAnimate',
    'ngProgress',
    'app.core'
]);

/* Setup routes */
app.config(function ($routeProvider) {
    $routeProvider
    .when('/', {
        templateUrl: '/Content/app/pages/about/about.html'
    })
    .when('/about', {
        templateUrl: '/Content/app/pages/about/about.html'
    })
    .when('/phrases', {
        templateUrl: '/Content/app/pages/phrases/phrases.html'
    })
    .when('/languages', {
        templateUrl: '/Content/app/pages/languages/index/languagesIndex.tpl.html',
        controller: 'languagesIndexController'
    })
    .when('/languages/details/:languageId', {
        templateUrl: '/Content/app/pages/languages/details/languagesDetails.tpl.html',
        controller: 'languagesDetailsController'
    })
    .when('/languages/edit/:languageId', {
        templateUrl: '/Content/app/pages/languages/edit/languagesEdit.tpl.html',
        controller: 'languagesEditController'
    })
    .when('/languages/delete/:languageId', {
        templateUrl: '/Content/app/pages/languages/delete/languagesDelete.tpl.html',
        controller: 'languagesDeleteController'
    });
});