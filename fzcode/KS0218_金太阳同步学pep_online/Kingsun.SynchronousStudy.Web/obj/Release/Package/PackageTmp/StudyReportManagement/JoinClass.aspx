<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JoinClass.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.JoinClass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>加入班级</title>
    <link href="../AppTheme/css/hear.css" rel="stylesheet" />
    <style type="text/css">
        .on {
            background: #1C9EEA;
            color: #fff;
            border: 1px solid #1C9EEA;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main main1">
            <div class="head_back">
                <a class="h_left" href="javascript:history.go(-1)"></a>
                <a class="h_close" id="close">[关闭]</a>
                <h2>加入班级</h2>
            </div>
            <div class="Html10">
                <div class="head1">
                    <img src="../AppTheme/images/bagg1.png" alt="" />
                </div>
                <div class="box1" id="box1">
                    <div class="box1_nr">
                        <img class="xinxin" src="../AppTheme/images/xinxin.png" alt="" />
                        <div class="tan_nr">
                            <b>亲爱的同学们：</b>
                            <p>我是英语老师<span id="name"></span><span id="welcome">，赶紧加入我们的班级吧！</span></p>
                            <img class="ping" src="../AppTheme/images/ping.png" alt="" />
                            <div class="touxiang">
                                <img src="../AppTheme/images/touxiang.png" alt="" id="userImg" />
                            </div>
                        </div>
                        <div class="tan_nr1" style="display: block;">
                            <img class="xian" src="../AppTheme/images/xian.png" alt="" />
                            <b>请选择您所在的班级</b>
                            <img class="pangxie" src="../AppTheme/images/pangxie.png" alt="" />
                            <ul id="classList">
                            </ul>
                            <a class="queding" href="#" id="sure">确&nbsp;&nbsp;&nbsp;认</a>
                        </div>

                        <div class="tan_nr2" style="display: none">
                            <img class="xian" src="../AppTheme/images/xian.png" alt="" />
                            <img class="pangxie" src="../AppTheme/images/pangxie.png" alt="" />
                            <img class="chahua" src="../AppTheme/images/chahua.png" alt="" />

                            <p>您已经成功加入到班级！</p>
                            <p>快去看看班级的其他小伙伴吧~</p>
                            <a class="queding" id="linkapp">打开同步学</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <%--<script src="../AppTheme/js/artDialog/artDialogAll.js?skin=aero"></script>--%>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/MobileTerminal/JsBridge.js"></script>
    <script src="../Scripts/MobileTerminal/mobile.js"></script>
    <script src="../Scripts/MobileTerminal/demo.js"></script>
    <script src="../Scripts/MobileTerminal/JoinClassInit.js"></script>
</body>
</html>
