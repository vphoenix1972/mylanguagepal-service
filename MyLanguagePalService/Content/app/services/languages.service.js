(function () {
    function LanguagesService(utils, connector) {
        this._utils = utils;
        this._connector = connector;
    }

    LanguagesService.prototype.getLanguages = function () {
        return this._connector.getLanguages();
    }

    LanguagesService.prototype.getLanguage = function (id) {
        id = this._utils.parseIntOrThrow(id, 'id');

        return this._connector.getLanguage(id);
    }

    LanguagesService.prototype.createLanguage = function (language) {
        return this._connector.createLanguage(language);
    }

    LanguagesService.prototype.deleteLanguage = function (id) {
        id = this._utils.parseIntOrThrow(id, 'id');

        return this._connector.deleteLanguage(id);
    }

    angular.module('app').service('languagesService', [
        'utils',
        'connectorService',
        LanguagesService
    ]);
})();