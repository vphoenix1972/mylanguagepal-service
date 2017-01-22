(function () {
    function TagsService(utils, connector) {
        this._utils = utils;
        this._connector = connector;
    }

    TagsService.prototype.getTags = function () {
        var self = this;

        return self._connector.get('/api/tags')
            .then(function (result) {
                return result.data;
            });
    }

    TagsService.prototype.getTag = function (id) {
        var self = this;

        id = self._utils.parseIntOrThrow(id, 'id');

        return self._connector.get('/api/tags/' + id)
            .then(function (result) {
                return result.data;
            });
    }

    TagsService.prototype.createTag = function (tag) {
        var self = this;

        return self._connector.postAndHandle422('/api/tags', tag);
    }

    TagsService.prototype.updateTag = function (id, tag) {
        var self = this;

        id = self._utils.parseIntOrThrow(id, 'id');

        return self._connector.putAndHandle422('/api/tags/' + id, tag);
    }

    TagsService.prototype.deleteTag = function (id) {
        var self = this;

        id = self._utils.parseIntOrThrow(id, 'id');

        return self._connector.delete('/api/tags/' + id);
    }

    angular.module('app').service('tagsService', [
        'utils',
        'connectorService',
        TagsService
    ]);
})();