<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateApplication.aspx.cs"
    Inherits="Kingsun.SynchronousStudy.Web.ApplicationManagement.UpdateApplication" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>版本更新</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            text-align: right;
        }

        .auto-style2 {
            width: 526px;
            text-align: left;
        }

        #appVersion {
            width: 495px;
            margin-left: 0px;
        }

        #introduce {
            height: 120px;
            width: 500px;
        }

        #uploadPerson {
            width: 500px;
        }

        .auto-style3 {
            text-align: right;
            height: 22px;
            width: 180px;
        }

        .auto-style4 {
            width: 526px;
            text-align: left;
            height: 22px;
        }

        #address {
            width: 445px;
        }

        #verifyValue {
            width: 500px;
        }

        .auto-style5 {
            height: 22px;
            width: 307px;
            text-align: left;
        }

        .auto-style6 {
            width: 307px;
            text-align: left;
        }

        .auto-style7 {
            text-align: right;
            width: 180px;
        }
    </style>
</head>
<body>
    <div id="userName" style="display: none"><%=UserInfo.UserName%></div>
    <div>
        <h4>同步学平台>>应用管理>>版本更新</h4>
    </div>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style7">版本类型：</td>
                    <td class="auto-style2">
                        <input class="radio" id="IOS" type="radio" name="form" value="IOS" checked="checked" />IOS<input
                            id="Android" class="radio" type="radio" name="form" value="Android" />
                    Android<td class="auto-style6">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style7">程序版本：</td>
                    <td class="auto-style2">
                        <span>V</span><input id="appVersion" type="text" /></td>
                    <td class="auto-style6">（只需填写相关数字即可）</td>
                </tr>
                <tr>
                    <td class="auto-style7">更新介绍：</td>
                    <td class="auto-style2">
                        <textarea id="introduce" name="S1" placeholder="最多可输入400个字符！" maxlength="400"></textarea>
                    </td>
                    <td class="auto-style6">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style7">上传人：</td>
                    <td class="auto-style2">
                        <span id="uploadPerson"></span>
                    </td>
                    <td class="auto-style6">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3">更新文件：</td>
                    <td class="auto-style4">
                        <span>http://</span><input id="address" type="text" /></td>
                    <td class="auto-style5"><span>（格式：www.xxx.com/xx.apk）</span></td>
                </tr>
                <tr>
                    <td class="auto-style7">更新文件MD5值：</td>
                    <td class="auto-style2">
                        <input id="verifyValue" type="text" /></td>
                    <td class="auto-style6"><span>（使用指定工具获取）</span></td>
                </tr>
                <tr>
                    <td class="auto-style7">强制更新：</td>
                    <td class="auto-style2">
                        <input id="mandatory" type="checkbox" /></td>
                    <td class="auto-style6">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style7">&nbsp;</td>
                    <td class="auto-style2">
                        <input id="makeSure" type="button" value="确定" />
                        <input id="cancel" type="button" value="取消" /></td>
                    <td class="auto-style6">&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/UpdateAppInit.js"></script>
</body>
</html>
