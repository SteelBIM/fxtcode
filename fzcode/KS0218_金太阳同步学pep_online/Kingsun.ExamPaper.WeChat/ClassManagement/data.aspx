<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="data.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.ClassManagement.WebForm1" %>

<!DOCTYPE html>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Repeater ID="rpApp" runat="server" OnItemCommand="rpApp_ItemCommand">
                <HeaderTemplate>
                    <table width="100%" border="1" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>OpenID</td>
                            <td>TelePhone</td>
                            <td>UserID</td>
                            <td>CreateDate</td>
                            <td>操作</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("OpenID") %></td>
                        <td><%#Eval("TelePhone") %></td>
                        <td><%#Eval("UserID") %></td>
                        <td><%#Eval("CreateDate") %></td>
                        <td>
                            <asp:LinkButton ID="lbDownloadExcel" runat="server" CommandName="dele" CommandArgument='<%#Eval("OpenID") %>'>删除</asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div class="filps">
                <div class="filps_div">
                    <webdiyer:AspNetPager ID="AspNetPager1" runat="server" AlwaysShow="True" CustomInfoHTML="共 %RecordCount% 条数据，当前第  %CurrentPageIndex% / %PageCount% 页"
                        FirstPageText="首页" LastPageText="末页" NextPageText="下一页" PrevPageText="上一页" OnPageChanged="AspNetPager1_PageChanged"
                        PageSize="15" CustomInfoSectionWidth="" ShowCustomInfoSection="Right" ShowNavigationToolTip="True"
                        Wrap="False"
                        Direction="LeftToRight" CustomInfoTextAlign="Center" NumericButtonCount="5" HorizontalAlign="Center"
                        CurrentPageButtonClass="cpb" PagingButtonSpacing="8px">
                    </webdiyer:AspNetPager>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
