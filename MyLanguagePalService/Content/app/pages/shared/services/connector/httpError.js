HttpError = (function () {
    function HttpError(statusCode) {
        CustomError.call(this, 'HttpError');
        this.name = 'HttpError';

        this.statusCode = statusCode;
    }

    HttpError.prototype = Object.create(CustomError.prototype);
    HttpError.prototype.constructor = HttpError;

    return HttpError;
})();