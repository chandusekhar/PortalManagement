app.controller('pm', function ($scope, $http) {
    var sp = $scope;

    sp.add = function () {
        $http.get(encodeURI(config.url + '/api/paymentapi/AddMoney?accountName=' + sp.accountName + '&amount=' + sp.price + '&type=' + $('#type').val() + '&reason=' + sp.detail)).
            then(function (response) {
                if (response.data == -1)
                    alert('Tên tài khoản không tồn tại')
                else if (response.data < 0) {
                    alert('Hệ thống bận');
                } else if (response.data > 0) {
                    alert('Cộng tiền thành công');
                    sp.getCurrentLog();
                    sp.accountName = '';
                    sp.price = '';
                    sp.detail = '';
                }
            })
    }

    sp.castType = function (type) {
        if (type == 1)
            return "Card";
        if (type == 2)
            return "Momo";
        if (type == 3)
            return "Đại lý";
        if (type == 4)
            return "Lỗi game";
    }

    sp.getCurrentLog = function () {
        $http.get(encodeURI(config.url + '/api/paymentapi/GetCurrentLog')).
            then(function (response) {
                sp.datatable = response.data;
            })
    }

    $scope.parseMoney = function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
        return strMoney;
    };
});