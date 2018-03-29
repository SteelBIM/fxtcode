<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="FxtUserCenterService.Hosting._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改密码</title>
    <link rel="stylesheet" type="text/css" href="/css/base.css"/>
    <script src="/js/jquery-1.8.2-min.js" type="text/javascript"></script>
    <script src="/js/init.js" type="text/javascript"></script>
    <style type="text/css">
    <!--
    body{margin:0;font-size:.7em;font-family:Verdana, Arial, Helvetica, sans-serif;background:#EEEEEE;}
    fieldset{padding:0 15px 10px 15px;} 
    h1{font-size:2.4em;margin:0;color:#FFF;}
    h2{font-size:1.7em;margin:0;color:#CC0000;} 
    h3{font-size:1.2em;margin:10px 0 0 0;color:#000000;} 
    #header{width:96%;margin:0 0 0 0;padding:6px 2% 6px 2%;font-family:"trebuchet MS", Verdana, sans-serif;color:#FFF;
    background-color:#555555;}
    #content{margin:0 0 0 2%;position:relative;}
    .content-container{background:#FFF;width:96%;margin-top:8px;padding:10px;position:relative;}
    -->
    </style>
</head>
<body>
    <form id="form1" runat="server"> 
    <div class="login  <%=returntype==0?"":"dn" %>">
	<div class="logo"><img src="/img/logo.png" width="150" height="44" /><img src="../img/title.png" width="152" height="29" class="img_zi" /></div>
	<div>
    <div class="land" id="pwdupdate">
    <div>
        <div class="error_e" style="display:block;width:550px;left: 0px;">
            <em></em>密码长度6~20（必须是含有字母、数字的组合）
        </div>
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td class="t_r">新密码：</td>
                <td>
                <input type="password" id="newpwd" class="i_w320" /></td>
              </tr>
              <tr>
                <td class="t_r">确认密码：</td>
                <td>
                <input type="password" id="surepwd" class="i_w320"/></td>
              </tr>
              <tr>
                <td>&nbsp;</td>
                <td><input type="button" id="btnUpdate" name="button" class="i_button" value="修改密码" />
              </tr>
          </table>
        <div class="expire" id="expire" style="display:none;width:350px;left: 50px;color:Black;">
            <em></em><span id="pwderror">用户或原密码错误</span>
        </div>
	 </div>
    </div>
    <div class="land dn" id="pwdsuccess" style="height:300px; padding-top:10px; text-align:center;">
        <div class="success dn" id="hasHistory" style="width:350px;left: 50px;">
            <em></em><span>密码修改成功，请返回并<a id="historyurl" style=" cursor:pointer; color: #1169EE; text-decoration:none;">重新登录</a></span>
        </div>
        <div class="success dn" id="noHistory" style="width:350px;left: 50px;">
            <em></em><span >密码修改成功，请返回软件并重新登录。</span>
        </div>
     </div>
     </div>
    </div>
    <div class="<%=returntype==-1?"":"dn" %>" id="illVisit">
    <div id="header"><h1>ERROR</h1></div>
    <div id="content">
     <div class="content-container"><fieldset>
      <h2>404。</h2>
      <h3>Do not try and find the page. That’s impossible. Instead only try to realise the truth:。</h3>
     </fieldset></div>
    </div>    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    $(function () {
        $("#btnUpdate").btnMouseSweep("i_button2", "i_button").click(function () {
            sendAjax();
        });
        $("#newpwd,#surepwd").keyup(function (e) {
            if (e.keyCode == 13) {
                sendAjax();
            }
        });
    });

    function sendAjax() {
        var $newpwd = $("#newpwd"),
            $surepwd = $("#surepwd"),
            newpwd = $("#newpwd").val(),
            surepwd = $("#surepwd").val();
        if (newpwd.length == 0) {
            $newpwd.focus();
            $("#pwderror").html("请输入新密码").parent().show();
        } else if (surepwd.length == 0) {
            $surepwd.focus();
            $("#pwderror").html("请输入确认密码").parent().show();
        } else if (surepwd != newpwd) {
            $surepwd.focus();
            $("#pwderror").html("输入的密码不一致").parent().show();
        } else {
            var rgx = /[a-zA-Z]+\S+[0-9]+|[0-9]+[a-zA-Z]+/;
            if (newpwd.length < 6) {
                $("#pwderror").html("密码长度不能小于6位").parent().show();
            } else if (newpwd.length > 20) {
                $("#pwderror").html("密码长度不能大于20位").parent().show();
            } else {
                if (rgx.test(newpwd)) {
                    var history = CAS.GetQuery("history");
                    CAS.API({ url: "/ucapi.ashx", data: { m: CAS.GetQuery("m"), d: CAS.GetQuery("d"), um: CAS.GetQuery("um"), np: newpwd, token: CAS.GetQuery("token") }, callback: function (data) {
                        if (data.returntype == 1) {
                            if (history == null || history.length <= 0) {
                                $("#noHistory").show();

                            } else {
                                $("#hasHistory").show();
                                $("#historyurl").attr("href", decodeURIComponent(history));
                            }
                            $("#pwdupdate").hide();
                            $("#pwdsuccess").siblings().hide()
                            $("#pwdsuccess").show();
                        } else if (data.returntype == -1) {
                            $("#illVisit").siblings().hide()
                            $("#illVisit").show();
                        } else if (data.returntype == -120 || data.returntype == -121) {
                            $("#pwderror").html(data.returntext).parent().show();
                        }
                    }
                    });
                } else {
                    $("#pwderror").html("密码长度6~20（必须是含有字母、数字的组合）").parent().show();
                }
            }
        }
    }
</script>

