app.controller('gcSummary', function ($scope, $http) {
    var sp = $scope;

    sp.getSummary = function () {
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/giftcodeapi/GiftcodeSummary')).
            then(function (response) {
                sp.summary = response.data;
                $.each(sp.summary, function (i) {
                    var _data = sp.summary[i];
                    switch (_data.Type) {
                        case 0:
                            _data.TypeStr = "Test";
                            return;
                        case 1:
                            _data.TypeStr = "Đại lý";
                            return;
                        case 2:
                            _data.TypeStr = "MTK/Cộng đồng";
                            return;
                        default:
                            _data.TypeStr = "";
                            return;
                    }
                });
                Util.removeLoadingAnimate();
            });
    }
});