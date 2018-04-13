/// <reference path="../../../AppTheme/js/jquery-1.11.2.min.js" />

var SelectRoleInit = function () {
    var current = this;
    this.CheckCode = '';//验证码
    this.Telephone = 0;
    this.UserType = 0;

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
        current.Type = window.Common.QueryString.GetValue("Type");
        current.UserID = window.Common.QueryString.GetValue("UserID");
        current.AppID = window.Common.QueryString.GetValue("AppID");

        //教师
        $("#tea").click(function () {
            current.UserType = "12";
        });

        //学生
        $("#stu").click(function () {
            current.UserType = "26";
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


        $("#queding").click(function () {
            if (current.UserType != "12" && current.UserType != "26") {
                popup("请选择角色！");
                return false;
            }

            //调用移动端接口
            var data = {
                ////传递的参数json
                //"data": {
                //"UserType": current.UserType
                //}
            };
            //调用移动端的方法
            window.WebViewJavascriptBridge.callHandler(
                'startWebPage', data, function (responseData) {
                    var sc = JSON.parse(responseData);
                    var obj = { UserID: sc.UserId, AppID: sc.AppId };
                    $.post("../Handler/WeChatHandler.ashx?queryKey=getusertypebyid", obj, function (objdata) {
                        var result = JSON.parse(objdata);
                        if (result.Success) {
                            if (current.UserType == "12") {
                                if (result.UserType == current.UserType) {
                                    window.location.href = "ClassList.aspx?UserID=" + sc.UserId + "&AppID=" + sc.AppId;
                                } else {
                                    popup("您尚未认证成为老师，请加添加微信号（tbx010）进行验证！");
                                }
                            } else {
                                window.WebViewJavascriptBridge.callHandler(
                                    'studentIdentity', data, function (responseData) { }
                                    );
                            }

                        }
                    });


                }
            );


        });
    };
}


var selectInit;
$(function () {
    selectInit = new SelectRoleInit();
    selectInit.Init();
});