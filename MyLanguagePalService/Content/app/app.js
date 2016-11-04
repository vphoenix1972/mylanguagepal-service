var app = angular.module('app', ['ui.bootstrap', 'ngRoute', 'ngAnimate']);

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
        templateUrl: '/Content/app/pages/languages/languages.html'
    });
});