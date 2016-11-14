(function () {
    function ConfigService() {
        var self = this;

        self.antiForgeryToken = angular.element('input[name=__RequestVerificationToken]').val();
    }

    angular.module('app').service('config', [
        ConfigService
    ]);
})();