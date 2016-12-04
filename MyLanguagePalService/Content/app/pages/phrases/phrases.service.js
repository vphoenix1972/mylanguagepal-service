(function () {
    function PhrasesService($q, utils, connector) {
        var self = this;

        self._$q = $q;
        self._utils = utils;
        self._connector = connector;
    }

    PhrasesService.prototype.getPhrases = function () {
        return this._connector.getPhrases();
    }

    PhrasesService.prototype.getPhrase = function (id) {
        id = this._utils.parseIntOrThrow(id, 'id');

        return this._connector.getPhrase(id);
    }

    PhrasesService.prototype.createPhrase = function (phrase) {
        return this._connector.createPhrase(phrase);
    }

    PhrasesService.prototype.updatePhrase = function (id, phrase) {
        id = this._utils.parseIntOrThrow(id, 'id');

        return this._connector.updatePhrase(id, phrase);
    }

    PhrasesService.prototype.deletePhrase = function (id) {
        id = this._utils.parseIntOrThrow(id, 'id');

        return this._connector.deletePhrase(id);
    }

    PhrasesService.prototype.getPhraseDetails = function (id) {
        var self = this;

        return self._$q.when(self._connector.getPhrase(id)).then(function (phraseResult) {
            return self._$q.when(self._connector.getTranslations(phraseResult.data.id)).then(function (translationsResult) {
                var result = phraseResult.data;
                result.translations = translationsResult.data;

                var promises = [];
                result.translations.forEach(function (translation) {
                    promises.add(self._$q.when(self._connector.getTranslations(translation.id)));
                });

                return self._$q.all(promises).then(function (synonimsResults) {
                    for (var i = 0; i < synonimsResults.length; i++) {
                        var synonimResult = synonimsResults[i];

                        result.translations[i].synonims = synonimResult.data;
                    }

                    return result;
                });
            });
        });
    }

    PhrasesService.prototype.translationsComparerByPrevalence = function (ts1, ts2) {
        return ts2.prevalence - ts1.prevalence;
    }

    /* Private */


    PhrasesService.$inject = [
        '$q',
        'utils',
        'connectorService'
    ];

    angular
        .module('app')
        .service('phrasesService', PhrasesService);
})();