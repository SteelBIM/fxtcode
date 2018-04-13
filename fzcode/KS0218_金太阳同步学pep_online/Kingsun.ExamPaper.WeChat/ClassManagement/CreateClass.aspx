<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateClass.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.ClassManagement.CreateClass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>创建班级</title>
    <script src="../Js/jquery-1.10.2.min.js"></script>
    <script src="../Js/CommonDB.js"></script>
    <script src="../Js/mobile.js"></script>
    <script src="../Js/OfficialAccounts/OfficialAccounts.js"></script>
    <link href="../Css/OfficialAccounts/OfficialAccounts.css" rel="stylesheet" />
    <script src="../Js/OfficialAccounts/CreateClass.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main main2">
            <div class="Html4" id="listHtml">
                <p class="p2">选择您所教的班级</p>
                <div class="bao">
                    <ul class="ul1">
                        <li class="li1 on" onclick="qiehuan(1)"><span>一年级</span></li>
                        <li class="li2" onclick="qiehuan(2)"><span>二年级</span></li>
                        <li class="li3" onclick="qiehuan(3)"><span>三年级</span></li>
                        <li class="li4" onclick="qiehuan(4)"><span>四年级</span></li>
                        <li class="li5" onclick="qiehuan(5)"><span>五年级</span></li>
                        <li class="li6" onclick="qiehuan(6)"><span>六年级</span></li>
                    </ul>
                    <div class="banji">
                        <dl>
                            <dt class="dt1">1班</dt>
                            <dt class="dt2">2班</dt>
                            <dt class="dt3">3班</dt>
                            <dt class="dt4">4班</dt>
                            <dt class="dt5">5班</dt>
                            <dt class="dt6">6班</dt>
                            <dt class="dt7">7班</dt>
                            <dt class="dt8">8班</dt>
                            <dt class="dt9">9班</dt>
                            <dt class="dt10">10班</dt>
                            <dt class="dt11">11班</dt>
                            <dt class="dt12">12班</dt>

                        </dl>
                    </div>
                </div>
                <p class="p3">已选班级<span>0</span>个--</p>
                <ul class="ul2">
                </ul>
            </div>
        </div>
        <div class="footer">
            <a class="next" id="nextStep" href="#" >完成</a>
        </div>
    </form>
</body>
</html>
