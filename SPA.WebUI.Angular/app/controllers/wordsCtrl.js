(function () {

	'use strict';
	angular.module('folderScanApp').controller('wordsCtrl', ['$scope', '$filter', '$q', 'ngTableParams', 'dataService', wordsCtrl]);

	function wordsCtrl($scope, $filter, $q, ngTableParams, dataService) {

		$scope.files = [];
		$scope.words = [];
		$scope.occurences = [];
		$scope.groups = [];

		$scope: { $data: { } }
		$scope.tableParams = new ngTableParams({
			page: 1,
			count: 10,
			sorting: { Name: 'asc' }
		}, {
			counts: [],
			total: $scope.words.length, // length of data
			getData: function ($defer, params) {
				var filteredData = params.filter() ? $filter('filter')($scope.words, params.filter()) : $scope.words;
				var orderedData = params.sorting() ? $filter('orderBy')(filteredData, params.orderBy()) : $scope.words;

				params.total(orderedData.length); // set total for recalc pagination
				$defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
			}
		});

		$scope.occurencesTableParams = new ngTableParams({
			page: 1,
			count: 10,
			sorting: { FileName: 'asc' }
		}, {
			counts: [],
			total: $scope.occurences.length, // length of data
			getData: function ($defer, params) {
				var filteredData = params.filter() ? $filter('filter')($scope.occurences, params.filter()) : $scope.occurences;
				var orderedData = params.sorting() ? $filter('orderBy')(filteredData, params.orderBy()) : $scope.occurences;

				params.total(orderedData.length); // set total for recalc pagination
				$defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
			}
		});

		$scope.groupTableParams = new ngTableParams({
			page: 1,
			count: 10,
			sorting: { FileName: 'asc' }
		}, {
			counts: [],
			total: $scope.groups.length, // length of data
			getData: function ($defer, params) {
				var filteredData = params.filter() ? $filter('filter')($scope.groups, params.filter()) : $scope.groups;
				var orderedData = params.sorting() ? $filter('orderBy')(filteredData, params.orderBy()) : $scope.groups;

				params.total(orderedData.length); // set total for recalc pagination
				$defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
			}
		});

		$scope.getWords = function () {
			dataService.getWords()
				.success(function (data) {
					$scope.words = data.data.Words;
					$scope.tableParams.reload();
				});
		};

		var getFilesDeferred = $q.defer();
		$scope.getFiles = function () {
			dataService.getFiles()
				.success(function (data) {
					$scope.files = data.data.Files;
					getFilesDeferred.resolve();
				});
		};

		$scope.getFiles();
		$q.all([getFilesDeferred.promise]).then(function () {
			$scope.getWords();
		});

		$scope.select = function (word) {
			dataService.getOccurences(word.Name)
				.success(function (data) {
					$scope.occurences = $.map(data.data.Occurences, function (value, index) {
						return { FileName: $scope.getFileName(value.FId), Line : value.Line, Index : value.Index }
					});

					var hist = {};
					$scope.occurences.map(function (item) {
						if (item.FileName in hist) hist[item.FileName]++; else hist[item.FileName] = 1;
					});
					$scope.groups = $.map(hist, function (value, index) {
						return { FileName: index, Count: value }
					});

					$scope.occurencesTableParams.$params.page = 1;
					$scope.occurencesTableParams.reload();
					$scope.groupTableParams.$params.page = 1;
					$scope.groupTableParams.reload();
				});
		};

		$scope.getFileName = function (id) {
			for (var i = 0; i < $scope.files.length; i++) {
				var file = $scope.files[i];
				if (file.Id == id)
					return file.Name;
			}
		};

	};

})();