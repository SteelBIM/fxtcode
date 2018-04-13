<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SunnyStudentHead.ascx.cs" Inherits="Kingsun.SunnyTask.Web.Account.SunnyStudentHead" %>
<div class="header" id="header">
    <div class="topTool">
        <div class="head">
            <div class="leftTopTool fl">
                <ul>
                    <li class="li">
                        <a class="kingsunHome" href="javascript:gotoStuList();">学习空间</a>
                    </li>
                </ul>
            </div>
            <div class="rightTopTool fright">
                <ul>
                    <li><a class="userName" id="aUserConter">个人中心</a></li>
                    <li><a class="helpCenter" id="aHelpConter">帮助中心</a></li>
                    <li><a class="exit" id="aLogout">退出</a></li>
                </ul>
            </div>
            <div class="clear"></div>
        </div>
    </div>
    <div class="topMenu">
        <div class="head">
            <h1 class="logo fl"></h1>
            <div class="mainMenu fright">
                <ul class="menuItem fl">
                    <li class="on" id="liStuTask"><a href="javascript:gotoStuList();">学习任务</a></li>
                    <%--<li><a href="">云课堂</a></li>--%>
                </ul>
                <div class="centerLink fright">
                    <a id="userName" class="fright">**同学</a><span class="fright"><em></em><img id="imgAvatar" width="53" height="53" src="../App_Themes/images/defultHeadStu.jpg"></span>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </div>
</div>

<script type="text/javascript">
    $().ready(function () {
        $.get('<%="/"+this.Task+"Account/GetUserState.aspx?Action=GetUserInfo&rand="%>' + Math.random(), function (data) {
            if (data) {
                var obj = eval("(" + data + ")");
                if (obj.TrueName) {
                    $("#userName").html(obj.TrueName);
                } else {
                    $("#userName").html(obj.UserName);
                }
                if (obj.AvatarUrl) {
                    $("#imgAvatar").attr("src", obj.AvatarUrl);
                }
            }
        });
        $("#aLogout").click(function () {
            $.get('<%="/"+this.Task+"Account/Logout.aspx?rand="%>' + Math.random(), function (data) {
                if (data == "LogOutSucceed") {
                    window.location.replace("/Login.aspx");
                }
                else {
                    alert(data);
                }
            });
        });

        $("#aUserConter,#userName").click(function () {
            //个人中心
            window.location.href = '<%="/"+this.Task+"Student/UserCenter.aspx"%>';
        });

        $("#aHelpConter").click(function () {
            //帮助中心
            window.location.href = '<%="/"+this.Task+"HelpCenter/HelpCenterIndex.aspx"%>';
        });
    });
    function gotoStuList(page) {
        location.href = '<%="/"+this.Task+"Student/StuTaskList.aspx"%>';
    }
</script>
