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
	'$scope', '$http', '$sce', '$interval',
	function ($scope, $http, $sce, $interval) {
		function loadContent() {
			function successfulContentLoad(response) {
				$scope.entries = response.data; 
				console.log("Loaded content successfully.");
				console.log("Response: " + JSON.stringify(response.data));
			}
		
			function failedContentLoad(response) {
				$scope.entries = [
					{
						"title": "Content failed to load",
						"content": "<p>Something went wrong, but never fear!  Reload the page or try again later.</p>"
					}
				];
			
				console.log("Response data: " + JSON.stringify(response.data));
			}
		
			// On Load...
			var dataPromise = $http({
				method: 'GET',
				url: 'content/blog-entries.json',
				responseType: 'json'
			});
		
			dataPromise.then(successfulContentLoad, failedContentLoad);
		}
		
		
		
		// Public functions...
		$scope.parseEntry = function(entry) {
			return $sce.trustAsHtml(entry);
		}
		
		// On Load...
		loadContent();
		
		$interval(loadContent, 60000);
	}
]);

appControllers.controller('WebplayerController', [
	function () {
	}
]);
