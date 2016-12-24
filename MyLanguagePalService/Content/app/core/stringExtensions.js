(function () {
    'use strict';

    // Taken from https://developer.mozilla.org/ru/docs/Web/JavaScript/Reference/Global_Objects/String/startsWith
    if (!String.prototype.startsWith) {
        Object.defineProperty(String.prototype, 'startsWith', {
            enumerable: false,
            configurable: false,
            writable: false,
            value: function (searchString, position) {
                position = position || 0;
                return this.lastIndexOf(searchString, position) === position;
            }
        });
    }

    // Taken from https://developer.mozilla.org/ru/docs/Web/JavaScript/Reference/Global_Objects/String/endsWith
    if (!String.prototype.endsWith) {
        Object.defineProperty(String.prototype, 'endsWith', {
            value: function (searchString, position) {
                var subjectString = this.toString();
                if (position === undefined || position > subjectString.length) {
                    position = subjectString.length;
                }
                position -= searchString.length;
                var lastIndex = subjectString.indexOf(searchString, position);
                return lastIndex !== -1 && lastIndex === position;
            }
        });
    }

    if (!String.prototype.getBetween) {
        String.prototype.getBetween = function (sub1, sub2) {
            /// <summary>
            /// Gets the first substring between sub1 and sub2. Returns null if no substring was found.
            /// </summary>

            var self = this;

            var indexSub1 = self.indexOf(sub1);
            var indexSub2 = self.indexOf(sub2);

            if (indexSub1 < 0 || indexSub2 < 0 ||
                indexSub1 >= indexSub2)
                return null;

            var start = indexSub1 + sub1.length;
            var end = indexSub2;

            return self.substring(start, end);
        }
    }

    if (!String.prototype.splitAndTrim) {
        String.prototype.splitAndTrim = function (separator, skipEmptyParts, limit) {
            /// <summary>
            /// Splits and trims the string using separator and skips empty parts if nesessary.
            /// </summary>

            var self = this;

            if (skipEmptyParts == null)
                skipEmptyParts = true;

            var result = self.split(separator, limit).map(function (part) { return part.trim(); });

            if (skipEmptyParts)
                result = result.filter(function (str) { return str.length > 0; });

            return result;
        }
    }
})();