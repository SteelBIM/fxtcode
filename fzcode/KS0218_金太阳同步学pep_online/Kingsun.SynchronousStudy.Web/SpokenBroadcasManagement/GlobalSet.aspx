<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GlobalSet.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.SpokenBroadcasManagement.GlobalSet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>全局设置</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <div>
                    <h4>同步学平台>>设置>>全局设置</h4>
                </div>
                <div id="tbdatagrid"></div>
            </div>
            <%--修改全局信息DIV--%>
            <div id="divUpdateGlobalSet" style="display: none; border: 0px solid red;">
                <table style="padding: 10px; width: 100%; height: auto; text-align: center; border: 1px dashed gray;">
                    <tr>
                        <td colspan="6" style="text-align: center; border-bottom: 1px dashed gray;">
                            <h1 style="font-size: 16px;">全局信息</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>ID:</td>
                        <td>
                            <span id="txtID"></span>
                        </td>
                        <td style="text-align:right;"><span style="color: red;">*</span>提前进入时间:</td>
                        <td style="text-align:left;">
                            <input type="text" id="txtAheadMinutes" />
                        </td>
                        <td style="text-align:right;"><span style="color: red;">*</span>人数限制:</td>
                        <td style="text-align:left;">
                            <input type="text" id="txtLimitNum" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../Scripts/Common/ajaxfileupload.js"></script>
    <script src="../AppTheme/js/jquery-easyui/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../AppTheme/js/jquery.json-2.4.js"></script>
    <script src="../AppTheme/js/jquery.cookie.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../AppTheme/js/Common.js"></script>
    <script src="../AppTheme/js/fzcontrols/jquery.form.controls.js"></script>
    <script src="../AppTheme/js/FzJsControl/Plugins/Kingsun.Select.js"></script>
    <script src="../AppTheme/WdatePicker.js"></script>
    <script src="../Scripts/SpokenBroadcasManagement/GlobalSet.js"></script>
</body>
</html>
