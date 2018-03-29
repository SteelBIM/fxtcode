<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="updatepassword.aspx.cs" Inherits="FxtUseCenterService.Web.updatepassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>修改密码</title>
    <link rel="stylesheet" type="text/css" href="/css/base.css"/>
    <script src="/js/jquery-1.8.2-min.js" type="text/javascript"></script>
    <script src="/js/init.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server"> 
    <div class="login">
	<div class="logo"><img src="/img/logo.png" width="150" height="44" /><img src="../img/title.png" width="152" height="29" class="img_zi" /></div>
	<div>
    <div class="land <%=returntype==0?"":"dn" %>" id="pwdupdate">
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
    <div class="land dn" id="pwdsuccess" style="display:none;height:300px; padding-top:10px; text-align:center;">
        <div class="success dn" id="hasHistory" style="display:none;width:350px;left: 50px;color:Black;">
            <em></em><span>密码修改成功，请<a id="A1">返回并重新登录</a></span>
        </div>
        <div class="success dn" id="noHistory" style="display:none;width:350px;left: 50px;color:Black;">
            <em></em><span >密码修改成功，请返回软件并重新登录。</span>
        </div>
        <span id="hasHistory" class="dn">密码修改成功，请<a id="historyurl">返回并重新登录</a></span>
        <span id="" class="dn">密码修改成功，请返回软件并重新登录。</span>
     </div>
    <div class="land <%=returntype==-1?"":"dn" %>" id="illVisit" style="height:300px; line-height:25px ;padding-top:10px; ">
        <div class="expire" style="display:block;width:480px; color:#000; left: 0px;">
            <em></em><span>访问链接无效！</span>
        </div>
      <%--  <p>请返回到<a href="#" id="A1" style="color:#007FFF;">登录页</a>重新登录，谢谢</p>--%>
     </div>
     </div>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    $(function () {
        $("#btnUpdate").btnMouseSweep("i_button2", "i_button").click(function () {
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
                        sendAjax();
                    } else {
                        $("#pwderror").html("密码长度6~20（必须是含有字母、数字的组合）").parent().show();
                    }
                }
            }

        });

    });

    function sendAjax() {
        var history = CAS.GetQuery("history");
        var newpwd = $("#newpwd").val()
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
            } else if (data.returntype == -120) {
                $("#pwderror").siblings().hide()
                $("#pwderror").html(data.returntext).parent().show();
            }
        }
        });
    }
</script>
