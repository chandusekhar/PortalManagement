app.controller('transfee', function ($scope, $http) {
    var sp = $scope;

    sp.init = function () {        
        sp.curPage = 0;

        $('#year').val(new Date().getFullYear());
        $('#month').val(new Date().getMonth() + 1);
    }

    sp.get = function () {
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/Transactionapi/GetTransactionFees?month=' + $('#month').val() + '&year=' + $('#year').val())).
            then(function (response) {
                sp.curPage = 0;
                sp.data = response.data;
                if (sp.data) {
                    sp.sumFee = 0;
                    $.each(sp.data, function (i) {
                        var _data = sp.data[i];

                        sp.sumFee += _data.TotalFee;
                    });
                }
                Util.removeLoadingAnimate();
            });
    }

    sp.showDetail = function (date) {
        sp.curSelect = date.substr(0, 10);
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/Transactionapi/GetTransactionByDate?date=' + sp.curSelect + '&page=' + sp.curPage)).
            then(function (response) {
                sp.dataDate = response.data;
                Util.removeLoadingAnimate();
            });
    }

    $scope.prev = function () {
        $scope.curPage--;
        if ($scope.curPage < 0)
            $scope.curPage = 0;

        sp.showDetail(sp.curSelect);
    }

    $scope.next = function () {
        $scope.curPage++;
        sp.showDetail(sp.curSelect);
    }
});