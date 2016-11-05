function Utils() {

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