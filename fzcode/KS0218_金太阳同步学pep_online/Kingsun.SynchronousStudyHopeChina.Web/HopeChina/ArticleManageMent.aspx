<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticleManageMent.aspx.cs" Inherits="Kingsun.SynchronousStudyHopeChina.Web.HopeChina.ArticleManageMent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>课程列表</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <script type="text/javascript">
        var s = "";
    </script>
</head>
<body>
    <form id="Form1" runat="server">
        <div>
           
            <div>
                <div>
                    <h4>同步学平台>>希望之星>>文章管理</h4>
                </div>
                <div class="toolBar" id="bookID">
                    <div class="rightTools">
                        <table>
                            <tr>
                                <td>
                                    <input type="text" id="searchValue" value="查询文章标题" />
                                    <a href="javascript:void(0)" id="search">查询</a>
                                </td>
                                <td>请选择excel文件：<asp:FileUpload ID="FileUpload1" runat="server" /><asp:Button ID="btnAdd" runat="server" Text="导入文章资源" OnClick="btnAdd_Click" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tbdatagrid"></div>
            </div>
        </div>
    </form>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../AppTheme/js/jquery.json-2.4.js"></script>
    <script src="../AppTheme/js/jquery.cookie.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../AppTheme/js/fzcontrols/jquery.form.controls.js"></script>
    <script src="../Scripts/Common/easyuiCommon.js"></script>
    <script src="../Scripts/Management/ArticleManagement.js"></script>
    <script src="../Scripts/Init/ArticlePage.js"></script>
    <script>
        $(function() {
            var init =new ArticlePageInit();
            init.Init();
        });
    </script>
</body>
</html>
