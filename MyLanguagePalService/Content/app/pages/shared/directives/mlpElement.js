MlpElement = (function () {
    'use strict';

    function MlpElement() {

    }

    MlpElement.prototype.isDisabled = function () {
        var self = this;

        // ReSharper disable PossiblyUnassignedProperty
        if (!angular.isObject(self.mlpDisabled))
            return false;

        return self.mlpDisabled.value();
        // ReSharper restore PossiblyUnassignedProperty
    }

    MlpElement.prototype.classes = function () {
        var self = this;

        return self.classesArray().join(' ');
    }

    MlpElement.prototype.classesArray = function () {
        var self = this;

        var result = [];

        // Add additional classes
        if (angular.isString(self.classes))
            result = result.concat(self.classes.split(' '));

        return result;
    }

    /* Private */


    MlpElement.$inject = [];

    return MlpElement;
})();