var app = angular.module("myLanguagePal", ["ngRoute"]);

// Configure routes
app.config(function ($routeProvider) {
    $routeProvider

        // GET /
        .when("/", {
            templateUrl: "app/home/home.html",
            controller: "HomeController"
        })

        // GET /phrases
        .when("/phrases", {
            templateUrl: "app/phrases/phrases.html",
            controller: "PhrasesController"
        })

        // GET /languages
        .when("/languages", {
            templateUrl: "app/languages/languages.html",
            controller: "LanguagesController"
        })

        // GET /about
        .when("/about", {
            templateUrl: "app/about/about.html",
            controller: "AboutController"
        });
});