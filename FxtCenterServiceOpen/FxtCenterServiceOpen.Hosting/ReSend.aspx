<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReSend.aspx.cs" Inherits="FxtCenterServiceOpen.Hosting.ReSend" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <div>
            文件名<asp:TextBox ID="txtSendFileName" runat="server"></asp:TextBox>
            <asp:Button ID="BtnSendFileInfo" runat="server" Text="发送" OnClick="BtnSendFileInfo_Click" />
        </div>
        <br />
        <br />  
        <div>说明：因网络等原因未发送到 运营中心 的数据 记录为文本文件； 通过此功能重新发送，防止时间过长一次最多发送1000个</div>
    </form>
</body>
</html>
