<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FxtGjbApi.aspx.cs" Inherits="FxtDemo.FxtGjbApi.FxtGjbApi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
    var ars =<%=jsArgs %>;
    console.log(ars);
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        路径： 
        <asp:DropDownList ID="ddlRoute" runat="server" Height="24px" Width="211px">
            <asp:ListItem Value="http://localhost:37335/gjb/active">估价宝OA接口</asp:ListItem>
            <asp:ListItem Value="http://192.168.0.124:80/datacenterservice/active">数据中心</asp:ListItem>
            <asp:ListItem Value="http://192.168.0.103:8726/uc/start">用户中心start</asp:ListItem>
            <asp:ListItem Value="http://192.168.0.212:8000/uc/active">小伍用户中心active</asp:ListItem>
            <asp:ListItem Value="http://192.168.0.103:8905/Handler1.ashx">测试一般处理程序</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
    
    </div>
    <asp:TextBox ID="txtArgs" runat="server" Height="259px" TextMode="MultiLine" 
        Width="373px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="acceptTb" runat="server" Height="256px" TextMode="MultiLine" 
        Width="298px"></asp:TextBox>
    <br />
    <asp:Button ID="BtnSubmit" runat="server" onclick="BtnSubmit_Click" 
        Text="提交按钮" />
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="ButtonLogin" runat="server" onclick="ButtonLogin_Click" 
        Text="登录" />
        &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="BtnData" runat="server" onclick="btnData_Click" Text="数据中心" />
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="BtnUC" runat="server" onclick="btnuc_Click" Text="用户中心" />
     &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="Btn09" runat="server" onclick="btn09_Click" Text="估价宝API"  />
     &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnFile" runat="server" onclick="btnFile_Click" Text="发送文件流" />
     <br />
     <asp:Image
            ID="Image1" runat="server" />
    </form>
</body>
</html>
