(function () {
    'use strict';

    Array.prototype.any = function (predicate) {
        /// <summary>
        /// Determines whether any element of a sequence satisfies a condition.
        /// </summary>

        var self = this;

        if (angular.isFunction(predicate)) {
            for (var i = 0; i < self.length; i++) {
                var item = self[i];
                if (predicate(item))
                    return true;
            }

            return false;
        }

        return self.length > 0;
    };

    Array.prototype.contains = function (searchElement/*, fromIndex*/) {
        /// <summary>
        /// Determines whether an array includes a certain element, returning true or false as appropriate.
        /// </summary>

        var o = Object(this);
        var len = parseInt(o.length) || 0;
        if (len === 0) {
            return false;
        }
        var n = parseInt(arguments[1]) || 0;
        var k;
        if (n >= 0) {
            k = n;
        } else {
            k = len + n;
            if (k < 0) {
                k = 0;
            }
        }
        while (k < len) {
            var currentElement = o[k];
            if (searchElement === currentElement ||
                // Code taken from https://developer.mozilla.org/ru/docs/Web/JavaScript/Reference/Global_Objects/Array/includes
                // ReSharper disable SimilarExpressionsComparison
                (searchElement !== searchElement && currentElement !== currentElement)
                // ReSharper restore SimilarExpressionsComparison
            ) {
                return true;
            }
            k++;
        }
        return false;
    };

    Array.prototype.add = function (value) {
        /// <summary>
        /// Adds a value to the array.
        /// </summary>

        var self = this;

        self.push(value);
    }

    Array.prototype.remove = function (value) {
        /// <summary>
        /// Removes the value from the array if value can be found.
        /// </summary>

        var self = this;

        var index = self.indexOf(value);
        if (index < 0)
            return;

        self.splice(index, 1);
    }

    Array.prototype.orderBy = function (comparer) {
        /// <summary>
        /// The orderBy() method orders the elements of an array using comparer and returns the array.        
        /// </summary>        

        return this.sort(comparer);
    };

    Array.prototype.merge = function (newArray, options) {
        /// <summary>
        /// Merges array with another array and returns new array. Does not modify given arrays.
        /// </summary>
        var self = this;

        /* Check arguments */
        if (!angular.isArray(newArray))
            throw new Error('Argument "newArray" must be an array');

        var defaultOptions = {
            predicate: function (newItem, oldItem) { return newItem === oldItem; },
            // ReSharper disable once UnusedParameter
            merge: function (newItem, oldItem) { return newItem; },
            createNew: function (newItem) { return newItem; },
            onRemove: undefined
        }

        var currentOptions = defaultOptions;
        if (options != null) {
            if (angular.isFunction(options.predicate))
                currentOptions.predicate = options.predicate;

            if (angular.isFunction(options.merge))
                currentOptions.merge = options.merge;

            if (angular.isFunction(options.createNew))
                currentOptions.createNew = options.createNew;

            if (angular.isFunction(options.onRemove))
                currentOptions.onRemove = options.onRemove;
        }

        /* Merge */
        var result = [];

        // Determine removed items
        if (angular.isFunction(currentOptions.onRemove)) {
            self.forEach(function (oldItem) {
                var newItem = newArray.find(function (newIt) { return currentOptions.predicate(newIt, oldItem); });
                if (newItem != null)
                    return; // Item still exists in new array

                currentOptions.onRemove(oldItem);
            });
        }

        // Create merge result
        newArray.forEach(function (newItem) {
            var oldItem = self.find(function (oldIt) { return currentOptions.predicate(newItem, oldIt); });
            if (oldItem != null) {
                result.push(currentOptions.merge(newItem, oldItem));
                return;
            }

            result.push(currentOptions.createNew(newItem));
        });

        return result;
    }
})();