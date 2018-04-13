<%@ Page Title="主页" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.ExamPaperManagement._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>53天天练后台管理系统</title>
    <link href="../Exam_Theme/css/frame.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="head">
            <h1>
                <a href="Default.aspx" title="53天天练后台管理系统">53天天练后台管理系统</a></h1>
            <div class="toptools">
                <div class="adminUser">
                    <b></b><span>
                        <%=UserInfo.UserName %></span>
                </div>
                <ul class="toolbar">
                    <li><b class="li_02"></b><a href="LoginOut.aspx">退出</a></li>
                </ul>
            </div>
        </div>
        <div class="mainbody">
            <div id="left">
                <div class="leftMenu">
                    <h3>53天天练后台管理</h3>
                    <ul class="menuUl" id="menuul">
                        <%=menuHtml %>
                    </ul>
                </div>
            </div>
            <div id="path">
                <a href="Welcome.aspx" id="aDesk" target="iframe1"><em class="homeIco"></em><span>桌面</span></a><div
                    id="divnev">
                </div>
            </div>
            <div id="main">
                <iframe src="Welcome.aspx" name="iframe1" id="iframe1" frameborder="0"></iframe>
            </div>
        </div>
        <div id="foot">
        </div>
        <script src="../Exam_Theme/js/jquery-1.7.1.js"></script>
        <script src="../Exam_Theme/js/frame.js"></script>
    </form>
</body>
</html>