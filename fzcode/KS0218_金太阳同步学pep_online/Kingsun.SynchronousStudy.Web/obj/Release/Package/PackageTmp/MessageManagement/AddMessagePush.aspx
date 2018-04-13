<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddMessagePush.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.MessageManagement.AddMessagePush" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>添加推送</title>
    <script src="../AppTheme/WdatePicker.js"></script>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <link href="../AppTheme/js/uploadify/uploadify.css" rel="stylesheet" />
    <script src="../AppTheme/js/uploadify/jquery.uploadify.min.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../AppTheme/js/jquery.json-2.4.js"></script>
    <script type="text/javascript">
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        if (getQueryString("Type") === "Serach") {
            $("#file_upload").css("display", "none");
            //$("#file_upload").remove();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width: 898px;" border="1" cellpadding="0" cellspacing="1">
                <tr>
                    <td>标题(<span style="color: red">*</span>)：<asp:TextBox ID="txtTitle" runat="server"></asp:TextBox></td>
                    <td>标题是否启用：<asp:RadioButtonList ID="rbTitleState" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="0" Selected="True">禁用</asp:ListItem>
                        <asp:ListItem Value="1">启用</asp:ListItem>
                    </asp:RadioButtonList></td>
                </tr>
                <tr>
                    <td>上传图片：<%--<asp:FileUpload ID="fileImage" runat="server" />--%>
                        <input type="file" name="" id="file_upload" runat="server" /><span style="margin-left: 10px; height: 32px" id="showFileName" class="fileName"></span>
                        <input type="hidden" name="name" runat="server" value="" id="fileinfo" />
                        <asp:Image ID="Image1" runat="server" />
                    </td>
                    <td>上传按钮：
                       <input type="file" name="file_upload" id="file_upload1" runat="server" /><span style="margin-left: 10px; height: 32px" id="showFileName1" class="fileName"></span>
                        <input type="hidden" name="name" runat="server" value="" id="fileinfo1" />
                        <asp:Image ID="btnImg" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>推送版本(<span style="color: red">*</span>)：<asp:DropDownList ID="ddlVersion" runat="server"></asp:DropDownList></td>
                    <td>跳转链接：<asp:TextBox ID="txtLink" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>使用时长大于：<asp:TextBox ID="txtUseTime" runat="server"></asp:TextBox><asp:DropDownList ID="ddlTime" runat="server">
                        <asp:ListItem Value="0">秒</asp:ListItem>
                        <asp:ListItem Value="1">分</asp:ListItem>
                        <asp:ListItem Value="2">时</asp:ListItem>
                    </asp:DropDownList></td>
                    <td>使用天数大于<asp:TextBox ID="txtNumber" runat="server"></asp:TextBox>天</td>
                </tr>
                <tr>
                    <td>班级ID：<asp:TextBox ID="txtClassID" runat="server"></asp:TextBox></td>
                    <td>起始时间(<span style="color: red">*</span>)：<input id="txtStartDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndDate\',{d:-1})}', dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        结束时间(<span style="color: red">*</span>)：<input id="txtEndDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'txtStartDate\',{d:1})}', dateFmt: 'yyyy-MM-dd HH:mm:ss' })" /></td>
                </tr>
                <tr>
                    <td colspan="2">文本内容(<span style="color: red">*</span>)：<asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" Height="89px" Width="445px"></asp:TextBox></td>
                </tr>
            </table>
            <asp:Button ID="btnAdd" runat="server" Text="保存" OnClick="btnAdd_Click" />
            <asp:Button ID="btnExit" runat="server" Text="返回" OnClick="btnExit_Click" />
        </div>
    </form>
    <script type="text/javascript">
        //上传图片到文件服务器
        $('#file_upload').uploadify({
            fileSizeLimit: '4MB',
            buttonText: "选择图片",
            fileTypeExts: '*.jpg;*.bmp;*.gif;*.png',
            auto: true,
            multi: false,
            swf: '../AppTheme/js/uploadify/uploadify.swf',
            uploader: Constant.file_Url + 'UploadHandler.ashx',
            onSelect: function (file) {
                $("#showFileName").html(file.name);
            },
            onUploadSuccess: function (file, data, respone) {
                if (respone) {
                    data = eval("(" + data + ")");
                    if (data.Success) {
                        var IDArr = [];
                        IDArr.push(data.Data.ID);
                        var IDJson = $.toJSON(IDArr);
                        $.ajax({
                            url: Constant.file_Url + "ConfirmHandler.ashx",
                            type: "post",
                            data: { t: IDJson },
                            dataType: "jsonp",
                            success: function (data) {
                            }
                        });

                        $("#fileinfo").val(data.Data.ID);
                    }
                }
            },
            onUploadError: function (file, errorCode, erorMsg, errorString) {
                $("#fileinfo").val("");
                Common.tips(erorMsg);
            }
        });

        //上传图片到文件服务器
        $('#file_upload1').uploadify({
            fileSizeLimit: '4MB',
            buttonText: "选择图片",
            fileTypeExts: '*.jpg;*.bmp;*.gif;*.png',
            auto: true,
            multi: false,
            swf: '../AppTheme/js/uploadify/uploadify.swf',
            uploader: Constant.file_Url + 'UploadHandler.ashx',
            onSelect: function (file) {
                $("#showFileName1").html(file.name);
            },
            onUploadSuccess: function (file, data, respone) {
                if (respone) {
                    data = eval("(" + data + ")");
                    if (data.Success) {
                        var IDArr = [];
                        IDArr.push(data.Data.ID);
                        var IDJson = $.toJSON(IDArr);
                        $.ajax({
                            url: Constant.file_Url + "ConfirmHandler.ashx",
                            type: "post",
                            data: { t: IDJson },
                            dataType: "jsonp",
                            success: function (data) {
                            }
                        });

                        $("#fileinfo1").val(data.Data.ID);
                    }
                }
            },
            onUploadError: function (file, errorCode, erorMsg, errorString) {
                $("#fileinfo1").val("");
                Common.tips(erorMsg);
            }
        });
    </script>
</body>

</html>
