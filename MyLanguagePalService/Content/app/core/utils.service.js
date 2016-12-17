(function () {
    function Utils($q) {
        var self = this;

        self._$q = $q;
    }

    Utils.prototype.isFinite = function (value) {
        return typeof value === 'number' && isFinite(value);
    }

    Utils.prototype.isInteger = function (value) {
        var self = this;

        return typeof value === 'number'
           && self.isFinite(value)
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

    Utils.prototype.getRandomInt = function (max, min) {
        /// <summary>
        /// Returns a random integer between min (included) and max (excluded). min = 0 by default.
        /// </summary>

        if (angular.isUndefined(min))
            min = 0;

        min = Math.ceil(min);
        max = Math.floor(max);

        if (min > max)
            throw new Error('min cannot be more than max, min = ' + min + ', max = ' + max);

        return Math.floor(Math.random() * (max - min)) + min;
    }

    Utils.prototype.getRandomIntExcluding = function (max, exclusions, min) {
        /// <summary>
        /// Returns a random integer between min (included) and max (excluded). min = 0 by default.
        /// Checks that returned number is not contained in exclusions array.
        /// </summary>

        if (!angular.isArray(exclusions))
            throw new Error('Argument "exclusions" must be an array.');

        if (angular.isUndefined(min))
            min = 0;

        min = Math.ceil(min);
        max = Math.floor(max);

        if (min > max)
            throw new Error('min cannot be more than max, min = ' + min + ', max = ' + max);

        // Prevent infinite cycle
        var maxAttemptsCount = 100;
        var attemptsCount = 0;

        var result;
        do {
            result = Math.floor(Math.random() * (max - min)) + min;
            attemptsCount++;
        } while (exclusions.contains(result) && attemptsCount < maxAttemptsCount)

        if (attemptsCount >= maxAttemptsCount)
            throw new Error('Cannot get random integer which is not present in exclusion list,' +
                ' min = ' + min + ', max = ' + max + ', exclusions = ' + exclusions);

        return result;
    }

    Utils.prototype.getRandomBool = function () {
        /// <summary>
        /// Returns a random bool.
        /// </summary>

        return Math.random() > 0.5;
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