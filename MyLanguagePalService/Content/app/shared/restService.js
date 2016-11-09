function RestService($http) {
    this._$http = $http;
}

RestService.prototype.get = function (url) {
    /// <summary>
    /// Gets the language list.
    /// Returns a promise:
    /// resolve(languages): Array of languages.
    /// </summary>

    return this._$http.get(url);
}