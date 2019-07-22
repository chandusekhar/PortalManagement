var Util = Util || {};

Util.parseMoney = function (money) {
    if (money == undefined) return '';
    var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
    return strMoney;
};

Util.addLoadingAnimate = function () {
    $('body').addClass('loading');
}

Util.removeLoadingAnimate = function () {
    $('body').removeClass('loading');
}

Util.Expand = function () {
    var lst = $($('.sidebar-menu a'))
    lst.each(function (i) {
        var str = $(lst[i]).attr('href');
        var url = location.href.split('?')[0];
        if (url.endsWith(str)) {
            $($(lst[i]).parents()[2]).addClass('active');
            $($(lst[i]).parents()[0]).addClass('active');
        }
    })
}

Util.formDateTimehms = function (date) {
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

$(function () {
    Util.Expand();
});