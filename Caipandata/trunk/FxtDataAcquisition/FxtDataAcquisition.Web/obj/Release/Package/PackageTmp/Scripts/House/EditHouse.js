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
    var txtun = $("#txt_RoomNo").val();
    var floorno = $("#txt_FloorNo").val();
    var housename = $("#txt_HouseName").val();
    if (txtun == "" || txtun == null) {
        VerifyErrorStyle($("#txt_RoomNo"));
        alert("室号不能为空");
        return false;
    }
    if (floorno == "" || floorno == null) {
        VerifyErrorStyle($("#txt_FloorNo"));
        alert("物理层不能为空");
        return false;
    }
    if (!VeriInt(floorno)) {
        VerifyErrorStyle($("#txt_FloorNo"));
        alert("物理层必须为整数");
        return false;
    }
    if (floorno < -5) {
        VerifyErrorStyle($("#txt_FloorNo"));
        alert("物理层不能小于-5");
        return false;
    }
    if (housename == "" || housename == null) {
        VerifyErrorStyle($("#txt_HouseName"));
        alert("房号名称不能为空");
        return false;
    }

    var data = $("form").serializeArray();

    $.extendAjax(
                {
                    url: "/House/SaveHouseDetails",
                    data: data,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    if (data != null) {
                        if (!data.result) {
                            alert(decodeURIComponent(data.message));
                            return;
                        }
                        else {
                            AlertFancybox2(data.message, function () {
                                parent.$.fancybox.close();
                            });
                            parent.location.reload(true);
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
        var patrn = /^(-|\+)?\d+$/;

        if (!patrn.exec(val)) {
            return false;
        }
    }
    return true;
}