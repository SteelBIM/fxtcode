<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseCount.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.SpokenBroadcasManagement.CourseCount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>课程统计</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <div>
                    <h4>同步学平台>>数据统计>>课程统计</h4>
                </div>
                <div class="toolBar" id="bookID">
                    <div class="rightTools">
                        <table>
                            <tr>
                                <td style="padding-right: 20px;"><a href="javascript:void(0)" id="exportExcel" class="easyui-linkbutton" data-options="iconCls:'icon-add'">导出EXCEL</a></td>
                                <td>
                                    <select id="selType" style="height: 25px;">
                                        <option value="0">课程名称</option>
                                        <option value="1">课时名称</option>
                                    </select>
                                    <input type="text" id="searchValue" class="easyui-validatebox" style="min-width: 150px; height: 20px;" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    上课时间：<input id="txtSearchStartTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                                    至 
                                    <input id="txtSearchEndTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                                    <a href="javascript:void(0)" id="search" class="easyui-linkbutton"
                                        data-options="iconCls:'icon-search'">搜索</a>

                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tbdatagrid"></div>
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
    <script src="../Scripts/SpokenBroadcasManagement/CourseCount.js"></script>
</body>
</html>
