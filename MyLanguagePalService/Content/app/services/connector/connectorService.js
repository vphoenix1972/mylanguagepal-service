function ConnectorService($q, restService) {
    this._$q = $q;
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

    return this._getAndHandleAllErrors('/api/languages/' + id);
}

ConnectorService.prototype._getAndHandleAllErrors = function (url) {
    var self = this;
    return self._restService.get(url).catch(
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