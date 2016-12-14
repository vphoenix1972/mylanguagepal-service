(function () {
    function PhrasesDeleteController($injector, $scope, phrasesService) {
        $injector.invoke(PageController, this, { $scope: $scope });

        var self = this;
        self._phrasesService = phrasesService;
        // ReSharper disable once PossiblyUnassignedProperty
        self._id = self.$routeParams.phraseId;

        /* View model */
        self.text = '';
        self.delete = self._delete;

        /* Initialize */
        self.init();
    }

    PhrasesDeleteController.prototype = Object.create(PageController.prototype);
    PhrasesDeleteController.prototype.constructor = PhrasesDeleteController;

    PhrasesDeleteController.prototype.init = function () {
        var self = this;

        self.asyncRequest({
            request: function () { return self._phrasesService.getPhrase(self._id); },
            success: function (result) {
                self.isLoading = false;

                self.text = result.data.text;
            },
            error: function () {
                self._redirectToIndex();
            }
        });
    }

    PhrasesDeleteController.prototype.onDelete = function () {
        var self = this;

        self.asyncRequest({
            request: function () {
                return self._phrasesService.deletePhrase(self._id);
            },
            success: function () {
                self._redirectToIndex();
            }
        });
    }

    /* Private */

    PhrasesDeleteController.prototype._redirectToIndex = function () {
        var self = this;
        self.$location.path('/phrases');
    }

    PhrasesDeleteController.$inject = ['$injector', '$scope', 'phrasesService'];

    angular
        .module('app')
        .controller('phrasesDeleteController', PhrasesDeleteController);
})();
