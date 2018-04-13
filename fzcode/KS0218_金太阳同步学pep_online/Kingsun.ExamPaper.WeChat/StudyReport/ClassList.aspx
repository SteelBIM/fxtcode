<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClassList.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.StudyReport.ClassList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>学习报告</title>
    <script src="../Js/jquery-1.10.2.min.js"></script>
    <script src="../Js/CommonDB.js"></script>
    <script src="../Js/mobile.js"></script>
    <script src="../Js/report/report.js"></script>
    <script src="../Js/report/ClassList.js"></script>
    <link href="../Css/report/report.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="classlist">
                <h2>班级列表</h2>
                <ul id="div_Class">
                </ul>
            </div>
        </div>
    </form>
</body>
</html>
