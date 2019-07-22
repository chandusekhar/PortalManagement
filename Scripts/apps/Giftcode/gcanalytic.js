app.controller('gcanalytic', function ($scope, $http) {
    var sp = $scope;

    sp.loadGc = function (page) {
        sp.curPage = page;
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/giftcodeapi/GetEvent?id=' + $('#event_id').val() + '&page=' + page + '&type=' + $('#event_type').val())).
            then(function (response) {
                sp.gcList = response.data;
                sp.selectedEvent = $("#event_id option:selected").text();
                sp.curEventId = $('#event_id').val();
                $.each(sp.gcList, function (i) {
                    var d = sp.gcList[i];
                    if (d.Status)
                        d.StatusStr = "Đã sử dụng";
                    else if (!d.StatusStr)
                        d.StatusStr = "Chưa sử dụng";
                });
                Util.removeLoadingAnimate();
            });
    }

    sp.downloadURI = function (uri, name) {
        var link = document.createElement("a");
        link.download = name;
        link.href = uri;
        link.click();
    }

    sp.dl = function () {
        sp.downloadURI(encodeURI(config.url + "/Giftcode/Download?eventId=" + sp.curEventId));
    }

    sp.prev = function () {
        sp.curPage--;
        if (sp.curPage < 0)
            sp.curPage = 0;

        sp.loadGc(sp.curPage);
    }

    sp.next = function () {
        sp.curPage++;
        sp.loadGc(sp.curPage);
    }
});