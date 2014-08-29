'use strict';

var app = angular.module('app', [
	'ngRoute',
	'app.controllers'
]);

app.config(['$routeProvider', 
	function ($routeProvider) {
		$routeProvider.when('/home', {
			templateUrl: 'partials/home.html',
			controller: 'HomeController'
		});
		$routeProvider.when('/blog', {
			templateUrl: 'partials/blog.html',
			controller: 'BlogController'
		});
		$routeProvider.when('/webplayer', {
			templateUrl: 'partials/player.html',
			controller: 'WebplayerController'
		});
		
		$routeProvider.otherwise({
			redirectTo: '/home'
		});
	}
]);
