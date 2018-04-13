<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.ClassManagement.SignIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>登陆</title>
    <script src="../Js/jquery-1.10.2.min.js"></script>
    <script src="../Js/mobile.js"></script>
    <script src="../Js/CommonDB.js"></script>
    <script src="../Js/OfficialAccounts/OfficialAccounts.js"></script>
    <link href="../Css/OfficialAccounts/OfficialAccounts.css" rel="stylesheet" />
    <script src="../Js/OfficialAccounts/SignIn.js"></script>
    <script>
        window.onload = function () {
            if (!isWeiXin()) {
                window.location.href = "../error.html";
                return;
            }
        }
        function isWeiXin() {
            var ua = window.navigator.userAgent.toLowerCase();
            if (ua.match(/MicroMessenger/i) == 'micromessenger') {
                return true;
            } else {
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="Html1">
                <div class="beijin">
                    <img src="../Images/OfficialAccounts/logo.png" alt="" />
                </div>
                <p>金太阳教育软件</p>
                <div class="phone">
                    <span>+86</span>
                    <input id="telephone" type="text" placeholder="输入手机号码" />
                    <a id="clearTele"></a>
                </div>
                <div class="mima">
                    <span>
                        <img src="../Images/OfficialAccounts/code.png" alt="" />
                    </span>
                    <input id="verifyCode" type="text" placeholder="输入验证码" />
                    <a id="clearCode"></a>
                </div>
                <a class="get" id="getCheckCode">获取验证码</a>
                <span class="hint"></span>
                <a class="queding" id="login">登&nbsp;&nbsp;&nbsp;录</a>
            </div>
        </div>
        <!--弹框-->
        <div class="box1">
            <h2>是否设置为老师身份？</h2>
            <p>注：此公众号仅供老师使用，请谨慎修改</p>
            <a id="sure">确定</a>
            <a id="close">取消</a>
        </div>
        <div class="shadow1"></div>
    </form>
    <input id="hfOpenId" type="hidden" runat="server" />
</body>
</html>
