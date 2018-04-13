<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LR_StuStudyInfo.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.LearningReportInfo.LrStuStudyInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
      <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>学生学习详情</title>
    <script type="text/javascript" src="../../AppTheme/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../../Scripts/MobileTerminal/mobile.js"></script>
    <script type="text/javascript" src="../../Scripts/MobileTerminal/demo1.js"></script>
    <link href="../../AppTheme/css/WeChat.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="html9">
                <div class="nr1">
                    <div class="nr_img">
                        <img src="<%=ImgUrl %>" />
                    </div>
                    <div class="nr_xx">
                        <h2><b id="UserName" runat="server"></b></h2>
                        <%-- <p>已学习：<span>1分钟48秒</span></p>--%>
                    </div>

                </div>
                <div class="nr2">
                    <b class="danyuan" id="ModuleName" runat="server"></b>
                    <span class="ceben" runat="server" id="JuniorGrade"></span>
                    <div id="content" style="height: 960px" runat="server">
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
