app.controller('agency', function ($scope, $http) {
    var sp = $scope;

    sp.init = function () {
        sp.getAll();
    }

    sp.getAll = function () {
        $http.get(encodeURI(config.url + '/api/AgencyApi/GetAll')).
            then(function (response) {
                if (response.data)
                    sp.data = response.data;
                else
                    sp.data = [];
            });
    }

    sp.deleteAgency = function () {
        $http.get(encodeURI(config.url + '/api/AgencyApi/DeleteAgency?id=' + sp.selected.ID)).
            then(function (response) {
                if (response.data)
                    alert("Xóa thành công !");
                else
                    alert("Xóa không thành công !");
            });
    }

    sp.hideAgency = function () {
        $http.get(encodeURI(config.url + '/api/AgencyApi/HideAgency?id=' + sp.selected.ID + '&display=' + sp.display)).
            then(function (response) {
                if (response.data) {
                    $("#hideModal").modal('hide');
                    sp.getAll();
                }
                else
                    alert("Ẩn không thành công !");
            });
    }

    sp.openAdd = function () {
        $('#confirmModal').modal('show');
    }

    sp.add = function () {
        Util.addLoadingAnimate();
        $('#confirmModal').modal('hide');
        $http.get(encodeURI(config.url + '/api/AgencyApi/AddAgency?display=' + sp.displayName + '&tel=' + sp.tel + '&fb=' + sp.fb + '&tele=' + sp.tele + '&address=' + sp.address + '&gameName=' + sp.gameName)).
            then(function (response) {
                if (response.data == false) {
                    alert('Thêm đại lý không thành công');
                    Util.removeLoadingAnimate();
                }
                else {
                    window.location.reload();
                }
            })
    }

    sp.select = function (x) {
        sp.selected = x;
    }

    sp.openHide = function (x, display) {
        sp.selected = x;
        sp.display = display;
    }
});