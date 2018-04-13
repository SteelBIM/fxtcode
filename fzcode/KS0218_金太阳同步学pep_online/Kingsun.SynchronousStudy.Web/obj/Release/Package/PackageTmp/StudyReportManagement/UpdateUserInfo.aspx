<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateUserInfo.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.UpdateUserInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>修改姓名</title>
    <link href="../AppTheme/css/hear.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="head_back">
                <a class="h_left" href="javascript:history.go(-1)"></a>
                <a class="h_close" id="close">[关闭]</a>
                <h2>修改姓名</h2>
            </div>
            <div class="Html8">
                <div class="name">
                    <input id="name" type="text" value="" />
                    <a id="clear"></a>
                </div>
                <a class="keepname" href="javascript:void(0);" id="saveName"><span>保存姓名</span></a>
                <div class="overlay" style="display: none" id="overlay">
                    <!--弹出层时背景层DIV-->
                    <div id="fade" class="black_overlay">
                    </div>
                    <div class="box2" id="box2">
                        <p>确认要修改吗？</p>
                        <a class="no" id="cancel">取消</a>
                        <a class="yes" id="sure">确认</a>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <%--<script src="../AppTheme/js/artDialog/artDialogAll.js?skin=aero"></script>--%>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/MobileTerminal/JsBridge.js"></script>
    <script src="../Scripts/MobileTerminal/mobile.js"></script>
    <script src="../Scripts/MobileTerminal/demo.js"></script>
    <script src="../Scripts/MobileTerminal/UpdateUserInfoInit.js"></script>
</body>
</html>
