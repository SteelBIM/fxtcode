<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectRole.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.ClassManagement.SelectRole" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>修改</title>
    <script src="../Js/jquery-1.10.2.min.js"></script>
    <script src="../Js/mobile.js"></script>
    <script src="../Js/CommonDB.js"></script>
    <script src="../Js/OfficialAccounts/OfficialAccounts.js"></script>
    <link href="../Css/OfficialAccounts/OfficialAccounts.css" rel="stylesheet" />
    <style>
        body{text-align:center}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <asp:Button ID="btnTea" Width="150" runat="server" Text="老师" class="get" OnClick="btnTea_Click"  />
                <br />
                <br />
                <asp:Button ID="btnStu" Width="150" runat="server" Text="学生" class="get" OnClick="btnStu_Click" />
                <br />
                <br />
                <asp:Button ID="btnDel" Width="150" runat="server" Text="清除学校" class="get" OnClick ="btnDel_Click" />
                <br />
                <br />
                <asp:Button ID="btnOut" Width="150" runat="server" Text="退出登陆" class="get" OnClick ="btnOut_Click" />
                <asp:HiddenField ID="hfUserid" runat="server" />
                <asp:HiddenField ID="hfOpenId" runat="server" />
            </div>
        </div>

    </form>
</body>
</html>
