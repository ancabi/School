angular.module('school').factory('redirectInterceptor', ['$q', '$location', '$window', function ($q, $location, $window) {
    return {
        'response': function (response) {
            if (typeof response.data === 'string' && response.data.indexOf("Login") > -1) {
                console.log("LOGIN!!");
                console.log(response.data);
                $window.location.href = webroot + "Account/Login";
                return $q.reject(response);
            } else {
                return response;
            }
        }
    }
}]).config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('redirectInterceptor');
}]);