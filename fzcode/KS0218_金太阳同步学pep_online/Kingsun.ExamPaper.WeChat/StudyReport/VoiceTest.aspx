<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VoiceTest.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.StudyReport.VoiceTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Js/jquery-1.10.2.min.js"></script>
    <script src="../Js/mobile.js"></script>
    <script src="../Js/report/report.js"></script>
    <link href="../Css/report/report.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivMain" runat="server" class="main">
            <div class="record">
                <div id="contentBest" runat="server" class="content content1">
                    <ul>
                        <li>
                            <p class="p2">
                                <span>
                                    <img src="/images/report/tou.png" alt="" /><em>金娜娜</em></span><b><em>90</em>分</b>
                            </p>
                            <div class="content_nr">
                                <div class="p3">
                                    <h2>Enjoy a story</h2>
                                    <p>2017-5-3</p>
                                </div>
                                <a onclick="PlayVoice(this)" playstate="true" style="background: url(/images/report/play.png) no-repeat; background-size: 100%;"></a>
                            </div>
                        </li>
                        <li>
                            <p class="p2">
                                <span>
                                    <img src="/images/report/tou.png" alt="" /><em>金娜娜</em></span><b><em>90</em>分</b>
                            </p>
                            <div class="content_nr">
                                <div class="p3">
                                    <h2>Enjoy a story</h2>
                                    <p>2017-5-3</p>
                                </div>
                                <a onclick="PlayVoice(this)" playstate="true" style="background: url(/images/report/play.png) no-repeat; background-size: 100%;"></a>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </form>
</body>
<audio id="audio1">
    <source src="http://synchronousstudy.oss-cn-shenzhen.aliyuncs.com/SynchronousStudy/KouyuCepingjuan/2f3371ed-cd75-49f7-bc5c-7a6914b2d07d.mp3">
    <source src="http://synchronousstudy.oss-cn-shenzhen.aliyuncs.com/SynchronousStudy/KouyuCepingjuan/e1f9eb7d-e0f1-493e-b85b-511a656f9b04.mp3">
    您的浏览器不支持audio标签。 
</audio>
</html>
<script type="text/javascript">
    function PlayVoice(obj) {
        var my = document.getElementById('audio1');
        $(".content_nr a").css("background", "url(/images/report/play.png) no-repeat");
        $(".content_nr a").css("background-size", "100%");
        var state = $(obj).attr("playState");
        if (state == "true") {   //播放
            $(obj).css("background", "url(/images/report/pasue.png) no-repeat");
            $(obj).css("background-size", "100%");
            $(".content_nr a").attr("playState", "true");
            $(obj).attr("playState", "false");
            my.play();
        } else {

            $(obj).css("background", "url(/images/report/play.png) no-repeat");
            $(obj).css("background-size", "100%");
            $(obj).attr("playState", "true");
        }

    }
</script>
