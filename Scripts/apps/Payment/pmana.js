app.controller('pmana', function ($scope, $http) {
    var sp = $scope;

    sp.init = function () {
        $('#year').val(new Date().getFullYear());
        $('#month').val(new Date().getMonth() + 1);
    }

    sp.do = function () {
        $http.get(encodeURI(config.url + '/api/paymentapi/Analytic?month=' + $('#month').val() + '&year=' + $('#year').val())).
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