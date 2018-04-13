<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HearResourcesInfo.aspx.cs"
    Inherits="Kingsun.SynchronousStudy.Web.HearResources.HearResourcesInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=0.5, user-scalable=0"
        name="viewport" />
    <title>说说看</title>
    <link href="css/base.css" rel="stylesheet" type="text/css" />
    <link href="css/report.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="js/common.js"></script>
</head>
<body>
    <audio id="audio1">
        <source src="">
        <%--<source src="audio/ceshi.ogg">--%>
            您的浏览器不支持audio标签。 
    </audio>
    <header class="head1">
        <img src="img/bagg.png" alt="" />
        <h2><span id="averageScore"></span>分</h2>
        <p>完成时间：<span class="time"></span></p>
    </header>
    <main class="main1">
        <section class="section1">

        </section> 
        <section class="section2">

        </section> 
    </main>

    <script src="js/jquery-1.11.2.min.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="js/HearResourcesInfoInit.js"></script>
</body>
</html>
