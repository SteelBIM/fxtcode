<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeacherInfo.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.TeacherInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <link href="../AppTheme/css/hear.css" rel="stylesheet" />
    <link href="../AppTheme/css/LArea.min.css" rel="stylesheet" />
    <title>个人中心</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="head_back">
                <a class="h_close" id="close">[关闭]</a>
                <h2 style="margin-right: 60px;">个人中心</h2>
            </div>
            <div class="Html7">
                <div class="head">
                    <img src="../AppTheme/images/touxiang.png" id="userImg" alt="" />
                    <%-- <p id="wealth">
                        <span class="sun">阳光值：<b id="sun"></b><img src="images/yangguang.png" alt="" /></span>
                        <span class="power">能量值：<b id="energy"></b><img src="images/nengliang.png" alt="" /></span>
                    </p>--%>
                </div>
                <ul>
                    <li><a id="xiugai"><span>姓名</span><b><span id="name"></span><img src="../AppTheme/images/xiugai.png" alt="" /></b></a></li>
                    <li><span>学科</span><b>英语</b></li>
                    <li><span>学段</span><b>小学</b></li>
                    <li><span>学校</span><b id="schoolName"></b></li>
                    <li><span>邀请码</span><b id="userID"></b></li>
                </ul>
                <%--<a class="log" id="log" href="javascript:void(0);"><span>退出登录</span></a>--%>
            </div>
        </div>
    </form>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <%--<script src="../AppTheme/js/artDialog/artDialogAll.js?skin=aero"></script>--%>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/MobileTerminal/JsBridge.js"></script>
    <script src="../Scripts/MobileTerminal/mobile.js"></script>
    <script src="../Scripts/MobileTerminal/demo1.js"></script>
    <script src="../Scripts/MobileTerminal/LArea.js"></script>

    <script src="../Scripts/MobileTerminal/TeacherInfoInit.js"></script>

</body>
</html>
