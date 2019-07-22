app.controller('agencyTransactionHis', function ($scope, $http, ngTableParams) {
    var sp = $scope;

    sp.init = function () {
        getAll();

        sp.sum = 0;
        sp.fee = 0;
        sp.total = 0;
        sp.gameAccountId = '';
        sp.state = 0;
    }

    function getAll() {
        $http.get(encodeURI(config.url + '/api/agencyapi/GetAll')).
            then(function (response) {
                if (response.data) {
                    sp.agencies = response.data;
                }
                Util.removeLoadingAnimate();
            })
    }

    sp.getState = function (i) {
        if (i == 0)
            return "Chưa thanh toán";
        else if (i == 1)
            return "Đã thanh toán";
        else return "Nghi vấn";
    }

    sp.select = function (x) {
        sp.selectedObj = x;
        sp.state = x.State.toString();
    }

    sp.changeState = function () {
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/transactionapi/UpdateState?transId=' + sp.selectedObj.ID + '&state=' + sp.state)).
            then(function (response) {
                if (response.data == true) {
                    alert("Thành công");
                    sp.selectedObj.State = sp.state;
                } else {
                    alert("Cập nhật thất bại")
                }
                Util.removeLoadingAnimate();
            });
    }

    sp.get = function (page) {
        sp.curPage = page;

        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/AgencyApi/AgencyTransaction?id=' + $('#agencyId').val() + '&start=' + $('#start').val() + '&end=' + $('#end').val() + '&type=' + $('#optionValue').val() + "&page=" + page)).
            then(function (response) {
                if (response.data == null || !response.data.Data.length) {
                    response.data = [];
                    Util.removeLoadingAnimate();
                    return;
                }
                    
                sp.data = response.data.Data;
                sp.totalMoney = response.data.TotalMoney;
                sp.totalFee = response.data.TotalFee;
                if (sp.trans == undefined) {
                    sp.trans = new ngTableParams({
                        page: 1,
                        count: 25
                    }, {
                            total: response.data.length, getData: function ($defer, params) {
                                sp.pars = params;
                                var b = [].concat(sp.data);

                                b = b.slice(((params.$params.page - 1) * 25), ((params.$params.page - 1) * 25 + 25));

                                $defer.resolve(b);
                            }
                        });
                } else {
                    sp.trans.reload();
                }
                Util.removeLoadingAnimate();
            })
    }

    sp.downloadURI = function (uri, name) {
        var link = document.createElement("a");
        link.download = name;
        link.href = uri;
        link.click();
    }

    sp.dl = function () {
        var gameAccountId = $("#gameAccountId").val();
        var start = $("#start").val();
        var end = $("#end").val();
        var type = $("#optionValue").val();
        sp.downloadURI(encodeURI(config.url + "/Agency/Download?gameAccountId=" + sp.gameAccountId + "&start=" + start + "&end=" + end + "&type=" + type), new Date().getDate());
    }
    
    sp.prev = function () {
        sp.curPage--;
        if (sp.curPage < 0)
            sp.curPage = 0;

        sp.get(sp.curPage);
    }

    sp.next = function () {
        sp.curPage++;
        sp.get(sp.curPage);
    }
});