<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShareApp.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.ShareApp.ShareApp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>一款与课本配套的app—同步学</title>
    <link href="css/shenqi.css" rel="stylesheet" />
    <script src="js/jquery-1.10.2.min.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="js/shenqi.js"></script>
    <script src="js/ShareApp.js"></script>
</head>
<body onload="$('.main').scrollTop(window.document.body.scrollHeight);">
    <form id="form1" runat="server">
        <%-- <audio id="myaudio" autoplay="autoplay" loop="loop" >
            <source src="music/bgmusic04.mp3" type="audio/mpeg" />
        </audio>--%>
        <div class="main">
            <img class="beijin1" src="img/background.png" alt="" />
            <div class="bian">
                
                <span class="biao_ti1"><b id="userName">小拳拳</b>邀请你加入学习</span>
                <div class="phone">
                   
                    <input id="telephone"  placeholder="输入手机号" type="text" />
                </div>
                <div class="mima">
                    <input id="verifyCode" type="text" placeholder="输入短信验证码" />
                    <a class="get" id="getCheckCode">获取验证码</a>
                </div>
                <a class="queding" id="login" href="#">立即学习</a>
            </div>

        </div>
    </form>
     <div style="display:none;">
    <script src="https://s11.cnzz.com/z_stat.php?id=1260315930&web_id=1260315930" language="JavaScript">
   </script>
    </div>
</body>
<%--<script type="text/javascript">
    var cnzz_protocol = (("https:" == document.location.protocol) ? " https://" : " http://");
    document.write(unescape("%3Cspan id='cnzz_stat_icon_1260315930'%3E%3C/span%3E%3Cscript src='" + cnzz_protocol + "s11.cnzz.com/z_stat.php%3Fid%3D1260315930' type='text/javascript'%3E%3C/script%3E"));
</script>--%>
</html>
