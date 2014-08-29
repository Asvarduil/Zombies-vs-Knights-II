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
				var entries = response.data;
				if(entries == null || entries.length == 0) {
					entries = [
						{
							"title": "No Entries Available Right Now!",
							"content": "Check back later for the latest ZvK2 news!"
						}
					];
				}
				
				$scope.entries = entries;
				console.log("Loaded content successfully.  Content: " + JSON.stringify(response));
			}
		
			function failedContentLoad(response) {
				$scope.entries = [
					{
						"title": "Content failed to load",
						"content": "<p>Something went wrong, but never fear!  Reload the page or try again later.</p>"
					}
				];
			
				console.log("An error occurred!  Response data: " + JSON.stringify(response));
			}
		
			// On Load...
			var dataPromise = $http({
				method: 'GET',
				url: 'content/blog-entries.json',
				responseType: 'text'
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
