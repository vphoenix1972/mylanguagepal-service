(function () {
    function TagsEditController($injector, $scope, tagsService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;
        self._tagsService = tagsService;
        // ReSharper disable once PossiblyUnassignedProperty
        self._tagId = self.$routeParams.tagId;
        
        /* View model */
        self.isNew = !angular.isDefined(self._tagId);
        self.tag = {};
        self.tagNameValidationError = '';
        self.saveTag = self._saveTag;

        /* Initialize */
        self._init();
    }

    TagsEditController.prototype = Object.create(PageController.prototype);
    TagsEditController.prototype.constructor = TagsEditController;

    TagsEditController.prototype._init = function () {
        var self = this;

        // If edit requested ...
        if (self.isNew) {
            self.isLoading = false;
        } else {
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
    }

    TagsEditController.prototype._saveTag = function () {
        var self = this;

        if (self.isNew) {
            self.asyncRequest({
                request: function () {
                    return self._tagsService.createTag(self.tag);
                },
                success: function (result) {
                    if (result instanceof ValidationConnectorResult) {
                        self.tagNameValidationError = result.validationState.name.join();
                        return;
                    }

                    self.$location.path('/tags');
                }
            });
        } else {
            self.asyncRequest({
                request: function () {
                    return self._tagsService.updateTag(self._tagId, self.tag);
                },
                success: function (result) {
                    if (result instanceof ValidationConnectorResult) {
                        self.tagNameValidationError = result.validationState.name.join();
                        return;
                    }

                    self.$location.path('/tags');
                }
            });
        }
    }

    TagsEditController.$inject = ['$injector', '$scope', 'tagsService'];

    angular
        .module('app')
        .controller('tagsEditController', TagsEditController);
})();
