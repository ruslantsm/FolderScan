(function () {

	'use strict';
	angular.module('folderScanApp').controller('filesCtrl', ['$scope', '$filter', 'ngTableParams', 'dataService', filesCtrl]);

	function filesCtrl($scope, $filter, ngTableParams, dataService) {

		$scope.files = [];

		$scope: { $data: { } }
		$scope.tableParams = new ngTableParams({
			page: 1,
			count: 25,
			sorting: { Name: 'asc' }
		}, {
			total: $scope.files.length, // length of data
			getData: function ($defer, params) {
				var filteredData = params.filter() ? $filter('filter')($scope.files, params.filter()) : $scope.files;
				var orderedData = params.sorting() ? $filter('orderBy')(filteredData, params.orderBy()) : $scope.files;

				params.total(orderedData.length); // set total for recalc pagination
				$defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
			}
		});

		$scope.getFiles = function () {
			dataService.getFiles()
				.success(function (data) {
					$scope.files = data.data.Files;
					$scope.tableParams.reload();
				});
		};

		$scope.getFiles();

	};

})();