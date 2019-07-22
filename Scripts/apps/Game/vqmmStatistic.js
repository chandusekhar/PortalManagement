app.controller('vqmmStatistic', function ($scope, $http) {
    var sp = $scope;

    sp.init = function () {
        $('#year').val(new Date().getFullYear());
        $('#month').val(new Date().getMonth() + 1);
    }

    sp.get = function () {
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/gameapi/GetLuckySpinFund?month=' + $('#month').val() + '&year=' + $('#year').val())).
            then(function (response) {
                sp.data = response.data;
                sp.TotalMonth = 0;
                $.each(sp.data, function (i) {
                    var d = sp.data[i];
                    sp.TotalMonth += d.Total;
                });
                Util.removeLoadingAnimate();
            });
    }

    sp.formatIntToDate = function (dtInt) {
        if (!dtInt) return "";
        dtInt = dtInt.toString();
        var year = dtInt.substring(0, 4);
        var month = dtInt.substring(4, 6);
        var day = dtInt.substring(6, 8);

        return day + "/" + month + "/" + year;
    }
});