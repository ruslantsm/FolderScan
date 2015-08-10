(function () {
	'use strict';

	var app = angular.module('folderScanApp', ['ngRoute', 'ngTable', 'angular-loading-bar']);

	function Config($routeProvider, $locationProvider, cfpLoadingBarProvider) {
		cfpLoadingBarProvider.includeSpinner = true;

		$routeProvider.when('/', { templateUrl: '../app/partials/files.html', controller: 'filesCtrl' });
		$routeProvider.when('/app/files', { templateUrl: '../app/partials/files.html', controller: 'filesCtrl' });
		$routeProvider.when('/app/words', { templateUrl: '../app/partials/words.html', controller: 'wordsCtrl' });
		$routeProvider.otherwise({ redirectTo: '/' });
	}

	app.config(['$routeProvider', '$locationProvider', 'cfpLoadingBarProvider', Config]);
	app.constant('configuration', {
		API_URL: 'http://localhost:8261/api/'
	});

})();
