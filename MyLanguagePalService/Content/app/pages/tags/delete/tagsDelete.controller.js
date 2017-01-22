(function () {
    function TagsDeleteController($injector, $scope, tagsService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;
        self._tagsService = tagsService;
        // ReSharper disable once PossiblyUnassignedProperty
        self._tagId = self.$routeParams.tagId;

        /* View model */
        self.tag = {};
        
        /* Initialize */
        self._init();
    }

    TagsDeleteController.prototype = Object.create(PageController.prototype);
    TagsDeleteController.prototype.constructor = TagsDeleteController;

    TagsDeleteController.prototype._init = function () {
        var self = this;

        self.asyncRequest({
            request: function () { return self._tagsService.getTag(self._tagId); },
            success: function (tag) {
                self.isLoading = false;

                self.tag = tag;
            },
            error: function () {
                self.$location.path('/tags');
            }
        });
    }

    TagsDeleteController.prototype.deleteTag = function () {
        var self = this;

        self.asyncRequest({
            request: function () {
                return self._tagsService.deleteTag(self._tagId);
            },
            success: function () {
                self.$location.path('/tags');
            }
        });
    }

    TagsDeleteController.$inject = ['$injector', '$scope', 'tagsService'];

    angular
        .module('app')
        .controller('tagsDeleteController', TagsDeleteController);
})();
