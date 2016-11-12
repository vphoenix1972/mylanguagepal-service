(function () {
    angular
        .module('app')
        .factory('NetworkErrorType', ['CustomErrorType', function (CustomErrorType) {
            function NetworkError() {
                CustomErrorType.call(this, 'NetworkError');
                this.name = 'NetworkError';
            }

            NetworkError.prototype = Object.create(CustomErrorType.prototype);
            NetworkError.prototype.constructor = NetworkError;

            return NetworkError;
        }]);
})();