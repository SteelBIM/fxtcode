<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectSchool.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.SelectSchool" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=no" />
    <title>信息选择</title>
    <link rel="stylesheet" href="../AppTheme/css/seleschool.css" />
    <link rel="stylesheet" href="../AppTheme/css/style.css" />
    <script type="text/javascript" src="../AppTheme/js/jquery-1.10.2.min.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/MobileTerminal/JsBridge.js"></script>
    <script src="../Scripts/MobileTerminal/selectSchoolInit.js"></script>
</head>
<body>
    <div class="head_back">
        <a class="h_left" href="javascript:history.go(-1)"></a>
        <a class="h_close" id="close">[关闭]</a>
        <h2>信息选择</h2>
    </div>
    <header class="fixed" style="display: none">
        <div class="header search">
            <a>
                <img src="../AppTheme/images/search.png" alt="" />
            </a>
            <input type="text" placeholder="输入学校名称搜索" />
        </div>
    </header>

    <div class="sort_box results" id="div_info">
        <div class="sort_list">
        </div>

    </div>
    <div class="initials">
        <ul>
            <li>
                <img src="../AppTheme/images/068.png" />
            </li>
        </ul>
    </div>


    <script type="text/javascript" src="../Scripts/MobileTerminal/jquery.charfirst.pinyin.js"></script>
    <script type="text/javascript" src="../Scripts/MobileTerminal/sort.js"></script>
    <script src="../Scripts/MobileTerminal/holmes.js"></script>
    <script src="../Scripts/MobileTerminal/microlight.js"></script>

    <script>
        // holmes setup
        holmes({
            input: '.search input',
            find: '.results .sort_list',
            placeholder: '',
            "class": {
                visible: 'visible',
                hidden: 'hidden'
            }
        });
        holmes({
            input: '.search input',
            find: '.results .sort_letter',
            placeholder: '',
            "class": {
                visible: 'visible',
                hidden: 'hidden'
            }
        });
    </script>
</body>
</html>
