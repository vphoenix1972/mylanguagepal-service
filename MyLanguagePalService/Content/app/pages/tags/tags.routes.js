(function () {
    'use strict';

    function TagsRoutes($routeProvider) {
        $routeProvider
           .when('/tags', {
               templateUrl: '/Content/app/pages/tags/index/tagsIndex.tpl.html',
               controller: 'tagsIndexController as vm'
           })
           .when('/tags/details/:tagId', {
               templateUrl: '/Content/app/pages/tags/details/tagsDetails.tpl.html',
               controller: 'tagsDetailsController as vm'
           })
           .when('/tags/create', {
               templateUrl: '/Content/app/pages/tags/edit/tagsEdit.tpl.html',
               controller: 'tagsEditController as vm'
           })
           .when('/tags/edit/:tagId', {
               templateUrl: '/Content/app/pages/tags/edit/tagsEdit.tpl.html',
               controller: 'tagsEditController as vm'
           })
           .when('/tags/delete/:tagId', {
               templateUrl: '/Content/app/pages/tags/delete/tagsDelete.tpl.html',
               controller: 'tagsDeleteController as vm'
           });
    }

    TagsRoutes.$inject = ['$routeProvider'];

    angular
        .module('app')
        .config(TagsRoutes);
})();