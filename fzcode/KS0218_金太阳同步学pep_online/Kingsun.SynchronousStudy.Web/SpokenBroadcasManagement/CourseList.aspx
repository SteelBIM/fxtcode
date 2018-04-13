<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseList.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.SpokenBroadcasManagement.CourseList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>课程管理</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <style type="text/css">
        #divUpdateCourse table tr td {
            height: 30px;
            text-align: left;
            border: 1px solid gray;
        }

        #divUpdateCours table tr td {
            height: 30px;
            text-align: left;
            border: 0px solid gray;
        }

        #divAddCourse table tr td {
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
                    <h4>同步学平台>>课程管理>>课程管理</h4>
                </div>
                <div class="toolBar" id="bookID">
                    <div class="rightTools">
                        <table>
                            <tr>
                                <td style="padding-right: 20px;"><a href="javascript:void(0)" id="addCourse" onclick="addCourse()" class="easyui-linkbutton" data-options="iconCls:'icon-add'">新增课程</a></td>
                                <td>
                                    <input type="text" id="searchValue" class="easyui-validatebox" placeholder="请输入课程名称" style="min-width: 150px; height: 20px;" /></td>
                                <td>

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
            <div id="divUpdateCourse" style="display: none; border: 1px solid red;">
                <table style="padding: 10px; width: 100%; height: auto; text-align: center">
                    <tr>
                        <td>
                            <img id="ImgShow"   style="width: 100%; height: 350px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 10px; padding-bottom: 10px;">
                            <input type="file" id="UploadImg" name="UploadImg" />
                        </td>
                    </tr>
                </table>
            </div>
            <%--修改课程DIV--%>
            <div id="divUpdateCours" style="display: none; border: 0px solid red;">
                <table style="padding: 10px; width: 100%; height: auto; text-align: center; border: 1px dashed gray;">
                    <tr>
                        <td colspan="4" style="text-align: center; border-bottom: 1px dashed gray;">
                            <h1 style="font-size: 16px;">课程信息</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>课程ID:</td>
                        <td>
                            <span id="txtCourseID"></span>
                        </td>
                        <td><span style="color: red;">*</span>课程名称:</td>
                        <td>
                            <input type="text" id="txtName" />
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>课程类型:</td>
                        <td>
                            <select id="selType">
                                <option value="0">电影课</option>
                                <option value="1">绘本课</option>
                            </select>
                        </td>
                        <td><span style="color: red;">*</span>级别:</td>
                        <td>
                            <input type="text" id="txtGroups" />
                        </td>
                    </tr>
                    <tr>
                        <%--<td>提前进入时间(分钟):</td>
                        <td>
                            <input type="text" id="txtAheadMinutes" />
                        </td>--%>
                        <td><span style="color: red;">*</span>课时数量:</td>
                        <td>
                            <input type="text" id="txtNum" />
                        </td>
                    </tr>
                    <tr>
                        <td>直播间地址:</td>
                        <td colspan="3">
                            <input type="text" id="txtStudioUrl" />
                        </td>
                    </tr>
                    <tr>
                        <td>直播间口令:</td>
                        <td>
                            <input type="text" id="txtStudioCommand" />
                        </td>
                        <td>开课时间:</td>
                        <td>
                            <input id="txtOpenDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>课程描述:</td>
                        <td colspan="3">
                            <textarea id="txtSummary" style="width: 95%; min-height: 100px;"></textarea>
                        </td>
                    </tr>
                </table>
            </div>
            <%--新增课程DIV--%>
            <div id="divAddCourse" style="display: none; border: 0px solid red;">
                <table style="padding: 10px; width: 100%; height: auto; text-align: center; border: 1px dashed gray;">
                    <tr>
                        <td colspan="4" style="text-align: center; border-bottom: 1px dashed gray;">
                            <h1 style="font-size: 16px;">课程信息</h1>
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>课程名称:</td>
                        <td>
                            <input type="text" id="txtAddName" />
                        </td>
                        <td>课程状态:</td>
                        <td>
                            <select id="selStatus">
                                <option value="0">禁用</option>
                                <option value="1">启用</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>课程类型:</td>
                        <td>
                            <select id="selAddType">
                                <option value="0">电影课</option>
                                <option value="1">绘本课</option>
                            </select>
                        </td>
                        <td><span style="color: red;">*</span>级别:</td>
                        <td>
                            <input type="text" id="txtAddGroups" />
                        </td>
                    </tr>
                    <tr>
                        <%--<td>提前进入时间(分钟):</td>
                        <td>
                            <input type="text" id="txtAddAheadMinutes" />
                        </td>--%>
                        <td><span style="color: red;">*</span>课时数量:</td>
                        <td>
                            <input type="text" id="txtAddNum" />
                        </td>
                    </tr>
                    <tr>
                        <td>开课时间:</td>
                        <td>
                            <input id="txtAddOpenDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </td>
                        <td><span style="color: red;">*</span>课程海报:</td>
                        <td>
                            <input type="file" id="AddUploadImg" name="AddUploadImg" />
                        </td>
                    </tr>
                     <tr>
                        <td>直播间地址:</td>
                        <td colspan="3">
                            <input type="text" id="txtAddStudioUrl" />
                        </td>
                    </tr>
                    <tr>
                        <td>直播间口令:</td>
                        <td>
                            <input type="text" id="txtAddStudioCommand" />
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>课程描述:</td>
                        <td colspan="3">
                            <textarea id="txtAddSummary" style="width: 95%; min-height: 100px;"></textarea>
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
    <script src="../Scripts/SpokenBroadcasManagement/CourseList.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#divUpdateCours table tr td:odd").attr("style", "text-align:right;padding-right:5px;");
            $("#divAddCourse table tr td:odd").attr("style", "text-align:right;padding-right:5px;");
        });
    </script>
</body>
</html>
