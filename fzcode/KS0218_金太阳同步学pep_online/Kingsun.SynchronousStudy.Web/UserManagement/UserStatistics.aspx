<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserStatistics.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.UserManagement.UserStatistics" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>用户列表</title>
    <script src="../AppTheme/WdatePicker.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            选择版本：<asp:DropDownList ID="ddlVersion" runat="server"></asp:DropDownList>
            使用时长大于：<asp:TextBox ID="txtUseTime" runat="server"></asp:TextBox><asp:DropDownList ID="ddlTime" runat="server">
                <asp:ListItem Value="0">秒</asp:ListItem>
                <asp:ListItem Value="1">分</asp:ListItem>
                <asp:ListItem Value="2">时</asp:ListItem>
            </asp:DropDownList>
            注册时间段：<input id="txtStartDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndDate\',{d:-1})}' })" />
            <input id="txtEndDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'txtStartDate\',{d:1})}' })" /><br />
            手机号：<asp:TextBox ID="txtTelePhone" runat="server"></asp:TextBox>
            班级ID：<asp:TextBox ID="txtClassID" runat="server"></asp:TextBox>
            最后登录时间段：<input id="txtLoginStart" runat="server" type="text" class="Wdate" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'txtLoginEnd\',{d:-1})}' })" />
            <input id="txtLoginEnd" runat="server" type="text" class="Wdate" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'txtLoginStart\',{d:1})}' })" />
            <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
            <asp:Button ID="btnExportExcel" runat="server" Text="导出用户数据" OnClick="btnExportExcel_Click" />
            <%--<asp:Button ID="btnUpdate" runat="server" Text="更新用户数据" OnClick="btnUpdate_Click" />--%>
        </div>
        <div>
            <asp:Repeater ID="rpStatistics" runat="server" OnItemCommand="rpStatistics_ItemCommand">
                <HeaderTemplate>
                    <table width="100%" border="1" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>用户名</td>
                            <td>昵称</td>
                            <td>手机号</td>
                            <td>当前版本</td>
                            <td>注册时间</td>
                            <td>7日内使用天数</td>
                            <td>使用时长</td>
                            <td>班级ID</td>
                            <td>最近启动时间</td>
                            <td>操作</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("UserName") %></td>
                        <td><%#Eval("NickName") %></td>
                        <td><%#Eval("TelePhone") %></td>
                        <td><%#Eval("VersionName") %></td>
                        <td><%#Eval("CreateTime") %></td>
                        <td><%#Eval("Number") %></td>
                        <td><%#TimeConversion(Eval("UseTime").ToString()) %></td>
                        <td><%#Eval("ClassShortID") %></td>
                        <td><%#Eval("LoginTime") %></td>
                        <td>
                            <asp:LinkButton ID="lbSerach" runat="server" Text="查看" CommandName="Serach" CommandArgument='<%#Eval("UserID") %>'></asp:LinkButton></td>
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
