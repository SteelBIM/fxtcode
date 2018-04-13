<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClassDetail.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.ClassDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>班级圈</title>
    <link href="../AppTheme/css/hear.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="head_back">
            <a class="h_left" href="javascript:history.go(-1)"></a>
            <a class="h_close" id="close">[关闭]</a>
            <h2>班级圈</h2>
        </div>
        <div class="header">
            <div class="dianji">
                <span class="bianji" id="studentNum"></span>
                <a class="chuangjian" id="edit">编辑</a>
                <a class="chuangjian1" id="cancel">取消</a>
            </div>
        </div>
        <div class="main">

            <div class="Html6">
                <div class="studentList">
                </div>
                
                <div class="box2" id="box2">
                    <p>确认要删这些学生吗？</p>
                    <a class="no" id="cancelDeleteStudents">取消</a>
                    <a class="yes" id="deleteStudents">确认</a>
                </div>
                <div class="box3" id="box3">
                    <p>确认要删除这个学生吗？</p>
                    <a class="no" id="cancelDeleteStudent">取消</a>
                    <a class="yes" id="deleteStudent">确认</a>
                </div>

            </div>
        </div>
        <div  class="black_overlay1">
        </div>
        <div class="footer">
            <a class="yaoqing" href="javascript:void(0);" id="InviteStu"><span>邀请学生</span></a>
            <a class="shanchu" href="javascript:void(0);"><span>删除所选学生(<i id="stuNum">0</i>)</span></a>
        </div>
        <div class="overlay" style="display: none" id="overlay">
            <!--弹出层时背景层DIV-->
            <div id="fade" class="black_overlay">
            </div>
            <!--弹出框-->
            <div class="box1" id="box1">
                <div></div>
                <%--        <div class="tishi">
                    <p>点击右上方</p>
                    <p>分享到班级学生群和家长群</p>
                </div>
                <img class="zhishi" src="../AppTheme/images/lianjie.png" alt="" />--%>
                <div class="box1_nr">
                    <img class="xinxin" src="../AppTheme/images/xinxin.png" alt="" />
                    <div class="tan_nr">
                        <b>亲爱的同学们：</b>
                        <img class="pangxie" src="../AppTheme/images/pangxie.png" alt="" />
                        <p>我是英语老师<span id="teaName_1"></span>，请下载<span id="edition">金太阳同步学牛津深圳版</span>，赶紧加入班级让我们一起学习！</p>
                        <i>英语老师:&nbsp;&nbsp;<span id="teaName_2"></span></i>
                        <img class="ping" src="../AppTheme/images/ping.png" alt="" />
                       <%-- <div class="touxiang">
                            <img src="../AppTheme/images/touxiang.png" id="userImg" alt="" />
                        </div>--%>
                    </div>
                    <div>
                        <a id="share"><span>立即邀请</span></a>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <%--<script src="../AppTheme/js/artDialog/artDialogAll.js?skin=aero"></script>--%>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/MobileTerminal/JsBridge.js"></script>
    <script src="../Scripts/MobileTerminal/demo.js"></script>
    <script src="../Scripts/MobileTerminal/mobile.js"></script>
    <script src="../Scripts/MobileTerminal/ClassDetailInit.js"></script>
</body>
</html>
