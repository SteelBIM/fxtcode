<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="beforeactivity.aspx.cs" Inherits="Kingsun.SynchronousStudyHopeChina.Web.H5.beforeactivity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>活动等待开始</title>
    <link rel="stylesheet" type="text/css" href="css/reset.css" />
    <link rel="stylesheet" type="text/css" href="css/beforeAcSkin.css" />
    <script src="js/JsBridge.js"></script>
    <script type="text/javascript">
        function changepage() {
            window.location.href = "DetailPage.html";
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
                    <img class="im3" src="images/acbtn2.jpg" onclick="changepage()" />
                    <img class="im4" src="images/chop.png" />
                </div>
                <div class="placeholder"></div>
            </div>
        </div>

    </form>
</body>
</html>
