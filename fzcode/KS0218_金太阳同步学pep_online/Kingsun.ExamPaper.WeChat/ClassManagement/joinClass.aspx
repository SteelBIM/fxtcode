<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="joinClass.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.ClassManagement.joinClass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>加入班级</title>
    <script src="../Js/jquery-1.10.2.min.js"></script>
    <script src="../Js/mobile.js"></script>
    <script src="../Js/CommonDB.js"></script>
    <script src="../Js/studentpro/studentpro.js"></script>
    <link href="../Css/studentpro/studentpro.css" rel="stylesheet" />
    <script src="../Js/studentpro/joinClass.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="joinclass">
                <!-- 第一步：手机号绑定老师-->
                <div class="content">
                    <div class="foreword">
                        <p>亲爱的同学们:</p>
                        <p class="p1">我是英语老师<span id="teaName_1">金老师</span>，赶紧加入我们的班级吧！</p>
                    </div>
                    <div class="step">
                        <img src="../Images/studentpro/plan.png" />
                    </div>

                    <div class="phone">
                        <div class="phone_img">
                            <img src="../Images/studentpro/phone.png" />
                        </div>
                        <input id="telephone" type="text" placeholder="输入家长手机号码" />
                    </div>
                    <div class="mima">
                        <div class="mima_img">
                            <img src="../Images/studentpro/code.png" />
                        </div>
                        <input id="verifyCode" type="text" placeholder="输入验证码" />
                    </div>
                    <a class="get" id="getCheckCode">获取验证码</a>
                    <a class="queding" id="next">加入班级</a>
                </div>
                <!--第二步：选择班级、填姓名-->
                <div class="content1">
                    <div class="step">
                        <img src="../Images/studentpro/plan1.png" />
                    </div>
                    <div class="chose">
                        <p>选择班级</p>
                        <ul id="classList">
                        </ul>
                        <p>填写姓名</p>
                        <div class="name">
                            <input type="text" />
                        </div>
                    </div>
                    <a class="confirm" id="sure">确定</a>
                </div>
                <!-- 第三步：完成-->
                <div class="content2">
                    <img src="../Images/studentpro/star.png" />
                    <h2>完成</h2>
                    <p>你已加入班级</p>
                    <p>快打开电脑去金太阳登录吧！</p>
                </div>
                <div class="foot">
                    <img src="../Images/studentpro/footer.png" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
