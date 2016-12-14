(function () {
    function Utils($q) {
        var self = this;

        self._$q = $q;
    }

    Utils.prototype.isInteger = function (value) {
        return typeof value === 'number'
           && Number.isFinite(value)
           && !(value % 1);
    }

    Utils.prototype.parseIntOrThrow = function (value, valueName, radix) {
        valueName = angular.isDefined(valueName) ? valueName : 'Value';
        radix = angular.isDefined(radix) ? radix : 10;

        var result = parseInt(value, radix);
        if (isNaN(result))
            throw new Error(valueName + ' is not an integer');
        return result;
    }

    Utils.prototype.cancelPromiseChaining = function () {
        var self = this;

        // Cancel promise chaining
        // Code taken from http://blog.zeit.io/stop-a-promise-chain-without-using-reject-with-angular-s-q/
        return self._$q(function () { return null; });
    }

    //Utils.prototype.asyncTryCatch = function (func, successHandler, errorHandler) {
    //    try {
    //        return func().then(successHandler, errorHandler);
    //    } catch (e) {
    //        errorHandler(e);
    //    }
    //}

    Utils.$inject = ['$q'];

    angular
        .module('app.core')
        .service('utils', Utils);
})();