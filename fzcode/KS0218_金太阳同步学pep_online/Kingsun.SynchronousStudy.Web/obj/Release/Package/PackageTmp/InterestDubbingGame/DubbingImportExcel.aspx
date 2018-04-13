<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DubbingImportExcel.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.InterestDubbingGame.DubbingImportExcel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            请选择excel文件：<asp:FileUpload ID="flStoryReading" runat="server" />
            <br />
            <asp:DropDownList ID="ddlAddress" runat="server">
                <asp:ListItem Value="0">http://183.47.42.221:8038/uploadfile/SyncCourse/QupeiyinMatch/V1.0/BJ/</asp:ListItem>
                <asp:ListItem Value="1">http://tbxcdn.kingsun.cn/SynchronousStudy/QupeiyinMatch/V1.0/BJ/</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnUpdate" runat="server" Text="更新课本剧路径" OnClick="btnUpdate_Click" />
            <br />
            <asp:Button ID="btnImportSR" runat="server" Text="导入故事朗读" OnClick="btnImportSR_Click" />
            <br />
            <asp:Button ID="btnImport" runat="server" Text="导入课本剧" OnClick="btnImport_Click" />
        </div>
    </form>
</body>
</html>
