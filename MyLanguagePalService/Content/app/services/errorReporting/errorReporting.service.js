(function () {
    function ErrorReportingService() {

    }

    ErrorReportingService.prototype.reportError = function (error) {
        if (error instanceof NetworkError) {
            alert('Network error');
            return;
        }

        if (error instanceof HttpError) {
            alert('Http error');
            return;
        }

        if (error instanceof Error) {
            alert('Unknown error');
            return;
        }

        alert(error);
    }

    angular.module('app').service('errorReportingService', [
        ErrorReportingService
    ]);
})();