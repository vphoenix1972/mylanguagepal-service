(function () {
    function ConnectorService(restService) {
        this._restService = restService;
    }

    ConnectorService.prototype.getLanguages = function () {
        /// <summary>
        /// Gets the language list.
        /// Returns a promise:
        /// resolve(languages): Array of languages.
        /// </summary>

        return this._restService.get('/api/languages');
    }

    ConnectorService.prototype.getLanguage = function (id) {
        /// <summary>
        /// Gets the language list.
        /// Returns a promise:
        /// resolve(languages): Array of languages.
        /// </summary>

        return this._restService.get('/api/languages/' + id);
    }

    angular.module('app').service('connectorService', [
        'restService',
        ConnectorService
    ]);
}());