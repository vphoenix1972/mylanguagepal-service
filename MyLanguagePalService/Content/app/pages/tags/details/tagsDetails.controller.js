(function () {
    function TagsDetailsController($injector, $scope, tagsService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self.tag = {};

        self.asyncRequest({
            // ReSharper disable once PossiblyUnassignedProperty
            request: function () { return tagsService.getTag(self.$routeParams.tagId); },
            success: function (tag) {
                self.isLoading = false;
                self.tag = tag;
            },
            error: function () {
                self.$location.path('/tags');
            }
        });
    }

    TagsDetailsController.prototype = Object.create(PageController.prototype);
    TagsDetailsController.prototype.constructor = TagsDetailsController;

    TagsDetailsController.$inject = ['$injector', '$scope', 'tagsService'];

    angular
        .module('app')
        .controller('tagsDetailsController', TagsDetailsController);
})();
