<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplicationManage.aspx.cs"
    Inherits="Kingsun.SynchronousStudy.Web.ApplicationManagement.ApplicationManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>应用列表</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            text-align: right;
        }

        .auto-style2 {
            text-align: left;
            width: 182px;
        }

        #appVersion {
            width: 184px;
        }

        #introduce {
            width: 196px;
            height: 53px;
        }

        #verifyValue {
            width: 193px;
        }

        #address {
            width: 135px;
        }
    </style>
</head>
<body>
    <div id="userName" style="display: none"><%=UserInfo.UserName%></div>
    <div>
        <h4>同步学平台>>应用管理>>应用列表</h4>
    </div>
    <form id="form1" runat="server">
        <div>
            <div>
                <input type="button" value="版本更新" id="addNew" />
                <input type="radio" name="version" value="Android" checked="checked" id="AndroidCheck" /><span>Android</span>
                <input type="radio" name="version" value="IOS" id="IOSCheck" /><span>IOS</span>
            </div>
            <div>
                <table id="tbdatagrid">
                </table>
            </div>
            <div id="appInfo" style="display: none;">
                <table class="auto-style1">
                    <tr>
                        <td class="auto-style7">版本类型：</td>
                        <td class="auto-style2">
                            <input class="radio" id="IOS" type="radio" name="form" value="IOS" checked="checked" />IOS
                            <input id="Android" class="radio" type="radio" name="form" value="Android" />Android
                        </td>
                        <td class="auto-style6">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style1">程序版本：</td>
                        <td class="auto-style2">
                            <span>V</span><span id="appVersion"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">更新介绍：</td>
                        <td class="auto-style2">
                            <textarea id="introduce" name="S1" placeholder="最多可输入400个字符！" maxlength="400"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">更新文件：</td>
                        <td class="auto-style2">
                            <span>http://</span><input id="address" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">更新文件MD5值：</td>
                        <td class="auto-style2">
                            <input id="verifyValue" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">强制更新：</td>
                        <td class="auto-style2">
                            <input id="mandatory" type="checkbox" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style7">&nbsp;</td>
                        <td class="auto-style2">
                            <input id="makeSure" type="button" value="确定" />
                            <input id="cancel" type="button" value="取消" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
    <div style="display: none" id="div_UpdateRevision">
        <iframe src="#" id="iframe_UpdateRevision" style="width: 850px; height: 450px;"></iframe>
    </div>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/ApplicationInit.js"></script>
</body>
</html>
