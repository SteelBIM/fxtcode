<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseManageMent.aspx.cs"
    Inherits="Kingsun.SynchronousStudy.Web.CourseManagement.CourseManageMent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>课程列表</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <script type="text/javascript">
        var s = "";
    </script>
</head>
<body>
    <form runat="server">
        <div>
            <div id="userName" style="display: none"><%=UserInfo.UserName%></div>
            <div>
                <div>
                    <h4>同步学平台>>课程管理>>课程列表</h4>
                </div>
                <div class="toolBar" id="bookID">
                    <div class="rightTools">
                        <table>
                            <tr>
                                <td>
                                    <a href="javascript:void(0)" id="addCourse" class="autobtn">添加教材</a>
                                </td>
                                <td>
                                    <input type="text" id="searchValue" value="查询关键字" />
                                    <a href="javascript:void(0)" id="search">查询</a>
                                </td>
                                <td>请选择excel文件：<asp:FileUpload ID="FileUpload1" runat="server" /><asp:Button ID="btnAdd" runat="server" Text="导入活动资源" OnClick="btnAdd_Click" /></td>
                                <td>（<span style="color: red">请导入活动视频信息之后再操作</span>）<asp:Button ID="Button1" runat="server" Text="同步对白表的ActiveID" OnClick="Button1_Click" /></td>
                                <td><asp:Button ID="btn2" runat="server" Text="导入书籍资源" OnClick="btn2_Click" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tbdatagrid"></div>
            </div>
            <%--添加 修改DIV--%>
            <div id="divAddCourse" style="display: none;">
                <table>
                    <tr>
                        <td>
                            <div>
                                <select id="selectStage">
                                    <option></option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <div>
                                <select id="selectSubject">
                                    <option></option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <div>
                                <select id="selectEdition">
                                    <option></option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <div>
                                <select id="selectGrade">
                                    <option></option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <div>
                                <select id="selectBreel">
                                    <option></option>
                                </select>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="filed">上传人：</td>
                        <td>
                            <%--<input type="text" class="single-text normal" style="width: 200px;" id="addPerson"
                            title="上传人" />--%>
                            <span id="addPerson"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>添加时间:</td>
                        <td>
                            <input type="text" readonly="readonly" id="AddTime" />
                        </td>
                    </tr>
                    <tr>
                        <td class="filed">下载页封皮：</td>
                        <td>
                        <%--    <input type="file" name="file_upload" id="file_upload" />
                            <span style="margin-left: 10px; height: 32px" id="showFileName" class="fileName"></span>
                            <input type="hidden" name="name" value="" id="fileinfo" />--%>
                             <input type="file" id="UploadImg" name="UploadImg"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div style="display: none" id="div_ImportResource">
            <iframe src="#" id="iframe_ImportResource" style="width: 550px; height: 650px;"></iframe>
        </div>
    </form>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script> 
    <script src="../Scripts/Common/ajaxfileupload.js"></script>
    <script src="../AppTheme/js/jquery-easyui/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <link href="../AppTheme/js/uploadify/uploadify.css" rel="stylesheet" />
    <script src="../AppTheme/js/uploadify/jquery.uploadify.min.js"></script>
    <script src="../AppTheme/js/jquery.json-2.4.js"></script>
    <script src="../AppTheme/js/jquery.cookie.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../AppTheme/js/fzcontrols/jquery.form.controls.js"></script>
    <script src="../AppTheme/js/FzJsControl/Plugins/Kingsun.Select.js"></script>
    <script src="../Scripts/CourseManager.js"></script>
 
    <script>
        var localUrl = '<%=Kingsun.SynchronousStudy.Common.AppSetting.Root%>';
        var uumsUrl = '<%=Kingsun.SynchronousStudy.Common.AppSetting.UumsRootUrl%>';
    </script>
</body>
</html>
