var jpc = angular.module('JoulsePowerClient', [ 'ngRoute' ])
				 .run(function ($rootScope) {
			 			$rootScope.baseUri = 'http://localhost:52242/api';
				 });

jpc.config(function ($routeProvider) {
	$routeProvider
		.when('/login', {
			templateUrl: 'partials/login.html',
			controller: 'LoginController'
		})
		.when('/contacts/add', {
			templateUrl: 'partials/add-contact.html',
			controller: 'AddContactController'
		})
		.when('/contacts/:contactId', {
			templateUrl: 'partials/contact.html',
			controller: 'ContactController'
		})
		.when('/contacts', {
			templateUrl: 'partials/contacts.html',
			controller: 'ContactsController'
		});
});

jpc.controller('LoginController', [ '$scope', '$rootScope', '$location', 'Server', function ($scope, $rootScope, $location, Server) {
	$scope.showTextArea = false;

	$scope.login = function(user) {
		var jsonData =  'username=' + user.username + '&password=' + user.password;

		Server.post($rootScope.baseUri + '/token', jsonData, null, true)
		.then(function (response) {
			localStorage.setItem('token', response.data.token);
			$location.path('/contacts');
		}, function (response) {
			$scope.showTextArea = true;
			$scope.console = JSON.stringify(response.data);
		});
	}
}]);

jpc.controller('ContactsController', [ '$scope', '$rootScope', '$location', 'Server', function($scope, $rootScope, $location, Server) {
	$scope.switchView = function(viewName) {
		$location.path('/' + viewName);
	};

	var token = localStorage.getItem('token');

	Server.get($rootScope.baseUri + '/contacts', token)
	.then(function(response) {
		$scope.contacts = response.data;
		$scope.console = 'token: ' + token;
	}, function(response) {
		$scope.console = JSON.stringify(response.data);
	});
}]);

jpc.controller('ContactController', [ '$scope', '$routeParams', '$rootScope', '$location', 'Server', function ($scope, $routeParams, $rootScope, $location, Server) {
	$scope.switchView = function(viewName) {
		$location.path('/' + viewName);
	};

	var token = localStorage.getItem('token');

	Server.get($rootScope.baseUri + '/contacts/' + $routeParams.contactId, token)
	.then(function(response) {
		$scope.contact = response.data;
		$scope.console = 'token: ' + token;
	}, function(response) {
		$scope.console = JSON.stringify(response.data);
	});
}]);

jpc.controller('AddContactController', [ '$scope', '$rootScope', '$location', 'Server', function ($scope, $rootScope, $location, Server) {
	$scope.switchView = function(viewName) {
		$location.path('/' + viewName);
	};

	$scope.emptyFields = false;

	$scope.validate = function (newContact) {
		if (newContact.LastName.length > 0 && newContact.FirstName.length > 0 && newContact.EmailAddress.length > 3) {
			var token = localStorage.getItem('token');

			console.log(JSON.stringify(newContact));

			Server.post($rootScope.baseUri + '/contacts', JSON.stringify(newContact), token)
				.then(function (response) {
					$scope.switchView('contacts');
				}, function (response) {
					$scope.console = JSON.stringify(response.data);
				}
			);
		} else {
			$scope.emptyFields = true;
		}
	}
}]);

jpc.factory('Server', [ '$http', function ($http) {
  	return {
	    get: function(url, token) {
	      	return $http({ method: 'GET', url: url, headers: { 'X-AUTH': token } });
	    },
	    post: function(url, data, token, isForm) {
	    	var contentType = isForm != undefined && isForm === true ? 'application/x-www-form-urlencoded' : 'application/json';
	      	return $http({ method: 'POST', url: url, data: data, headers: { 'Content-Type': contentType, 'X-AUTH': token } });
	    }
  	};
}]);