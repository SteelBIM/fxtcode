<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpeechEval.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.StudyReport.SpeechEval" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>口语评测</title>
    <script src="../Js/jquery-1.10.2.min.js"></script>
    <script src="../Js/mobile.js"></script>
    <script src="../Js/report/report.js"></script>
    <link href="../Css/report/report.css" rel="stylesheet" /> 
</head>
<body>
    <form id="form1" runat="server"> 
        <div id="DivMain" runat="server" class="main">
            <div id="DivSpeechEval" runat="server" class="speecheval">
                <h2>口语评测</h2>
                <%--  <ul>
                    <li><a href="#">跟读单词 Look and lenarn</a></li>
                    <li><a href="#">跟读句子 Listen and say<em></em></a></li>
                    <li><a href="#">跟读课文 Look and lenarn</a></li>
                    <li><a href="#">跟读课文 Listen and say</a></li>
                    <li class="noclick"><a href="#">跟读语音 Look and lenarn</a></li>
                </ul> --%> 
            </div>
        </div>
    </form>
</body>
</html>
