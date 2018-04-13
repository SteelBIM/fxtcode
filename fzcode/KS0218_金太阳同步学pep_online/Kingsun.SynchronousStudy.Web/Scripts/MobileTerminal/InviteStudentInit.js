/// <reference path="../../../AppTheme/js/jquery-1.11.2.min.js" />

var InviteStudentInit = function () {
    var Current = this;
    this.UserID = '';
    this.SchoolID = 0;

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
        Current.UserID = Common.QueryString.GetValue("UserID");
        Current.APPID = Common.QueryString.GetValue("AppID");
        Current.GetUserInfo();
    };

    //通过用户ID获取用户信息
    this.GetUserInfo = function () {
        var obj = { UserID: Current.UserID, APPID: Current.APPID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=GetUserName", obj, function (data) {
            if (data) {
                var result = JSON.parse(data);//eval("(" + data + ")");
                if (result.Success) {
                    $("#userName").html(result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName);
                    $("#userID").html(result.UserInfo.PersonID);
                    if (result.UserInfo.AvatarUrl != '00000000-0000-0000-0000-000000000000' && result.UserInfo.AvatarUrl != "" && result.UserInfo.AvatarUrl != null) {
                        $("#userImg").attr("src", Constant.file_Url + 'GetFiles.ashx?FileID=' + result.UserInfo.AvatarUrl);
                    }
                    Current.UserTrueName = $("#userName").html();
                } else {
                    popup("用户信息获取失败");
                    return false;
                }
            }
        });
    }

    //发送验证码
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

    $("#next").click(function () {
        var code = $.trim($('#verifyCode').val());
        //if (Current.CheckCode != '') {
        //    if (code != Current.CheckCode) {
        //        popup("请输入正确的验证码");
        //        return false;
        //    } else {

        var obj = { Telephone: $.trim($("#telephone").val()), Type: 26, Code: $.trim($('#verifyCode').val()) };
        $.post("../Handler/WeChatHandler.ashx?queryKey=getClassByTelePhone", obj, function (data) {
            var result = eval("(" + data + ")");
            if (result.Success) {
                if (result.UserType == "12") {
                    popup("老师用户不需要加入班级～");
                    return false;
                } else {
                    window.location.href = "JoinClass.aspx?UserID=" + Current.UserID + "&StudentID=" + result.UserID + "&AppID=" + Current.APPID;
                }
            } else {
                popup(result.Msg);
                return false;
            }
        });

    });

    $("#close").click(function () {
        //调用移动端接口
        var data = {

        };
        //调用移动端的方法
        window.WebViewJavascriptBridge.callHandler(
            'finish', data, function (responseData) {

            }
        );
    });

    $("#linkapp").click(function () {
        if (Current.APPID == "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385") {//广州版
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.gz.syslearning";
        } else if (Current.APPID == "43716a9b-7ade-4137-bdc4-6362c9e1c999") {//上海本地
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.shbd.syslearning";
        }
        else if (Current.APPID == "241ea176-fce7-4bd7-a65f-a7978aac1cd2") {//深圳版
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.sz.syslearning";
        }
        else if (Current.APPID == "9426808e-da8e-488c-9827-b082c19b62a7") {//上海全国
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.shqg.syslearning";
        }
        else if (Current.APPID == "0a94ceaf-8747-4266-bc05-ed8ae2e7e410") {//北京
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.bj.syslearning";
        }
        else if (Current.APPID == "5373bbc9-49d4-47df-b5b5-ae196dc23d6d") {//人教pep
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.rj.syslearning";
        }
        else if (Current.APPID == "37ca795d-42a6-4117-84f3-f4f856e03c62") {//广东
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.gd.syslearning";
        } else {
            window.location.href = "http://tbx.kingsun.cn/downloadList.html";
        }
    });
}


var InviteStudentInit;
$(function () {
    inviteStudentInit = new InviteStudentInit();
    inviteStudentInit.Init();
});