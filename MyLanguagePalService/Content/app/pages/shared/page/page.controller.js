PageController = (function () {
    function PageController($q, $location, $scope, $routeParams, utils, errorReportingService, progressBarService) {
        var self = this;

        self.$q = $q;
        self.$location = $location;
        self.$scope = $scope;
        self.$routeParams = $routeParams;
        self.utils = utils;
        self.errorReportingService = errorReportingService;
        self.progressBarService = progressBarService;

        self._userHasLeftThePage = false;

        self.isLoading = true;

        // Subscribe to route change event
        $scope.$on('$routeChangeStart', function () {
            self._userHasLeftThePage = true;

            // Stop progress bar loading if any
            self.progressBarService.reset();
        });


        self.asyncRequest = function (options) {
            /// <summary>
            /// Asynchronously runs the request.
            /// Handles progress bar starting / completition and error reporting.
            /// <param name="options">
            /// options: {
            ///     request: function () { /* Request */ },
            ///     success: function ([arguments]) { /* Success callback */ },
            ///     error: function ([arguments]) { /* Error callback */ }
            /// }
            /// </param>
            /// </summary>



            /* Parameters check */
            if (angular.isUndefined(options))
                throw new Error('Argument "options" must be defined');
            if (!angular.isFunction(options.request))
                throw new Error('Argument "options.request" must be a function');

            /* Run request */
            self.progressBarService.start();

            options.request().then(function () {
                if (self._userHasLeftThePage)
                    return; // Do not process the response

                // Complete progress bar
                self.progressBarService.complete();

                // Pass the response to caller
                if (angular.isFunction(options.success)) {
                    options.success.apply(self, arguments);
                }
            }, function () {
                // Report about the error
                self.errorReportingService.reportError.apply(self.errorReportingService, arguments);

                if (self._userHasLeftThePage)
                    return;

                // Complete progress bar
                self.progressBarService.complete();

                // Pass the error to caller
                if (angular.isFunction(options.error)) {
                    options.error.apply(self, arguments);
                }
            });
        }
    }

    PageController.prototype.doAsync = function (promise) {
        var self = this;

        /* Run request */
        self.progressBarService.start();

        return self.$q.when(promise).then(function (result) {
            if (self._userHasLeftThePage) {
                return self.utils.cancelPromiseChaining();
            }

            // Complete progress bar
            self.progressBarService.complete();

            // Continue handling
            return self.$q.resolve(result);
        }, function (result) {
            // Report about the error
            self.errorReportingService.reportError(result);

            if (self._userHasLeftThePage)
                return self.utils.cancelPromiseChaining();

            // Complete progress bar
            self.progressBarService.complete();

            // Continue handling
            return self.$q.reject(result);
        });
    }

    PageController.prototype.validationFailed = function (result) {
        var self = this;

        if (result instanceof ValidationConnectorResult) {
            self.bindValidationErrorResult(result);
            return true;
        }

        return false;
    }

    PageController.prototype.bindValidationErrorResult = function (result) {
        var self = this;

        // Reset validation errors
        angular.forEach(self, function (value, key) {
            if (!key.endsWith('ValidationError'))
                return;

            delete self[key];
        });

        // Set new errors
        angular.forEach(result.validationState, function (value, key) {
            self[key + 'ValidationError'] = value.join();
        });
    }

    PageController.$inject = ['$q', '$location', '$scope', '$routeParams', 'utils', 'errorReportingService', 'progressBarService'];

    return PageController;
})();