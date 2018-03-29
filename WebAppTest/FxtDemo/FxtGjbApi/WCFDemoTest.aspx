﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WCFDemoTest.aspx.cs" Inherits="FxtDemo.FxtGjbApi.WCFDemoTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    
<script type="text/javascript">
    $(document).ready(function () {

        $("#focus .input_txt").each(function () {
            var thisVal = $(this).val();
            //判断文本框的值是否为空，有值的情况就隐藏提示语，没有值就显示
            if (thisVal != "") {
                $(this).siblings("span").hide();
            } else {
                $(this).siblings("span").show();
            }
            //聚焦型输入框验证	
            $(this).focus(function () {
                $(this).siblings("span").hide();
            }).blur(function () {
                var val = $(this).val();
                if (val != "") {
                    $(this).siblings("span").hide();
                } else {
                    $(this).siblings("span").show();
                }
            });
        })
        $("#keydown .input_txt").each(function () {
            var thisVal = $(this).val();
            //判断文本框的值是否为空，有值的情况就隐藏提示语，没有值就显示
            if (thisVal != "") {
                $(this).siblings("span").hide();
            } else {
                $(this).siblings("span").show();
            }
            $(this).keyup(function () {
                var val = $(this).val();
                $(this).siblings("span").hide();
            }).blur(function () {
                var val = $(this).val();
                if (val != "") {
                    $(this).siblings("span").hide();
                } else {
                    $(this).siblings("span").show();
                }
            })
        })
    })
</script>
<style type="text/css">
	form{width:400px;margin:10px auto;border:solid 1px #E0DEDE;background:#FCF9EF;padding:30px;box-shadow:0 1px 10px rgba(0,0,0,0.1) inset;}
	label{display:block;height:40px;position:relative;margin:20px 0;}
	span{position:absolute;float:left;line-height:40px;left:10px;color:#BCBCBC;cursor:text;}
	.input_txt{width:398px;border:solid 1px #ccc;box-shadow:0 1px 10px rgba(0,0,0,0.1) inset;height:38px;text-indent:10px;}
	.input_txt:focus{box-shadow:0 0 4px rgba(255,153,164,0.8);border:solid 1px #B00000;}
	.border_radius{border-radius:5px;color:#B00000;}
	h2{font-family:"微软雅黑";text-shadow:1px 1px 3px #fff;}
</style>
</head>
<body>
<form class="border_radius" id="focus">
        <h2>聚焦型提示语消失</h2>
        <label><span>花瓣注册邮箱</span><input type="text" class="input_txt border_radius"  /></label>
        <label><span>密码</span><input type="text" class="input_txt border_radius" /></label>
    </form>
</form>
</body>
</html>