/// <reference path="../../../AppTheme/js/jquery-1.11.2.min.js" />

var CreateClassInit = function () {
    var Current = this;
    this.UserID = '';
    this.AppID = '';


    this.Init = function () {
        Current.UserID = window.Common.QueryString.GetValue("UserID");
        Current.AppID = window.Common.QueryString.GetValue("AppID");
    };

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

    $("#addClass").click(function () {
        var obj = { UserID: Current.UserID, APPID: Current.APPID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=GetUserName", obj, function (data) {
            if (data) {
                var result = JSON.parse(data); //eval("(" + data + ")");
                if (result.Success) {
                    Current.SchoolID = result.UserInfo.SchoolID;
                    if (result.UserInfo.SchoolID != null && result.UserInfo.SchoolID !== "" && result.UserInfo.SchoolID != 0) {
                        window.location.href = "ChooseClass.aspx?UserID=" + Current.UserID + "&SchoolID=" + Current.SchoolID;
                    } else {
                        window.location.href = "AddInformation.aspx?UserID=" + Current.UserID;
                    }
                }
            }
        });
    });
}


var createClassInit;
$(function () {
    createClassInit = new CreateClassInit();
    createClassInit.Init();
});