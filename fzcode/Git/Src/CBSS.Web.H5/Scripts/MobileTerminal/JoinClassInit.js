/// <reference path="../../../AppTheme/js/jquery-1.11.2.min.js" />

var JoinClassInit = function () {
    var Current = this;
    this.UserID = '';
    this.SchoolID = 0;
    this.ClassID = '';

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
        $("#name").val('');
        Current.ID = '';
        Current.UserID = Common.QueryString.GetValue("UserID");
        Current.StudentID = Common.QueryString.GetValue("StudentID");
        Current.APPID = Common.QueryString.GetValue("AppID");
        Current.GetUserInfo();
        Current.GetClassList();
    };

    //通过用户ID获取用户信息
    this.GetUserInfo = function () {
        var data = {
            UserID: Current.UserID,
            PKey: "", RTime: Common.DateNow()
        };
        var obj = { FunName: "GetUserInfo", Info: $.toJSON(data), FunWay: "0", Flag: "", Key: "" }
        $.post("http://192.168.3.1:8026/dc/active", $.toJSON(obj), function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    $("#name").html(result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName);
                    Current.UserTrueName = result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName;
                    if (result.UserInfo.AvatarUrl != '00000000-0000-0000-0000-000000000000' && result.UserInfo.AvatarUrl != null && result.UserInfo.AvatarUrl != '') {
                        $("#userImg").attr("src", Constant.file_Url + 'GetFiles.ashx?FileID=' + result.UserInfo.AvatarUrl);
                    }
                } else {
                    $("#name").html('');
                }
            }
        });
    }

    //获取老师班级列表
    this.GetClassList = function () {
        var data = {
            UserID: Current.UserID ,
            PKey: "", RTime: Common.DateNow()
        };
        var obj = { FunName: "QueryClassList", Info: $.toJSON(data), FunWay: "0", Flag: "", Key: "" }
        $.post("http://192.168.3.1:8026/dc/active", $.toJSON(obj), function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    var classList = result.ClassList;
                    Current.SchoolID = classList[0].SchoolID;
                    var html = "";
                    for (var i = 0, length = classList.length; i < length; i++) {
                        html += '<li id="' + classList[i].Id + '">' + classList[i].ClassName + '</li>';
                    }
                    $("#classList").html('');
                    $(html).prependTo("#classList");

                    $("#classList li").click(function() {
                        var id = $(this).attr("id");
                        Current.ClassID = id;
                        var liArr = $("#classList li");
                        for (var i = 0, length = liArr.length; i < length; i++) {
                            if ($(liArr[i]).hasClass('on')) {
                                $(liArr[i]).removeClass('on')
                            }
                        }
                        $(this).addClass("on");
                    });
                }
            } else {
                popup("发送请求失败，请重新发送");
            }
        });
    }

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


    //确认你绑定
    $("#sure").click(function() {
        if (Current.ClassID == '') {
            popup("请先选择需要绑定的班级");
            return false;
        }
        var obj = {  };
        var data = {
            ClassID: Current.ClassID, StudentID: Current.StudentID, UserID: Current.UserID,
            PKey: "", RTime: Common.DateNow()
        };
        var obj = { FunName: "StudentBindClass", Info: $.toJSON(data), FunWay: "0", Flag: "", Key: "" }
        $.post("http://192.168.3.1:8026/dc/active", $.toJSON(obj), function (data) {
            if (data) {
                var result = JSON.parse(data); //eval("(" + data + ")");
                if (result.Success) {
                    $("#welcome").html("，欢迎你加入我们的班级！");
                    $(".tan_nr1").css("display", "none");
                    $(".tan_nr2").css("display", "block");
                } else {
                    popup(result.Msg);
                    return false;
                }
            }
        });
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


var joinClassInit;
$(function () {
    joinClassInit = new JoinClassInit();
    joinClassInit.Init();
});