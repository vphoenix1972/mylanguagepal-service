(function () {

    function LanguagesService($q, $timeout, utils, connector) {
        this._$q = $q;
        this._$timeout = $timeout;
        this._utils = utils;
        this._connector = connector;

        this._languages = [
            { id: 1, name: 'English' },
            { id: 2, name: 'Русский' }
        ];
    }

    LanguagesService.prototype.getLanguages = function () {
        /// <summary>
        /// Gets the language list.
        /// Returns a promise:
        /// resolve(languages): Array of languages.
        /// </summary>

        return this._connector.getLanguages().then(function(response) {
            return response.data;
        });
    }

    LanguagesService.prototype.getLanguage = function (id) {
        /// <summary>
        /// Gets the language with the specified id.
        /// Returns a promise:
        /// resolve(language): Returns the language or 'undefined' if language was not found.
        /// </summary>

        id = this._utils.parseIntOrThrow(id, 'id');

        var self = this;

        return this._$timeout(function () {
            return angular.copy(self._languages.find(function (l) {
                return l.id === id;
            }));
        }, 100);
    }

    angular.module('app').service('languagesService', [
        '$q',
        '$timeout',
        'utils',
        'connectorService',
        LanguagesService
    ]);
}());