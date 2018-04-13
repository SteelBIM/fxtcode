<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResourceManager.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.ExamPaperManagement.ResourceManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Exam_Theme/js/jquery-1.8.0.min.js"></script>
    <script src="../Exam_Theme/js/jquery.easyui.min.js"></script>
    <script src="../Exam_Theme/js/Page/ResourceManager.js"></script>
    <script src="../Exam_Theme/js/Common.js"></script>
    <link href="../Exam_Theme/themes/default/easyui.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="divtoolbar">

                <select id="selEdition">
                    <option value="0" selected="selected">版本</option>
                    <%
                        for (int i = 0; i < listEdition.Count; i++)
                        {
                            Response.Write(" <option value=\"" + listEdition[i].EditionID + "\">" + listEdition[i].EditionName + "</option>");
                        }
                    %>
                </select>
                <select id="selGrade">
                    <option value="0" selected="selected">年级</option>
                    <option value="2">一年级</option>
                    <option value="3">二年级</option>
                    <option value="4">三年级</option>
                    <option value="5">四年级</option>
                    <option value="6">五年级</option>
                    <option value="7">六年级</option>
                    <option value="8">七年级</option>
                    <option value="9">八年级</option>
                    <option value="10">九年级</option>
                </select>
                <select id="selBookReel">
                    <option value="0" selected="selected">册别</option>
                    <option value="1">上册</option>
                    <option value="2">下册</option>
                    <option value="3">全册</option>
                </select>
                <input type="button" id="btnAdd" value="添加" />
            </div>
            <table id="tbdatagrid">
            </table>
        </div>
    </form>
</body>
</html>