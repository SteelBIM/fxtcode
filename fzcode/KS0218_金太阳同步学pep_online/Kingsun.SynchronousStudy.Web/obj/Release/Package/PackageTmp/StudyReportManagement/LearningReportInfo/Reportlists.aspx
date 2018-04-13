<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reportlists.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.LearningReportInfo.Reportlists" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>班级学习情况</title>
    <script type="text/javascript" src="../../AppTheme/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../../Scripts/MobileTerminal/mobile.js"></script>
    <script type="text/javascript" src="../../Scripts/MobileTerminal/demo1.js"></script>
    <script type="text/javascript" src="../../AppTheme/js/CommonDB.js"></script>
    <link href="../../AppTheme/css/calendar.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../../AppTheme/css/stylesheet.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="../../AppTheme/css/simple-calendar.css" />
    <script type="text/javascript" src="../../Scripts/MobileTerminal/Reportlists.js"></script>
</head>
<body>
    <div id="divUserID" style="display: none"></div>
    <div id="divEditionID" style="display: none"></div>
    <div class="main">
        <h2 class="head"><a class="cebie" onclick="chosegrade1();"><span id="span" classid="">一年级1班</span><img src="../../AppTheme/images/la2.png" alt="" /></a></h2>
        <div class="htmlg2">
            <div class="inner clearfix">
                <div id="calendar">
                </div>
            </div>
        </div>
        <!--弹出框-->
        <div class="toolwin">
            <div class="to_ul" id="divClass">
                <%--  <ul class="timelist">
                    <li>一年级1班</li>
                    <li>一年级2班</li>
                    <li>一年级3班</li>
                    <li>一年级4班</li>
                    <li>一年级5班</li>
                </ul>
                <a>全部</a>--%>
            </div>
        </div>
        <div class="maskLayer"></div>
        <div class="date"></div>
    </div>
    <script type="text/javascript" src="../../Scripts/MobileTerminal/simple-calendar.js"></script>
    <script>
        var myCalendar = new SimpleCalendar('#calendar');
        var strjson = "";
        //Common.QueryString.GetValue("EditionID")
        var obj = { EditionID: Common.QueryString.GetValue("EditionID"), UserID: Common.QueryString.GetValue("UserID"), ClassID: $("#span").attr("classid") }
        $.post("../../Handler/LR_WeChatHandler.ashx?queryKey=getreporttime", obj, function (data) {
            if (data) {
                var result = JSON.parse(data);
                if (result.Success) {
                    strjson = JSON.parse(result.data);
                    for (var i = 0; i < strjson.length; i++) {
                        myCalendar.addMark(strjson[i].Time, "上学");
                    }
                }
            }
        });
    </script>
</body>
</html>
