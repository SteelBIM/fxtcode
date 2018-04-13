<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Share.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.Share" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1, user-scalable=0"
        name="viewport" />
    <title>金太阳同步学</title>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <link href="Share/css/share.css" rel="stylesheet" />
    <script src="../AppTheme/js/jquery-easyui/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../AppTheme/js/Common.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script type="text/javascript">

        $(function () {
            //  var userID = "1847530068";
            //  var videoFileID = "83b27434-ce30-477f-8e2c-7941aa00b9b6";
            var userID = Common.QueryString.GetValue("userID");
            var videoFileID = Common.QueryString.GetValue("VideoFileID");
            var IsEnableOss = Common.QueryString.GetValue("IsEnableOss");
            var AppID = Common.QueryString.GetValue("AppID");
            var BookAppID = Common.QueryString.GetValue("BookAppID");
            var fromModule = Common.QueryString.GetValue("fromModule");

            var obj = { userID: userID };
            $.post("?action=getUser", obj, function (data) {
                var fileamgeid = "";
                if (data) {
                    var result = eval("(" + data + ")");
                    $("#username").html(result.TrueName);
                    if (result.UserImage == '00000000-0000-0000-0000-000000000000') {
                        fileamgeid = "Share/img/app/hea.png";
                    } else {
                        if (IsEnableOss == 0) {
                            fileamgeid = Constant.file_Url + "/GetFiles.ashx?FileID=" + result.UserImage;
                        } else {
                            if (result.IsEnableOss == 0) {
                                fileamgeid = Constant.file_Url + "/GetFiles.ashx?FileID=" + result.UserImage;
                            } else {
                                fileamgeid = Constant.Ossfile_Url + result.UserImage;
                            }

                        }

                    }

                    //  var fileamgeid = Constant.file_Url + "/GetFiles.ashx?FileID=491794BA-78BA-4D8A-AA59-7B0D2537C040";
                    $("#img").attr("src", fileamgeid);

                    var objs = { videoFileID: videoFileID, IsEnableOss: IsEnableOss, UserID: userID, fromModule: fromModule };
                    $.post("?action=getvideo", objs, function (data) {
                        if (data) {
                            var result = eval("(" + data + ")");
                            // autoplay='autoplay' loop='loop'
                            var htmls = "<video controls='controls' class='vid' id='fidleid' autoplay='autoplay' webkit-playsinline webkit-playsinline>";
                            htmls += "<source type='video/mp4' src=" + result.UserVideoID + ">示例视频1</source>";
                            htmls += "</video>";

                            $("#videoclass").html(htmls);
                            $("#Span2").html(result.NumberOfOraise);//点赞数
                            $("#perfectnum").html(result.perfectnum);
                            $("#execlentnum").html(result.execlentnum);
                            $("#greetnum").html(result.greetnum);
                            $("#goodnum").html(result.goodnum);
                            $("#score").html(Math.floor(result.TotalScore * 10) / 10);
                            $("#dates").html(Common.FormatTime(result.creatTime, 'yyyy-MM-dd hh:mm'));
                            if (result.TotalScore >= 90) {
                                $("#img1").attr("src", "Share/img/app/four_star.png");

                            } else if (result.TotalScore >= 80) {
                                $("#img1").attr("src", "Share/img/app/three_star.png");

                            } else if (result.TotalScore >= 60) {
                                $("#img1").attr("src", "Share/img/app/two_star.png");

                            } else {
                                $("#img1").attr("src", "Share/img/app/one_star.png");
                            }
                        }
                    });
                }
            });

            $(".linkapp").click(function () {
                if (AppID == "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385") {//广州版
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.gz.syslearning";
                } else if (AppID == "43716a9b-7ade-4137-bdc4-6362c9e1c999") {//上海本地
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.shbd.syslearning";
                }
                else if (AppID == "241ea176-fce7-4bd7-a65f-a7978aac1cd2") {//深圳版
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.sz.syslearning";
                }
                else if (AppID == "9426808e-da8e-488c-9827-b082c19b62a7") {//上海全国
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.shqg.syslearning";
                }
                else if (AppID == "0a94ceaf-8747-4266-bc05-ed8ae2e7e410") {//北京
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.bj.syslearning";
                }
                else if (AppID == "5373bbc9-49d4-47df-b5b5-ae196dc23d6d") {//人教pep
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.rj.syslearning";
                }
                else if (AppID == "37ca795d-42a6-4117-84f3-f4f856e03c62") {//广东
                    window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.gd.syslearning";
                } else {
                    window.location.href = "http://tbx.kingsun.cn/downloadList.html";
                }
            });
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 0px; height: 0px; overflow: hidden;"></div>
        <div class="wrap w1">
            <header>
                <%-- autoplay='autoplay' loop='loop'--%>
                <div id="videoclass"></div>

                <%-- <video controls="controls" class="vid" id="fidleid">  --%>
                <%-- <source id="fidleid"  type="video/mp4" >示例视频1</source>  
                        您的浏览器不支持video标签 --%>
                <%-- </video>--%>
                <div class="message">
                    <div class="xinxi">
                        <img id="img" src="Share/img/app/me.png" alt="" />
                        <div class="name">
                            <h3><span id="username"></span></h3>
                            <p id="dates"></p>
                        </div>

                    </div>
                    <div class="fenshu">
                        <img id="img1" src="Share/img/app/one_star.png" alt="" />
                        <p id="score"></p>
                    </div>
                </div>
            </header>
            <div class="content">
                <p class="p1">同步学更多内容</p>
                <div class="content_main">
                    <a class="linkapp" href="javascript:void(0);">
                        <img src="Share/img/app/ebook.png" alt="" />
                        <h2>E-BOOK</h2>
                        <p>电子书、点读、复读、翻译</p>
                    </a>
                </div>
                <div class="content_main">
                    <a class="linkapp" href="javascript:void(0);">
                        <img src="Share/img/app/listen.png" alt="" />
                        <h2>随身听</h2>
                        <p>磨耳朵、定时定量、保护视力</p>
                    </a>
                </div>
                <div class="content_main">
                    <a class="linkapp" href="javascript:void(0);">
                        <img src="Share/img/app/tack.png" alt="" />
                        <h2>说说看</h2>
                        <p>听说作业、口语评测</p>
                    </a>
                </div>
                <div class="content_main">
                    <a class="linkapp" href="javascript:void(0);">
                        <img src="Share/img/app/qupei.png" alt="" />
                        <h2>趣配音</h2>
                        <p>课本剧、电影、自然拼读</p>
                    </a>
                </div>
            </div>
            <footer class="ft">
                <span>同步学一款与课本配套的APP</span>
                <a class="linkapp" href="javascript:void(0);">打开</a>
            </footer>
        </div>
    </form>
</body>
</html>
