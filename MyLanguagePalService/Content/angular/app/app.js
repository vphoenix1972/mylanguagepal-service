﻿var app = angular.module("myLanguagePal", ["ngRoute"]);

// Configure routes
app.config(function ($routeProvider) {
    $routeProvider

        // GET /
        .when("/", {
            templateUrl: "app/home/home.html",
            controller: "mainController"
        })

        // GET /phrases
        .when("/phrases", {
            templateUrl: "app/phrases/phrases.html",
            controller: "phrasesController"
        })

        // GET /languages
        .when("/languages", {
            templateUrl: "app/languages/languages.html",
            controller: "languagesController"
        })

        // GET /about
        .when("/about", {
            templateUrl: "app/about/about.html",
            controller: "aboutController"
        });
});

// Create the controller and inject Angular's $scope
app.controller("mainController", function ($scope) {
    
});

app.controller("phrasesController", function ($scope) {

});

app.controller("languagesController", function ($scope) {

});

app.controller("aboutController", function ($scope) {
    
});