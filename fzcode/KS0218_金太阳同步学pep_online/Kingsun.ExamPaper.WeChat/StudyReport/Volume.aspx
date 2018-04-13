<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Volume.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.StudyReport.Volume" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>学习报告</title>
    <script src="../Js/jquery-1.10.2.min.js"></script>
    <script src="../Js/CommonDB.js"></script>
    <script src="../Js/mobile.js"></script>
    <script src="../Js/report/report.js"></script>
    <link href="../Css/report/report.css" rel="stylesheet" />
    <script src="../Js/report/Volume.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="volume">

                <div class="rule-single-select single-select1">
                    <div class="boxwrap">
                        <a class="select-tit" href="javascript:;">
                            <span>--选择册别--</span><i></i>
                        </a>
                        <div class="select-items">
                            <ul id="div_Class">
                            </ul>
                        </div>
                    </div>
                </div>
                <ul id="ul_Unit">
                    <li><a href="#">Module1/Unit 1 Colours</a></li>
                    <li><a href="#">Module1/Unit 2 Tastes<em></em></a></li>
                    <li><a href="#">Module1/Unit 3 Sounds</a></li>
                    <li><a href="#">Module2/Unit 4 Animals in the zoo</a></li>
                    <li class="noclick"><a href="#">Module3/Unit 4 Animals in the zoo</a></li>
                </ul>

            </div>
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    /*切换册别*/
    $(".single-select1").click(function () {
        $(".single-select1").find(".select-items").toggle();
    }); 
</script>
