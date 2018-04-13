 
$(function () { 
    $(".bian .queding").click(function () {
        var UserName = $("#txtUserName").val().trim();
        var TelePhone = $("#txtTelePhone").val().trim();
        if (UserName == "" || TelePhone == "") {
            $(".bian .p3").css("display", "none");
            $(".bian .p1").css("display", "block"); 
        }
        else {
            if (checkMobileStrict($("#txtTelePhone").val())) {
                $.post("/AppointIndex/AddCollectUserInfo", { UserName: UserName, TelePhone: TelePhone }, function (data) {
                    data = eval(data);
                    if (data.Success) {
                        $(".bian p").css("display", "none");
                        $(".box").css("display", "block");
                        $(".shadow1").css("display", "block");
                    } else {
                        $(".bian .p2").css("display", "block");
                    }
                });
            } 
        }
    });
    $(".shadow1").click(function () {
        $(".bian .p1").css("display", "none");
        $(".bian .p2").css("display", "none");
        $(".box").css("display", "none");
        $(".shadow1").css("display", "none"); 
    });
    $("#txtTelePhone").blur(function () {
        yanzheng2();
    });
});
/*验证手机号*/
function yanzheng2() {
    if (!checkMobileStrict($("#txtTelePhone").val())) {
        $(".bian .p1").css("display", "none");
        $(".bian .p3").css("display", "block");
        return false;
    }
    else {
        $(".bian .p3").css("display", "none");
    }
}
function checkMobileStrict(v) {
    if (/^1[3|4|5|7|8][0-9]\d{8}$/.test(v)) {
        return true;
    } else {
        return false;
    }
}



