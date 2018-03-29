$(function () {
    $("#btnUserPoint").bind("click", function () {
        $("#MapPointBtn").find(".mapBtn").removeClass("btn-white");
        $("#MapPointBtn").find(".mapBtn").addClass("btn-white");
        $(this).removeClass("btn-white");
        UserPosition();
    });
    $("#btnHousePoint").bind("click", function () {
        $("#MapPointBtn").find(".mapBtn").removeClass("btn-white");
        $("#MapPointBtn").find(".mapBtn").addClass("btn-white");
        $(this).removeClass("btn-white");
        HousePosition();
    });
    $("#searchAroundMap").change(function () {
        var searchAround = $("#searchAroundMap").find("option:selected").val();
        SearchAroundPosition(searchAround);
    });
    $("#btnSubmit").bind("click", function () {
        SubmitData();
    });

    //行政区联动片区
    $("#txt_areaid").change(function () {
        select_subarea();
    });
});

/*********提交数据*****/
function SubmitData() {
    VerifyErrorStyleRemove();
    var projectname = $("#txt_projectname").val();
    var address = $("#txt_address").val();
    var areaid = $("#txt_areaid").val();
    //var buildingarea = $("#txt_buildingarea").val();/*建筑面积*/
    //var landarea = $("#txt_landarea").val();/*占地面积*/
    //var cubagerate = $("#txt_cubagerate").val();/*容积率*/
    //var greenrate = $("#txt_greenrate").val();/*绿化率*/
    //var managerprice = $("#txt_managerprice").val();/*物业管理费*/
    var parkingnumber = $("#txt_parkingnumber").val();/*车位数*/
    var totalnum = $("#txt_totalnum").val();/*总户数*/
    var buildingnum = $("#txt_buildingnum").val();/*总栋数*/
    var purposecode = $("#txt_purposecode").val();/*主用途*/
    var rightcode = $("#txt_rightcode").val();/*主用途*/
    //if (!VeriDecimal(buildingarea)) {
    //    VerifyErrorStyle($("#txt_buildingarea"));
    //    alert("建筑面积格式填写错误");
    //    return false;
    //}
    //if (!VeriDecimal(landarea)) {
    //    VerifyErrorStyle($("#txt_landarea"));
    //    alert("占地面积格式写填错误");
    //    return false;
    //}
    //if (!VeriDecimal(cubagerate)) {
    //    VerifyErrorStyle($("#txt_cubagerate"));
    //    alert("容积率格式写填错误");
    //    return false;
    //}
    //if (!VeriDecimal(greenrate)) {
    //    VerifyErrorStyle($("#txt_greenrate"));
    //    alert("绿化率格式写填错误");
    //    return false;
    //}
    //if (!VeriDecimal(managerprice)) {
    //    VerifyErrorStyle($("#txt_managerprice"));
    //    alert("物业管理费格式写填错误");
    //    return false;
    //}
    if (!VeriInt(parkingnumber)) {
        VerifyErrorStyle($("#txt_parkingnumber"));
        alert("车位数必须为整数");
        return false;
    }
    if (!VeriInt(totalnum)) {
        VerifyErrorStyle($("#txt_totalnum"));
        alert("总户数必须为整数");
        return false;
    }
    //if (buildingnum == "" || buildingnum == null) {
    //    VerifyErrorStyle($("#txt_buildingnum"));
    //    alert("总栋数不能为空");
    //    return false;
    //}
    //if (!VeriInt(buildingnum)) {
    //    VerifyErrorStyle($("#txt_buildingnum"));
    //    alert("总栋数必须为整数");
    //    return false;
    //}
    if (projectname == "" || projectname == null) {
        VerifyErrorStyle($("#txt_projectname"));
        alert("楼盘名不能为空");
        return false;
    }
    if (areaid == "" || areaid == "0") {
        VerifyErrorStyle($("#txt_areaid"));
        alert("请选择行政区");
        return false;
    }
    //if (address == "" || address == null) {
    //    VerifyErrorStyle($("#txt_address"));
    //    alert("地址不能为空");
    //    return false;
    //}
    if (purposecode == 0) {
        VerifyErrorStyle($("#txt_purposecode"));
        alert("请选择主用途");
        return false;
    }
    //if (rightcode == 0) {
    //    VerifyErrorStyle($("#txt_rightcode"));
    //    alert("请选择产权形式");
    //    return false;
    //}

    /**********楼盘信息************/
    var projectColDom = $("#SubmitPanel").find(".datproject");
    /**楼盘信息**/
    var projJson = "{";
    for (var i = 0; i < projectColDom.length; i++) {
        var key = $(projectColDom.get(i)).attr("id");
        key = key.replace("txt_", "");
        var value = $(projectColDom.get(i)).val();
        projJson = projJson + "\"" + key + "\":\"" + value + "\","
    }
    projJson = projJson.TrimEnd(',') + "}";
    /**关联公司信息**/
    var developersCompany = $("#txt_developerscompany").val();
    var managerCompany = $("#txt_managercompany").val();
    //var developersCompany = encodeURIComponent($("#txt_developerscompany").val());
    //var managerCompany = encodeURIComponent($("#txt_managercompany").val());
    var paraJson = { projectJson: projJson, developersCompany: developersCompany, managerCompany: managerCompany };
    $.extendAjax(
                {
                    url: "/DatabaseCall/Add",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    if (data != null) {
                        if (data.result != 1) {
                            alert(decodeURIComponent(data.message));
                            return;
                        }
                        else {
                            AlertFancybox2("新增成功", function () {
                                parent.$.fancybox.close();
                            });
                            //parent.location.reload(true);
                        }
                    }
                },
               { dom: "#SubmitPanel" });
}
function VerifyErrorStyle(dom) {
    $(dom).css("borderColor", "red").removeClass("vererror").addClass("vererror");
}
function VerifyErrorStyleRemove() {
    $("#SubmitPanel").find(".vererror").css("borderColor", "#B5B5B5");
}
/*****验证带小数类型数据格式******/
function VeriDecimal(val) {
    if (val != null && val != "") {
        var patrn = /^-?\d+\.{0,}\d{0,}$/;

        if (!patrn.exec(val)) {
            return false;
        }
    }
    return true;
}
/*****验证整数数据格式******/
function VeriInt(val) {
    if (val != null && val != "") {
        var patrn = /^\d+$/;

        if (!patrn.exec(val)) {
            return false;
        }
    }
    return true;
}



//片区下拉列表
function select_subarea() {
    var areaid = $("#txt_areaid").val();
    $.get('/AllotFlowInfo/GetSubAreaSelect', { areaid: areaid }, function (data) {
        $("#txt_subareaid").val(null).trigger("change");
        $("#txt_subareaid").html("");
        $.each(data, function (i, item) {
            if (item.Selected) {
                $("#txt_subareaid").append($("<option selected=\"selected\"></option>").val(item.Value).html(item.Text));
            } else {
                $("#txt_subareaid").append($("<option></option>").val(item.Value).html(item.Text));
            }
        });
    });
}