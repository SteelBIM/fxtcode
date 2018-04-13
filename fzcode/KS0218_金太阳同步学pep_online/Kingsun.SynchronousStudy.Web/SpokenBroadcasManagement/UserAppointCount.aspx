<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAppointCount.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.SpokenBroadcasManagement.UserAppointCount" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>预约用户统计</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <style type="text/css">
        #divShowCoursePeriod .toolBar table tr td {
            border: 0px solid red;
            width: 50%;
            height: 35px;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
        <div>
            <div>
                <div>
                    <h4>同步学平台>>数据统计>>预约用户统计</h4>
                </div>
                <div class="toolBar" id="bookID">
                    <div class="rightTools">
                        <table style="width: 100%;">
                            <tr>

                                <td>
                                    <select id="selType" style="height: 25px;">
                                        <option value="0">联系方式</option>
                                        <option value="1">课程名称</option>
                                        <option value="2">课时名称</option>
                                    </select>
                                    <input type="text" id="searchValue" class="easyui-validatebox"  style="min-width: 150px; height: 20px;" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    上课时间：<input id="txtSearchStartTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                                    至 
                                    <input id="txtSearchEndTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                                    <a href="javascript:void(0)" id="search" class="easyui-linkbutton"
                                        data-options="iconCls:'icon-search'">搜索</a>

                                </td>

                                <td style="width: auto; text-align: right;">
                                    <a href="javascript:void(0)" id="addUserAppoint" class="easyui-linkbutton" data-options="iconCls:'icon-add'" >添加预约用户</a>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    请选择excel文件：<asp:FileUpload ID="FileUpload1" runat="server" />
                                    <asp:Button ID="btnImportExcel" runat="server" Text="导入预约用户" OnClick="btnImportExcel_Click" />
                                    <%--     <a href="javascript:void(0)" id="importExcel" class="easyui-linkbutton" data-options="iconCls:'icon-add'">导入预约用户</a>--%></td>
                                <td style="width: 8%; padding-left: 10px;"><a href="javascript:void(0)" id="exportExcel" class="easyui-linkbutton" data-options="iconCls:'icon-add'">导出EXCEL</a></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tbdatagrid"></div>
            </div>
            <%--显示用户信息DIV--%>
            <div id="divShowUserInfo" style="display: none; border: 0px solid red;">
                <div class="toolBar" style="text-align: center; padding: 20px;">
                    <table style="width: 100%;">
                        <tr>
                            <td>联系方式：<input type="text" id="searchUserInfoValue" class="easyui-validatebox" placeholder="请输入联系方式" style="min-width: 150px; height: 20px;" />
                                <a href="javascript:void(0)" id="searchUserInfoShow" class="easyui-linkbutton"
                                    data-options="iconCls:'icon-search'">搜索</a>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tbdatagridUserInfo"></div>
            </div>
            <%--显示课时信息DIV--%>
            <div id="divShowCoursePeriod" style="display: none; border: 0px solid red;">
                <div class="toolBar" style="text-align: center; padding: 20px;">
                    <table style="width: 100%; border: 0px solid red;">
                        <tr>
                            <td colspan="2" style="text-align: left; height: auto;">
                                <span style="color: #169BD5">用户信息</span>
                            </td>
                        </tr>
                        <tr>
                            <td>用户名：<span id="spanUserName">科比</span>
                            </td>
                            <td>用户ID：<span id="spanUserID">4548745</span></td>
                        </tr>
                        <tr>
                            <td>真实姓名：<span id="spanTrueName">科比</span>
                            </td>
                            <td>联系方式：<span id="spanTelePhone">188888888888</span></td>
                        </tr>
                    </table>
                    <table id="tabCoursePeriod" style="width: 100%; border: 0px solid red;">
                        <tr>
                            <td colspan="2" style="text-align: left; height: auto;">
                                <span style="color: #169BD5">选择课时</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 70%;">课时名称：
                                <input type="text" id="CoursePeriodNameValue" class="easyui-validatebox" placeholder="请输入课时名称" style="min-width: 150px; height: 20px;" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;等级：
                                <input type="text" id="GroupValue" class="easyui-validatebox" placeholder="请输入等级" style="min-width: 150px; height: 20px;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 70%;">上课时间：<input id="txtSelectStartTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                                至 
                                    <input id="txtSelectEndTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                                &nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" id="searchCoursePeriodShow" class="easyui-linkbutton"
                                    data-options="iconCls:'icon-search'">搜索</a>

                            </td>
                        </tr>
                    </table>

                </div>
                <div id="divCoursePeriod">
                    <div id="tbdatagridCoursePeriod"></div>
                </div>
                <div id="divCoursePeriodAppoint" style="display: none;">
                    <div id="tbdatagridCoursePeriodAppoint"></div>
                    <div style="text-align: center; line-height: 100px;">
                        <input id="btnGoOn" type="button" value="继续添加课时" style="background-color: rgba(22, 155, 213, 1); color:white; width: 120px; height: 30px;margin-right: 10px; border:0px;" />
                        <input id="btnOK" type="button" value="确认添加预约" style="background-color: rgba(22, 155, 213, 1); color:white; width: 120px; height: 30px;margin-right: 10px;border:0px;" />
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hiddenAppointCoursePeriod" value="0" />
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
    <script src="../Scripts/SpokenBroadcasManagement/UserAppointCount.js?v=1.0"></script>
</body>
</html>
