<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdataVideoDetails.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.ManageMent.UpdataVideoDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            对白：<asp:TextBox ID="txtDialogueText" runat="server"></asp:TextBox>
            开始时间：<asp:TextBox ID="txtStartTime" runat="server"></asp:TextBox>
            结束时间：<asp:TextBox ID="txtEndTime" runat="server"></asp:TextBox>

            <asp:Button ID="btnAdd" runat="server" Text="保存" OnClick="btnAdd_Click" />
            <asp:Button ID="btnExit" runat="server" Text="返回" OnClick="btnExit_Click" />
        </div>
    </form>
</body>
</html>
