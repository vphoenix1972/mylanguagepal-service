(function () {
    function ConnectorService($q, rest) {
        var self = this;

        self._$q = $q;
        self._restService = rest;
    }

    ConnectorService.prototype.getLanguages = function () {
        /// <summary>
        /// Gets the language list.
        /// Returns a promise:
        /// resolve(languages): Array of languages.
        /// reject(error): Network or Http error
        /// </summary>

        return this._getDataAndHandleAllErrors('/api/languages');
    }

    ConnectorService.prototype.getLanguage = function (id) {
        /// <summary>
        /// Gets the language with specified id.
        /// Returns a promise:
        /// resolve(language): Language object.
        /// reject(error): Network or Http error
        /// </summary>

        return this._getDataAndHandleAllErrors('/api/languages/' + id);
    }

    ConnectorService.prototype._getDataAndHandleAllErrors = function (url) {
        var self = this;
        return self._restService.get(url).then(
            function (response) {
                return response.data;
            },
            function (response) {
                return self._handleError(response);
            });
    }

    ConnectorService.prototype._handleError = function (response) {
        // If a network error occured...
        if (response.status < 0)
            return this._$q.reject(new NetworkError());

        // Otherwise it is a server error
        return this._$q.reject(new HttpError(response.status));
    }


    angular.module('app').service('connectorService', [
        '$q',
        'rest',
        ConnectorService
    ]);
})();