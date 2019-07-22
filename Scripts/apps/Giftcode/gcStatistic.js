﻿app.controller('gcStatistic', function ($scope, $http) {
    var sp = $scope;

    sp.init = function () {
        $('#year').val(new Date().getFullYear());
        $('#month').val(new Date().getMonth() + 1);
    }

    sp.get = function () {
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/giftcodeapi/GetStatistic?month=' + $('#month').val() + '&year=' + $('#year').val())).
            then(function (response) {
                sp.data = response.data;

                Util.removeLoadingAnimate();
            });
    }
});