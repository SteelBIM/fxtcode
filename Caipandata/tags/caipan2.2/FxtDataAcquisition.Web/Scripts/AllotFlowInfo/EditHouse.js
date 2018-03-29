$(function () {
    BindHouseInfo();
    $("#btnSubmit").bind("click", function () {
        SubmitData();
    });
});
function BindHouseInfo() {

    /***绑定朝向***/
    var nowFrontCode = $("#hdFrontCode").val() * 1;
    for (var i = 0; i < frontCodeListJson.length; i++) {
        var isSelect = "";
        var obj = frontCodeListJson[i];
        if (nowFrontCode == obj.code) {
            isSelect = "selected=\"selected\"";
        }
        $("#txt_FrontCode").append("<option value=\"" + obj.code + "\" " + isSelect + " >" + obj.codename + "</option>");
    }
    /***绑定户型**/
    var nowHouseTypeCode = $("#hdHouseTypeCode").val() * 1;
    for (var i = 0; i < houseTypeCodeListJson.length; i++) {
        var isSelect = "";
        var obj = houseTypeCodeListJson[i];
        if (nowHouseTypeCode == obj.code) {
            isSelect = "selected=\"selected\"";
        }
        $("#txt_HouseTypeCode").append("<option value=\"" + obj.code + "\" " + isSelect + " >" + obj.codename + "</option>");
    }
    /***绑定景观***/
    var nowSightCode = $("#hdSightCode").val() * 1;
    for (var i = 0; i < sightCodeListJson.length; i++) {
        var isSelect = "";
        var obj = sightCodeListJson[i];
        if (nowSightCode == obj.code) {
            isSelect = "selected=\"selected\"";
        }
        $("#txt_SightCode").append("<option value=\"" + obj.code + "\" " + isSelect + " >" + obj.codename + "</option>");
    }
}

/*********提交房号数据*****/
function SubmitData() {
    VerifyErrorStyleRemove();

    var txthn = $("#txt_hn").val();
    var floorno = $("#txt_floorno").val();
    var endfloorno = $("#txt_endfloorno").val();
    if (txthn == "" || txthn == null) {
        VerifyErrorStyle($("#txt_hn"));
        alert("室号不能为空");
        return false;
    }
    if (floorno == "" || floorno == null) {
        VerifyErrorStyle($("#txt_floorno"));
        alert("起始楼层");
        return false;
    }
    if (!VeriInt(floorno)) {
        VerifyErrorStyle($("#txt_floorno"));
        alert("起始楼层必须为整数");
        return false;
    }
    if (endfloorno == "" || endfloorno == null) {
        VerifyErrorStyle($("#txt_endfloorno"));
        alert("结束楼层");
        return false;
    }
    if (!VeriInt(endfloorno)) {
        VerifyErrorStyle($("#txt_endfloorno"));
        alert("结束楼层必须为整数");
        return false;
    }

    //别墅
    var purposeCode = $("#txt_purposecode").val();
    //用途为：独立别墅、联排别墅、双拼别墅、别墅 判断：起始层与终止层只能为1
    if (purposeCode == 1002005 || purposeCode == 1002006 || purposeCode == 1002008 || purposeCode == 1002027) {
        if ($("#txt_floorno").val() != 1 || $("#txt_endfloorno").val() != 1) {
            alert("起始层与终止层只能为1");
            return false;
        }
    }

    //单元
    $("#UnitNo").val($("#txt_un").val() + "$" + $("#txt_hn").val());

    var paraJson = $("form").serializeArray();
    $.extendAjax(
                {
                    url: "/AllotFlowInfo/EditHouse_SubmitData_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    if (!data.result) {
                        alert(data.message);
                        return;
                    }
                    else {
                        AlertFancybox2("保存成功", function () {
                            DA_Confirm("生成房号", "是否生成房号？", function (ok) {
                                if (ok) {
                                    var hid = data.data.houseid;
                                    $.post("/House/CreateHouseDetailsByHouseId", { houseId: hid }, function (result) {
                                        if (result.result) {
                                            AlertFancybox2("生成房号成功", function () {
                                                parent.$.fancybox.close();
                                            });
                                        } else {
                                            if (result.message) {
                                                alert(result.message);
                                            } else {
                                                alert("生成失败");
                                            }
                                        }
                                    });
                                } else {
                                    parent.$.fancybox.close();
                                }
                            });
                        });
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
        var patrn = /^(-|\+)?\d+$/;

        if (!patrn.exec(val)) {
            return false;
        }
    }
    return true;
}
/*****选择用途******/
function SelectPurposeCode(t) {
    var purposeCode = $(t).val();
    if (purposeCode == 1002005 || purposeCode == 1002006 || purposeCode == 1002008 || purposeCode == 1002027) {
        $("#txt_floorno").val(1);
        $("#txt_endfloorno").val(1);
    }
}