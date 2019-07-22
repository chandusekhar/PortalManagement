app.controller('gcManagement', function ($scope, $http) {
    var sp = $scope;

    sp.getEvents = function (page) {
        sp.curPage = page;
        $http.get(encodeURI(config.url + '/api/GiftcodeApi/GetEvents?type=' + sp.type + "&page=" + page)).
            then(function (response) {
                sp.data = response.data;
                if (!sp.data.length)
                    return;
                $.each(sp.data, function (i) {
                    var _data = sp.data[i];
                    switch (_data.Type) {
                        case 0:
                            _data.TypeStr = "Test";
                            return;
                        case 1:
                            _data.TypeStr = "Đại lý";
                            return;
                        case 2:
                            _data.TypeStr = "MKT/Cộng đồng";
                            return;
                        default:
                            _data.TypeStr = "";
                            return;
                    }
                });
            });
    }

    sp.openDelete = function (id) {
        sp.curEventId = id;
        $('#deleteEvent').modal('show');
    }

    sp.openExtend = function (id) {
        sp.curEventId = id;
        $('#extendGC').modal('show');
    }

    sp.deleteEvent = function () {
        $http.get(encodeURI(config.url + '/api/GiftcodeApi/DeleteEvent?id=' + sp.curEventId)).
            then(function (response) {
                if (response.data)
                    alert("Xóa Giftcode thành công !");
                else
                    alert("Lỗi !");
            });
    }

    sp.extendEvent = function (newDt) {
        $http.get(encodeURI(config.url + '/api/GiftcodeApi/ExtendEvent?id=' + sp.curEventId + '&newDt=' + $('#newDt').val())).
            then(function (response) {
                if (response.data)
                    alert("Gia hạn Giftcode thành công !");
                else
                    alert("Lỗi !");
            });
    }

    sp.prev = function () {
        sp.curPage--;
        if (sp.curPage < 0)
            sp.curPage = 0;

        sp.getEvents(sp.curPage);
    }

    sp.next = function () {
        sp.curPage++;
        sp.getEvents(sp.curPage);
    }
});