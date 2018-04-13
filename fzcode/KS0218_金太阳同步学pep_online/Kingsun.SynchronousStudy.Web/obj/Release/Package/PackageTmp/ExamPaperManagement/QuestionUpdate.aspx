<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuestionUpdate.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.ExamPaperManagement.QuestionUpdate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>编辑题目</title>
    <script src="../Exam_Theme/js/jquery-1.8.0.min.js"></script>
    <script src="../Exam_Theme/js/jquery.easyui.min.js"></script>
    <link href="../Exam_Theme/themes/default/easyui.css" rel="stylesheet" />
    <script src="../Scripts/Common/Common.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="diaQuestion">
            <div id="divQuestion">
                <table style="margin: 20px;">
                    <tr>
                        <td style="text-align: right;">题目标题：</td>
                        <td>
                            <textarea id="txtQuestionTitle" style="width: 400px; height: 40px; max-width: 400px;
                                max-height: 25px; overflow: auto"><%=questionInfo.QuestionTitle %></textarea>
                        </td>
                        <td style="text-align: right;">题目模板：</td>
                        <td>
                            <input type="text" style="width: 100px; height: 40px;" id="txtQuestionModel" value="<%=questionInfo.QuestionModel %>"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">主题干：</td>
                        <td colspan="3">
                            <textarea id="txtQuestionContent" style="width: 650px; height: 150px; max-width: 680px;
                                max-height: 200px; overflow: auto"><%=Server.HtmlEncode(questionInfo.QuestionContent) %></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">副题干：</td>
                        <td colspan="3">
                            <textarea id="txtSecondContent" style="width: 650px; height: 150px; max-width: 680px;
                                max-height: 200px; overflow: auto"><%=Server.HtmlEncode(questionInfo.SecondContent) %></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">音频：</td>
                        <td colspan="3">
                            <textarea id="txtMp3Url" style="width: 650px; height: 40px; max-width: 680px;
                                max-height: 200px; overflow: auto"><%=Server.HtmlEncode(questionInfo.Mp3Url) %></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">图片：</td>
                        <td colspan="3">
                            <textarea id="txtImgUrl" style="width: 650px; height: 40px; max-width: 680px;
                                max-height: 200px; overflow: auto"><%=Server.HtmlEncode(questionInfo.ImgUrl) %></textarea>
                        </td>
                    </tr>
                    
                </table>
                <input type="button" value="保  存" id="btnSave" style="width: 80px; height: 24px;" />
                        &nbsp;&nbsp;
                <input type="button" value="取  消" id="btnCancel" style="width: 80px; height: 24px;" />
            </div>
        </div>
    </form>
</body>
    <script type="text/javascript">
        var questionID = '';
        $(function () {
            questionID = Common.QueryString.GetValue("QuestionID");
            $("#btnSave").click(function () {
                var qtitle = $("#txtQuestionTitle").val().trim();
                var qmodel = $("#txtQuestionModel").val().trim();
                var qcontent = $("#txtQuestionContent").val();
                var scontent = $("#txtSecondContent").val();
                var mp3url = $("#txtMp3Url").val().trim();
                var imgurl = $("#txtImgUrl").val().trim();
                if (qtitle == "") {
                    alert("题目标题为空！");
                    $("#txtQuestionTitle").focus();
                    return null;
                }
                if (qmodel == "") {
                    alert("题目模板为空！");
                    $("#txtQuestionModel").focus();
                    return null;
                }

                obj = {
                    QuestionID: questionID, QuestionTitle: Common.HtmlEncode(qtitle), QuestionModel: qmodel, QuestionContent: Common.HtmlEncode(qcontent),
                    SecondContent: Common.HtmlEncode(scontent), Mp3Url: mp3url, ImgUrl: imgurl
                };
                $.post("?action=SaveQuestion", obj, function (data) {
                    if (data) {
                        var result = eval("(" + data + ")");
                        if (result.Success) {
                            //新开页面关闭之前，刷新原页面
                            window.opener.document.location.href = window.opener.document.location.href;
                            window.close();
                        }
                        else {
                            alset(result.ErrorMsg);
                        }
                    }
                });
            });

            $("#btnCancel").click(function () {
                window.close();
            });
        });
    </script>
</html>