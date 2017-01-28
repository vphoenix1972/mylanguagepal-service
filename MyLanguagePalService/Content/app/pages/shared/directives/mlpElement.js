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

    /* Private */


    MlpElement.$inject = [];

    return MlpElement;
})();