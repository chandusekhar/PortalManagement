app.controller('jackpot', function ($scope, $http) {
    var sp = $scope;

    sp.getJackpot = function () {
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/GameApi/GetListJackpot?start=' + $('#start').val() + '&end=' + $('#end').val())).
            then(function (response) {
                sp.data = response.data;
                Util.removeLoadingAnimate();
            })
    }

    $scope.parseMoney = function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
        return strMoney;
    };
});