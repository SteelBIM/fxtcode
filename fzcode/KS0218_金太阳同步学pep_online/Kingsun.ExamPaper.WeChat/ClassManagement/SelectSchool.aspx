<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectSchool.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.ClassManagement.SelectSchool" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=no" />
    <title>选择学校</title>
    <link href="../Css/OfficialAccounts/seleschool.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <header class="fixed">
            <div class="header search">
                <a>
                    <img src="../Images/OfficialAccounts/search.png" />
                </a>
                <input type="text" placeholder="输入中文搜索" onkeyup="keyup()" />
            </div>
        </header>
        <div class="sort_box sort_box1 results" id="div_info">
        </div>
        <div class="initials">
            <ul>
                <li>
                    <img src="../Images/OfficialAccounts/068.png" />
                </li>
            </ul>
        </div>
    </form>
    <script src="../Js/jquery-1.10.2.min.js"></script>
    <script src="../Js/CommonDB.js"></script>
    <script src="../Js/OfficialAccounts/SelectSchoolInit.js"></script>
    <script src="../Js/jquery.charfirst.pinyin.js"></script>
    <script src="../Js/sort.js"></script>
    <script src="../Js/holmes.js"></script>
    <script src="../Js/microlight.js"></script>
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
    </script>
</body>
</html>
