// JavaScript Document

$(function () {



    $("#Name").blur(function () {
        yanzheng1();
    });
    $("#Phone").blur(function () {
        yanzheng2();

    });
    $(".box .draw").click(function () {
        if (checkChineseName($("#Name").val()) == 0 && checkMobileStrict($("#Phone").val()) == 0) {
            $.post("Index/AddCollectUserInfo", { Name: $("#Name").val(), Phone: $("#Phone").val(), FromUrl: window.location.href }, function (data) {
                data = eval(data);
                //alert(data.Success);
                if (data.Success) {
                    $(".box").css("display", "none");
                    $(".box1").css("display", "block");
                } else {
                    //alert(data.Msg);
                    $(".box span").addClass("error");
                    $(".box span").html(data.Msg);
                }
            });
        }
        if ($(".box input").val() == "") {
            $(".box span").addClass("error");
            $(".box span").html("请输入姓名和手机号");
        }
    });
});
/*验证姓名*/
function yanzheng1() {
    if (checkChineseName($("#Name").val())) {
        $(".box span").addClass("error");
        $(".box span").html("请输入正确的姓名");
        return false;
    }
    else {
        $(".box span").removeClass("error");
        if (checkMobileStrict($("#Phone").val())) {
            $(".box span").addClass("error");
            $(".box span").html("请输入正确的手机号");
            return false;
        }
    }
}
/*验证手机号*/
function yanzheng2() {
    if (checkMobileStrict($("#Phone").val())) {

        $(".box span").addClass("error");
        $(".box span").html("请输入正确的手机号");
        return false;
    }
    else {
        $(".box span").removeClass("error");
        if (checkChineseName($("#Name").val())) {
            $(".box span").addClass("error");
            $(".box span").html("请输入正确的姓名");
            return false;
        }
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
    var yd = ['134', '135', '136', '137', '138', '139', '150', '151', '152', '157', '158', '159', '187', '188'];
    var lt = ['130', '131', '132', '155', '156', '185', '186'];
    var dx = ['133', '153', '180', '189'];
    var whole = []; whole = whole.concat(yd, lt, dx);
    if (v == '') return 1;
    if (v.length != 11) { return 2; }
    if (isNaN(v)) { return 2; }
    var phone_sect = v.substr(0, 3);
    var find = false;
    var i = 0;
    for (i = 0; (i < whole.length) ; i++)
    { if (phone_sect == whole[i]) { find = true; break; } }
    if (find) {
        return 0;
    }
    else {
        return 2;
    }
}