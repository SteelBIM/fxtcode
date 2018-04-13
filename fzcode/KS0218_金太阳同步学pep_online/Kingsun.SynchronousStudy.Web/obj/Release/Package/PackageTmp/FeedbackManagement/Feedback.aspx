<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Feedback.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.FeedbackManagement.Feedback" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>用户反馈信息表</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <h4>同步学平台>>用户反馈>>反馈列表</h4>
                <%--<asp:Button ID="Button1" runat="server" Text="导出列表" OnClick="Button1_Click" />--%>
            </div>
            <div>
                <table id="tbdatagrid">
                </table>
            </div>
            <div id="feedBackInfo" style="display:none">
                <textarea id="feedBackDetail" style="height:165px;width:420px;"></textarea>
            </div>
        </div>
    </form>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/FeedBackInit.js"></script>
</body>
</html>
