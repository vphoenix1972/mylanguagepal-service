﻿(function () {
    function ConnectorService($q, config, rest) {
        var self = this;

        self._$q = $q;

        self._restService = rest;
        self._restService.setAntiForgeryToken(config.antiForgeryToken);
    }

    /* *** Public *** */

    /* Languages */
    ConnectorService.prototype.getLanguages = function () {
        var self = this;
        return self.get('/api/languages');
    }

    ConnectorService.prototype.getLanguage = function (id) {
        var self = this;
        return self.get('/api/languages/' + id);
    }

    ConnectorService.prototype.createLanguage = function (language) {
        var self = this;
        return self.postAndHandle422('/api/languages', language);
    }

    ConnectorService.prototype.updateLanguage = function (id, language) {
        var self = this;
        return self.putAndHandle422('/api/languages/' + id, language);
    }

    ConnectorService.prototype.deleteLanguage = function (id) {
        var self = this;
        return self.delete('/api/languages/' + id);
    }

    /* Phrases */
    ConnectorService.prototype.getPhrases = function () {
        var self = this;
        return self.get('/api/phrases');
    }

    ConnectorService.prototype.getPhrase = function (id) {
        var self = this;
        return self.get('/api/phrases/' + id);
    }

    ConnectorService.prototype.createPhrase = function (phrase) {
        var self = this;
        return self.postAndHandle422('/api/phrases', phrase);
    }

    ConnectorService.prototype.updatePhrase = function (id, phrase) {
        var self = this;
        return self.putAndHandle422('/api/phrases/' + id, phrase);
    }

    ConnectorService.prototype.deletePhrase = function (id) {
        var self = this;
        return self.delete('/api/phrases/' + id);
    }

    /* Translations */
    ConnectorService.prototype.getTranslations = function (phraseId) {
        var self = this;
        return self.get('/api/phrases/' + phraseId + '/translations');
    }

    ConnectorService.prototype.get = function (url) {
        var self = this;
        return self._restService.get(url).then(
            function (response) {
                return self._createResult(response);
            },
            function (response) {
                return self._handleError(response);
            });
    }

    ConnectorService.prototype.post = function (url, data) {
        var self = this;
        return self._restService.post(url,data ).then(
            function (response) {
                return self._createResult(response);
            },
            function (response) {
                return self._handleError(response);
            });
    }

    ConnectorService.prototype.postAndHandle422 = function (url, data) {
        var self = this;
        return self._restService.post(url, data).then(
            function (response) {
                return self._createResult(response);
            },
            function (response) {
                if (response.status === 422) {
                    // Validation error occured
                    return self._createValidationResult(response);
                }

                return self._handleError(response);
            });
    }

    ConnectorService.prototype.putAndHandle422 = function (url, data) {
        var self = this;
        return self._restService.put(url, data).then(
            function (response) {
                return self._createResult(response);
            },
            function (response) {
                if (response.status === 422) {
                    // Validation error occured
                    return self._createValidationResult(response);
                }

                return self._handleError(response);
            });
    }

    ConnectorService.prototype.delete = function (url, data) {
        var self = this;
        return self._restService.delete(url, data).then(
            function (response) {
                return self._createResult(response);
            },
            function (response) {
                return self._handleError(response);
            });
    }

    /* *** Private *** */    

    ConnectorService.prototype._handleError = function (response) {
        // If a network error occured...
        if (response.status < 0)
            return this._$q.reject(new NetworkError());

        // Otherwise it is a server error
        return this._$q.reject(new HttpError(response.status));
    }

    ConnectorService.prototype._createResult = function (response) {
        var result = new ConnectorResult();

        result.response = response;
        result.data = response.data;

        return result;
    }

    ConnectorService.prototype._createValidationResult = function (response) {
        var result = new ValidationConnectorResult();

        result.response = response;

        /* Extract validation errors */

        // modelState is in protocol contract

        // ReSharper disable PossiblyUnassignedProperty
        if (angular.isObject(response.data.modelState)) {
            result.validationState = response.data.modelState;
        }
        // ReSharper restore PossiblyUnassignedProperty

        return result;
    }

    angular.module('app').service('connectorService', [
        '$q',
        'config',
        'rest',
        ConnectorService
    ]);
})();