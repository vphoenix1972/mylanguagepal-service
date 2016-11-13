(function () {
    function Rest($http, promiseQueue) {
        this._$http = $http;
        this._promiseQueue = promiseQueue;
    }

    Rest.prototype.get = function (url) {
        /// <summary>
        /// Executes GET request.
        /// Returns a $http.get promise.
        /// </summary>

        var self = this;
        return self._promiseQueue.enqueue(function () { return self._$http.get(url); });
    }

    Rest.prototype.post = function (url, data) {
        /// <summary>
        /// Executes POST request.
        /// Returns a $http.post promise.
        /// </summary>

        var self = this;
        return self._promiseQueue.enqueue(function () { return self._$http.post(url, data); });
    }

    Rest.prototype.put = function (url, data) {
        /// <summary>
        /// Executes PUT request.
        /// Returns a $http.put promise.
        /// </summary>

        var self = this;
        return self._promiseQueue.enqueue(function () { return self._$http.put(url, data); });
    }

    Rest.prototype.delete = function (url, data) {
        /// <summary>
        /// Executes DELETE request.
        /// Returns a $http.delete promise.
        /// </summary>

        var self = this;
        return self._promiseQueue.enqueue(function () { return self._$http.delete(url, data); });
    }

    Rest.$inject = ['$http', 'promiseQueue'];

    angular
        .module('app.core')
        .factory('rest', [
            '$injector',
            function ($injector) {
                return $injector.instantiate(Rest);
            }
        ]);
})();