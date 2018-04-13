<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportExcel.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.CourseManagement.ExportExcel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>下载模板</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="lbBookName" runat="server" Text="Label"></asp:Label>
        <asp:Button ID="btnAdd" runat="server" Text="下载目录页码模板" OnClick="btnAdd_Click" />
        <div>
            <asp:Repeater ID="rpModule" runat="server" OnItemCommand="rpModule_ItemCommand">
                <HeaderTemplate>
                    <table width="100%" border="1" cellpadding="0" cellspacing="0">
                </HeaderTemplate>

                <ItemTemplate>
                    <tr  style="<%#Eval("ModuleName").ToString()=="点读"?"display:none": ""%>">
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
