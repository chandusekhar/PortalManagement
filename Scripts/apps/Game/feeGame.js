app.controller('fee', function ($scope, $http) {
    var sp = $scope;

    sp.getFee = function () {
        Util.addLoadingAnimate();
        $http.get(encodeURI(config.url + '/api/GameApi/GetListFee?start=' + $('#start').val() + '&end=' + $('#end').val())).
            then(function (response) {
                sp.data = response.data;

                // TEMPORARY HARD CODE
                $.each(sp.data, function (i) {
                    if (sp.data[i]["Day"] == "26/10/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 55059003;
                        _data["10"] = 419517;
                        _data["16"] = 10545238;
                    }
                    if (sp.data[i]["Day"] == "27/10/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 34416198;
                        _data["10"] = 257150;
                        _data["16"] = 5261694;
                    }
                    if (sp.data[i]["Day"] == "28/10/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 35127874;
                        _data["10"] = 418918;
                        _data["16"] = 7094432;
                    }
                    if (sp.data[i]["Day"] == "29/10/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 46897544;
                        _data["10"] = 211780;
                        _data["16"] = 7813836;
                    }
                    if (sp.data[i]["Day"] == "30/10/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 48657717;
                        _data["10"] = 379579;
                        _data["16"] = 26709291;
                    }
                    if (sp.data[i]["Day"] == "31/10/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 42150646;
                        _data["10"] = 111716;
                        _data["16"] = 30659422;
                    }
                    if (sp.data[i]["Day"] == "01/11/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 49145226;
                        _data["10"] = 545107;
                        _data["16"] = 11153169;
                    }
                    if (sp.data[i]["Day"] == "02/11/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 51826849;
                        _data["10"] = 283564;
                        _data["16"] = 20963689;
                    }
                    if (sp.data[i]["Day"] == "03/11/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 62091670;
                        _data["10"] = 272882;
                        _data["16"] = 22076761;
                    }
                    if (sp.data[i]["Day"] == "04/11/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 31815048;
                        _data["10"] = 131292;
                        _data["16"] = 11579895;
                    }
                    if (sp.data[i]["Day"] == "05/11/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 45566581;
                        _data["10"] = 193670;
                        _data["16"] = 4202640;
                    }
                    if (sp.data[i]["Day"] == "06/11/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 35683219;
                        _data["10"] = 319354;
                        _data["16"] = 10825428;
                    }
                    if (sp.data[i]["Day"] == "07/11/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 30149955;
                        _data["10"] = 369980;
                        _data["16"] = 1594115;
                    }
                    if (sp.data[i]["Day"] == "08/11/2018") {
                        var _data = sp.data[i].Fees;
                        _data["8"] = 35360232;
                        _data["10"] = 260696;
                        _data["16"] = 3878441;
                    }
                });
                //

                Util.removeLoadingAnimate();
            })
    }

    $scope.parseMoney = function (money) {
        if (money == undefined) return '';
        var strMoney = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
        return strMoney;
    };
});