'use strict';

var appControllers = angular.module('app.controllers', [
	'ui.bootstrap',
	'ngSanitize'
]);

appControllers.controller('HomeController', [
	function () {
	}
]);

appControllers.controller('BlogController', ['$scope', '$http',
	function ($scope, $http) {
		var dataPromise = $http({
			method: 'GET',
			url: 'content/blog-entries.json',
			responseType: 'text'
		});
		
		dataPromise.then(function(result){
			$scope.entries = result.data;                
		});
	}
]);

appControllers.controller('WebplayerController', [
	function () {
	}
]);
