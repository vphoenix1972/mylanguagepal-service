CustomError = (function () {
    function CustomError(message) {
        this.name = 'CustomError';
        this.message = message;

        if (Error.captureStackTrace) {
            Error.captureStackTrace(this, this.constructor);
        } else {
            this.stack = (new Error()).stack;
        }

    }

    CustomError.prototype = Object.create(Error.prototype);
    CustomError.prototype.constructor = CustomError;

    return CustomError;
})();