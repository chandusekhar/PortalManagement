app.controller('refundTransaction', function ($scope, $http) {
    var sp = $scope;

    $scope.search = function () {
        $http.get(encodeURI(config.url + '/api/transactionapi/Search?id=' + $scope.transactionId)).
            then(function (response) {
                if (response.data)
                    $scope.logs = response.data;
                else
                    alert("Không có kết quả !");
            });
    }

    $scope.getRefundLog = function () {
        if (sp.curPage < 0)
            return;

        $http.get(encodeURI(config.url + '/api/transactionapi/GetRefundLog?accountId=' + $scope.selected.AccountID + "&page=" + sp.curPage)).
            then(function (response) {
                $scope.logs = response.data;
                if ($scope.logs.length)
                    $('#noneResult').hide();
                else
                    $('#noneResult').show();
            });
    }

    sp.openRefundModal = function (id, sendName, recvName, amount) {
        sp.curRefundID = id;
        $('#refundTransaction').modal('show');
    }

    sp.refundTransaction = function () {
        var reason = "";
        $http.get(encodeURI(config.url + '/api/transactionapi/RefundTran?id=' + sp.curRefundID + '&reason=' + reason)).
            then(function (response) {
                if (response.data > 0) {
                    alert("Hoàn giao dịch thành công");
                    $scope.search();
                }
                else {
                    alert("Có lỗi xảy ra | Response:" + response.data);
                }
            });
    }

    sp.prev = function () {
        sp.curPage--;
        if (sp.curPage < 0)
            sp.curPage = 0;

        sp.logs(sp.searchType);
    }

    sp.next = function () {
        sp.curPage++;
        sp.logs(sp.searchType);
    }
});