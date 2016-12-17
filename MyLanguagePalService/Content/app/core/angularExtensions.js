(function () {
    'use strict';

    // Depends on numberExtensions.js

    if (angular != null && typeof angular === 'object') {
        angular.isInteger = function (value) { return Number.isInteger(value); }
    }
})();