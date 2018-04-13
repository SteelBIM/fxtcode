<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VideoDetailsList.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.ManageMent.VideoDetailsList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

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
            <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
        </div>
        <div>
            <asp:Repeater ID="rpVideoDialogue" runat="server" OnItemCommand="rpVideoDialogue_ItemCommand">
                <HeaderTemplate>
                    <table width="100%" border="1" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>书籍名称</td>
                            <td>对白</td>
                            <td>开始时间</td>
                            <td>结束时间</td>
                            <td>操作</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("BookName") %></td>
                        <td><%#Eval("DialogueText") %></td>
                        <td><%#Eval("StartTime") %></td>
                        <td><%#Eval("EndTime") %></td>
                        <td>
                            <asp:LinkButton ID="lbUpdata" runat="server" Text="修改" CommandName="Updata" CommandArgument='<%#Eval("ID") %>'></asp:LinkButton>
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
                        PageSize="15" CustomInfoSectionWidth="" ShowCustomInfoSection="Right" ShowNavigationToolTip="True" Wrap="False"
                        Direction="LeftToRight" CustomInfoTextAlign="Center" NumericButtonCount="5" HorizontalAlign="Center" CurrentPageButtonClass="cpb" PagingButtonSpacing="8px">
                    </webdiyer:AspNetPager>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
