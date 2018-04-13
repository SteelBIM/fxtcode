<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RevisionHistory.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.CourseManagement.RevisionHistory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>历史版本</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
</head>
<body>
    <form runat="server">
        <div>
            <div>
                <table id="tbdatagrid">
                </table>
            </div>
        </div>
    </form>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/RevisionHistoryInit.js"></script>
</body>
</html>
