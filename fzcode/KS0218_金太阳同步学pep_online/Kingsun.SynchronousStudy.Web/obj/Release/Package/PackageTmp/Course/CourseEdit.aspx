<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseEdit.aspx.cs" Inherits="Kingsun.AppLibrary.Web.Course.CourseEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>课程编辑</title>


    <script src="../Scripts/Common/jquery-1.4.4.min.js"></script>
    <script src="../Scripts/Common/jquery.cookie.js"></script>
    <script src="../Scripts/Common/jquery.json-2.4.js"></script>
    <script src="../Scripts/Common/jquery-easyui/jquery.easyui.min-1.4.js"></script>
    <link href="../Scripts/Common/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <script src="../Scripts/Common/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../Scripts/Common/Common.js"></script>
    <script src="../Scripts/Common/Constant.js"></script>
    <script src="../Scripts/Common/swfUpload/swfupload.js"></script>
    <script src="../Scripts/Common/swfUpload/jquery.swfupload.js"></script>
    <script src="../Scripts/Management/CourseManagement.js" type="text/javascript"></script>
    <script src="../Scripts/Management/ServiceManagement.js" type="text/javascript"></script>
    <script src="../Scripts/Init/CourseEditInit.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#swfupload-control').swfupload({
                upload_url: "../FileUpload.ashx",
                file_size_limit: "1024",
                file_types: "*.jpg",
                file_types_description: "图片文件",
                file_upload_limit: "0",
                flash_url: "../Scripts/Common/swfUpload/swfupload.swf",
                button_image_url: '../Scripts/Common/swfUpload/XPButtonUploadText_61x22.png',
                button_width: 70,
                button_height: 30,
                button_placeholder: $('#btn_upload')[0]
            }).bind('fileQueueError', function (fileobject, errorcode, message) {
                alert("文件不能超过1M");
            })
		    .bind('fileQueued', function (event, file) {
		        $(this).swfupload('startUpload');
		    })
		    .bind('uploadSuccess', function (event, file, serverData) {
		        $("#hidden_filePath").val(serverData);
		        $('#img_pic').attr("src", "../" + serverData);
		    })
		    .bind('uploadComplete', function (event, file) {
		        //$(this).swfupload('startUpload');
		    });
            var course = new CourseEidtInit();
            course.Init();

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
        });
    </script>
    <style type="text/css">
        BODY {
            font-size: 9pt;
        }

        TH {
            font-size: 9pt;
        }

        TD {
            font-size: 9pt;
        }
    </style>
</head>
<body>
    <form runat="server">
        <table>
            <tbody>
                <tr>
                    <td>
                        <input type="hidden" id="hidden_courseID" name="hidden_courseID" />
                        <input type="hidden" id="hidden_appID" name="hidden_appID" />
                        <input type="hidden" id="hidden_filePath" name="hidden_filePath" value="" />
                    </td>
                    <td>
                        <select id="select_stage" name="select_subject" class="easyui-combobox" data-options="valueField:'id',textField:'text',editable:false">
                            <option label="—请选择学段—" value="0">—请选择学段—</option>
                        </select>
                    </td>
                    <td>
                        <select id="select_subject" name="select_subject" class="easyui-combobox" data-options="valueField:'id',textField:'text',editable:false">
                            <option label="—请选择学科—" value="0">—请选择学科—</option>
                        </select>
                    </td>
                    <td>
                        <select id="select_edition" name="select_edition" class="easyui-combobox" data-options="valueField:'id',textField:'text',editable:false">
                            <option label="—请选择版本—" value="0">—请选择版本—</option>
                        </select>
                    </td>
                    <td>
                        <select id="select_grade" name="select_grade" class="easyui-combobox" data-options="valueField:'id',textField:'text',editable:false">
                            <option label="—请选择年级—" value="0">—请选择年级—</option>
                        </select>
                    </td>
                    <td>
                        <select id="select_term" name="select_term" class="easyui-combobox" data-options="valueField:'id',textField:'text',editable:false">
                            <option label="—请选择学期—" value="0">—请选择学期—</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">课程版本<span style="color: Red">*</span>：
                    </td>
                    <td>V<input type="text" id="input_version" name="input_version" readonly="readonly" style="width: 108px" />
                    </td>
                    <td rowspan="4" style="text-align: right;">下载页封皮<span style="color: Red">*</span>:
                    </td>
                    <td rowspan="4">
                        <img id="img_pic" src="../Image/CoursePic.png" style="width: 120px; height: 160px;"
                            alt="" />
                    </td>
                    <td rowspan="4">
                        <div id="swfupload-control" style="margin-top: 120px;">
                            <ol id="log">
                            </ol>
                            <input type="button" id="btn_upload" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">创建时间：
                    </td>
                    <td>
                        <input type="text" disabled="disabled" id="input_createTime" style="width: 115px;" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">上传人：
                    </td>
                    <td>
                        <input type="text" disabled="disabled" id="input_creator" style="width: 115px;" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;"></td>
                </tr>
                <tr>
                    <td style="text-align: right;">书籍介绍<span style="color: Red">*</span>：
                    </td>
                    <td colspan="4">
                        <textarea id="text_desc" name="text_desc" rows="10" cols="100" style="width: 510px;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td colspan="5"></td>
                </tr>
                <tr id="tr_filePath">
                    <td style="text-align: right;">课程文件<span style="color: Red"></span>：
                    </td>
                    <td colspan="4">
                        <input type="text" id="input_file" style="width: 300px" /><span style="font-size: 12px; color: Gray;">（格式：http：//www.xxx.com/xx.zip）</span>
                    </td>
                </tr>
                <tr id="tr_fileMD5">
                    <td style="text-align: right;">课程文件MD5<span style="color: Red"></span>：
                    </td>
                    <td colspan="4">
                        <input onkeyup="value=value.replace(/[^\d\w]/g,'')" type="text" id="input_fileMD5"
                            style="width: 300px" /><span style="font-size: 12px; color: Gray;">(用工具获取)</span>
                    </td>
                </tr>
                <tr>
                    <td>前端页面排序<span style="color: Red"></span>：
                    </td>
                    <td colspan="4">
                        <input type="text" style="width: 300px;" id="sort" /><span style="font-size: 12px; color: Gray;">(数字越小,排靠越前)</span>
                    </td>
                </tr>
                <%--            <tr id="tryupdatetr">
                <td style="text-align: right; width: 130px;">
                    是否强制更新<span style="color: Red"></span>：
                </td>
                <td>
                    <input type="checkbox" style=" height: 24px;" id="tryupdate" />
                </td>
            </tr>--%>
                <%--<tr>
            <td style=" text-align:right;">首页页码：</td>
            <td colspan="3">
            <input type="text" maxlength="2" onkeyup="value=value.replace(/[^\d\.]/g,'')" style="width: 200px; height: 24px;" id="FirstPageNum" /><span style=" font-size:12px; color:Gray;">(1-20)</span>
            </td>
        </tr>--%>
                <tr>
                    <td colspan="5">
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <input type="button" id="bt_save" style="width: 80px; height: 22px; margin-left: 240px"
                            value="保存" />
                        <input type="button" id="bt_cancel" style="width: 80px; height: 22px;" value="取消" />
                    </td>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>
