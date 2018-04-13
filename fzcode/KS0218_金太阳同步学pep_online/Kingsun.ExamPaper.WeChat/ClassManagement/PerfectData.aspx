<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerfectData.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.ClassManagement.PerfectData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>完善个人信息</title>
    <script src="../Js/jquery-1.10.2.min.js"></script>
    <script src="../Js/CommonDB.js"></script>
    <script src="../Js/mobile.js"></script>
    <script src="../Js/OfficialAccounts/OfficialAccounts.js"></script>
    <link href="../Css/OfficialAccounts/OfficialAccounts.css" rel="stylesheet" />
    <script src="../Js/OfficialAccounts/PerfectData.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="Html2">
                <div class="name">
                    <span>姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;名：</span>
                    <input id="txtName" type="text" placeholder="请输入姓名" />
                </div>
                <div class="school" id="div_school">
                    <span>学&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;校：</span>
                    <input id="school" type="text" placeholder="请输入学校" />
                    <input id="schoolID" type="hidden" />
                </div>

                <a class="next" id="next">下一步</a>

            </div>

        </div>
    </form>
</body>
</html>
