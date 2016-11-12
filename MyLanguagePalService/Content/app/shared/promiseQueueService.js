function PromiseQueueService($q) {
    this._$q = $q;

    this._isExecuting = false;
    this._queue = [];
}

PromiseQueueService.prototype.enqueue = function (fn) {
    var self = this;

    var deferred;
    if (self._isExecuting) {
        // Enqueue the function
        deferred = self._$q.defer();

        self._queue.push(function () {
            self._run(fn, deferred);
        });

        return deferred.promise;
    }

    // Run the function immediately
    deferred = self._$q.defer();

    self._run(fn, deferred);

    return deferred.promise;
}

PromiseQueueService.prototype._run = function (fn, deferred) {
    var self = this;

    self._isExecuting = true;

    fn().then(function () {
        // Notify caller
        deferred.resolve.apply(deferred, arguments);

        // Run next
        var next = self._queue.shift();
        if (angular.isUndefined(next)) {
            self._isExecuting = false;
            return;
        }
        next();
    }, function () {
        // Notify caller
        deferred.reject.apply(deferred, arguments);

        // Run next
        var next = self._queue.shift();
        if (angular.isUndefined(next)) {
            self._isExecuting = false;
            return;
        }
        next();
    });
}