app.controller('fund-edit', function ($scope, $http) {
    var sp = $scope;

    sp.editFund = function () {
        if (sp.game == null) {
            alert('Chưa chọn game');
            return;
        }
        if (sp.fundChange == null || sp.fundChange == 0) {
            alert('Chưa nhập giá trị thay đổi');
            return;
        }

        var roomId = $('#selectRoom').val();

        if (sp.game == 8 || sp.game == 9)
            roomId = 0;

        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/GameApi/ModifyFund?gameId=' + sp.game + '&roomId=' + roomId + '&value1=' + sp.fundChange)).
            then(function (response) {
                if (response.data) {
                    sp.fundChange = 0;
                    sp.getHistory()
                } else {
                    alert('Cập nhật thất bại');
                    Util.removeLoadingAnimate();
                }
            })
    }

    sp.getHistory = function () {
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/GameApi/GetFundChangeHistory')).
            then(function (response) {
                sp.data = response.data;
                Util.removeLoadingAnimate();
            });
    }

    sp.changeGame = function () {
        $('#selectRoom').val("1");
    }

    $scope.parseMoney = function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
        return strMoney;
    };
});