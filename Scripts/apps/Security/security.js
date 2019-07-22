app.controller('security', function ($scope, $http) {
    var sp = $scope;

    sp.getRoles = function (id) {
        sp.cur = id;
        $http.get(encodeURI(config.url + '/api/SecurityApi/GetActiveRoles?user=' + id)).
            then(function (response) {
                sp.roles = response.data;
            })
    }

    sp.updateRole = function () {
        $http.post(encodeURI(config.url + '/api/SecurityApi/UpdateRole?user=' + sp.cur), sp.roles ).
            then(function (response) {
                if (response.data)
                    alert('Cập nhật thành công');
                else alert('Cập nhật thất bại');
            })
    }

    sp.getAccounts = function () {
        $http.get(encodeURI(config.url + '/api/SecurityApi/GetListUser')).
            then(function (response) {
                sp.accounts = response.data;
            })
    }

    sp.deleteAccount = function (x) {
        $http.get(encodeURI(config.url + '/api/SecurityApi/DeleteAccount?accountId=' + x)).
            then(function (response) {
                if (!response.data)
                    alert('Xóa không thành công');
                else sp.getAccounts();
            })
    }

    sp.addAccount = function () {
        if (!sp.username || !sp.password) {
            alert('Tài khoản và mật khẩu không được bỏ trống')
            return;
        }

        $http.get(encodeURI(config.url + '/api/SecurityApi/AddAccount?username=' + sp.username + '&password=' + sp.password)).
            then(function (response) {
                if (!response.data)
                    alert('Thêm không thành công');
                else sp.getAccounts();
            })
    }

    $scope.parseMoney = function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
        return strMoney;
    };
});