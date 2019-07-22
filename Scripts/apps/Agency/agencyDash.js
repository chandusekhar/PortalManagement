app.controller('agencyDash', function ($rootScope, $scope, $http) {
    var sp = $scope;

    sp.init = function () {
        $('#year').val(new Date().getFullYear());
        $('#month').val(new Date().getMonth() + 1);

        sp.getdata();
    }

    sp.getdata = function () {
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/agencyapi/GetDashboard?month=' + $('#month').val() + '&year=' + $('#year').val())).
            then(function (response) {
                sp.data1 = [];
                sp.data2 = [];

                for (var res in response.data) {
                    if (response.data[res].Level == 1)
                        sp.data1.push(response.data[res]);
                    else if (response.data[res].Level == 2)
                        sp.data2.push(response.data[res]);
                }

                sp.rank = sp.data1.concat(sp.data2);78
                sp.rank = sp.rank.sort(function (a, b) { return b.TotalGold - a.TotalGold; });

                Util.removeLoadingAnimate();
            });
    }

    $scope.parseMoney = function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
        return strMoney;
    };

    function rankAgency(a1, a2) {
        return a1.TotalGold - a2.TotalGold;
    }
});