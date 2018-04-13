<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HearResourcesShareResult.aspx.cs"
    Inherits="Kingsun.SynchronousStudy.Web.HearResourcesShareResult.HearResourcesShareResult" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=0.5, user-scalable=0"
        name="viewport" />
    <title>分享</title>
    <link href="css/fenvideo.css" rel="stylesheet" />
    <script src="js/jquery-1.11.2.min.js"></script>
    <script>
        $(function() {
            $(".linkapp").click(function() {
                if (AppID == "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385") { //广州版
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.gz.syslearning";
                } else if (AppID == "43716a9b-7ade-4137-bdc4-6362c9e1c999") { //上海本地
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.shbd.syslearning";
                } else if (AppID == "241ea176-fce7-4bd7-a65f-a7978aac1cd2") { //深圳版
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.sz.syslearning";
                } else if (AppID == "9426808e-da8e-488c-9827-b082c19b62a7") { //上海全国
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.shqg.syslearning";
                } else if (AppID == "0a94ceaf-8747-4266-bc05-ed8ae2e7e410") { //北京
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.bj.syslearning";
                } else if (AppID == "5373bbc9-49d4-47df-b5b5-ae196dc23d6d") { //人教pep
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.rj.syslearning";
                } else if (AppID == "37ca795d-42a6-4117-84f3-f4f856e03c62") { //广东
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.gd.syslearning";
                } else {
                    window.location.href = "http://tbx.kingsun.cn/downloadList.html";
                }
            });
        });
    </script>
</head>
<body>
    <div class="wrap w1">
        <audio id="audio1">
            <source src="">
            <%--<source src="audio/ceshi.ogg">--%>
            您的浏览器不支持audio标签。 
        </audio>
        <header>
            <div class="head_bag">
                <div class="head_bagg">
                    <img src="img/backdrop.png" alt="" />
                </div>
                <%--  <h2><span>一年级</span> / <span>Unit 1</span> / <span>lesson 2</span></h2>--%>
                <div class="head_img">
                    <img id="headImg" src="img/xin1.png" alt="" />
                    <p id="headText">默默一星，累觉不爱</p>
                </div>

            </div>
            <div class="head_bag1" id="invincible" style="display: none">
                <audio id="audio4">
                    <source src="audio/invincible.mp3">
                    您的浏览器不支持audio标签。 
                </audio>
                <img src="img/wudi1.gif" alt="" />
            </div>
        </header>
        <div class="main_foot" id="userImg">
            <img id="userImg1" width="100px" height="100px" src="img/tou1.png" alt="" />
            <div class="userImg">
                <h2 id="userName">嘟嘟</h2>
                <span id="creatTime">2016.10.12</span>
            </div>
        </div>
        <a class="challenge" id="linkapp">
            <img src="img/challenge.png" alt="" /><span>打开同步学，发起挑战</span></a>
        <div class="content1">
            <h2>详细得分</h2>
            <div class="contentDetail" id="detail">
            </div>
        </div>
        <div class="content">
                <p class="p1">同步学更多内容</p>
                <div class="content_main">
                    <a class="linkapp" href="javascript:void(0);">
                        <img src="img/app/ebook.png" alt="" />
                        <h2>E-BOOK</h2>
                        <p>电子书、点读、复读、翻译</p>
                    </a>
                </div>
                <div class="content_main">
                    <a class="linkapp" href="javascript:void(0);">
                        <img src="img/app/listen.png" alt="" />
                        <h2>随身听</h2>
                        <p>磨耳朵、定时定量、保护视力</p>
                    </a>
                </div>
                <div class="content_main">
                    <a class="linkapp" href="javascript:void(0);">
                        <img src="img/app/tack.png" alt="" />
                        <h2>说说看</h2>
                        <p>听说作业、口语评测</p>
                    </a>
                </div>
                <div class="content_main">
                    <a class="linkapp" href="javascript:void(0);">
                        <img src="img/app/qupei.png" alt="" />
                        <h2>趣配音</h2>
                        <p>课本剧、电影、自然拼读</p>
                    </a>
                </div>
            </div>
        <%--<footer class="ft">
            <img src="img/app/logoImg.png" alt="logo" />
            <img class="text2" src="img/app/text2.png" alt="text2" />
            <a href="http://tbx.kingsun.cn/downloadList.html"><span>打开</span></a>
        </footer>--%>
    </div>


    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="js/ShareResultInit.js"></script>
</body>
</html>
