<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="APPManagement.aspx.cs"
    Inherits="Kingsun.SynchronousStudy.Web.ApplicationManagement.APPManagement" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>APP管理</title>

    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../AppTheme/js/FzJsControl/Plugins/Kingsun.Select.js"></script>
    <style type="text/css">
        .black_overlay {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }

        .white_content {
            display: none;
            position: absolute;
            top: 10%;
            left: 10%;
            width: 80%;
            height: 80%;
            border: 16px solid lightblue;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }

        .white_content_small {
            display: none;
            position: absolute;
            top: 20%;
            left: 30%;
            width: 40%;
            height: 50%;
            border: 16px solid lightblue;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            Common.CodeAjax("do.jsonp", "ED", function (data) {
                var edition = data["ED"];
                $("#selectGrade").append("<option value='0' select>请选择版本</option>");
                $.each(edition, function (e, obj) {
                    $("#selectGrade").append("<option value='" + obj.ID + "'>" + obj.CodeName + "</option>");
                });
                $("select option[value='" + 0 + "']").attr("selected", "selected");
            });


            //$('#btnAddApp').click(function () {
            //    $('#DivApp').toggle();
            //});

        });

        function GetSelect() {
            $("#hfVersionName").val($("#selectGrade  option:selected").text());
            $("#hfVersionID").val($("#selectGrade").val());
        }

        //弹出隐藏层
        function ShowDiv(show_div, bg_div) {
            document.getElementById(show_div).style.display = 'block';
            document.getElementById(bg_div).style.display = 'block';
            var bgdiv = document.getElementById(bg_div);
            bgdiv.style.width = document.body.scrollWidth;
            // bgdiv.style.height = $(document).height();
            $("#" + bg_div).height($(document).height());
        };
        //关闭弹出层
        function CloseDiv(show_div, bg_div) {
            document.getElementById(show_div).style.display = 'none';
            document.getElementById(bg_div).style.display = 'none';
        };


    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h4>同步学平台>>应用管理>>应用列表</h4>
        </div>
        <input id="Button1" type="button" value="添加App" onclick="ShowDiv('MyDiv', 'fade')" />
        <%--<asp:Button ID="btnAddApp" runat="server" Text="添加App"  />--%>

        <asp:HiddenField ID="hfVersionName" runat="server" />
        <asp:HiddenField ID="hfVersionID" runat="server" />
        <div>
            <asp:Repeater ID="rpApp" runat="server" OnItemCommand="rpApp_ItemCommand">
                <HeaderTemplate>
                    <table width="100%" border="1" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>APP版本名</td>
                            <td>创建人</td>
                            <td>创建时间</td>
                            <td></td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("VersionName") %></td>
                        <td><%#Eval("CreatePerson") %></td>
                        <td><%#Eval("CreateDate") %></td>
                        <td>
                            <asp:LinkButton ID="lbDownloadExcel" runat="server" CommandName="down" CommandArgument='<%#Eval("VersionID") %>'>查看应用版本</asp:LinkButton>
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
        <!--弹出层时背景层DIV-->
        <div id="fade" class="black_overlay">
        </div>
        <div id="MyDiv" class="white_content">
            <div style="text-align: right; cursor: default; height: 40px;">
                <span style="font-size: 16px;" onclick="CloseDiv('MyDiv','fade')">关闭</span>
            </div>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>创建人：</td>
                    <td><%=UserInfo.UserName%></td>
                </tr>
                <tr>
                    <td>APP版本：</td>
                    <td>
                        <div>
                            <select id="selectGrade" runat="server">
                                <option></option>
                            </select>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnAdd" runat="server" Text="添加" OnClick="btnAdd_Click" OnClientClick="GetSelect()" /><asp:Button
                ID="btnCancel" runat="server" Text="取消" OnClientClick="" />
        </div>

    </form>
</body>
</html>
