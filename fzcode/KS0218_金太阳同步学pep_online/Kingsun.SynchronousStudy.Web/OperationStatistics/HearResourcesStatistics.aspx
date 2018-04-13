<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HearResourcesStatistics.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.OperationStatistics.HearResourcesStatistics" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>说说看</title>
        <script src="../AppTheme/WdatePicker.js"></script>
</head>
<body>
     <form id="form1" runat="server">
        <div>
            选择版本：<asp:DropDownList ID="ddlVersion" runat="server"></asp:DropDownList>
            书籍名：<asp:TextBox ID="txtBookName" runat="server"></asp:TextBox>
            资源上架时间段：<input id="txtLoginStart" runat="server" type="text" class="Wdate" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'txtLoginEnd\',{d:-1})}' })" />
            <input id="txtLoginEnd" runat="server" type="text" class="Wdate" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'txtLoginStart\',{d:1})}' })" />
            <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
        </div>
        <div>
            <asp:Repeater ID="rpStatistics" runat="server" >
                <HeaderTemplate>
                    <table width="100%" border="1" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>序号</td>
                            <td>说说看文本</td>
                            <td>资源上架时间</td>
                            <td>配音次数</td>
                            <td>配音人数</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("num") %></td>
                        <td><%#Eval("TextDesc") %></td>
                        <td><%#Eval("CreateDate") %></td>
                        <td><%#Eval("VideoNum") %></td>
                        <td><%#Eval("VideoPeopleCount") %></td>
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
