<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClassList.aspx.cs" Inherits="Kingsun.ExamPaper.WeChat.ClassManagement.ClassList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>班级列表</title>
    <script src="../Js/jquery-1.10.2.min.js"></script>
    <script src="../Js/mobile.js"></script>
    <script src="../Js/OfficialAccounts/OfficialAccounts.js"></script>
    <link href="../Css/OfficialAccounts/OfficialAccounts.css" rel="stylesheet" />
    <script src="../Js/CommonDB.js"></script>
    <script src="../Js/OfficialAccounts/ClassList.js"></script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script>
        window.onload = function () {
            if (!isWeiXin()) {
                window.location.href = "../error.html";
                return;
            }
        }
        function isWeiXin() {
            var ua = window.navigator.userAgent.toLowerCase();
            if (ua.match(/MicroMessenger/i) == 'micromessenger') {
                return true;
            } else {
                return false;
            }
        }
        wx.config({
            debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。 
            appId: '<%=WXappid%>', // 必填，公众号的唯一标识 
            timestamp: '<%=Timestamp%>', // 必填，生成签名的时间戳 
            nonceStr: '<%=NonceStr%>', // 必填，生成签名的随机串 
            signature: '<%=Signature%>', // 必填，签名，见附录1 
            jsApiList: ['hideOptionMenu', 'onMenuShareAppMessage', 'onMenuShareTimeline', 'onMenuShareQQ'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2 
        });
        wx.ready(function () {
            //wx.hideOptionMenu(); //隐藏分享 
            //获取“分享到朋友圈”按钮点击状态及自定义分享内容接口
            wx.onMenuShareTimeline({
                title: '<%=TrueName%>', // 分享标题 
                 link: '<%=Link%>', // 分享链接 
                 desc: '<%=Desc%>', // 分享描述
                 imgUrl: '<%=ImgUrl%>', // 分享图标 
                 success: function () {
                     // 用户确认分享后执行的回调函数 
                     alert('分享成功');
                 },
                 cancel: function () {
                     // 用户取消分享后执行的回调函数 
                 }
             });
             //获取“分享给朋友”按钮点击状态及自定义分享内容接口
             wx.onMenuShareAppMessage({
                 title: '<%=TrueName%>', // 分享标题 
                link: '<%=Link%>', // 分享链接 
                imgUrl: '<%=ImgUrl%>', // 分享图标 
                desc: '<%=Desc%>', // 分享描述
                type: '', // 分享类型,music、video或link，不填默认为link
                dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                success: function () {
                    // 用户确认分享后执行的回调函数 
                    alert('分享成功');
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数 
                }
            });
             //获取“分享到QQ”按钮点击状态及自定义分享内容接口
             wx.onMenuShareQQ({
                 title: '<%=TrueName%>', // 分享标题 
                link: '<%=Link%>', // 分享链接 
                desc: '<%=Desc%>', // 分享描述
                imgUrl: '<%=ImgUrl%>', // 分享图标 
                success: function () {
                    // 用户确认分享后执行的回调函数 
                    alert('分享成功');
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数 
                }
            });
             //获取“分享到腾讯微博”按钮点击状态及自定义分享内容接口
             wx.onMenuShareWeibo({
                 title: '<%=TrueName%>', // 分享标题 
                link: '<%=Link%>', // 分享链接 
                desc: '<%=Desc%>', // 分享描述
                imgUrl: '<%=ImgUrl%>', // 分享图标 
                success: function () {
                    // 用户确认分享后执行的回调函数 
                    alert('分享成功');
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数 
                }
            });
             //获取“分享到QQ空间”按钮点击状态及自定义分享内容接口
             wx.onMenuShareQZone({
                 title: '<%=TrueName%>', // 分享标题 
                link: '<%=Link%>', // 分享链接 
                desc: '<%=Desc%>', // 分享描述
                imgUrl: '<%=ImgUrl%>', // 分享图标 
                success: function () {
                    // 用户确认分享后执行的回调函数 
                    alert('分享成功');
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数 
                }
            });
         });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main main2">
            <div class="Html3">
                <p class="p2">班级列表</p>
                <ul id="cList">
                    <%-- <li><a href="members.html"><span>一年级3班</span><em><i>3</i>人</em></a></li>
                    <li><a><span>一年级4班</span><em><i>8</i>人</em></a></li>
                    <li><a><span>一年级5班</span><em><i>90</i>人</em></a></li>
                    <li><a><span>一年级8班</span><em><i>7</i>人</em></a></li>--%>
                </ul>
                <a class="add" id="creatClass">创建班级</a>

            </div>

        </div>
        <div class="footer">
            <a class="invite1" href="#" id="InviteStu">邀请学生</a>
        </div>
        <!--弹框-->
        <div class="box">
            <div class="wai">
                <img src="../Images/OfficialAccounts/zhi.png" />
                <p>点击右上方</p>
                <p>分享到班级学生群和家长群</p>
            </div>
            <div class="box_nr">
                <div class="nr1">
                    <p class="p1">亲爱的同学们：</p>
                    <p class="p3">我是英语老师<span id="teaName_1">金老师</span>,快在金太阳教育软件中加入我们的班级，一起学习吧！</p>
                    <p class="p4">英语老师<span id="teaName_2">金老师</span></p>
                    <img class="a1" src="../Images/OfficialAccounts/A.png" />
                    <img class="b1" src="../Images/OfficialAccounts/B.png" />
                    <img class="c1" src="../Images/OfficialAccounts/C.png" />
                </div>
                <a class="close"></a>
            </div>
        </div>
        <div class="shadow1"></div>
    </form>
</body>
</html>
