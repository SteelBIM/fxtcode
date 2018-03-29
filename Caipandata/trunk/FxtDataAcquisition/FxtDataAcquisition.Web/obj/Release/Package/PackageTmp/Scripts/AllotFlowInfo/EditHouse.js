$(function () {
    $("#btnSubmit").bind("click", function () {
        SubmitData();
    });
});

/*********提交房号数据*****/
function SubmitData() {
    VerifyErrorStyleRemove();

    var txthn = $("#txt_hn");
    if (txthn) {
        if (txthn.val() == "" || txthn.val() == null) {
            VerifyErrorStyle($("#txt_hn"));
            alert("室号不能为空");
            return false;
        }
    }

    var floorno = $("#txt_floorno");
    if (floorno) {
        if (floorno.val() == "" || floorno.val() == null) {
            VerifyErrorStyle($("#txt_floorno"));
            alert("起始楼层不能为空");
            return false;
        }
        if (!VeriInt(floorno.val())) {
            VerifyErrorStyle($("#txt_floorno"));
            alert("起始楼层必须为整数");
            return false;
        }
    }

    var endfloorno = $("#txt_endfloorno");
    if (endfloorno) {
        if (endfloorno.val() == "" || endfloorno.val() == null) {
            VerifyErrorStyle($("#txt_endfloorno"));
            alert("结束楼层不能为空");
            return false;
        }
        if (!VeriInt(endfloorno.val())) {
            VerifyErrorStyle($("#txt_endfloorno"));
            alert("结束楼层必须为整数");
            return false;
        }
    }

    /*建筑面积*/
    var BuildArea = $("#BuildArea");
    if (BuildArea) {
        if (!VeriDecimal(BuildArea.val())) {
            VerifyErrorStyle(BuildArea);
            alert("建筑面积格式填写错误");
            return false;
        }
    }

    /*附属房屋面积*/
    var SubHouseArea = $("#SubHouseArea");
    if (SubHouseArea) {
        if (!VeriDecimal(SubHouseArea.val())) {
            VerifyErrorStyle(SubHouseArea);
            alert("附属房屋面积格式填写错误");
            return false;
        }
    }

    /*阳台数*/
    var Balcony = $("#Balcony");
    if (Balcony) {
        if (!VeriInt(Balcony.val())) {
            VerifyErrorStyle(Balcony);
            alert("阳台数必须为整数");
            return false;
        }
    }

    /*单价*/
    var UnitPrice = $("#UnitPrice");
    if (UnitPrice) {
        if (!VeriDecimal(UnitPrice.val())) {
            VerifyErrorStyle(UnitPrice);
            alert("单价格式填写错误");
            return false;
        }
    }

    /*总价*/
    var TotalPrice = $("#TotalPrice");
    if (TotalPrice) {
        if (!VeriDecimal(TotalPrice.val())) {
            VerifyErrorStyle(TotalPrice);
            alert("总价格式填写错误");
            return false;
        }
    }

    /*价格系数*/
    var Weight = $("#Weight");
    if (Weight) {
        if (!VeriDecimal(Weight.val())) {
            VerifyErrorStyle(Weight);
            alert("价格系数格式填写错误");
            return false;
        }
    }

    /*套内面积*/
    var InnerBuildingArea = $("#InnerBuildingArea");
    if (InnerBuildingArea) {
        if (!VeriDecimal(InnerBuildingArea.val())) {
            VerifyErrorStyle(InnerBuildingArea);
            alert("套内面积格式填写错误");
            return false;
        }
    }

    /*洗手间数*/
    var Toilet = $("#Toilet");
    if (Toilet) {
        if (!VeriInt(Toilet.val())) {
            VerifyErrorStyle(Toilet);
            alert("洗手间数必须为整数");
            return false;
        }
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
    var un = $("#txt_un").val();
    if (!un) {
        un = "";
    }
    $("#UnitNo").val(un + "$" + $("#txt_hn").val());

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