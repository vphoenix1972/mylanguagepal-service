(function () {
    function TagsIndexController($injector, $scope, tagsService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;

        self.tags = [];

        self.asyncRequest({
            request: function () { return tagsService.getTags(); },
            success: function (tags) {
                self.isLoading = false;
                self.tags = tags;
            }
        });
    }

    TagsIndexController.prototype = Object.create(PageController.prototype);
    TagsIndexController.prototype.constructor = TagsIndexController;

    TagsIndexController.$inject = ['$injector', '$scope', 'tagsService'];

    angular
        .module('app')
        .controller('tagsIndexController', TagsIndexController);
})();
