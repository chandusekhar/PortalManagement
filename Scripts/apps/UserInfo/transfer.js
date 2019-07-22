app.controller('transfer', function ($scope, $http) {
    var sp = $scope;

    $scope.transactionId = '';
    $scope.username = '';
    $scope.displayName = '';
    $scope.accountid = '';

    sp.curPage = 0;

    $scope.search = function () {
        if ($scope.transactionId)
            $http.get(encodeURI(config.url + '/api/transactionapi/Search?id=' + $scope.transactionId)).
                then(function (response) {
                    $scope.logs = [];
                    $scope.result = [];
                    if (response.data) {
                        $scope.logs.push(response.data);
                        $('#resultSearch').hide();
                    }
                    else
                        alert("Không có kết quả !");
                });
        else {
            $http.get(encodeURI(config.url + '/api/userinfoapi/search?displayname=' + $scope.displayName + '&username=' + $scope.username + '&id=' + $scope.accountid + '&phone=')).
                then(function (response) {
                    $scope.logs = [];
                    if (!response.data.length || !response.data) {
                        alert("Không có kết quả !");
                        return;
                    }
                    $scope.result = response.data;
                    $('#resultSearch').show();
                });
        }
    }

    $scope.clear = function (type) {
        if (type == 1) {
            $scope.username = '';
            $scope.displayName = '';
            $scope.accountid = '';
        }
        else if (type == 2) {
            $scope.username = '';
            $scope.displayName = '';
            $scope.transactionId = '';
        }
        else if (type == 3) {
            $scope.transactionId = '';
            $scope.username = '';
            $scope.accountid = '';
        }
        else if (type == 4) {
            $scope.displayName = '';
            $scope.transactionId = '';
            $scope.accountid = '';
        }
    }

    $scope.selectUser = function (x) {
        $scope.selected = x;
        $scope.getTransfer();
    }

    $scope.getTransfer = function () {
        if (sp.curPage < 0)
            return;

        $http.get(encodeURI(config.url + '/api/userinfoapi/GetTransferLog?accountId=' + $scope.selected.AccountID + "&page=" + sp.curPage)).
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
        //if (!sp.reason) {
        //    alert("Nhập lí do !");
        //    return;
        //}

        $http.get(encodeURI(config.url + '/api/transactionapi/RefundTran?id=' + sp.curRefundID + "&reason=" + sp.reason)).
            then(function (response) {
                if (response > 0)
                    alert("Hoàn giao dịch thành công");
                else {
                    alert("Có lỗi xảy ra | Response:" + response);
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