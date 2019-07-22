var config = config || {
    url: "//localhost:55687/"
};

var app = app || angular.module('game_cms', ['ngTable']);

app.filter('formatDate', function () {
    return function (date) {
        if (!date) return "";
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

app.filter('formatDateOnly', function () {
    return function (date) {
        if (!date) return "";
        date = date.replace(/\-/g, '\/').replace(/[T|Z]/g, ' ');
        if (date.indexOf('.') > 0)
            date = date.substring(0, date.indexOf('.'));
        var d = new Date(date);
        var curr_date = d.getDate();
        var curr_month = d.getMonth() + 1;
        var curr_year = d.getFullYear();
        if (curr_date < 10) curr_date = "0" + curr_date;
        if (curr_month < 10) curr_month = "0" + curr_month;
        return curr_date + "/" + curr_month + "/" + curr_year;
    };
});

app.filter('formatIntToDate', function () {
    return function (dtInt) {
        if (!dtInt) return "";
        dtInt = dtInt.toString();
        var year = dtInt.substring(0, 4);
        var month = dtInt.substring(4, 6);
        var day = dtInt.substring(6, 8);

        return day + "/" + month + "/" + year;
    };
});

app.filter('parseMoney', function () {
    return function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        return strMoney;
    }
});

