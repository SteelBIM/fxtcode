<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserStatisticsInfo.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.UserManagement.UserStatisticsInfo" %>

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
            选择模块：<asp:DropDownList ID="ddlModular" runat="server"></asp:DropDownList>
            选择版本：<asp:DropDownList ID="ddlVersion" runat="server"></asp:DropDownList>
            时间段：<input id="txtStartDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndDate\',{d:-1})}' })" />
            <input id="txtEndDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'txtStartDate\',{d:1})}' })" />
            <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
        </div>
        <div>
            <asp:Repeater ID="rpStatistics" runat="server">
                <HeaderTemplate>
                    <table width="100%" border="1" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>当前版本</td>
                            <td>当前模块</td>
                            <td>一级目录</td>
                            <td>二级目录</td>
                            <td>故事名称</td>
                            <td>得分</td>
                            <td>操作时间</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("BookName") %></td>
                        <td><%#Eval("FirstModular") %></td>
                        <td><%#Eval("FirstTitle") %></td>
                        <td><%#Eval("SecondTitle") %></td>
                        <td><%#Eval("VideoTitle") %></td>
                        <td><%#Eval("TotalScore") %></td>
                        <td><%#Eval("CreateTime") %></td>
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
