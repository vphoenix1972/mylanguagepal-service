function LanguagesService(utils, connector) {
    this._utils = utils;
    this._connector = connector;
}

LanguagesService.prototype.getLanguages = function () {
    /// <summary>
    /// Gets the language list.
    /// Returns a promise:
    /// resolve(languages): Array of languages.
    /// </summary>

    return this._connector.getLanguages().then(function (response) {
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

    return this._connector.getLanguage(id).then(function (response) {
        return response.data;
    });
}