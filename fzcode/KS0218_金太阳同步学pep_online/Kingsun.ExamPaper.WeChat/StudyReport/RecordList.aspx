<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecordList.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.StudyReport.RecordList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>学习记录</title>
    <script src="../Js/jquery-1.10.2.min.js"></script>
    <script src="../Js/mobile.js"></script>
    <script src="../Js/report/report.js"></script>
    <link href="../Css/report/report.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivMain" runat="server" class="main">
            <div class="record">
                <ul class="ul1">
                    <li class="on">最佳排行</li>
                    <li>最近记录</li>
                </ul>
                <!-- 最佳排行-->
                <div id="contentBest" runat="server" class="content content1">
                    <p class="p1">完成人数<span>3</span>人</p>
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
                                <a url="/images/report/video01.mp3;/images/report/video02.mp3"></a>
                            </div>
                        </li>
                        <li>
                            <p class="p2">
                                <span>
                                    <img src="/images/report/tou.png" alt="" /><em>金闹闹</em></span><b><em>90</em>分</b>
                            </p>
                            <div class="content_nr">
                                <div class="p3">
                                    <h2>Enjoy a story</h2>
                                    <p>2017-5-3</p>
                                </div>
                                <a url="/images/report/video01.mp3;/images/report/video02.mp3"></a>
                            </div>
                        </li>
                        <li>
                            <p class="p2">
                                <span>
                                    <img src="/images/report/tou.png" alt="" /><em>金闪闪</em></span><b><em>90</em>分</b>
                            </p>
                            <div class="content_nr">
                                <div class="p3">
                                    <h2>Enjoy a story</h2>
                                    <p>2017-5-3</p>
                                </div>
                                <a url="/images/report/video01.mp3;/images/report/video02.mp3"></a>
                            </div>
                        </li>
                    </ul>
                </div>

                <!--  最近记录-->
                <div id="contentLately" runat="server" class="content content2">
                    <p class="p1">完成人数<span>4</span>人</p>
                    <ul>
                        <li>
                            <p class="p2">
                                <span>
                                    <img src="/images/report/tou.png" alt="" /><em>金让人</em></span><b><em>90</em>分</b>
                            </p>
                            <div class="content_nr">
                                <div class="p3">
                                    <h2>Enjoy a book</h2>
                                    <p>2017-5-3</p>
                                </div>
                                <a></a>
                            </div>
                        </li>
                        <li>
                            <p class="p2">
                                <span>
                                    <img src="/images/report/tou.png" alt="" /><em>金给个</em></span><b><em>90</em>分</b>
                            </p>
                            <div class="content_nr">
                                <div class="p3">
                                    <h2>Enjoy a book</h2>
                                    <p>2017-5-3</p>
                                </div>
                                <a></a>
                            </div>
                        </li>
                        <li>
                            <p class="p2">
                                <span>
                                    <img src="/images/report/tou.png" alt="" /><em>金方法</em></span><b><em>90</em>分</b>
                            </p>
                            <div class="content_nr">
                                <div class="p3">
                                    <h2>Enjoy a book</h2>
                                    <p>2017-5-3</p>
                                </div>
                                <a></a>
                            </div>
                        </li>
                    </ul>
                </div>

            </div>
        </div>
        <audio id="audio1">
            <source src="audio/yp1.mp3">
            <source src="audio/yp1.ogg">
            您的浏览器不支持audio标签。 
        </audio>
         <div class="hide" style="display:none"></div>
    </form>
</body>
</html>
