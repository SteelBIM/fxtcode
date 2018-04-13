/// <reference path="../../../AppTheme/js/jquery-1.11.2.min.js" />

var TeacherInfoInit = function () {
    var Current = this;
    this.UserID = '';
    this.UserTrueName = '';
    this.SchoolID = 0;
    this.Telephone = '';

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
        var loginState = Common.QueryString.GetValue("UserID");
        Current.UserID = Common.QueryString.GetValue("UserID");
        Current.APPID = window.Common.QueryString.GetValue("AppID");
        if (loginState != "" && loginState != "undefined") {
            $("#name").val('');
            Current.GetUserInfo();
        } else {
            // window.location.href = "Login.aspx?Type=3";
        }
    };

    //通过用户ID获取用户信息
    this.GetUserInfo = function () {
        var data = { UserID: Current.UserID, APPID: Current.APPID, PKey: "", RTime: Common.DateNow() };
        var obj = { FunName: "GetUserName", Info: $.toJSON(data), FunWay: "0", Flag: "", Key: "" }
        $.post("http://192.168.3.1:8024/dc/active", $.toJSON(obj), function (data) {
            if (data) {
                var result = JSON.parse(data);//eval("(" + data + ")");
                if (result.Success) {
                    $("#name").html(result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName);
                    $("#userID").html(result.UserInfo.PersonID);
                    $("#schoolName").html(result.UserInfo.SchoolName == null ? '' : result.UserInfo.SchoolName);

                    Current.SchoolID = result.UserInfo.SchoolID;
                    Current.UserTrueName = result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName;
                    if (result.UserInfo.AvatarUrl != '00000000-0000-0000-0000-000000000000') {
                        if (result.IsEnableOss == 1) {
                            $("#userImg").attr("src", Constant.Ossfile_Url + result.UserInfo.AvatarUrl);
                        } else {
                            $("#userImg").attr("src", Constant.file_Url + 'GetFiles.ashx?FileID=' + result.UserInfo.AvatarUrl);
                        }
                        
                    }
                } else {
                    $("#name").val('');
                }
            }
        });
    }

    $("#ma").click(function () {
        window.location.href = Constant.classInfo_Url +"Class/ClassList.html?UserID=" + Current.UserID + "&AppID=" + Current.APPID;
    });

    $("#xiugai").click(function () {
        window.location.href = Constant.classInfo_Url +"Account/UpdateUserInfo.html?UserID=" + Current.UserID + "&AppID=" + Current.APPID;
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

    $("#log").click(function () {
        var data = { OpenID: $("#divOpenId").html(), PKey: "", RTime: Common.DateNow() };
        var obj = { FunName: "LogOut", Info: $.toJSON(data), FunWay: "0", Flag: "", Key: "" }
        $.post("http://192.168.3.1:8024/dc/active", $.toJSON(obj), function (data) {
            if (data) {
                var result = JSON.parse(data); //eval("(" + data + ")");
                if (result.Success) {
                    //window.location.href = "Login.aspx?Type=3";
                } else {
                    popup("退出登录失败，请重试");
                    return false;
                }
            }
        });
    });
}


var teacherInfoInit;
$(function () {
    teacherInfoInit = new TeacherInfoInit();
    teacherInfoInit.Init();
});