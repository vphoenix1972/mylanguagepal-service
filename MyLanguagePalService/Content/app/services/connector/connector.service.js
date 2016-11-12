﻿(function () {
    app.service('connectorService', [
        '$q',
        'rest',
        'NetworkErrorType',
        'CustomErrorType',
        function($q, rest, NetworkErrorType, CustomErrorType) {
            function ConnectorService() {
                this._$q = $q;
                this._restService = rest;
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
                    return this._$q.reject(new NetworkErrorType());

                // Otherwise it is a server error
                return this._$q.reject(new CustomErrorType(response.status));
            }

            return ConnectorService;
        }
    ]);
})();