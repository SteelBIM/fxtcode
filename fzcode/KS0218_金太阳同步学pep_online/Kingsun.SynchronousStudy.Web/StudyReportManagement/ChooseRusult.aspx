<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseRusult.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.ChooseRusult" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <link href="../AppTheme/css/hear.css" rel="stylesheet" />
    <title>选择结果</title>
</head>
<body>
    <div class="main">
        <div class="head_back">
            <a class="h_left" href="javascript:history.go(-1)"></a>
            <a class="h_close" id="close">[关闭]</a>
            <h2>选择结果</h2>
        </div>
        <div class="Html13">
            <div class="baogao">
                <div class="bao1">
                    <p>好好学习，</p>
                    <p>长大后当了老师</p>
                </div>
                <img src="../AppTheme/images/dudu.png" alt="" />
                <p class="bao2">就可以使用金太阳教师端咯</p>
            </div>
        </div>
    </div>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../Scripts/MobileTerminal/JsBridge.js"></script>
    <script src="../Scripts/MobileTerminal/demo.js"></script>
    <script src="../Scripts/MobileTerminal/mobile.js"></script>
    <script>
        $(function () {
            $("#close").click(function () {
                //调用移动端接口
                var data = {
                    ////传递的参数json
                    //"data": {
                    //    "MessageStr": MessageStr

                    //}

                };
                //调用移动端的方法
                window.WebViewJavascriptBridge.callHandler(
                    'finish', data, function (responseData) {

                    }
                );
            });
        });
    </script>
</body>
</html>
