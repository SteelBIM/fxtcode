<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectItemManager.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.ExamPaperManagement.SelectItemManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>修改题目数据(小题页面)</title>
    <script src="../Exam_Theme/js/jquery-1.8.0.min.js"></script>
    <script src="../Exam_Theme/js/jquery.easyui.min.js"></script>
    <link href="../Exam_Theme/themes/default/easyui.css" rel="stylesheet" />
    <script src="../Exam_Theme/js/uploadify/swfobject.js" type="text/javascript"></script>
    <link href="../Exam_Theme/js/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="../Exam_Theme/js/uploadify/jquery.uploadify.min.js" type="text/javascript"></script>
    <script src="../Scripts/Common/Common.js"></script>
    <script type="text/javascript">
        var qid;
        $(function () {
            qid = Common.QueryString.GetValue("QuestionID");
        });
        function SaveSelectItem(obj) {
            var sort = $(obj).attr("sortid");
            var selItem = $("#item_" + sort).val().trim();
            var imgUrl = $("#img_" + sort).val().trim();
            var isAnswer = $("#answer_" + sort).val().trim();
            $.post("?Action=saveselectitem", { QuestionID: qid, Sort: sort, SelectItem: selItem, ImgUrl: imgUrl, IsAnswer: isAnswer }, function (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    alert("保存成功");
                } else {
                    alert("保存失败：" + result.Message);
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table border="1" cellpadding="0" cellspacing="0">
                <thead>
                    <tr>
                        <th style="width: 10%">排序</th>
                        <th style="width: 30%">选项</th>
                        <th style="width: 30%">图片</th>
                        <th style="width: 10%">是否为答案</th>
                        <th style="width: 10%">操作</th>
                    </tr>
                </thead>
                <tbody>
                    <%
                        for (int i = 0; i < listSelectItem.Count; i++)
                        {
                            string html = "";
                            html = "<tr id=\"tr_" + listSelectItem[i].Sort + "\">";
                            html += "<td>" + listSelectItem[i].Sort + "</td>";
                            html += "<td><input style=\" width:95%; margin:0 auto; padding:1px; display:block;\" type=\"text\" id=\"item_" + listSelectItem[i].Sort + "\" value=\"" + listSelectItem[i].SelectItem.Replace("\"", "&quot;") + "\"/></td>";
                            html += "<td><input style=\" width:95%; margin:0 auto; padding:1px; display:block;\" type=\"text\" id=\"img_" + listSelectItem[i].Sort + "\" value=\"" + listSelectItem[i].ImgUrl + "\"/></td>";
                            html += "<td><input style=\" width:80%; margin:0 auto; padding:1px; display:block;\" type=\"text\" id=\"answer_" + listSelectItem[i].Sort + "\" value=\"" + listSelectItem[i].IsAnswer.Value.ToString() + "\"/></td>";
                            html += "<td><a href=\"javascript:void(0)\" sortid=\"" + listSelectItem[i].Sort + "\" onclick=\"SaveSelectItem(this)\" title=\"保存选项信息\">保存</a>" + "</td>";
                            Response.Write(html);
                        }
                    %>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
