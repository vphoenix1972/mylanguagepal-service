(function () {
    function Rest($http, promiseQueue) {
        var self = this;

        self._$http = $http;
        self._promiseQueue = promiseQueue;
        self._requestConfig = {};
    }

    Rest.prototype.get = function (url) {
        /// <summary>
        /// Executes GET request.
        /// Returns a $http.get promise.
        /// </summary>

        var self = this;
        return self._promiseQueue.enqueue(function () { return self._$http.get(url, self._requestConfig); });
    }

    Rest.prototype.post = function (url, data) {
        /// <summary>
        /// Executes POST request.
        /// Returns a $http.post promise.
        /// </summary>

        var self = this;
        return self._promiseQueue.enqueue(function () { return self._$http.post(url, data, self._requestConfig); });
    }

    Rest.prototype.put = function (url, data) {
        /// <summary>
        /// Executes PUT request.
        /// Returns a $http.put promise.
        /// </summary>

        var self = this;
        return self._promiseQueue.enqueue(function () { return self._$http.put(url, data, self._requestConfig); });
    }

    Rest.prototype.delete = function (url) {
        /// <summary>
        /// Executes DELETE request.
        /// Returns a $http.delete promise.
        /// </summary>

        var self = this;
        return self._promiseQueue.enqueue(function () { return self._$http.delete(url, self._requestConfig); });
    }

    Rest.prototype.setAntiForgeryToken = function (token) {
        var self = this;

        if (angular.isString(token)) {
            self._requestConfig.headers = { 'X-XSRF-Token': token };
        } else {
            self._requestConfig.headers = undefined;
        }
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