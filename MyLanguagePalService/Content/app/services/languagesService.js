(function () {

    function LanguagesService($q, utils) {
        this._$q = $q;
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

        var deferred = this._$q.defer();

        setTimeout(function () {
            deferred.resolve(angular.copy(self._languages));
        }, 100);

        return deferred.promise;
    }

    LanguagesService.prototype.getLanguage = function (id) {
        /// <summary>
        /// Gets the language with the specified id.
        /// Returns a promise:
        /// resolve(language): Returns the language or 'undefined' if language was not found.
        /// </summary>
        
        id = this._utils.parseIntOrThrow(id, 'id');

        var self = this;

        var deferred = this._$q.defer();

        setTimeout(function () {
            deferred.resolve(angular.copy(self._languages.find(function (l) {
                return l.id === id;
            })));
        }, 100);

        return deferred.promise;
    }

    angular.module('app').service('languagesService', [
        '$q',
        'utils',
        LanguagesService
    ]);
}());