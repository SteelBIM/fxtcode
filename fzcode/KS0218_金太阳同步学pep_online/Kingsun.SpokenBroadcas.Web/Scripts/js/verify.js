// JavaScript Document

$(function () {

    yanzheng1();
    yanzheng2();

    $("#Name").blur(function () {
        yanzheng1();
    });
    $("#Phone").blur(function () {
        yanzheng2();
    });
    $("#btn_Submit").click(function () {
       
        //if ($(".cominformation .main .phoneinpt span").hasClass("good") && $(".cominformation .main .nameinpt span").hasClass("good")) {
        //    $.post("/ConfirmInfo/UpdateUserInfo", { TrueName: $("#Name").val(), TelePhone: $("#Phone").val() }, function (data) {
        //        data = eval(data);
        //        if (data.Success) {
        //            //支付页面
        //            alert('修改成功，进入支付页面');
        //        } else {
        //            alert(data.Msg);
        //        }
        //    });
        //}
    });
    /*点击遮罩弹窗消失*/
    $(".cominformation .shadow1").click(function () {
        //$(".cominformation .box").css("display", "none");
        $(".cominformation .box1").css("display", "none");
        $(".cominformation .shadow1").css("display", "none");
    });

});
/*验证姓名*/
function yanzheng1() {
    if (checkChineseName($("#Name").val())) {
        $(".cominformation .name h3 span").addClass("error");
        $(".cominformation .main .nameinpt span").removeClass("good");
        return false;
    }
    else {
        $(".cominformation .name h3 span").removeClass("error");
        $(".cominformation .main .nameinpt span").addClass("good");
    }

}
/*验证手机号*/
function yanzheng2() {
    if (!checkMobileStrict($("#Phone").val())) {

        $(".cominformation .phone h3 span").addClass("error");
        $(".cominformation .main .phoneinpt span").removeClass("good");
        return false;
    }
    else {
        $(".cominformation .phone h3 span").removeClass("error");
        $(".cominformation .main .phoneinpt span").addClass("good");
    }
}

function checkChinese(str) {
    var re = /[^\u4e00-\u9fa5]/;
    if (re.test(str)) {
        return false;
    }
    return true;
}

function checkChineseName(v) {
    if (v == '') return 1;
    if (v.length < 2) {
        return 2;
    }
    var name = v.replace(/·/g, '');
    name = name.replace(/•/g, '');
    if (checkChinese(name)) {
        return 0;
    }
    else {
        return 2;
    }
}

function checkMobileStrict(v) {
    if (/^1[3|4|5|7|8][0-9]\d{8}$/.test(v)) {
        return true;
    } else {
        return false;
    }
}