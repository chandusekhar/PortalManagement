app.controller('pmhistory', function ($scope, $http) {
    var sp = $scope;

    sp.init = function () {
        sp.curPage = 0;
    }

    sp.do = function () {
        $http.get(encodeURI(config.url + '/api/paymentapi/GetLog?start=' + $('#start').val() + '&end=' + $('#end').val() + '&page=' + sp.curPage)).
            then(function (response) {
                sp.datatable = response.data;
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

    $scope.prev = function () {
        $scope.curPage--;
        if ($scope.curPage < 0)
            $scope.curPage = 0;

        sp.do();
    }

    $scope.next = function () {
        $scope.curPage++;
        sp.do();
    }

    $scope.parseMoney = function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
        return strMoney;
    };
});