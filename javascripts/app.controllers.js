'use strict';

var appControllers = angular.module('app.controllers', [
	'ngSanitize',
	'ui.bootstrap'
]);

appControllers.controller('HomeController', [
	function () {
	}
]);

appControllers.controller('BlogController', [
	'$scope', '$http', '$sce',
	function ($scope, $http, $sce) {
		var dataPromise = $http({
			method: 'GET',
			url: 'content/blog-entries.json',
			responseType: 'text'
		});
		
		dataPromise.then(function(result){
			$scope.entries = result.data; 
		});
		
		$scope.parseEntry = function(entry) {
			return $sce.trustAsHtml(entry);
		}
	}
]);

appControllers.controller('WebplayerController', [
	function () {
	}
]);
