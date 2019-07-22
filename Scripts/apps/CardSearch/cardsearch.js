app.controller('cardsearch', function ($scope, $http) {
    var sp = $scope;

    sp.search = function (type) {
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/cardsearchapi/SearchCard?searchType=' + type + '&serial=' + $scope.keyword + '&pin=' + $scope.keyword)).
            then(function (response) {
                sp.result = response.data;
                Util.removeLoadingAnimate();
            });
    }

    sp.selectCard = function (x) {
        $scope.selected = x;
    }

    $scope.parseMoney = function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
        return strMoney;
    };
});