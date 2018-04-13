<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectRole.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.SelectRole" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>选择角色</title>
    <script type="text/javascript" src="../AppTheme/js/jquery-1.10.2.min.js"></script>
    <%--<script src="../AppTheme/js/artDialog/artDialogAll.js?skin=aero"></script>--%>
    <link href="../AppTheme/js/artDialog/skins/idialog.css" rel="stylesheet" />
    <link href="../AppTheme/js/artDialog/skins/default.css" rel="stylesheet" />
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/MobileTerminal/JsBridge.js"></script>
    <script type="text/javascript" src="../Scripts/MobileTerminal/mobile.js"></script>
    <script type="text/javascript" src="../Scripts/MobileTerminal/demo.js"></script>
    <script type="text/javascript" src="../Scripts/MobileTerminal/SelectRole.js"></script>
    <link href="../AppTheme/css/hear.css" rel="stylesheet" />
    <script>
        $(function () {

            //选择角色
            $(".Html12 ul li .a1").click(function () {
                $(".Html12 ul li .a2 img").attr("src", "../AppTheme/images/students.png");
                $(this).find("img").attr("src", "../AppTheme/images/Img/teacheron.png");

            });

            $(".Html12 ul li .a2").click(function () {
                $(".Html12 ul li .a1 img").attr("src", "../AppTheme/images/teacher.png");
                $(this).find("img").attr("src", "../AppTheme/images/Img/studentson.png");

            });
        });


    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="head_back">
                <a class="h_close" id="close">[关闭]</a>
                <h2 style=" margin-right:60px;">选择角色</h2>
            </div>
            <div class="Html12">
                <ul>
                    <li><a class="a1" id="tea">
                        <img src="../AppTheme/images/teacher.png" alt="" /></a><p>老师</p>
                    </li>
                    <li><a class="a2" id="stu">
                        <img src="../AppTheme/images/students.png" alt="" /></a><p>学生</p>
                    </li>
                </ul>
               <%-- <p class="h_p">选择后不可更改角色</p>--%>
                <a class="ok" id="queding">确定</a>
            </div>
        </div>
        <%-- <div class="footer">
            <a class="next" id="nextStep">下一步</a>
        </div>--%>
    </form>
</body>
</html>
