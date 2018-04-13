<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InviteStudent.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.InviteStudent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>邀请学生</title>
    <link href="../AppTheme/css/hear.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            
            <div class="Html11">
                <div class="head1">
                    <img src="../AppTheme/images/bagg1.png" alt="" />
                </div>
                <div class="box1" id="box1">
                    <div class="box1_nr">
                        <img class="xinxin" src="../AppTheme/images/xinxin.png" alt="" />
                        <div class="tan_nr">
                            <b>亲爱的同学们：</b>
                            <p>我是英语老师<span id="userName"></span>，赶紧加入我们的班级吧！</p>
                            <img class="ping" src="../AppTheme/images/ping.png" alt="" />
                            <div class="touxiang">
                                <img src="../AppTheme/images/touxiang.png" alt="" id="userImg" />
                            </div>

                        </div>

                        <div class="tan_nr1">
                            <img class="xian" src="../AppTheme/images/xian.png" alt="" />
                            <h3>方法一：先下载APP再加入班级</h3>
                            <p>在app中输入我的邀请码，解锁更多功能</p>
                            <div class="bianma">
                                <b>邀请码：</b>
                                <span id="userID"></span>
                                <img class="pangxie" src="../AppTheme/images/pangxie.png" alt="" />
                                <p>(长按可复制编码)</p>

                            </div>
                            <a class="queding" id="linkapp">打开同步学</a>
                        </div>

                        <div class="tan_nr2">
                            <img class="xian" src="../AppTheme/images/xian.png" alt="" />
                            <h3>方法二：先加入班级再下载APP</h3>
                            <div class="phone">
                                <input id="telephone" type="text" placeholder="输入手机号码，加入班级" />

                            </div>
                            <div class="mima">
                                <input id="verifyCode" type="text" placeholder="输入验证码" />

                            </div>
                            <a class="get" id="getCheckCode">获取验证码</a>
                            <a class="queding" id="next">加入班级</a>
                        </div>
                    </div>
                    <div class="foot">
                        <img src="../AppTheme/images/bagg2.png" alt="" />
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
    <script src="../Scripts/MobileTerminal/InviteStudentInit.js"></script>
</body>
</html>
