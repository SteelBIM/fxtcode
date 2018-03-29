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
    $("#AreaID").change(function () {
        select_subarea();
    });
});

/*********提交数据*****/
function SubmitData() {
    VerifyErrorStyleRemove();
    var projectname = $("#txt_projectname").val();
    var address = $("#txt_address").val();
    var areaid = $("#txt_areaid").val();
    //var buildingnum = $("#txt_buildingnum").val();/*总栋数*/
    //var purposecode = $("#txt_purposecode").val();/*主用途*/
    //var rightcode = $("#txt_rightcode").val();/*产权形式*/

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
    if (address == "" || address == null) {
        VerifyErrorStyle($("#txt_address"));
        alert("地址不能为空");
        return false;
    }
    //if (purposecode == 0) {
    //    VerifyErrorStyle($("#txt_purposecode"));
    //    alert("请选择主用途");
    //    return false;
    //}
    //if (rightcode == 0) {
    //    VerifyErrorStyle($("#txt_rightcode"));
    //    alert("请选择产权形式");
    //    return false;
    //}

    /*占地面积*/
    var LandArea = $("#LandArea");
    if (LandArea) {
        if (!VeriDecimal(LandArea.val())) {
            VerifyErrorStyle(LandArea);
            alert("占地面积格式填写错误");
            return false;
        }
    }

    /*土地使用年限*/
    var UsableYear = $("#UsableYear");
    if (UsableYear) {
        if (!VeriInt(UsableYear.val())) {
            VerifyErrorStyle(UsableYear);
            alert("土地使用年限必须为整数");
            return false;
        }
    }

    /*建筑面积*/
    var BuildingArea = $("#BuildingArea");
    if (BuildingArea) {
        if (!VeriDecimal(BuildingArea.val())) {
            VerifyErrorStyle(BuildingArea);
            alert("建筑面积格式填写错误");
            return false;
        }
    }

    /*可销售面积*/
    var SalableArea = $("#SalableArea");
    if (SalableArea) {
        if (!VeriDecimal(SalableArea.val())) {
            VerifyErrorStyle(SalableArea);
            alert("可销售面积格式填写错误");
            return false;
        }
    }

    /*容积率*/
    var CubageRate = $("#CubageRate");
    if (CubageRate) {
        if (!VeriDecimal(CubageRate.val())) {
            VerifyErrorStyle(CubageRate);
            alert("容积率格式填写错误");
            return false;
        }
    }

    /*绿化率*/
    var GreenRate = $("#GreenRate");
    if (GreenRate) {
        if (!VeriDecimal(GreenRate.val())) {
            VerifyErrorStyle(GreenRate);
            alert("绿化率格式填写错误");
            return false;
        }
    }

    /*车位数*/
    var txt_parkingnumber = $("#txt_parkingnumber");
    if (txt_parkingnumber) {
        if (!VeriInt(txt_parkingnumber.val())) {
            VerifyErrorStyle(txt_parkingnumber);
            alert("车位数必须为整数");
            return false;
        }
    }

    /*项目均价*/
    var AveragePrice = $("#AveragePrice");
    if (AveragePrice) {
        if (!VeriDecimal(AveragePrice.val())) {
            VerifyErrorStyle(AveragePrice);
            alert("项目均价格式填写错误");
            return false;
        }
    }

    /*总套数*/
    var txt_totalnum = $("#txt_totalnum");
    if (txt_totalnum) {
        if (!VeriInt(txt_totalnum.val())) {
            VerifyErrorStyle(txt_totalnum);
            alert("总套数必须为整数");
            return false;
        }
    }

    /*办公面积*/
    var OfficeArea = $("#OfficeArea");
    if (OfficeArea) {
        if (!VeriDecimal(OfficeArea.val())) {
            VerifyErrorStyle(OfficeArea);
            alert("办公面积格式填写错误");
            return false;
        }
    }

    /*其他用途面积*/
    var OtherArea = $("#OtherArea");
    if (OtherArea) {
        if (!VeriDecimal(OtherArea.val())) {
            VerifyErrorStyle(OtherArea);
            alert("其他用途面积格式填写错误");
            return false;
        }
    }

    /*商业面积*/
    var BusinessArea = $("#BusinessArea");
    if (BusinessArea) {
        if (!VeriDecimal(BusinessArea.val())) {
            VerifyErrorStyle(BusinessArea);
            alert("商业面积格式填写错误");
            return false;
        }
    }

    /*工业面积*/
    var IndustryArea = $("#IndustryArea");
    if (IndustryArea) {
        if (!VeriDecimal(IndustryArea.val())) {
            VerifyErrorStyle(IndustryArea);
            alert("工业面积格式填写错误");
            return false;
        }
    }

    /*开盘均价*/
    var SalePrice = $("#SalePrice");
    if (SalePrice) {
        if (!VeriDecimal(SalePrice.val())) {
            VerifyErrorStyle(SalePrice);
            alert("开盘均价格式填写错误");
            return false;
        }
    }

    /**********楼盘信息************/
    //var projectColDom = $("#SubmitPanel").find(".datproject");
    ///**楼盘信息**/
    //var projJson = "{";
    //for (var i = 0; i < projectColDom.length; i++) {
    //    var key = $(projectColDom.get(i)).attr("id");
    //    key = key.replace("txt_", "");
    //    var value = $(projectColDom.get(i)).val();
    //    projJson = projJson + "\"" + key + "\":\"" + value + "\","
    //}
    //projJson = projJson.TrimEnd(',') + "}";
    /**关联公司信息**/
    var developersCompany = $("#txt_developerscompany").val();
    var managerCompany = $("#txt_managercompany").val();
    //var developersCompany = encodeURIComponent($("#txt_developerscompany").val());
    //var managerCompany = encodeURIComponent($("#txt_managercompany").val());

    var form = $("form");

    var templet = { templetname: form.attr("templetname"), fieldgroups: [] };

    var groups = form.find("fieldset");

    if (groups != null && groups.length > 0) {

        for (var i = 0; i < groups.length; i++) {

            var groupdow = $(groups[i]);

            var group = { title: groupdow.attr("groupname"), fields: [] }

            var fieils = groupdow.find(".datproject");

            if (fieils != null && fieils.length > 0) {

                for (var f = 0; f < fieils.length; f++) {

                    var fielddow = $(fieils[f]);

                    fieldchoise = fielddow.attr("fieldchoise")

                    var choises = [];

                    if (fieldchoise) {

                        choisesvalue = fieldchoise.split(',');

                        for (var c = 0; c < choisesvalue.length; c++) {
                            choisesvalue2 = choisesvalue[c].split('|');
                            choises.push({ code: choisesvalue2[1], codename: choisesvalue2[0] });
                        }
                    }

                    var field = {
                        fieldname: fielddow.attr("name"),
                        type: fielddow.attr("fieldtype"),
                        fieldtype: fielddow.attr("fieldtype2"),
                        title: fielddow.attr("fieldtitle"),
                        maxlength: fielddow.attr("fieldmaxlength"),
                        isrequire: fielddow.attr("fieldisrequire"),
                        editexttype: fielddow.attr("fieldeditexttype"),
                        value: fielddow.val(),
                        choise: choises,
                    }

                    group.fields.push(field);
                }
            }


            templet.fieldgroups.push(group);
        }
    }

    var data = $("form").serializeArray();
    data.push({ name: "TempletContent", value: JSON.stringify(templet) })
    //var paraJson = { projectJson: projJson, developersCompany: developersCompany, managerCompany: managerCompany };
    $.extendAjax(
                {
                    url: "/DatabaseCall/Add",
                    data: data,
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
    var areaid = $("#AreaID").val();
    $.get('/AllotFlowInfo/GetSubAreaSelect', { areaid: areaid }, function (data) {
        $("#SubAreaID").val(null).trigger("change");
        $("#SubAreaID").html("");
        $.each(data, function (i, item) {
            if (item.Selected) {
                $("#SubAreaID").append($("<option selected=\"selected\"></option>").val(item.Value).html(item.Text));
            } else {
                $("#SubAreaID").append($("<option></option>").val(item.Value).html(item.Text));
            }
        });
    });
}