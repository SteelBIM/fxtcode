<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns="http://www.w3.org/1999/xhtml"><head id="head"><meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" /><meta http-equiv="content-type" content="text/html;charset=utf-8" />
<link rel="Stylesheet" type="text/css" href="http://localhost:5100/css/default/public.css?130710180753673" /><link rel="Stylesheet" type="text/css" href="http://localhost:5100/css/default/form.css?130710180753673" /><link rel="Stylesheet" type="text/css" href="/templates/main/css/default/style.css?130710180753673" /><script type="text/javascript" src="http://localhost:5100/js/jquery-1.7.1.min.js"></script><script type="text/javascript" src="http://localhost:5100/js/json2.js"></script><script type="text/javascript" src="http://localhost:5100/js/init.js?130710180753673"></script><script type="text/javascript">CAS = $.extend({},CAS,{Domain:'',RootUrlFull:'http://localhost:3158/',RootUrl:'/',StaticUrl:'http://localhost:5100/',APIUrl:'/api/',StaticVersion:'130710180753673',Style:'default', DataCenterService: 'http://localhost:9998/'});CAS.Define = $.extend({},CAS.Define,{companyid:0,userid:0,username:'',truename:'',departmentname:'',mobilephone:'',systypecode:1003018});if (!top.dialog) {CAS.Include(['jquery.dialog.js']);} CAS.Debug=true;</script><script type="text/javascript" src="http://localhost:5100/js/log4javascript.js"></script><!--[if lt IE 7]><style type="text/css">*html{background-image:url(about:blank)}</style><script type="text/javascript" src="http://localhost:5100/js/jquery.bgiframe.js"></script><![endif]-->
<script type="text/javascript">
    //测试账号
    var testusers = [
    { username: "admin@gjb", truename: "管理员" },
     { username: "szyw1@gjb", truename: "深圳业务员1-登记业务、提交业务" },
     { username: "szyw2@gjb", truename: "深圳业务员2-登记业务、提交业务" },
    { username: "szgjs1@gjb", truename: "深圳估价师1-回价、做报告" },
    { username: "szgjs2@gjb", truename: "深圳估价师2-回价、做报告" },
    { username: "szjsjl@gjb", truename: "深圳技术经理-分配业务" },
    { username: "szxz@gjb", truename: "深圳行政-复印、盖章、归档、收费" },
    { username: "gzyw@gjb", truename: "广州业务员1-登记业务、提交业务" },
    { username: "gzgjs1@gjb", truename: "广州估价师1-回价、做报告" },
    { username: "gzgjs2@gjb", truename: "广州估价师2-回价、做报告" },
    { username: "gzjsjl@gjb", truename: "广州技术经理-分配业务" },
    { username: "gzxz@gjb", truename: "广州行政-复印、盖章、归档、收费"}];
    $(function () {
        //测试账号
        var html = ['<option value="">选择测试账号登录</option>'];
        $.each(testusers, function (i, user) {
            html.push('<option value="' + user.username + '" href="javascript:void(0)">' + user.truename + '</option>')
        });
        $("#testusers").html(html.join(""))
        .bind("change", function () {
            $("#txtusername").val($(this).val());
            $("#txtpassword").val("654321");
        });

        $(document.body).addClass("loginbg");
        if ($("#txtusername").val() != "") $("#txtusername").addClass("user-input-focus");
        if ($("#txtpassword").val() != "") $("#txtpassword").addClass("pass-input-focus");
        $("input").removeClass("input");
        $(document).keydown(function (event) {
            var e = event || window.event || arguments.callee.caller.arguments[0];
            if (e && e.keyCode == 13) {
                $("#btnlogin").click();
            }
        });
        $("#txtusername").focus(function () {
            $(this).addClass("user-input-focus");
        }).blur(function () {
            if ($(this).val() == "") {
                $(this).removeClass("user-input-focus");
            }
        });
        $("#txtpassword").focus(function () {
            $(this).addClass("pass-input-focus");
        }).blur(function () {
            if ($(this).val() == "") {
                $(this).removeClass("pass-input-focus");
            }
        });
        $("#btnlogin").hover(function () {
            $(this).addClass("login-btn-hover");
        }, function () {
            $(this).removeClass("login-btn-hover");
        }).click(function () {
            var username = $("#txtusername").val();
            var password = $("#txtpassword").val();
            if (username == null || username == "") {
                CAS.Alert("请输入登录名");
                return;
            }
            if (password == null || password == "") {
                CAS.Alert("请输入密码");
                return;
            }
            var progress = CAS.Progress("正在登录，请稍候...");
            CAS.API({
                type: "post",
                api: "user.login",
                data: { username: username, password: password, autologin: $("#autologin").attr("checked") ? 1 : 0 },
                callback: function (data) {
                    progress.close();
                    if (data.returntype == 1) {
                        if (CAS.Debug) {
                            $(window).cashref("main.aspx");
                        }
                        else {
                            //修复IE弹出窗口自动关闭的BUG，IE会出现提醒，kevin
                            $("#lnkmain")[0].click();
                            setTimeout(function () {
                                if (win != null) {
                                    window.opener = null;
                                    window.open("", "_self");
                                    window.close();
                                }
                            }, 10);
                        }
                    } else {
                        CAS.Alert(data.returntext);
                    }
                }
            });
        });
    });
    var win = null;
    function openwin() {
        win = window.open('main.aspx', '_blank', 'width=' + screen.availWidth + ',height=' + (screen.availHeight - 20) + ', left=0,top=0,toolbar=no, menubar=no, scrollbars=no,location=no, status=no,resizable=yes');
    }
    </script>

<title>
	估价宝
</title></head><body>
<div class="login">
    <div class="w220 l di ml40">
        <h1 class="login-logo"></h1>
        <p class="20 lh20 gray2 mt15 tr">技术支持：深圳房讯通信息技术有限公司<a class="dn" href="javascript:onclick=openwin()" id="lnkmain"></a></p>
    </div>
    <div class="login-form w210 di l">
        <h1 class="login-tit">管理后台登录</h1>
        <p><input tabindex="1" maxlength="20" type="text" id="txtusername" class="user-input" value="" /></p>
        <p><input tabindex="1" maxlength="20" type="password" id="txtpassword" class="pass-input" value="" /></p>
        <p class="remember"><label><input type="checkbox" id="autologin" />下次自动登录</label>&nbsp;</p>
        <p><input type="button" id="btnlogin" class="login-btn" value="登录" /></p>
        <p class="m10"><select id="testusers"></select></p>
    </div>
</div>
</body></html>