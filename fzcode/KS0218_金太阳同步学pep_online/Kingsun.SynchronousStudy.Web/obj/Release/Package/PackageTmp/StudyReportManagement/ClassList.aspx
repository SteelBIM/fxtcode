<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClassList.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.ClassList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>班级列表</title>
    <link href="../AppTheme/css/hear.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="head_back">
                <a class="h_close" id="close">[关闭]</a>
                <h2 style=" margin-right:60px;">班级列表</h2>
            </div>
            <div class="Html5">
                <div class="classList">
                </div>
                <div class="dianji">
                    <a class="bianji" id="edit">编辑管理</a>
                    <a class="chuangjian" id="creatClass">创建班级</a>
                </div>
                <div class="dianji1" style="display: none;">
                    <a class="bianji1" id="cancel">取消</a>
                </div>

                <div class="box2" id="box2">
                    <p>确认要删除所选班级吗？</p>
                    <a class="no" id="cancelDeleteClasses">取消</a>
                    <a class="yes" id="deleteClasses">确认</a>
                </div>
                <div class="box3" id="box3">
                    <p>确认要删除这个班级吗？</p>
                    <a class="no" id="cancelDeleteClass">取消</a>
                    <a class="yes" id="deleteClass">确认</a>
                </div>

            </div>
        </div>
        <div class="footer">
            <a class="yaoqing" id="InviteStu"><span >邀请学生</span></a>
            <a class="shanchu" href="javascript:void(0);"><span>删除所选班级(<i id="classNum">0</i>)</span></a>
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
    <script src="../Scripts/MobileTerminal/mobile.js"></script>
    <script src="../Scripts/MobileTerminal/JsBridge.js"></script>
    <script src="../Scripts/MobileTerminal/demo.js"></script>
    <script src="../Scripts/MobileTerminal/ClassListInit.js"></script>

</body>
</html>
