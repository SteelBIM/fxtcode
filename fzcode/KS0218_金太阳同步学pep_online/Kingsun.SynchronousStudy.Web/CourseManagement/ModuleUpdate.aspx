<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModuleUpdate.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.CourseManagement.ModuleUpdate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>模块更新</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .auto-style2 {
            width: 169px;
            text-align: right;
        }

        #TextArea1 {
            height: 100px;
            width: 280px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <h2 id="courseDetails"></h2>
            </div>
            <div>
                <h3 id="unitDetails"></h3>
                <h4 id="moduleDetails"></h4>
            </div>
            <div>
                <table class="auto-style1">
                    <tr>
                        <td class="auto-style2">模块地址：</td>
                        <td>
                            <span>http://</span><input id="moduleAddress" type="text" /></td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <div id="u1372" class="text">
                                <p>
                                    <span>MD5值</span><span style="color: #FF0000;">*</span><span>：</span>
                                </p>
                            </div>
                        </td>
                        <td>
                            <input id="moduleMD5" type="text" /></td>
                    </tr>
                    <tr>
                        <td class="auto-style2"><span>增量包地址：</span></td>
                        <td>
                            <span>http://</span><input id="addModuleAddress" type="text" /></td>
                    </tr>
                    <tr>
                        <td class="auto-style2"><span>增量包MD5值：</span></td>
                        <td>
                            <input id="addModuleMD5" type="text" /></td>
                    </tr>
                    <tr>
                        <td class="auto-style2"><span>模块版本号：</span></td>
                        <td>
                            <span>V</span><input id="moduleVersion" type="text" /></td>
                    </tr>
                    <tr>
                        <td class="auto-style2"><span>更新描述：</span></td>
                        <td>
                            <textarea id="description" name="S1" maxlength="1250" placeholder="最多可输入1250个字符！"></textarea></td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <div id="u1383" class="text">
                                <p>
                                    <span>强制更新</span><span style="color: #FF0000;">*</span><span>：</span>
                                </p>
                            </div>
                        </td>
                        <td>
                            <input id="mandatory" type="checkbox" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td>
                            <input id="confirm" type="button" value="确定" /><input id="cancel" type="button" value="取消" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/ModuleUpdateInit.js"></script>
</body>
</html>
