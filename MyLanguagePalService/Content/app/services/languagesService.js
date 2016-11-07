(function () {

    function LanguagesService($q, $timeout, utils) {
        this._$q = $q;
        this._$timeout = $timeout;
        this._utils = utils;

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

        var self = this;

        return this._$timeout(function () {
            return angular.copy(self._languages);
        }, 100);
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
        LanguagesService
    ]);
}());