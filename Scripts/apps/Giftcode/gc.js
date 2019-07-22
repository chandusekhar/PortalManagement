app.controller('gc', function ($scope, $http) {
    var sp = $scope;

    sp.init = function () {
        sp.getMinusGameFund();
        sp.getEventInfo();
        sp.getAllAgencies();
    }

    sp.getAllAgencies = function () {
        $http.get(encodeURI(config.url + '/api/AgencyApi/GetAllAgencies')).
            then(function (response) {
                if (response.data)
                    sp.agencies = response.data;
                else
                    sp.agencies = [];
            });
    }

    sp.getEventInfo = function () {
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/giftcodeapi/GetEventInfo')).
            then(function (response) {
                sp.events = response.data;
                if (sp.events == null)
                    sp.events = [];

                Util.removeLoadingAnimate();
            });
    }

    sp.gen = function () {
        if (sp.agencies.length && sp.type == 1) {
            var agencyId = $('#agencyId').val();
            if (!agencyId) {
                alert("Chọn đại lý !");
                return;
            }            
        }

        if (!sp.type) {
            alert("Chọn loại Giftcode !");
            return;
        }

        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/giftcodeapi/Gen?total=' + sp.total + '&prefix=' + sp.prefix + '&name=' + sp.name + '&price=' + sp.price + '&expired=' + $('#date').val() + '&type=' + sp.type + '&isUseFund=' + sp.isUseFund + '&agencyId=' + sp.agencyId)).
            then(function (response) {
                if (response.data > 0) {
                    sp.events = [{
                        ID: response.data,
                        Name: sp.name,
                        Price: sp.price,
                        Total: sp.total,
                        Used: 0
                    }].concat(sp.events);
                    sp.getMinusGameFund();
                }
                else {
                    if (response.data == -3)
                        alert("Quỹ không đủ !");

                    Util.removeLoadingAnimate();
                }
            });
    }

    sp.getMinusGameFund = function () {
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/GameApi/GetMinusGameFund')).
            then(function (response) {
                sp.minusGameFund = response.data[0].Total;
                Util.removeLoadingAnimate();
            });
    }

    sp.search = function () {
        if (!$scope.gCode) {
            alert("Nhập giftcode !");
            return;
        }
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/giftcodeapi/Search?code=' + $scope.gCode)).
            then(function (response) {
                sp.result = response.data;
                Util.removeLoadingAnimate();
            });
    }

    sp.selectGiftcode = function (x) {
        $scope.selected = x;
    }

    sp.downloadURI = function (uri, name) {
        var link = document.createElement("a");
        link.download = name;
        link.href = uri;
        link.click();
    }

    sp.dl = function (eventId) {
        sp.downloadURI(encodeURI(config.url + "/Giftcode/Download?eventId=" + eventId));
    }

    $scope.parseMoney = function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        return strMoney;
    };
});