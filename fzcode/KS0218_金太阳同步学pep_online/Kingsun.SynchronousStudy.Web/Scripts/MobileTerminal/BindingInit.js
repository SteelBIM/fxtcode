/// <reference path="../../../AppTheme/js/jquery-1.11.2.min.js" />

var BindingInit = function () {
    var Current = this;
    this.CheckCode = '';//验证码
    this.Telephone = 0;

    function popup(aaa) {
        var black = document.createElement("div");
        var black1 = document.createElement("div");
        var black2 = document.createElement("div");
        var black3 = document.createElement("p");
        var black4 = document.createTextNode(aaa);
        black.className = 'zong';
        black1.className = 'zhezhao';
        black2.className = 'hezi';
        black3.appendChild(black4);
        black2.appendChild(black3);
        black.appendChild(black1);
        black.appendChild(black2);
        document.body.appendChild(black);
        black1.onclick = function () {
            black.parentNode.removeChild(black);

        }
    };

    this.Init = function () {
        $("#displayZone").css("display", "block");
        $("#hiddenZone").css("display", "none");

        Current.OpenID = $("#openID").html();
        if (!Current.OpenID) {
            popup("用户OPenID获取失败");
            return false;
        }
        Current.getUserOpenIDInfo();
    };

    this.getUserOpenIDInfo = function () {
        var obj = { OpenID: Current.OpenID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=getUserOpenIDInfo", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    $("#displayZone").css("display", "none");
                    $("#hiddenZone").css("display", "block");
                }
            }
        });
    }

    //获取验证码
    $("#getCheckCode").click(myclick = function () {
        var validCode = true;
        var telephone = $.trim($("#telephone").val());
        var pattern = /^(0|86|17951)?(13[0-9]|15[012356789]|17[0678]|18[0-9]|14[57])[0-9]{8}$/;
        if (!pattern.test(telephone)) {
            popup("请输入正确的手机号");
            return false;
        }
        Current.Telephone = telephone;
        var obj = { Telephone: telephone };
        $.post("../Handler/WeChatHandler.ashx?queryKey=sendCode", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    Current.CheckCode = result.CheckCode;
                    popup("验证码已发送，请注意查收！");
                    var time = 60;
                    $("#getCheckCode").css("backgroundColor", "#C7C6C6")
                    var getCode = $('#getCheckCode');
                    //获取验证码清除绑定事件 直到倒计时结束添加绑定事件
                    getCode.unbind("click");
                    //验证码倒计时
                    if (validCode) {
                        validCode = false;
                        var t = setInterval(function () {
                            time--;
                            getCode.html(time + "秒后重新获取");
                            if (time == 0) {
                                clearInterval(t);
                                $('#getCheckCode').css("backgroundColor", "#1C9EEA");
                                getCode.html("获取验证码");
                                validCode = true;
                                getCode.bind("click", myclick);
                            }
                        }, 1000);
                    }
                } else {
                    popup(result.Msg);
                }
            } else {
                popup("发送请求失败，请重新发送");
            }
        });
    });

    // 绑定
    $("#bind").click(function () {
        var code = $.trim($('#verifyCode').val());
        if (Current.CheckCode != '') {
            if (code != Current.CheckCode) {
                popup("请输入正确的验证码");
                return false;
            } else {
                Current.Telephone = $.trim($("#telephone").val());
                var obj = { OpenID: Current.OpenID, Telephone: Current.Telephone };
                $.post("../Handler/WeChatHandler.ashx?queryKey=getuserphone", obj, function (data) {
                    if (data) {
                        var result = JSON.parse(data); //eval("(" + data + ")");
                        if (result.Success) {
                            Current.Bind();
                            return false;
                        } else {
                            popup("当前手机号已绑定,请替换手机号绑定！");
                            return false;
                        }
                    }
                });

            }
        } else {
            popup("请先获取验证码");
            return false;
        }
    });

    $("#clearTele").click(function () {
        $("#telephone").val("");
    });

    $("#clearCode").click(function () {
        $("#verifyCode").val("");
    });

    this.Bind = function () {
        Current.Telephone = $.trim($("#telephone").val());
        var obj = { OpenID: Current.OpenID, Telephone: Current.Telephone };
        $.post("../Handler/WeChatHandler.ashx?queryKey=insertOpenID", obj, function (data) {
            if (data) {
                var result = JSON.parse(data); //eval("(" + data + ")");
                if (result.Success) {
                    //art.dialog.tips(result.Msg, "2");
                    //$("#telephone").val("");
                    //$("#verifyCode").val("");
                    ////window.location.href("http://tbxwxtest.kingsun.cn/WeChat/Handler/Binding.aspx?fon=1");
                    ////$("#bind").unbind('click');
                    //$("#displayZone").css("display", "none");
                    //$("#Div2").css("display", "block");
                    closeWxP();
                    return false;
                } else {
                    popup(result.Msg);
                    return false;
                }
            }
        });
    }

    function closeWxP() {
        WeixinJSBridge.invoke('closeWindow', {}, function (res) {
            //alert(res.err_msg);
        });

    }
}


var bindingInit;
$(function () {
    bindingInit = new BindingInit();
    bindingInit.Init();
});