<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClassUserList.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.ClassUserList" %>

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
            手机号：<asp:TextBox ID="txtTelephone" runat="server"></asp:TextBox>
            <asp:Button ID="btnSerach" runat="server" Text="搜索" OnClick="btnSearch_Click" />
        </div>
        <div>
            <asp:Repeater ID="rpClassUserList" runat="server">
                <HeaderTemplate>
                    <table width="100%" border="1" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>用户名</td>
                            <td>昵称</td>
                            <td>手机号</td>
                            <td>班级ID</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("UserName") %></td>
                        <td><%#Eval("TrueName") %></td>
                        <td><%#Eval("TelePhone") %></td>
                        <td><%#Eval("ClassShortID") %></td>
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
