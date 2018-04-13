<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseManager.aspx.cs"
    Inherits="Kingsun.AppLibrary.Web.Course.CourseManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>课程管理</title>
    <script src="../Scripts/Common/jquery-1.4.4.min.js"></script>
    <script src="../Scripts/Common/jquery.cookie.js"></script>
    <script src="../Scripts/Common/jquery.json-2.4.js"></script>
    <script src="../Scripts/Common/jquery-easyui/jquery.easyui.min-1.4.js"></script>
    <link href="../Scripts/Common/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <script src="../Scripts/Common/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../Scripts/Common/Common.js"></script>
    <script src="../Scripts/Common/Constant.js"></script>
    <script src="../Scripts/Management/CourseManagement.js" type="text/javascript"></script>
    <script src="../Scripts/Init/CourseInit.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        var CourseInit;
        $(function () {
            CourseInit = new CoursesInit();
            CourseInit.Init();

        });
        /*刷新两个框架*/
        function refurFrame(a) {
            window.parent.mainFrame.setPath(a); //传值
        }
    </script>
</head>
<body onload="refurFrame('应用管理>>课程列表')">
    <form id="form1" runat="server">
    <div>
        <div>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 50%">
                        <%--<input type="button" id="btn_back" value="返回应用列表" />--%>
                        <input type="button" id="btnNewCourse" value="添加课程" />
                    </td>
                    <td>
                    </td>
                    <td style="text-align: right;">
                        <input type="text" id="txtSearchKey" style="color: Gray" value="请输入要查询课程名称" />
                        <input type="button" id="btnSearch" value="搜 索" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table id="tbCourse">
                <thead>
                    <tr>
                        <th data-options="field:'IDs',width:100,hidden:true">
                            课程ID
                        </th>
                        <th data-options="field:'CourseName',width:120, align:'center'">
                            课程名称
                        </th>
                        <th data-options="field:'Version',width:20, align:'center'">
                            版本
                        </th>
                        <th data-options="field:'Disable',width:30,formatter:CourseStatus, align:'center'">
                            状态
                        </th>
                        <th data-options="field:'Creator',width:50, align: 'center'">
                            添加人
                        </th>
                        <th data-options="field:'CreateDateTime',width:120,formatter:CourseDate,align:'center'">
                            添加时间
                        </th>
                        <th data-options="field:'Sort',width:20, align: 'center'">
                            排序顺序
                        </th>
                        <th data-options="field:'ID',width:120,formatter:CourseOpetate, align:'center'">
                            操作
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    <!-- 版本更新-->
    <div id="divAddVersion">
        <input type="hidden" id="hidCourseidSelect" value="" />
        <table cellpadding="3" style="margin: 20px;">
            <tr>
                <td style="text-align: right;">
                    程序版本<span style="color: Red">*</span>：
                </td>
                <td>
                    <span>V</span><input type="text" style="width: 100px; height: 24px;" id="txtVersion"
                        onkeyup="value=value.replace(/[^\d\.]/g,'')" /><span style="font-size: 12px; color: Gray;">(只需要填写相关数字即可)</span><span
                            id="spanVersion"></span>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    更新简介<span style="color: Red">*</span>：
                </td>
                <td>
                    <textarea id="txtVersionDescription" style="width: 350px; height: 150px; max-width: 450px;
                        max-height: 200px; overflow: auto"></textarea>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    更新文件地址<span style="color: Red">*</span>：
                </td>
                <td>
                    <input type="text" style="width: 300px; height: 24px;" id="txtUpdateUrl" /><span
                        style="font-size: 12px; color: Gray;">(格式：http：//www.xxx.com/xx.zip)</span>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 130px;">
                    更新文件MD5值<span style="color: Red">*</span>：
                </td>
                <td>
                    <input onkeyup="value=value.replace(/[^\d\w]/g,'')" type="text" style="width: 300px;
                        height: 24px;" id="txtUpdateMD5" /><span style="font-size: 12px; color: Gray;">(使用指定工具获取)</span>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    增量包地址<span style="color: Red"></span>：
                </td>
                <td>
                    <input type="text" style="width: 300px; height: 24px;" id="txtCompleteURL" /><span
                        style="font-size: 12px; color: Gray;">(格式：http：//www.xxx.com/xx.zip)</span>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 130px;">
                    增量包MD5值<span style="color: Red"></span>：
                </td>
                <td>
                    <input type="text" style="width: 300px; height: 24px;" id="txtCompleteMD5" /><span
                        style="font-size: 12px; color: Gray;">(使用指定工具获取)</span>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 130px;">
                    是否强制更新<span style="color: Red"></span>：
                </td>
                <td>
                    <input type="checkbox" style="height: 24px;" id="tryupdate" />
                </td>
            </tr>
            <%--<tr>
                <td style="text-align: right;">
                    课程首页页码：
                </td>
                <td>
                    <input type="text" maxlength="2" value="1" onkeyup="value=value.replace(/[^\d\.]/g,'')" style="width: 200px; height: 24px;" id="FirstPageNum" /><span style=" font-size:12px; color:Gray;">(1-20)</span>
                </td>
            </tr>--%>
            <tr>
                <td>
                </td>
                <td>
                    <input type="button" value="保  存" id="btnSaveVersion" style="width: 80px; height: 24px;" />
                    &nbsp;&nbsp;
                    <input type="button" value="取  消" id="btnCancelVersion" style="width: 80px; height: 24px;" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
