app.controller('agencyTransAnalytic', function ($rootScope, $scope, $http) {
    var sp = $scope;

    sp.init = function () {
        $('#year').val(new Date().getFullYear());
        $('#month').val(new Date().getMonth() + 1);
    }

    sp.get = function () {
        $http.get(encodeURI(config.url + '/api/AgencyAPI/GetAgencyTransactionsAnalytic?month=' + $('#month').val() + '&year=' + $('#year').val())).
            then(function (response) {
                sp.datatable = response.data;
                sp.totalBuy = 0;
                sp.totalSell = 0;
                $.each(sp.datatable, function (i) {
                    sp.totalBuy += sp.datatable[i].TotalRecv;
                    sp.totalSell += sp.datatable[i].TotalSend;
                });
            })
    }
});