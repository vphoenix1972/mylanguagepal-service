function RestService($http, promiseQueue) {
    this._$http = $http;
    this._promiseQueue = promiseQueue;
}

RestService.prototype.get = function (url) {
    /// <summary>
    /// Executes GET request.
    /// Returns a $http.get promise.
    /// </summary>

    var self = this;
    return self._promiseQueue.enqueue(function () { return self._$http.get(url); });
}