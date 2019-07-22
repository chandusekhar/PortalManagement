app.controller('rewardInfo', function ($scope, $http) {
    var sp = $scope;

    sp.init = function () {
        sp.agencies = [];
        sp.getAllAgencies();        
    }

    sp.getAllAgencies = function () {
        $http.get(encodeURI(config.url + '/api/AgencyApi/GetAllAgencies')).
            then(function (response) {
                if (response.data)
                    sp.agencies = response.data;
                else
                    sp.agencies = [];
            });
    }

    sp.get = function () {
        var selectedAgen = $('#agencyId').val();
        if (sp.agencies.length && !selectedAgen) {
            alert("Chọn đại lý !");
            return;
        }
        if (!sp.agencies.length)
            selectedAgen = 0;

        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/AgencyApi/GetRewardInfo?gameAccountId=' + selectedAgen + '&from=' + $('#from').val() + '&to=' + $('#to').val())).
            then(function (response) {
                if (response.data) {
                    sp.data = response.data;
                    sp.SumTotalMoney = 0;
                    sp.SumReward = 0;
                    sp.SumTransaction = 0;
                    $.each(sp.data, function (i) {
                        var _data = sp.data[i];

                        sp.SumTotalMoney += _data.WeekTotalMoney;
                        sp.SumReward += _data.Reward;
                        sp.SumTransaction += _data.WeekTotalTransaction;
                    });

                    sp.curAgenGameId = selectedAgen;
                }
                else
                    sp.data = [];
                Util.removeLoadingAnimate();
            });
    }

    sp.listTransAccount = function (weekName) {
        var week = sp.data.filter(obj => {
            return obj.WeekName === weekName
        })[0];

        var toDt = week.Items[0].CreatedDateInt;
        var fromDt = week.Items[week.Items.length - 1].CreatedDateInt;

        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/AgencyApi/GetListTransAccount?gameAccountId=' + sp.curAgenGameId + '&from=' + fromDt + '&to=' + toDt)).
            then(function (response) {
                var totalWk = 0;
                if (response.data) {
                    sp.dataList = response.data;
                    $('#listTransactionAccount').modal('show');
                }
                else
                    sp.data = [];
                Util.removeLoadingAnimate();
            });
    }
});