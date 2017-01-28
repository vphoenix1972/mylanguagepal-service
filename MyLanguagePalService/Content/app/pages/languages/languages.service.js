(function () {
    function LanguagesService(utils, connector) {
        var self = this;

        self._utils = utils;
        self._connector = connector;

        self._englishLanguageId = 1;
        self._russianLanguageId = 2;
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

    LanguagesService.prototype.updateLanguage = function (id, language) {
        id = this._utils.parseIntOrThrow(id, 'id');

        return this._connector.updateLanguage(id, language);
    }

    LanguagesService.prototype.deleteLanguage = function (id) {
        id = this._utils.parseIntOrThrow(id, 'id');

        return this._connector.deleteLanguage(id);
    }

    LanguagesService.prototype.detectLanguage = function (userInput) {
        var self = this;

        userInput = userInput.trim();

        if (userInput.length < 1)
            return self._englishLanguageId;

        // If the input starts with a cyrrilic letter, assume the russian language
        if (/[а-яА-ЯЁё]/.test(userInput.substr(0, 1)))
            return self._russianLanguageId;

        return self._englishLanguageId;
    }


    angular.module('app').service('languagesService', [
        'utils',
        'connectorService',
        LanguagesService
    ]);
})();