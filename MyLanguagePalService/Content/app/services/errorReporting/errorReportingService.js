function ErrorReportingService() {

}

ErrorReportingService.prototype.reportError = function (error) {
    alert(error);
}

angular.module('app').service('errorReportingService', [
    ErrorReportingService
]);