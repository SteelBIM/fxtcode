<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="callback.aspx.cs" Inherits="WebAppTest.callback" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#key").keyup(function () {
                var va = $(this).val()
                len = strLen(va);
                $(this).width(len * 14+7 + "px");
            });
        });

        function strLen(str) {
            var charset = document.charset;
            var len = 0;
            for (var i = 0; i < str.length; i++) {
                len += str.charCodeAt(i) < 0 || str.charCodeAt(i) > 255 ? (charset == "utf-8" ? 2 : 1) : 0.5;
            }
            return len;
        }
    </script>
<style type="text/css">
        .toproject{ border:2px solid #5D738E; padding:5px; font:1em/1.5em 'Microsoft YaHei'; width:80%; overflow:hidden;
        }
        .divnormal{ background-color:#66FFFF; float:left; cursor:pointer; margin-right:2px;margin-bottom:2px; *display:inline-block;}
        
        .divtext input{text-indent:0; text-shadow:none; border:0;float:left;margin-right:5px;*display:inline-block;}
    </style>
    <script src="../js/default.js" type="text/javascript"></script>
</head>
<body class="bgwhite">
<div id="panel">
    <div id="projectlist" class="toproject">
        <div class="divtext"><input style="width:1em; line-height:24px; height:24px;"  id="searchkey"/></div>
        <div style="clear:both;"></div>
    </div>
</div>
<div>
    <input type="button" class="searchBtn" id="btnsearch" value="确定" />
</div>
</body>
</html>
