<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdataGeneralTools.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.GeneralTools.UpdataGeneralTools" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>更新书籍资源</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Repeater ID="rpModule" runat="server" OnItemCommand="rpModule_ItemCommand">
                <HeaderTemplate>
                    <table width="100%" border="1" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>书籍名称</td>
                            <td>一级标题</td>
                            <td>二级标题</td>
                            <td>一级模块</td>
                            <td>二级模块</td>
                            <td>视频序号</td>
                            <td>操作</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("BookName").ToString()%>
                        </td>
                        <td>
                            <%#Eval("FirstTitle").ToString()%>
                        </td>
                        <td>
                            <%#Eval("SecondTitle").ToString()%>
                        </td>
                        <td>
                            <%#Eval("FirstModular").ToString()%>
                        </td>
                        <td>
                            <%#Eval("SecondModular").ToString()%>
                        </td>
                        <td>
                            <%#Eval("VideoNumber").ToString()%>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbDownloadExcel" runat="server" CommandName="down" CommandArgument='<%#Eval("ID") %>'>下载模板</asp:LinkButton>
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
