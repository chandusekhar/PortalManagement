app.controller('searchPlayHis', function ($scope, $http) {
    var sp = $scope;

    sp.search = function () {
        var gameId = $('#gameId').val();

        if (!gameId) {
            alert('Chưa chọn game');
            return;
        }
        if (sp.sessionId == null || sp.sessionId == 0) {
            alert('Chưa nhập phiên tìm kiếm !');
            return;
        }
        $http.get(encodeURI(config.url + '/api/UserInfoApi/GetPlayLog?gameId=' + gameId + '&sessionId=' + sp.sessionId)).
            then(function (response) {
                if (response.data) {
                    if (!response.data.length) {
                        alert('Không có kết quả !');
                        return;
                    }
                    sp.data = response.data;
                } else {
                    alert('Cập nhật thất bại');
                }
            });
    }
});