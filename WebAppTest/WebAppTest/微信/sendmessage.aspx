<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sendmessage.aspx.cs" Inherits="WebAppTest.微信.sendmessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    
    <form id="form1" runat="server">
    <div>
        <span style="color: rgb(34, 34, 34); font-family: 'Microsoft YaHei', 微软雅黑, Helvetica, 黑体, Arial, Tahoma; font-size: 14px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 30px; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none;">
        AppId</span>
        <asp:TextBox ID="txtAppid" runat="server" Height="23px" Width="232px">wxec39a42b6ad6735c</asp:TextBox>
&nbsp; <span style="color: rgb(34, 34, 34); font-family: 'Microsoft YaHei', 微软雅黑, Helvetica, 黑体, Arial, Tahoma; font-size: 14px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 30px; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none;">
        AppSecret</span>
        <asp:TextBox ID="txtAppSecret" runat="server" Height="28px" Width="267px">e061941e124146dbc805f909671533af</asp:TextBox>
    <h3></h3>
    <asp:TextBox ID="txtwxopenid" runat="server" Width="279px">ocjayjhOyCFo3D5hN-Elr0jsXp3o</asp:TextBox>

    &nbsp;&nbsp;&nbsp;

    <asp:Button ID="Button1" runat="server" Text="WXopenid检测" onclick="Button1_Click" />
    </div>
    </form>
</body>
</html>
