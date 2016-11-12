(function () {
    angular
        .module('app')
        .factory('HttpErrorType', ['CustomErrorType', function (CustomErrorType) {
            function HttpError(statusCode) {
                CustomErrorType.call(this, 'HttpError');
                this.name = 'HttpError';

                this.statusCode = statusCode;
            }

            HttpError.prototype = Object.create(CustomErrorType.prototype);
            HttpError.prototype.constructor = HttpError;

            return HttpError;
        }]);
})();