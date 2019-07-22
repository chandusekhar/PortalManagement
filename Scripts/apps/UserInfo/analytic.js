app.controller('analytic', function ($scope, $http) {
    var sp = $scope;
    sp.type = -1;

    sp.do = function () {
        if (sp.type == 1)
            nru();
        else if (sp.type == 2)
            dau();
        else if (sp.type == 3)
            pu();
        else if (sp.type == 4)
            ccu();
        else if (sp.type == 5)
            topBalance();
    }

    sp.init = function () {
        sp.type = -1;
        $('#year').val(new Date().getFullYear());
        $('#month').val(new Date().getMonth() + 1);
    }

    sp.changeType = function () {
        sp.curPage = 0;
        $("#graph-lines").html('');
    }

    sp.prev = function () {
        sp.curPage--;
        if (sp.curPage < 0)
            sp.curPage = 0;
        topBalance();
    }

    sp.next = function () {
        sp.curPage++;
        topBalance();
    }

    function nru() {
        $http.get(encodeURI(config.url + '/api/userinfoapi/GetNRU?month=' + $('#month').val() + '&year=' + $('#year').val())).
            then(function (response) {
                if (response.data != null) {
                    sp.NRUMonth = 0;
                    sp.nruAna = response.data;
                    $.each(sp.nruAna, function (i) {
                        sp.NRUMonth += sp.nruAna[i].IOS + sp.nruAna[i].Android + sp.nruAna[i].Web + sp.nruAna[i].EXE + sp.nruAna[i].OSX;
                    });
                }
            })
    }

    function dau() {
        $http.get(encodeURI(config.url + '/api/userinfoapi/GetDAU?month=' + $('#month').val() + '&year=' + $('#year').val())).
            then(function (response) {
                if (response.data != null) {
                    sp.DAUMonth = 0;
                    sp.dauAna = response.data;
                    $.each(sp.dauAna, function (i) {
                        sp.DAUMonth += sp.dauAna[i].IOS + sp.dauAna[i].Android + sp.dauAna[i].Web + sp.dauAna[i].EXE + sp.dauAna[i].OSX;
                    });
                }
            })
    }

    function pu() {
        $http.get(encodeURI(config.url + '/api/userinfoapi/GetPU?month=' + $('#month').val() + '&year=' + $('#year').val())).
            then(function (response) {
                sp.PUMonth = 0;
                $scope.puAna = response.data;
                $.each(sp.puAna, function (i) {
                    sp.PUMonth += sp.puAna[i].Total;
                });
            })
    }

    function ccu() {

    }

    function topBalance() {
        $http.get(encodeURI(config.url + '/api/userinfoapi/GetTopbalance?page=' + sp.curPage)).
            then(function (response) {
                $scope.topBalance = response.data;
            })
    }

    $scope.parseMoney = function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
        return strMoney;
    };
});