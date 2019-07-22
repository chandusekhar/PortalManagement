app.controller('cardconfig', function ($scope, $http) {
    var sp = $scope;

    sp.init = function () {
        sp.getCards();
    }

    sp.getCards = function () {
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/cardconfigapi/GetCards')).
            then(function (response) {
                sp.datatable = response.data;
                $.each(sp.datatable, function (i) {
                    var d = sp.datatable[i];
                    switch (d.Type) {
                        case 1:
                            d.TypeStr = "Viettel";
                            break;
                        case 2:
                            d.TypeStr = "Mobi";
                            break;
                        case 3:
                            d.TypeStr = "Vina";
                            break;
                        case 4:
                            d.TypeStr = "Zing";
                            break;
                    }
                });
                Util.removeLoadingAnimate();
            });
    }

    sp.updateConfig = function () {
        Util.addLoadingAnimate();
        $http.post(encodeURI(config.url + '/api/cardconfigapi/Update'), sp.datatable).then(function (response) {
            if (!response.data) {
                alert('cập nhật không thành công');
                Util.removeLoadingAnimate();
            }
            else {
                sp.getCards();
                alert('cập nhật thành công');
            }
        });
    }

    $scope.parseMoney = function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        return strMoney;
    };
});