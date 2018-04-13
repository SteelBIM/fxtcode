<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registfinish.aspx.cs" Inherits="Kingsun.SynchronousStudyHopeChina.Web.H5.registfinish" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>开始报名</title>
    <link rel="stylesheet" type="text/css" href="css/reset.css" />
    <link rel="stylesheet" type="text/css" href="css/finishSkin.css" />
    <script src="js/jquery-1.10.2.min.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="js/JsBridge.js"></script>
    <script type="text/javascript">
        $(function () {
            var param = Common.QueryString.GetValue("error");
            $("#ptext").val("");
            console.log(param);
            if (param == 'success') {
                $("#ptext").text("恭喜您，报名成功！");
            } else if (param == 'timeout') {
                $("#ptext").text("报名已截止！");
                $("#ptext1").hide();

            }
        });
        function OpentrainingChannel() {
            var data = {};
            window.WebViewJavascriptBridge.callHandler(
                'trainingChannel', data, function (responseData) {

                }
            );
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="skell">
            <div class="header">
                <a class="return" onclick="Finish()"></a>
                <span><a onclick="Finish()">关闭</a></span>
                <img src="images/stamp1.png" />
            </div>
            <div class="content">
                <p class="p1" id="ptext">恭喜您，报名成功！</p>

                <img src="images/acbtn2.png" onclick="OpentrainingChannel()"/>
                <p class="p0" id="ptext1">比赛还未开始,您可以进入训练.</p>
                <p class="textT">比赛规则：</p>
                <div>
                    <b>1.</b><p>报名截止时间： 2017年3月17日；</p>
                </div>
                <div>
                    <b>2.</b><p>按照个人实际情况报名参加相应项目和组别选拔，严禁跨组别报名；</p>
                </div>
                <div>
                    <b>3.</b><p>交卷前有三次朗读机会，系统自动选择最高得分做完最终成绩；</p>
                </div>
                <div>
                    <b>4.</b><p>为了保障您能顺利考出好成绩，建议开始比赛前确认自己的手机处于良好的网络环境和使用环境,以免比赛过程中出现故障，影响比赛。</p>
                </div>
            </div>
        </div>
        <div class="placeholder"></div>

    </form>
</body>
</html>
