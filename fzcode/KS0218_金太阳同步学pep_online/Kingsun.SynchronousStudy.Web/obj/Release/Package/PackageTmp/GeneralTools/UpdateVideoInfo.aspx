<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateVideoInfo.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.GeneralTools.UpdateVideoInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <style type="text/css">
        table thead tr td {
            width: 50px;
        }

        #videoDetails tr td input {
            width: 300px;
        }
    </style>
</head>
<body>
    <div>
        <div>
            <table id="videoDetails">
                <tr>
                    <td><span>书籍名称:</span></td>
                    <td style="width: 300px">
                        <input type="text" id="bookName" readonly="true" />
                    </td>
                </tr>
                <tr>
                    <td><span>一级标题:</span></td>
                    <td>
                        <input type="text" id="firstTitle" readonly="true" />
                    </td>
                </tr>
                <tr>
                    <td><span>二级标题:</span></td>
                    <td>
                        <input type="text" id="secondTitle" readonly="true" />
                    </td>
                </tr>
                <tr>
                    <td><span>一级模块:</span></td>
                    <td>
                        <input type="text" id="firstModule" readonly="true" />
                    </td>
                </tr>
                <tr>
                    <td><span>二级模块:</span></td>
                    <td>
                        <input type="text" id="secondModule" readonly="true" /></td>
                </tr>
                <tr>
                    <td><span>视频标题:</span></td>
                    <td>
                        <input type="text" id="videoTitle" />
                    </td>
                </tr>
                <tr>
                    <td><span>视频序号:</span></td>
                    <td>
                        <input type="text" id="videoNumber" />
                    </td>
                </tr>
                <tr>
                    <td><span>静音视频:</span></td>
                    <td>
                        <input type="text" id="muteVideo" />
                    </td>
                </tr>
                <tr>
                    <td><span>完整视频:</span></td>
                    <td>
                        <input type="text" id="completeVideo" /></td>
                </tr>
                <tr>
                    <td><span>视频时长:</span></td>
                    <td>
                        <input type="text" id="videoTime" />
                    </td>
                </tr>
                <tr>
                    <td><span>背景音频:</span></td>
                    <td>
                        <input type="text" id="backgroundAudio" /></td>
                </tr>
                <tr>
                    <td><span>视频封面:</span></td>
                    <td>
                        <input type="text" id="videoCover" /></td>
                </tr>
                <tr>
                    <td><span>视频简介:</span></td>
                    <td>
                        <input type="text" id="videoDesc" /></td>
                </tr>
                <tr>
                    <td><span>视频难度程度:</span></td>
                    <td>
                        <input type="text" id="videoDifficulty" /></td>
                </tr>
            </table>
        </div>
        <div>
            <table border="1" style="margin: 15px">
                <thead>
                    <tr>
                        <td>序号</td>
                        <td>对话序号</td>
                        <td>对话文本</td>
                        <td>开始时间</td>
                        <td>结束时间</td>
                    </tr>
                </thead>
                <tbody id="tbody">
                </tbody>
            </table>
        </div>
        <input type="button" id="makeSure" value="确认修改" onclick="updateVideoInfoInit.UpdateVideoDetails()" />
        <input type="button" id="cancel" value="取消" onclick="updateVideoInfoInit.Cancel()" />
    </div>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../AppTheme/js/jquery.json-2.4.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/UpdateVideoInfoInit.js"></script>
</body>
</html>
