<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseVersion.aspx.cs"
    Inherits="Kingsun.AppLibrary.Web.Course.CourseVersion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>课程版本管理</title>
    <script src="../Scripts/Common/jquery-1.4.4.min.js"></script>
    <script src="../Scripts/Common/jquery.cookie.js"></script>
    <script src="../Scripts/Common/jquery.json-2.4.js"></script>
    <script src="../Scripts/Common/jquery-easyui/jquery.easyui.min-1.4.js"></script>
    <link href="../Scripts/Common/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <script src="../Scripts/Common/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../Scripts/Common/Common.js"></script>
    <script src="../Scripts/Common/Constant.js"></script>
    <script src="../Scripts/Management/CourseManagement.js" type="text/javascript"></script>

    <script src="../Scripts/Init/CourseVersion.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            var CourseVersionInit = new CourseVersionPageInit();
            CourseVersionInit.Init();
            $("#btn_back").click(function () {
                window.history.go(-1);
            });
        });
        document.documentElement.onkeydown = function (evt) {
            var b = !!evt, oEvent = evt || window.event;
            if (oEvent.keyCode == 8) {
                var node = b ? oEvent.target : oEvent.srcElement;
                var reg = /^(input|textarea)$/i, regType = /^(text|textarea)$/i;
                if (!reg.test(node.nodeName) || !regType.test(node.type) || node.readOnly || node.disabled) {
                    if (b) {
                        oEvent.stopPropagation();
                    }
                    else {
                        oEvent.cancelBubble = true;
                        oEvent.keyCode = 0;
                        oEvent.returnValue = false;
                    }
                }
            }
        }

        /*刷新两个框架*/
        function refurFrame(a) {
            window.parent.mainFrame.setPath(a); //传值
        }
    </script>
</head>
<body onload="refurFrame('应用管理>>课程列表>>版本更新列表')">
    <form id="form1" runat="server" enctype="multipart/form-data" method="post">
        <div>
            <div>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 50%">
                            <input type="button" id="btn_back" value="返回课程管理" />
                            <input type="button" id="btnNewCourseVersion" value="版本更新" />
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div>
                <table id="tbCourseVersion">
                    <thead>
                        <tr>
                            <th data-options="field:'Version',width:100">课程版本号
                            </th>
                            <th data-options="field:'Description',width:150">版本描述
                            </th>
                            <th data-options="field:'ModuleID',width:150,formatter:GetModuleName">模块名称
                            </th>
                            <th data-options="field:'Disable',width:50,formatter:CourseVersionStatus">状态
                            </th>
                            <th data-options="field:'DownloadTimes',width:50">下载次数
                            </th>
                            <th data-options="field:'CreateDateTime',width:100,formatter:CourseVersionDate">发布时间
                            </th>
                            <th data-options="field:'Creator',width:100">上传人
                            </th>
                            <th data-options="field:'ID',width:200,formatter:CourseVersionOpetate">操作
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
        <div id="divAdd">
            <input type="hidden" id="hidAddOREdit" value="" />
            <input type="hidden" id="hidID" value="" />
            <input type="hidden" id="hidCourseIDSelect" value="" />
            <table cellpadding="3" style="margin: 20px;">
                <tr>
                    <td style="text-align: right;">模块<span style="color: Red">*</span>：
                    </td>
                    <td>
                        <asp:DropDownList ID="slModuleID" runat="server"></asp:DropDownList>
                       <%-- <select id="slModuleID">
                            <option value="0">----请选择----</option>
                            <option value="1">点读</option>
                            <option value="13">练习册</option>
                        </select>--%>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">课程版本<span style="color: Red">*</span>：
                    </td>
                    <td>
                        <span>V</span><input type="text" style="width: 100px; height: 24px;" id="txtCourseVersion"
                            onkeyup="value=value.replace(/[^\d\.]/g,'')" /><span style="font-size: 12px; color: Gray;">(只需要填写相关数字即可)</span><span
                                id="spanVersion"></span>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">更新简介<span style="color: Red">*</span>：
                    </td>
                    <td>
                        <textarea id="txtUpdateDescription" style="width: 350px; height: 150px; max-width: 450px; max-height: 200px; overflow: auto"></textarea>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">更新文件地址<span style="color: Red">*</span>：
                    </td>
                    <td>
                        <input type="text" style="width: 200px; height: 24px;" id="txtUpdateUrl" /><span
                            style="font-size: 12px; color: Gray;">(http：//www.xxx.com/xx.zip)</span>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">更新文件MD5值<span style="color: Red">*</span>：
                    </td>
                    <td>
                        <input type="text" onkeyup="value=value.replace(/[^\d\w]/g,'')" style="width: 300px; height: 24px;"
                            id="txtUpdateMD5" /><span style="font-size: 12px; color: Gray;">(使用指定工具获取)</span>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">增量包地址<span style="color: Red"></span>：
                    </td>
                    <td>
                        <input type="text" style="width: 300px; height: 24px;" id="txtCompleteURL" /><span
                            style="font-size: 12px; color: Gray;">(格式：http：//www.xxx.com/xx.zip)</span>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 130px;">增量包MD5值<span style="color: Red"></span>：
                    </td>
                    <td>
                        <input type="text" style="width: 300px; height: 24px;" id="txtCompleteMD5" /><span
                            style="font-size: 12px; color: Gray;">(使用指定工具获取)</span>
                    </td>
                </tr>
                <tr id="tryupdatetr">
                    <td style="text-align: right; width: 130px;">是否强制更新<span style="color: Red"></span>：
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
                    <td></td>
                    <td>
                        <input type="button" value="保  存" id="btnSave" style="width: 80px; height: 24px;" />
                        &nbsp;&nbsp;
                    <input type="button" value="取  消" id="btnCancel" style="width: 80px; height: 24px;" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
