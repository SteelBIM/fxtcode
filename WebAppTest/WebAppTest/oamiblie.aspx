<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="oamiblie.aspx.cs" Inherits="WebAppTest.oamiblie" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:TextBox ID="sendTb" runat="server" Height="247px" TextMode="MultiLine" 
            Width="429px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="acceptTb" runat="server" Height="248px" TextMode="MultiLine" 
            Width="313px"></asp:TextBox>
        <br />
        
        <asp:Button ID="Button1" runat="server" Text="提交" onclick="Button1_Click" />
    </div>
    </form>
</body>
</html>
