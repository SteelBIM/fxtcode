<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PushList.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.InterestDubbingGame.PushList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>推送列表</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <style type="text/css">
        #divPush table tr td {
            height: 30px;
            text-align: left;
            border: 0px solid gray;
        } 

        table input {
            width: 90%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <div>
                    <h4>同步学平台>>推送列表>>推送列表</h4>
                </div>
                <div class="toolBar" id="bookID">
                    <div class="rightTools">
                        <table>
                            <tr>
                                <td style="padding-right: 20px;"><a href="javascript:void(0)" id="setting" onclick="AddUpdate(0,0)" data-options="iconCls:'icon-add'" class="easyui-linkbutton">新增</a></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tbdatagrid"></div>
            </div>

            <div id="divPush" style="display: none; border: 0px solid red;"> 
                <table style="padding: 10px; width: 100%; height: auto; text-align: center; border: 1px dashed gray;">
                    <tr>
                        <td colspan="4" style="text-align: center; border-bottom: 1px dashed gray;">
                            <h1 id="hTypeName" style="font-size: 16px;">新增推送</h1>
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>版本:</td>
                        <td>
                            <select id="VersionName">
                                <option value="0">广东版</option>
                                <option value="1">人教版</option>
                            </select>
                        </td>
                        <td>身份:</td>
                        <td>
                            <select id="IdentityType">
                                <option value="1">老师</option>
                                <option value="2">学生</option>
                                <option value="3">普通</option>
                                <option value="4">游客</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>推送版本号:</td>
                        <td>
                            <select id="VersionNumber">
                                <option value="0">V2.6.9</option>
                                <option value="1">V2.7.0</option>
                            </select>
                        </td>
                        <td>推送时间:</td>
                        <td>
                            <input id="PushTime" runat="server" type="text" class="Wdate" onfocus="WdatePicker({readOnly:true,minDate:'%y-%M-%d',dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>跳转到:</td>
                        <td>
                            <select id="Jump">
                                <option value="0">启动</option>
                                <option value="1">模块页面</option>
                                <option value="2">配音比赛报名页面</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>推送内容:</td>
                        <td colspan="3">
                            <textarea id="txtContent" style="width: 95%; min-height: 100px;"></textarea>
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
    <script src="../Scripts/Push.js"></script>
    <script type="text/javascript">
        $(function () {
            //$("#divPush table tr td:odd").attr("style", "text-align:right;padding-right:5px;");
        });
    </script>
</body>
</html>
