app.controller('playHis', function ($scope, $http) {
    var sp = $scope;

    sp.curPage = 0;
    $scope.init = function () {
    }

    $scope.historyDefault = function (searchType) {
        sp.searchType = searchType
        if (sp.curPage < 0)
            return;
        
        $http.get(encodeURI(config.url + '/api/playhistoryapi/GetGameHistory?searchType=' + searchType + '&accountId=' + $scope.accountid + '&username=' + $scope.username
            + '&displayName=' + $scope.displayName + "&page=" + sp.curPage)).
            then(function (response) {
                $scope.historyPlay = response.data;
                if ($scope.historyPlay.length) {
                    $('#noneResult').hide();
                    sp.curAccountId = $scope.historyPlay[0].AccountId;
                }
                else
                    $('#noneResult').show();
            });
    }

    sp.downloadURI = function (uri, name) {
        var link = document.createElement("a");
        link.download = name;
        link.href = uri;
        link.click();
    }

    sp.dl = function (eventId) {
        sp.downloadURI(encodeURI(config.url + "/PlayHistory/Download?accountId=" + sp.curAccountId));
    }

    sp.prev = function () {
        sp.curPage--;
        if (sp.curPage < 0)
            sp.curPage = 0;

        sp.historyDefault(sp.searchType);
    }

    sp.next = function () {
        sp.curPage++;
        sp.historyDefault(sp.searchType);
    }
});