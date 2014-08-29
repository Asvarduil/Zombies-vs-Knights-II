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
		}
		
		function failedContentLoad(response) {
			$scope.entries = [
				{
					"title": "Content failed to load",
					"content": "Something went wrong, but never fear!  Reload the page or try again later."
					           + "Reponse Blob: " + response
				}
			];
		}
		
		// Public functions...
		$scope.parseEntry = function(entry) {
			return $sce.trustAsHtml(entry);
		}
		
		// On Load...
		var dataPromise = $http({
			method: 'GET',
			url: 'content/blog-entries.json',
			responseType: 'text'
		});
		
		dataPromise.then(successfulContentLoad, failedContentLoad);
	}
]);

appControllers.controller('WebplayerController', [
	function () {
	}
]);
