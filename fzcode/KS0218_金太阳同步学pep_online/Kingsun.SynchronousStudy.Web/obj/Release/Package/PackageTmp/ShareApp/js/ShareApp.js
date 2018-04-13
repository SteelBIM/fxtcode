/// <reference path="../../../AppTheme/js/jquery-1.11.2.min.js" />

var ShareApp = function () {
    var Current = this;
    this.CheckCode = '';//验证码
    this.Telephone = 0;
    this.file_Url = "http://file.kingsun.cn/";

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
    }

    this.Init = function () {
        Current.UserID = Common.QueryString.GetValue("UserID");

        Current.UserImage = Common.QueryString.GetValue("UserImage");

        var obj = { UserID: Current.UserID };
        $.post("Handler/ShareHandler.ashx?queryKey=getusername", obj, function (data) {
            var result = JSON.parse(data);
            if (result.Success) {
                $("#userName").html(result.userName);
                $("#img1").attr("src", result.UserImg);
            }
        });
        //if (Current.UserImage != "00000000-0000-0000-0000-000000000000") {
        //    $("#img1").attr("src", Current.file_Url + 'GetFiles.ashx?FileID=' + Current.UserImage);
        //}


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
            $.post("Handler/ShareHandler.ashx?queryKey=sendCode", obj, function (data) {
                if (data) {
                    var result = eval("(" + data + ")");
                    if (result.Success) {
                        Current.CheckCode = result.CheckCode;
                        popup("验证码已发送，请注意查收！");
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
            var code = $.trim($('#verifyCode').val());
            //if (Current.CheckCode != '') {
            //    if (code != Current.CheckCode) {
            //        popup("请输入正确的验证码");
            //        return false;
            //    } else {
            Current.Login();
            //    }
            //} else {
            //    popup("请先获取验证码");
            //    return false;
            //}
        });

        //登录(测试)
        //$("#login").click(function () {
        //    Current.Login();
        //})

        $("#clearTele").click(function () {
            $("#telephone").val("");
        });

        $("#clearCode").click(function () {
            $("#verifyCode").val("");
        });
    };

    this.Login = function () {
        Current.Telephone = $.trim($("#telephone").val());
        //Current.Telephone = '13845235722';

        var obj = { Telephone: Current.Telephone, Code: $.trim($('#verifyCode').val()) };
        $.post("Handler/ShareHandler.ashx?queryKey=loginbyphone", obj, function (data) {
            var result = JSON.parse(data);
            Current.UserID = result.UserID;
            if (result.Success) {
                window.location.href = "http://tbx.kingsun.cn/downloadList.html";
            } else {
                popup(result.Msg);
            }
        });

    }
}


var shareApp;
$(function () {
    shareApp = new ShareApp();
    shareApp.Init();
});