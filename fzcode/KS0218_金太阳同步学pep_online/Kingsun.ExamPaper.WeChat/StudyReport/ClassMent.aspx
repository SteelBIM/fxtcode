<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClassMent.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.StudyReport.ClassMent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>创建班级</title>
    <script src="../Js/jquery-1.10.2.min.js"></script>
    <script src="../Js/CommonDB.js"></script>
    <script src="../Js/mobile.js"></script>
    <script src="../Js/report/report.js"></script>
    <link href="../Css/report/report.css" rel="stylesheet" />
    <script src="../Js/report/ClassMent.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="noclass">
                <p>暂无班级，赶快创建吧</p>
                <a id="create">创建班级</a>
            </div>
        </div>
    </form>
</body>
</html>
