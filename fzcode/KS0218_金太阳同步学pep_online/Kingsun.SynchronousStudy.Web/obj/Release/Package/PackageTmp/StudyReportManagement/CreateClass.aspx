<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateClass.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.CreateClass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>班级管理</title>
    <link href="../AppTheme/css/hear.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="head_back">
                <a class="h_close" id="close" style="font-family: '宋体';margin-right: 50px;font-size: 25px;font-weight: 600;"><</a>
                <h2>班级管理</h2>
            </div>
            <div class="Html3">
                <div class="peo_img">
                    <img src="../AppTheme/images/peoples.png" alt="" />
                </div>
               
                <%--<asp:Button ID="btnAdd" runat="server" Text="创建班级" OnClick="btnAdd_Click" />--%>
                <p>别让孩子们继续流浪了，</p>
                <p  class="p1">赶快创建属于你们的班级吧</p>
                 <a id="addClass" runat="server">创建班级</a>
            </div>
        </div>
        <asp:HiddenField ID="hfID" runat="server" />

    </form>

    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/MobileTerminal/JsBridge.js"></script>
    <script src="../Scripts/MobileTerminal/mobile.js"></script>
    <script src="../Scripts/MobileTerminal/demo.js"></script>
    <script src="../Scripts/MobileTerminal/CreateClassInit.js"></script>
</body>
</html>
