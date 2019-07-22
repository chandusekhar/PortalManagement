app.controller('vipManagement', function ($scope, $http) {
    var sp = $scope;

    $scope.accountname = '';
    $scope.username = '';
    $scope.accountid = '';
    $scope.phonenumber = '';

    $scope.init = function () {
    }

    $scope.search = function () {
        $http.get(encodeURI(config.url + '/api/userinfoapi/search?displayname=' + $scope.accountname + '&username=' + $scope.username + '&id=' + $scope.accountid + '&phone=' + $scope.phonenumber)).
            then(function (response) {
                $scope.result = response.data;
                sp.currentRank = -1;
            })
    }

    $scope.getVIPInfo = function (x) {
        $http.get(encodeURI(config.url + '/api/userinfoapi/GetVIPInfo?accountId=' + x.AccountID)).
            then(function (response) {
                sp.currentRank = 0;
                sp.vipInfo = response.data;

                if (sp.vipInfo.length) {
                    sp.currentRank = Math.max.apply(Math, sp.vipInfo.map(function (o) { return o.Rank; }));
                }
            });
    }
});