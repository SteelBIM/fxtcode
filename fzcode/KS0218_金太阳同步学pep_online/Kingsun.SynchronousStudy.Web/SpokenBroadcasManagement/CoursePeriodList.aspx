<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CoursePeriodList.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.SpokenBroadcasManagement.CoursePeriodList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>课时管理</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <style type="text/css">
        #divUpdateCoursPeriod table tr td {
            height: 30px;
            text-align: left;
        }

        #divUpdateCoursPeriod table input {
            width: 90%;
        }

        #divAddCoursePeriod table tr td {
            height: 30px;
            text-align: left;
            border: 0px solid gray;
        }

        #divAddCoursePeriod table input {
            width: 90%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <div>
                    <h4>同步学平台>>课程管理>>课时管理</h4>
                    <h1 style="float: right;"><a style="font-size: 18px;" href="CourseList.aspx">返回</a></h1>
                </div>
                <div class="toolBar" id="bookID">
                    <div class="rightTools">
                        <table>
                            <tr>
                                <td style="padding-right: 20px;"><a href="javascript:void(0)" id="addCourse" onclick="addCoursePeriod()" class="easyui-linkbutton" data-options="iconCls:'icon-add'">新增课时</a></td>
                                <td>
                                    <input type="text" id="searchValue" class="easyui-validatebox" placeholder="请输入课时名称" style="min-width: 150px; height: 20px;" /></td>
                                <td>
                                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
            <%--修改图片DIV--%>
            <div id="divUpdateCoursPeriodImg" style="display: none; border: 0px solid red;">
                <table style="padding: 10px; width: 100%; height: auto; text-align: center">
                    <tr>
                        <td>
                            <img id="ImgShow" src="" style="width: 100%; height: 350px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 10px; padding-bottom: 10px;">
                            <input type="file" id="UploadImg" name="UploadImg" />
                        </td>
                    </tr>
                </table>
            </div>
            <%--修改课时DIV--%>
            <div id="divUpdateCoursPeriod" style="display: none; border: 0px solid red;">
                <table style="padding: 10px; width: 100%; height: auto; text-align: center; border: 1px dashed gray;">
                    <tr>
                        <td colspan="4" style="text-align: center; border-bottom: 1px dashed gray;">
                            <h1 style="font-size: 16px;">课时信息</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>课时ID:</td>
                        <td>
                            <span id="txtCoursePeriodID"></span>
                        </td>
                        <td><span style="color: red;">*</span>课时名称:</td>
                        <td>
                            <input type="text" id="txtName" />
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>课时原价:</td>
                        <td>
                            <input type="text" id="txtPrice" />
                        </td>
                        <td><span style="color: red;">*</span>课时现价:</td>
                        <td>
                            <input type="text" id="txtNewPrice" />
                        </td>
                    </tr>
                    <%--  <tr>
                        <td>提前进入时间(分钟):</td>
                        <td>
                            <input type="text" id="txtAheadMinutes" />
                        </td>
                        <td>课时人数:</td>
                        <td>
                            <input type="text" id="txtLimitNum" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td>课时描述:</td>
                        <td colspan="3">
                            <textarea id="txtSummary" style="width: 95%"></textarea>
                        </td>
                    </tr>
                </table>
                <table style="padding: 10px; width: 100%; height: auto; text-align: center; border: 1px dashed gray;">
                    <tr>
                        <td colspan="4" style="text-align: center; border-bottom: 1px dashed gray;">
                            <h1 style="font-size: 16px;">课时时间信息</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>课时时间ID:</td>
                        <td>
                            <span id="txtCoursePeriodTimeID"></span>
                        </td>
                        <td>教师类型:</td>
                        <td>
                            <input type="text" id="txtTeacherType" />
                        </td>
                    </tr>
                    <tr>
                        <td>上课开始时间:</td>
                        <td>
                            <input id="txtStartTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </td>
                        <td>上课结束时间:</td>
                        <td>
                            <input id="txtEndTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', minDate: '%y-%M-%d' })" />
                        </td>
                    </tr>
                </table>
            </div>
            <%--新增课时DIV--%>
            <div id="divAddCoursePeriod" style="display: none; border: 0px solid red;">
                <table style="padding: 10px; width: 100%; height: auto; text-align: center; border: 1px dashed gray;">
                    <tr>
                        <td colspan="4" style="text-align: center; border-bottom: 1px dashed gray;">
                            <h1 style="font-size: 16px;">课时信息</h1>
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>课时名称:</td>
                        <td>
                            <input type="text" id="txtAddName" />
                        </td>
                        <td>课时状态:</td>
                        <td>
                            <select id="selStatus">
                                <option value="0">禁用</option>
                                <option value="1">启用</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>课时原价:</td>
                        <td>
                            <input type="text" id="txtAddPrice" />
                        </td>
                        <td><span style="color: red;">*</span>课时现价:</td>
                        <td>
                            <input type="text" id="txtAddNewPrice" />
                        </td>
                    </tr>
                    <%-- <tr>
                        <td>提前进入时间(分钟):</td>
                        <td>
                            <input type="text" id="txtAddAheadMinutes" />
                        </td>
                        <td>课时人数:</td>
                        <td>
                            <input type="text" id="txtAddLimitNum" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td><span style="color: red;">*</span>课时海报:</td>
                        <td>
                            <input type="file" id="AddUploadImg" name="AddUploadImg" />
                        </td>
                    </tr>
                    <tr>
                        <td>课时描述:</td>
                        <td colspan="3">
                            <textarea id="txtAddSummary" style="width: 95%"></textarea>
                        </td>
                    </tr>
                </table>
                <table style="padding: 10px; width: 100%; height: auto; text-align: center; border: 1px dashed gray;">
                    <tr>
                        <td colspan="6" style="text-align: center; border-bottom: 1px dashed gray;">
                            <h1 style="font-size: 16px;">课时时间信息</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>上课开始时间1:</td>
                        <td>
                            <input id="txtAddStartTime1" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </td>
                        <td>上课结束时间1:</td>
                        <td>
                            <input id="txtAddEndTime1" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', minDate: '%y-%M-%d' })" />
                        </td>
                        <td>教师类型1:</td>
                        <td>
                            <input type="text" id="txtAddTeacherType1" />
                        </td>
                    </tr>
                    <tr>
                        <td>上课开始时间2:</td>
                        <td>
                            <input id="txtAddStartTime2" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </td>
                        <td>上课结束时间2:</td>
                        <td>
                            <input id="txtAddEndTime2" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', minDate: '%y-%M-%d' })" />
                        </td>
                        <td>教师类型2:</td>
                        <td>
                            <input type="text" id="txtAddTeacherType2" />
                        </td>
                    </tr>
                    <tr>
                        <td>上课开始时间3:</td>
                        <td>
                            <input id="txtAddStartTime3" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </td>
                        <td>上课结束时间3:</td>
                        <td>
                            <input id="txtAddEndTime3" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', minDate: '%y-%M-%d' })" />
                        </td>
                        <td>教师类型3:</td>
                        <td>
                            <input type="text" id="txtAddTeacherType3" />
                        </td>
                    </tr>
                </table>
            </div>
            <%--新增课时时间DIV--%>
            <div id="divAddCoursePeriodTime" style="display: none; border: 0px solid red;">
                     <table style="padding: 10px; width: 100%; height: auto; text-align: center; border: 1px dashed gray;">
                    <tr>
                        <td colspan="6" style="text-align: center; border-bottom: 1px dashed gray;">
                            <h1 style="font-size: 16px;">课时时间信息</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>上课开始时间1:</td>
                        <td>
                            <input id="txtAddTimeStartTime1" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </td>
                        <td>上课结束时间1:</td>
                        <td>
                            <input id="txtAddTimeEndTime1" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', minDate: '%y-%M-%d' })" />
                        </td>
                        <td>教师类型1:</td>
                        <td>
                            <input type="text" id="txtAddTimeTeacherType1" />
                        </td>
                    </tr>
                    <tr>
                        <td>上课开始时间2:</td>
                        <td>
                            <input id="txtAddTimeStartTime2" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </td>
                        <td>上课结束时间2:</td>
                        <td>
                            <input id="txtAddTimeEndTime2" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', minDate: '%y-%M-%d' })" />
                        </td>
                        <td>教师类型2:</td>
                        <td>
                            <input type="text" id="txtAddTimeTeacherType2" />
                        </td>
                    </tr>
                    <tr>
                        <td>上课开始时间3:</td>
                        <td>
                            <input id="txtAddTimeStartTime3" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </td>
                        <td>上课结束时间3:</td>
                        <td>
                            <input id="txtAddTimeEndTime3" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', minDate: '%y-%M-%d' })" />
                        </td>
                        <td>教师类型3:</td>
                        <td>
                            <input type="text" id="txtAddTimeTeacherType3" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:HiddenField ID="hiddenCourseID" runat="server" />
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
    <script src="../Scripts/SpokenBroadcasManagement/CoursePeriodList.js"></script>
    <script type="text/javascript">
        $(function () {
            //$("#divUpdateCoursPeriod table tr td:odd").attr("style", "text-align:right;padding-right:5px;");
            //$("#divAddCoursePeriod table tr td:odd").attr("style", "text-align:right;padding-right:5px;");
        });
    </script>
</body>
</html>
