(function () {
    'use strict';

    // Polyfills taken from
    // https://developer.mozilla.org/ru/docs/Web/JavaScript/Reference/Global_Objects/Number/isInteger

    Number.isFinite = Number.isFinite || function (value) {
        return typeof value === 'number' && isFinite(value);
    }

    Number.isInteger = Number.isInteger || function (value) {
        return typeof value === 'number'
               && Number.isFinite(value)
               && !(value % 1);
    };
})();