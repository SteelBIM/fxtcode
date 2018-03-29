<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetValidCode.aspx.cs" Inherits="FxtDemo.FxtGjbApi.GetValidCode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
        <table>
        <tr><td><asp:Label ID="lb_appid" runat="server" Text="appid"></asp:Label></td><td><asp:TextBox ID="txt_appid" runat="server"></asp:TextBox></td></tr>
        <tr><td><asp:Label ID="lb_apppwd" runat="server" Text="apppwd"></asp:Label></td><td><asp:TextBox ID="txt_apppwd" runat="server"></asp:TextBox></td></tr>
        <tr><td><asp:Label ID="lb_signname" runat="server" Text="signname"></asp:Label></td><td><asp:TextBox ID="txt_signname" runat="server"></asp:TextBox></td></tr>
        <tr><td><asp:Label ID="lb_time" runat="server" Text="time"></asp:Label></td><td><asp:TextBox ID="txt_time" runat="server"></asp:TextBox></td></tr>
        <tr><td><asp:Label ID="lb_functionname" runat="server" Text="functionname"></asp:Label></td><td><asp:TextBox ID="txt_functionname" runat="server"></asp:TextBox></td></tr>
        <tr><td><asp:Label ID="lb_appkey" runat="server" Text="appkey"></asp:Label></td><td><asp:TextBox ID="txt_appkey" runat="server"></asp:TextBox></td></tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btn_GetCode" runat="server" Text="生成" OnClick="btn_GetCode_Click" />
            </td>
        </tr>
        </table>
    </form>
</body>
</html>
