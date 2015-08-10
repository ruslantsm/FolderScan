(function () {

	'use strict';

	angular.module('folderScanApp').factory('dataService', ['$http', 'configuration', function ($http, configuration) {
		var dataService = {};

		dataService.get = function (resource) {
			return $http({ method: 'GET', url: configuration.API_URL + resource });
		}

		dataService.getFiles = function () {
			return dataService.get('files');
		};

		dataService.getWords = function () {
			return dataService.get('words');
		};

		dataService.getOccurences = function (wordId) {
			return dataService.get('occurences/' + wordId);
		};

		return dataService;
	}]);

})();