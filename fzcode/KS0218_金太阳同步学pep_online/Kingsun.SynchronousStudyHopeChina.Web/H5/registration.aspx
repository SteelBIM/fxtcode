<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registration.aspx.cs" Inherits="Kingsun.SynchronousStudyHopeChina.Web.H5.registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>开始报名</title>
    <link rel="stylesheet" type="text/css" href="css/reset.css"/>
    <link rel="stylesheet" type="text/css" href="css/registSkin.css"/>
     <script src="js/jquery-1.10.2.min.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
     <script src="js/JsBridge.js"></script>
    <script type="text/javascript">
        $(function()
        {
            var userid = Common.QueryString.GetValue("userid");
        });

        function starRegister() {
            var data = {};
            window.WebViewJavascriptBridge.callHandler(
                'startSign', data, function (responseData) {
                    window.location.href = "hopestar.aspx";
                }
            );
            window.location.href = "hopestar.aspx";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="skell">
            <div class="header">
                <a class="return" onclick="Finish()"></a>
                <span><a onclick="Finish()">关闭</a></span>
                <img src="images/stamp.png" />
            </div>
            <div class="content">
                <div class="left">
                    <img src="images/b1.png">
                    <div>
                        <img class="im1" src="images/text1.png" />
                        <img class="im2" src="images/text2.png">
                    </div>
                </div>
                <div class="right">
                    <div class="p1">
                        <p>I Believe I Can Shine!</p>
                        <p>Super Host/Hostess(Public Speaker)!</p>
                        <p>Super English Reader</p>
                        <p>Super English Singer</p>
                    </div>
                    <div class="p2">
                        <p>我是最闪亮的明星双语达人！</p>
                        <p>---双语主持（演讲）达人</p>
                        <p>---双语拼词&阅读达人</p>
                        <p>---英文歌曲达人</p>
                    </div>
                    <p class="p3">报名时间：2017.3.10-2017.3.17</p>
                </div>
                <div class="clear"></div>
                <div class="btom">
                    <a href="DetailPage.html"><img  class="im4" src="images/acbtn2.jpg" /></a>
                    <img onclick="starRegister()" id="btnAction" class="im4" src="images/chop3.jpg"/>
                </div>
                <div class="placeholder"></div>
            </div>
        </div>

    </form>
</body>
</html>
