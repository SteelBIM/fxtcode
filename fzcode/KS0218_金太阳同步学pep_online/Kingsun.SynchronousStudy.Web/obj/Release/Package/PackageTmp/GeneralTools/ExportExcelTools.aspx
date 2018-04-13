<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportExcelTools.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.GeneralTools.ExportExcelTools" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>下载模板</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="lbBookName" runat="server" Text="Label"></asp:Label>
        <div>
            <asp:Repeater ID="rpModule" runat="server" OnItemCommand="rpModule_ItemCommand">
                <HeaderTemplate>
                    <table width="100%" border="1" cellpadding="0" cellspacing="0">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#(Eval("ModuleName").ToString())%>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbDownloadExcel" runat="server" CommandName="down" CommandArgument='<%#Eval("ID")+","+Eval("ModuleID") %>'>下载模板</asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
