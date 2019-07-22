app.controller('agencyAuthorize', function ($scope, $http) {
    var sp = $scope;

    sp.init = function () {
        sp.displayName = '';
        sp.tel = '';
        sp.fb = '';
        sp.tele = '';
        sp.address = '';
        sp.gameName = '';

        $scope.accountname = '';
        $scope.username = '';
        $scope.accountid = '';
        $scope.phonenumber = '';
    }


    $scope.search = function () {
        $http.get(encodeURI(config.url + '/api/userinfoapi/search?displayname=' + $scope.accountname + '&username=' + $scope.username + '&id=' + $scope.accountid + '&phone=' + $scope.phonenumber)).
            then(function (response) {
                $scope.result = response.data;
            })
    }

    sp.openAdd = function () {
        $('#confirmModal').modal('show');
    }

    sp.authorize = function () {
        Util.addLoadingAnimate();
        $('#confirmModal').modal('hide');
        $http.get(encodeURI(config.url + '/api/AgencyApi/AuthorizeAgency?gameAccountId=' + sp.selected.AccountID + '&display=' + sp.selected.DisplayName + '&tel=' + $('#agencyTel').val() + '&fb=' + sp.fb + '&tele=' + sp.tele + '&address=' + sp.address + '&gameName=' + sp.selected.DisplayName)).
            then(function (response) {
                if (response.data == false) {
                    alert('Thêm đại lý không thành công');
                }
                else {
                    alert("Ủy quyền thành công !");
                    delete sp.selected;
                }
                Util.removeLoadingAnimate();
            })
    }

    sp.select = function (x) {
        sp.selected = x;
    }
});