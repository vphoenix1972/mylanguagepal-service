(function () {
    function ConnectorService($q, rest) {
        var self = this;

        self._$q = $q;
        self._restService = rest;
    }

    /* Public */

    ConnectorService.prototype.getLanguages = function () {
        var self = this;
        return self._get('/api/languages');
    }

    ConnectorService.prototype.getLanguage = function (id) {
        var self = this;
        return self._get('/api/languages/' + id);
    }

    ConnectorService.prototype.createLanguage = function (language) {
        var self = this;
        return self._postAndHandle422('/api/languages', language);
    }

    ConnectorService.prototype.deleteLanguage = function (id) {
        var self = this;
        return self._delete('/api/languages/' + id);
    }

    /* Private */

    ConnectorService.prototype._get = function (url) {
        var self = this;
        return self._restService.get(url).then(
            function (response) {
                return self._createResult(response);
            },
            function (response) {
                return self._handleError(response);
            });
    }

    ConnectorService.prototype._postAndHandle422 = function (url, data) {
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

    ConnectorService.prototype._putAndHandle422 = function (url, data) {
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

    ConnectorService.prototype._delete = function (url, data) {
        var self = this;
        return self._restService.delete(url, data).then(
            function (response) {
                return self._createResult(response);
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
        if (angular.isObject(response.data.modelState)) {
            result.validationState = response.data.modelState;
        }

        return result;
    }

    angular.module('app').service('connectorService', [
        '$q',
        'rest',
        ConnectorService
    ]);
})();