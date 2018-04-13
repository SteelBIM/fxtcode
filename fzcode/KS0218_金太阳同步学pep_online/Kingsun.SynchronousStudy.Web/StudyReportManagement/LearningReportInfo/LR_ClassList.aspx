<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LR_ClassList.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.LearningReportInfo.LR_ClassList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>学习报告--报告列表</title>
    <script type="text/javascript" src="../../AppTheme/js/CommonDB.js"></script>
    <script type="text/javascript" src="../../AppTheme/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../../Scripts/MobileTerminal/mobile.js"></script>
    <script type="text/javascript" src="../../Scripts/MobileTerminal/demo1.js"></script>
    <script type="text/javascript" src="../../Scripts/MobileTerminal/LR_ClassListInit.js"></script>
    <link href="../../AppTheme/css/calendar.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <div class="main">
        <div class="header1">
            <a class="left on">管理</a>
            <a class="right">报告</a>
            <a class="me">我</a>
        </div>
        <h2 class="head"><a class="cebie" onclick="chosegrade();"><span id="span" bookid="">一年级上册</span><img src="../../AppTheme/images/la2.png" alt="" /></a><a class="yesterday">昨天（周三）</a></h2>
        <div class="htmlg1" id="ht">
        </div>
        <!--弹出框-->
        <div class="toolwin1">
            <div class="to_ul1">
                <ul class="timelist1">
                    <li id="li1" class="wu"><span>一年级上册</span><p>暂无内容</p>
                    </li>
                    <li id="li2" class="wu"><span>一年级下册</span><p>暂无内容</p>
                    </li>
                    <li id="li3" class="wu"><span>二年级上册</span><p>暂无内容</p>
                    </li>
                    <li id="li4" class="wu"><span>二年级下册</span><p>暂无内容</p>
                    </li>
                    <li id="li5" class="wu"><span>三年级上册</span><p>暂无内容</p>
                    </li>
                    <li id="li6" class="wu"><span>三年级下册</span><p>暂无内容</p>
                    </li>
                    <li id="li7" class="wu"><span>四年级上册</span><p>暂无内容</p>
                    </li>
                    <li id="li8" class="wu"><span>四年级下册</span><p>暂无内容</p>
                    </li>
                    <li id="li9" class="wu"><span>五年级上册</span><p>暂无内容</p>
                    </li>
                    <li id="li10" class="wu"><span>五年级下册</span><p>暂无内容</p>
                    </li>
                    <li id="li11" class="wu"><span>六年级上册</span><p>暂无内容</p>
                    </li>
                    <li id="li12" class="wu"><span>六年级下册</span><p>暂无内容</p>
                    </li>
                </ul>
            </div>
        </div>
        <div class="maskLayer1"></div>
    </div>
</body>
</html>
