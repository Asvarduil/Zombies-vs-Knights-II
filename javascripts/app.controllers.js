'use strict';

var appControllers = angular.module('app.controllers', []);

appControllers.controller('HomeController', [
	function () {
	}
]);

appControllers.controller('BlogController', ['$scope', '$http',
	function ($scope, $http) {
		var dataPromise = $http.get('~/content/blog-entries.json');
		
		dataPromise.then(function(result){
			$scope.entries = result.data;                
		});
	}
]);

appControllers.controller('WebplayerController', [
	function () {
	}
]);