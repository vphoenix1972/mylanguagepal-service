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