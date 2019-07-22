app.controller('search', function ($scope, $http)
{
    $scope.accountname = '';
    $scope.username = '';
    $scope.accountid = '';
    $scope.phonenumber = '';
    $scope.tab = 0;
    $scope.curPage = 0;


    $scope.search = function () {
        $http.get(encodeURI(config.url + '/api/userinfoapi/search?displayname=' + $scope.accountname + '&username=' + $scope.username + '&id=' + $scope.accountid + '&phone=' + $scope.phonenumber)).
            then(function (response) {
                $scope.result = response.data;
            })
    }

    $scope.selectUser = function (x) {
        $scope.selected = x;
    }

    $scope.parseMoney = function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
        return strMoney;
    };

    $scope.getBalanceHistory = function (accountId, page) {
        $scope.tab = 1;
        if (page < 0)
            return;
        $scope.curPage = page;
        $http.get(encodeURI(config.url + '/api/userinfoapi/GetBalanceHistory?accountId=' + accountId + "&page=" + page)).
            then(function (response) {
                $scope.balanceHistory = response.data;
            })
    }

    $scope.getTopupHistory = function (accountId, page) {
        $scope.tab = 6;
        if (page < 0)
            return;
        $scope.curPage = page;
        $http.get(encodeURI(config.url + '/api/userinfoapi/GetTopupHistory?accountId=' + accountId + "&page=" + page)).
            then(function (response) {
                $scope.topupHistory = response.data;
            })
    }

    $scope.getCashoutHistory = function (accountId, page) {
        $scope.tab = 7;
        if (page < 0)
            return;
        $scope.curPage = page;
        $http.get(encodeURI(config.url + '/api/userinfoapi/GetCashoutHistory?accountId=' + accountId + "&page=" + page)).
            then(function (response) {
                $scope.cashoutHistory = response.data;
            })
    }

    $scope.getLogin = function (accountId, page) {
        $scope.tab = 2;
        if (page < 0)
            return;
        $scope.curPage = page;
        $http.get(encodeURI(config.url + '/api/userinfoapi/GetLoginLog?accountId=' + accountId + "&page=" + page)).
            then(function (response) {
                $scope.historyLog = response.data;
            })
    }

    $scope.historyDefault = function () {
        $scope.tab = 0;
        $scope.curPage = 0;
        $scope.getGameHistory($scope.selected.AccountID, 0);
        $('#myTabs li').removeClass('active');
        $($('#myTabs li')[0]).addClass('active');
        $('#myTabContent .tab-pane').removeClass('active in');
        $($('#myTabContent .tab-pane')[0]).addClass('active  in');
    }

    $scope.getGameHistory = function (accountId, page) {
        $scope.tab = 0;
        if (page < 0)
            return;
        $scope.curPage = page;
        $http.get(encodeURI(config.url + '/api/playhistoryapi/GetGameHistory?searchType=0&accountId=' + accountId + '&username=&displayName=&page=' + page)).
            then(function (response) {
                $scope.historyPlay = response.data;
            });
    }

    $scope.prev = function(){
        $scope.curPage--;
        if ($scope.curPage < 0)
            $scope.curPage = 0;
        if ($scope.tab == 0)
            $scope.getGameHistory($scope.selected.AccountID, $scope.curPage);
        else if ($scope.tab == 1)
            $scope.getBalanceHistory($scope.selected.AccountID, $scope.curPage);
        else if ($scope.tab == 2) 
            $scope.getLogin($scope.selected.AccountID, $scope.curPage);
        else if ($scope.tab == 4)
            $scope.lstBlockLogin($scope.curPage)
        else if ($scope.tab == 5)
            $scope.lstBlockChat($scope.curPage)
        else if ($scope.tab == 6)
            $scope.getTopupHistory($scope.curPage)
        else if ($scope.tab == 7)
            $scope.getCashoutHistory($scope.curPage)
    }

    $scope.next = function () {
        $scope.curPage++;
        if ($scope.tab == 0)
            $scope.getGameHistory($scope.selected.AccountID, $scope.curPage);
        else if ($scope.tab == 1)
            $scope.getBalanceHistory($scope.selected.AccountID, $scope.curPage);
        else if ($scope.tab == 2)
            $scope.getLogin($scope.selected.AccountID, $scope.curPage);
        else if ($scope.tab == 4)
            $scope.lstBlockLogin($scope.curPage)
        else if ($scope.tab == 5)
            $scope.lstBlockChat($scope.curPage)
        else if ($scope.tab == 6)
            $scope.getTopupHistory($scope.curPage)
        else if ($scope.tab == 7)
            $scope.getCashoutHistory($scope.curPage)
    }

    $scope.BlockChat = function () {
        $http.get(encodeURI(config.url + '/api/userinfoapi/BlockChat?accountId=' + $scope.selected.AccountID + "&state=1")).
            then(function (response) {
                if (response.data == true)
                    $scope.selected.IsMute = true;
            })
    }

    $scope.setIndex = function (idx, x) {
        $scope.idx = idx;
        $scope.selected = x;
    }

    $scope.OffLoginOTP = function () {
        $http.get(encodeURI(config.url + '/api/userinfoapi/OffLoginOTP?accountId=' + $scope.selected.AccountID)).
            then(function (response) {
                if (response.data == true) {
                    $scope.selected.IsOTP = false;
                }
            });
    }

    $scope.ResetPassword = function () {
        $http.get(encodeURI(config.url + '/api/userinfoapi/ResetPassword?accountId=' + $scope.selected.AccountID)).
            then(function (response) {
                if (response.data == true) {
                    alert("Reset mật khẩu thành công !");
                }
            });
    }

    $scope.UnBlockChat = function () {
        $http.get(encodeURI(config.url + '/api/userinfoapi/BlockChat?accountId=' + $scope.selected.AccountID + "&state=0")).
            then(function (response) {
                if (response.data == true) {
                    if ($scope.blockChat != undefined) {
                        $scope.blockChat.splice($scope.idx, 1);
                        return;
                    }
                    $scope.selected.IsMute = false;
                }
            })
    }

    $scope.BlockLogin = function () {
        var reason = $('#blockReason').val()
        if (!reason) {
            alert("Nhập lý do khóa !");
            return;
        }

        $http.get(encodeURI(config.url + '/api/userinfoapi/BlockLogin?accountId=' + $scope.selected.AccountID + "&reason=" + reason + "&state=1")).
            then(function (response) {
                if (response.data == true) {
                    $scope.selected.IsBlocked = true;

                    alert("Khóa đăng nhập thành công!");
                }
            })
    }

    $scope.UnBlockLogin = function () {
        $http.get(encodeURI(config.url + '/api/userinfoapi/BlockLogin?accountId=' + $scope.selected.AccountID + "&reason=" + "&state=0")).
            then(function (response) {
                if (response.data == true) {
                    if ($scope.blockLogins != undefined) {
                        $scope.blockLogins.splice($scope.idx, 1);
                        return;
                    }
                    $scope.selected.IsBlocked = false;

                    alert("Mở khóa đăng nhập thành công!");
                }
            })
    }

    $scope.lstBlockLogin = function (page) {
        $scope.tab = 4;
        if (page < 0)
            return;
        $scope.curPage = page;
        $http.get(encodeURI(config.url + '/api/userinfoapi/ListBlockLogin?page=' + page)).
            then(function (response) {
                $scope.blockLogins = response.data;
            })
    }

    $scope.lstBlockChat = function (page) {
        $scope.tab = 5;
        if (page < 0)
            return;
        $scope.curPage = page;
        $http.get(encodeURI(config.url + '/api/userinfoapi/ListBlockChat?page=' + page)).
            then(function (response) {
                $scope.blockChat = response.data;
            })
    }
});