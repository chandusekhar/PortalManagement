app.controller('pmverify', function ($scope, $http) {
    var sp = $scope;

    sp.init = function () {
        sp.getUnverifyCard();
    }

    sp.getUnverifyCard = function () {
        $http.get(encodeURI(config.url + '/api/paymentapi/GetUnverifyPayment')).
            then(function (response) {
                sp.datatable = response.data;
            });
    }

    sp.acceptCard = function (cardId) {
        $http.get(encodeURI(config.url + '/api/paymentapi/AcceptCard?cardId=' + cardId)).
            then(function (response) {
                if (response.data) {
                    sp.getUnverifyCard();
                    alert("Duyệt thẻ thành công !");
                }
            });
    }

    sp.deleteCard = function (cardId) {
        $http.get(encodeURI(config.url + '/api/paymentapi/DeleteCard?cardId=' + cardId)).
            then(function (response) {
                if (response.data) {
                    sp.getUnverifyCard();
                    alert("Xóa thẻ thành công !");
                }
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