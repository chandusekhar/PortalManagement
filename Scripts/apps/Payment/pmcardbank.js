app.controller('pmcardbank', function ($scope, $http) {
    var sp = $scope;

    sp.init = function () {
        sp.getCardInBank();
        sp.getErrCard();
    }

    sp.getCardInBank = function () {
        $http.get(encodeURI(config.url + '/api/paymentapi/GetCardInBank')).
            then(function (response) {
                sp.datatable = response.data;
                $.each(sp.datatable, function (i) {
                    var d = sp.datatable[i];

                    switch (d.CardType) {
                        case 1:
                            d.CardTypeStr = "Viettel";
                            break;
                        case 2:
                            d.CardTypeStr = "Mobi";
                            break;
                        case 3:
                            d.CardTypeStr = "Vina";
                            break;
                    }
                });
            })
    }

    sp.getErrCard = function () {
        $http.get(encodeURI(config.url + '/api/paymentapi/GetErrorCardTransaction')).
            then(function (response) {
                sp.datatableErr = response.data;
            })
    }

    sp.deleteCardFromBank = function (cardId) {
        $http.get(encodeURI(config.url + '/api/paymentapi/DeleteCardInBank?cardId=' + cardId)).
            then(function (response) {
                sp.datatableErr = response.data;
                alert("Xóa thẻ thành công !");
            })
    }

    sp.chargeCardBank = function () {
        var cardType = $('#cardType').val();
        var amount = $('#valueCard').val();
        var total = $('#totalCard').val();

        if (!total || total <= 0) {
            alert("Nhập số thẻ !");
            return;
        }

        $http.get(encodeURI(config.url + '/api/paymentapi/Charge?cardType=' + cardType + '&amount=' + amount + '&total=' + total)).
            then(function (response) {
                if (response.data) {
                    alert("Số thẻ thành công: " + response.data.suc + "\nSố thẻ thất bại: " + response.data.err);
                    sp.getCardInBank();
                    sp.getErrCard();
                }
                else
                    alert("Hệ thống bận, vui lòng thử lại sau !");
            });
    }

    $scope.parseMoney = function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        return strMoney;
    };

    $scope.formDateTimehms = function (date) {
        date = date.replace(/\-/g, '\/').replace(/[T|Z]/g, ' ');
        if (date.indexOf('.') > 0)
            date = date.substring(0, date.indexOf('.'));
        var d = new Date(date);
        var curr_date = d.getDate();
        var curr_month = d.getMonth() + 1;
        var curr_year = d.getFullYear();
        var _hour = d.getHours();
        var _minute = d.getMinutes();
        var _second = d.getSeconds();
        if (curr_date < 10) curr_date = "0" + curr_date;
        if (curr_month < 10) curr_month = "0" + curr_month;
        if (_hour < 10) _hour = "0" + _hour;
        if (_minute < 10) _minute = "0" + _minute;
        return curr_date + "/" + curr_month
            + "/" + curr_year + " " + _hour + ":" + _minute;
    };
});