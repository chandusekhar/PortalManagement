app.controller('pmbilling', function ($scope, $http) {
    var sp = $scope;

    sp.init = function () {
        $('#year').val(new Date().getFullYear());
        $('#month').val(new Date().getMonth() + 1);

        sp.curPage = 0;
        sp.do();
    }

    sp.do = function () {
        sp.getTopup(year, month);
        sp.getCashout(year, month);
    }

    sp.getTopup = function () {
        $http.get(encodeURI(config.url + '/api/paymentapi/GetTopupBilling?month=' + $('#month').val() + '&year=' + $('#year').val())).
            then(function (response) {
                if (response.data) {
                    sp.resultTopup = response.data;
                    sp.totalTopup = 0;
                    $.each(sp.resultTopup, function (i) {
                        var d = sp.resultTopup[i];
                        sp.totalTopup += d.Amount;
                    });
                }
            });
    }

    sp.getCashout = function () {
        $http.get(encodeURI(config.url + '/api/paymentapi/GetCashoutBilling?month=' + $('#month').val() + '&year=' + $('#year').val())).
            then(function (response) {
                if (response.data) {
                    sp.resultCashout = response.data;
                    sp.totalCashout = 0;
                    $.each(sp.resultCashout, function (i) {
                        var d = sp.resultCashout[i];
                        sp.totalCashout += d.Amount;
                    });
                }
            });
    }
});