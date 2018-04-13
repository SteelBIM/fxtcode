<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LR_ClassInfo.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.LearningReportInfo.LR_ClassInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>班级学生学习情况</title>
    <script type="text/javascript" src="../../AppTheme/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../../Scripts/MobileTerminal/mobile.js"></script>
    <script type="text/javascript" src="../../Scripts/MobileTerminal/demo1.js"></script>
    <link href="../../AppTheme/css/WeChat.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../AppTheme/js/CommonDB.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="html5">
                <div class="nr2">
                    <p>学习内容/成绩/作品</p>
                    <div id="content" style="height: 960px" runat="server">
                    </div>
                </div>
            </div>
        </div>
    </form>
     
    <div id="FTitleID" style="display: none" runat="server"></div>
    <div id="STitleID" style="display: none" runat="server"></div>
    <div id="BName" style="display: none" runat="server"></div>
    <div id="BID" style="display: none" runat="server"></div>
    <div id="MName" style="display: none" runat="server"></div>
    <script src="../../Scripts/MobileTerminal/LArea.js"></script>

    <script>
        var userId = Common.QueryString.GetValue("UserID");
        var ClassLongID = Common.QueryString.GetValue("ClassID");
        var Times = Common.QueryString.GetValue("Times");
        var fid = Common.QueryString.GetValue("fTitleId");

        $(".userinfo").click(function () {
            var ftitle = $('#FTitleID').html() + "_" + $('#STitleID').html();
            window.location = "LR_StuStudyInfo.aspx?UserID=" + this.id + "&BookID=" + $('#BID').html() + "&fTitleId=" + ftitle + "&ClassID=" + ClassLongID;
        });

        function top() {
            $(".gear")[0].style["-webkit-transform"] = 'translate3d(0px,0em,0px)';
            $(".gear")[0].setAttribute('top', '0em');
        }

    </script>
</body>
</html>
