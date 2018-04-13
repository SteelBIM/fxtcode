var SignIn = function () {
    var current = this;
    this.CheckCode = '';//验证码
    this.Telephone = 0;
    this.randomChar = '';
    this.userTel = '';


    this.Init = function () {
        current.Type = window.Common.QueryString.GetValue("Type");
        //获取验证码
        $("#getCheckCode").click(myclick = function () {
            var validCode = true;
            var telephone = $.trim($("#telephone").val());
            var pattern = /^(0|86|17951)?(13[0-9]|15[012356789]|17[0678]|18[0-9]|14[57])[0-9]{8}$/;
            if (!pattern.test(telephone)) {
                //popup("请输入正确的手机号", "2");
                $(".hint").html("请输入正确的手机号");
                return false;
            }
            current.Telephone = telephone;
            var obj = { Telephone: telephone };
            $.post("../Handler/WeChatHandler.ashx?queryKey=sendCode", obj, function (data) {
                if (data) {
                    var result = eval("(" + data + ")");
                    if (result.Success) {
                        current.CheckCode = result.CheckCode;
                        current.randomChar = result.RandomChar;
                        current.userTel = result.TelePhone;
                        // art.dialog.tips('验证码已发送，请注意查收！');
                        $(".hint").html("验证码已发送，请注意查收！");
                        var time = 300;
                        $("#getCheckCode").css("backgroundColor", "#C7C6C6");
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

        // 登录
        $("#login").click(function () {
            current.Login();
        });

        $("#clearTele").click(function () {
            $("#telephone").val("");
        });

        $("#clearCode").click(function () {
            $("#verifyCode").val("");
        });
    };

    this.Login = function () {
        current.Telephone = $.trim($("#telephone").val());
        var obj = { Telephone: current.Telephone };
        $.post("../Handler/WeChatHandler.ashx?queryKey=GetUserInfoByTelephone", obj, function (data) {
            var result = JSON.parse(data);
            if (result.Success) {
                if (result.UserInfo.UserType != 12) {
                    $(".box1").css("display", "block");
                    $(".shadow1").css("display", "block");
                } else {
                    current.loginbyphone();
                }
            } else {
                current.loginbyphone();
            }
        });

    }

    //弹窗--关闭按钮
    $("#close").click(function () {
        $(".box1").css("display", "none");
        $(".shadow1").css("display", "none");
    });

    //弹窗--确认按钮
    $("#sure").click(function () {
        current.loginbyphone();
    });

    //通过手机登陆
    this.loginbyphone = function () {
        var obj = { Telephone: current.Telephone, Type: 12, Code: $.trim($('#verifyCode').val()), OpenId: $('#hfOpenId').val() };
        $.post("../Handler/WeChatHandler.ashx?queryKey=loginbyphone", obj, function (data) {
            var result = JSON.parse(data);
            current.UserID = result.UserID;
            if (result.Success) {
                var obj = { UserID: current.UserID };
                $.post("../Handler/WeChatHandler.ashx?queryKey=GetUserInfoByUserId", obj, function (data) {
                    var result = JSON.parse(data);
                    if (result.Success) {
                        if (result.UserInfo.SchoolID == 0) {
                            window.location.href = "../ClassManagement/PerfectData.aspx?UserID=" + current.UserID;
                        } else {
                            window.location.href = "../StudyReport/ClassList.aspx?UserID=" + current.UserID;
                        }
                    }
                });
            } else {
                $(".box1").css("display", "none");
                $(".shadow1").css("display", "none");
                popup(result.Msg);
            }
        });
    }
}

var signInInit;
$(function () {
    signInInit = new SignIn();
    signInInit.Init();
});