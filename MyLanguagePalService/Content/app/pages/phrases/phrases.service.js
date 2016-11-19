(function () {
    function PhrasesService(utils, connector) {
        this._utils = utils;
        this._connector = connector;
    }

    PhrasesService.prototype.getPhrases = function () {
        return this._connector.getPhrases();
    }

    PhrasesService.prototype.getPhrase = function (id) {
        id = this._utils.parseIntOrThrow(id, 'id');

        return this._connector.getPhrase(id);
    }

    PhrasesService.prototype.createPhrase = function (phrase) {
        return this._connector.createPhrase(phrase);
    }

    PhrasesService.prototype.updatePhrase = function (id, phrase) {
        id = this._utils.parseIntOrThrow(id, 'id');

        return this._connector.updatePhrase(id, phrase);
    }

    PhrasesService.prototype.deletePhrase = function (id) {
        id = this._utils.parseIntOrThrow(id, 'id');

        return this._connector.deletePhrase(id);
    }

    PhrasesService.$inject = [
        'utils',
        'connectorService'
    ];

    angular
        .module('app')
        .service('phrasesService', PhrasesService);
})();