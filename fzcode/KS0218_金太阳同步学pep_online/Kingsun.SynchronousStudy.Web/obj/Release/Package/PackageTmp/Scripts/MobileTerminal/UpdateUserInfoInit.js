/// <reference path="../../../AppTheme/js/jquery-1.11.2.min.js" />

var TeacherInfoInit = function () {
    var Current = this;
    this.UserID = '';
    this.UserTrueName = '';
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
        Current.GetUserInfo();
    };

    //通过用户ID获取用户信息
    this.GetUserInfo = function () {
        var obj = { UserID: Current.UserID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=GetUserName", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    $("#name").val(result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName);
                    Current.UserTrueName = result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName;
                    Current.SchoolID = result.UserInfo.SchoolID;
                    Current.SchoolName = result.UserInfo.SchoolName;
                } else {
                    $("#name").val('');
                }
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

    //清空姓名
    $("#clear").click(function () {
        $("#name").val('');
    });

    $("#saveName").click(function () {
        $(".overlay").css("display", "block");
    });

    $("#cancel").click(function () {
        $(".overlay").css("display", "none");
    });

    $("#sure").click(function () {
        $(".overlay").css("display", "none");
        Current.UpdateUserInfo();
    });

    this.UpdateUserInfo = function () {
        var name = $.trim($("#name").val());
        var reg = /^[\u4e00-\u9fa5][0-9\u4e00-\u9fa5]+$/;
        if (name == '') {
            popup("姓名不能为空");
            return false;
        }
        else if (!reg.test(name)) {
            popup("姓名要以汉字开头，中文或数字结尾");
            return;
        }
        else if (name == Current.UserTrueName) {
            popup("修改成功");
            var time = 3;
            //setInterval(function () {
            //    time--;
            //    if (time == 0) {
            window.location.href = "TeacherInfo.aspx?UserID=" + Current.UserID;
            //    }
            //}, 1000)
        } else {
            var obj = { UserID: Current.UserID, TrueName: name, SchoolID: Current.SchoolID, SchoolName: Current.SchoolName }
            $.post("../Handler/WeChatHandler.ashx?queryKey=updateuserinfobylocal", obj, function (data) {
                if (data) {
                    var result = eval("(" + data + ")");
                    if (result.Success) {
                        popup("修改成功");
                        var time = 3;
                        //setInterval(function() {
                        //    time--;
                        //    if (time == 0) {
                        window.location.href = "TeacherInfo.aspx?UserID=" + Current.UserID;
                        //}
                        //}, 1000);
                    } else {
                        popup(result.ErrorMsg);
                        return false;
                    }
                }
            });
        }
    }
}


var teacherInfoInit;
$(function () {
    teacherInfoInit = new TeacherInfoInit();
    teacherInfoInit.Init();
});

function countDown(secs, userid) {
    if (--secs > 0) {
        setTimeout("countDown(" + secs + ")", 1000);
    } else {
        window.location.href = "TeacherInfo.aspx?UserID=" + userid;
    }
    countDown(secs);
}