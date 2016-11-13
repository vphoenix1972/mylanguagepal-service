PageController = (function () {
    function PageController($scope, errorReportingService, progressBarService) {
        var self = this;

        self._$scope = $scope;
        self._errorReportingService = errorReportingService;
        self._progressBarService = progressBarService;

        self._userHasLeftThePage = false;

        self._$scope.isLoading = true;

        // Subscribe to route change event
        $scope.$on('$routeChangeStart', function () {
            self._userHasLeftThePage = true;

            // Stop progress bar loading if any
            self._progressBarService.reset();
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
            self._progressBarService.start();

            options.request().then(function () {
                if (self._userHasLeftThePage)
                    return; // Do not process the response

                // Complete progress bar
                self._progressBarService.complete();

                // Pass the response to caller
                if (angular.isFunction(options.success)) {
                    options.success.apply(self, arguments);
                }
            }, function () {
                // Report about the error
                self._errorReportingService.reportError.apply(self._errorReportingService, arguments);

                if (self._userHasLeftThePage)
                    return;

                // Complete progress bar
                self._progressBarService.complete();

                // Pass the error to caller
                if (angular.isFunction(options.error)) {
                    options.error.apply(self, arguments);
                }
            });
        }
    }

    return PageController;
})();