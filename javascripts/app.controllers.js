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
		// Private functions...
		function successfulContentLoad(response) {
			$scope.entries = response.data; 
			console.log("Loaded content successfully.");
		}
		
		function failedContentLoad(response) {
			$scope.entries = [
				{
					"title": "Content failed to load",
					"content": "<p>Something went wrong, but never fear!  Reload the page or try again later.</p>"
				}
			];
			
			console.log("Response data: " + JSON.stringify(response));
		}
		
		// Public functions...
		$scope.parseEntry = function(entry) {
			return $sce.trustAsHtml(entry);
		}
		
		// On Load...
		var dataPromise = $http({
			method: 'GET',
			url: 'content/blog-entries.json',
			responseType: 'json'
		});
		
		dataPromise.then(successfulContentLoad, failedContentLoad);
	}
]);

appControllers.controller('WebplayerController', [
	function () {
	}
]);
