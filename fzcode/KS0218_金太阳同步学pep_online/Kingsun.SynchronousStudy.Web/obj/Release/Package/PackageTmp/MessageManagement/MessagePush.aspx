<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MessagePush.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.MessageManagement.MessagePush" %>

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
            标题：<asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
            发布时间段：<input id="txtStartDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndDate\',{d:-1})}' })" />
            <input id="txtEndDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'txtStartDate\',{d:1})}' })" /><br />
            截止时间段：<input id="txtLoginStart" runat="server" type="text" class="Wdate" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'txtLoginEnd\',{d:-1})}' })" />
            <input id="txtLoginEnd" runat="server" type="text" class="Wdate" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'txtLoginStart\',{d:1})}' })" />
            <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
        </div>
        <div>
            <asp:Repeater ID="rpMessagePush" runat="server" OnItemCommand="rpMessagePush_ItemCommand">
                <HeaderTemplate>
                    <table width="100%" border="1" cellpadding="0" cellspacing="0">
                        <tr>
                            <td></td>
                            <td>标题</td>
                            <td>发布时间</td>
                            <td>截止时间</td>
                            <td>操作</td>
                            <td>打开次数</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cbox" runat="server" /></td>
                        <td><%#Eval("MessageTitle") %></td>
                        <td><%#Eval("CreateDate") %></td>
                        <td><%#Eval("EndTime") %></td>
                        <td>
                            <asp:LinkButton ID="lbSerach" runat="server" Text="查看" CommandName="Serach" CommandArgument='<%#Eval("ID") %>'></asp:LinkButton>
                            <asp:LinkButton ID="lbUpdata" runat="server" Text="修改" CommandName="Updata" CommandArgument='<%#Eval("ID") %>'></asp:LinkButton>
                            <asp:LinkButton ID="lbDelete" runat="server" Text="删除" CommandName="Delete" OnClientClick="return confirm('确定要删除？')" CommandArgument='<%#Eval("ID") %>'></asp:LinkButton>
                            <asp:LinkButton ID="lbState" runat="server" Text='<%#Eval("State").ToString()=="0"?"审核":"禁用" %>' CommandName="State" CommandArgument='<%#Eval("ID") +","+Eval("State") %>'></asp:LinkButton>
                        </td>
                        <td><%#Eval("OpenNumber") %></td>
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
