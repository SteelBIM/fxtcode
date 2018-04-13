<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SunnyTeachHead.ascx.cs" Inherits="Kingsun.SunnyTask.Web.Account.SunnyTeachHead" %>
<div class="header" id="header">
    <div class="topTool">
        <div class="head">
            <div class="leftTopTool fl">
                <ul>
                    <li><a href="http://www.kingsun.cn" target="_blank" class="kingsunHome">金太阳首页</a></li><li>&nbsp;| </li>
                    <li><a href="http://www.kingsunedu.com" target="_blank">方直科技</a></li>
                </ul>
            </div>
            <div class="rightTopTool fright">
                <ul>
                    <li class="telnumberTool">400-111-8180</li>
                    <li><a href="<%=this.Teach+"HelpCenter/Page/Index.aspx" %>" target="_blank">帮助中心</a></li>
                    <li><a  href="<%=this.Teach +"TeacherCenter/Page/Index.aspx?type=SystemMessages.aspx" %>" class="mesNum" target="_blank"></a></li>
         	   <%--         	     <li><a href="javascript:void(0);" class="pointNum">300</a></li>--%>
                    <li class="liName"><a class="userName" href="<%=this.Teach +"TeacherCenter/Page/Index.aspx" %>" target="_blank">
                        <label id="userName">老师</label></a>
                       <%-- <ol class="downList">
                            <li><a href="<%=this.Teach +"TeacherCenter/Page/Index.aspx" %>">个人中心</a></li>
                            <li><a href="<%=this.Teach +"TeacherCenter/Page/Index.aspx?type=SystemMessages.aspx" %>">我的消息</a></li>
                            <li><a id="alogout">退出</a></li>
                        </ol>--%>
                    </li>
                    <li style="padding-left:3px;">|</li>
             <li style="padding-left:0;"><a class="alogout" title="退出登录" id="alogout">退出</a></li>
                </ul>
            </div>
            <div class="clearfix"></div>
        </div>
    </div>
    <script src="../App_Themes/js/headfoot.js"></script>
</div>

<div class="mainTitle T1" id="mainTitle">
    <h1 class="homework"><a class="goBack" id="aGoBack" title="返回首页">&nbsp;</a><span id="spHomework">作业</span></h1>
    <div class="w_box"></div>
</div>

<div class="navDiv">
    <div class="head">
        <ul class="navS">
            <li><a id="aClassTaskList" href="javascript:gotoClassTaskList()">管理作业</a></li>
            <li><a id="aTaskArrange" href="javascript:gotoTaskArrange()">布置作业</a></li>
        </ul>
    </div>
</div>
<script src="../Scripts/Common.js"></script>
<script type="text/javascript">
    $().ready(function () {
        if (Common.QueryString.GetValue("Csstype") != "undefined") {
            $("#header").addClass("cloudHeader");
            $(".mesNum").parent().hide();
            $("#aGoBack").hide();
            $("#alogout").hide();
            $(".userName").removeAttr("href");
            $(".liName").next().hide();
        }
        $.get("../Account/GetUserState.aspx?Action=GetUserInfo&rand=" + Math.random(), function (data) {
            if (data) {
                var obj = eval("(" + data + ")");
                if (obj.UserInfo.TrueName) {
                    $("#userName").html(obj.UserInfo.TrueName + "老师");
                } else {
                    $("#userName").html(obj.UserInfo.UserName + "老师");
                }
                if (obj.MsgCount != 0) {
                    $(".mesNum").html("<em>" + obj.MsgCount + "</em>");
                }
            }
        });
        $("#alogout").click(function () {
            $.get("../Account/Logout.aspx?rand=" + Math.random(), function (data) {
                if (data == "LogOutSucceed") {
                    window.location.replace("/Login.aspx");
                }
                else {
                    alert(data);
                }
            });
        });
        $("#aGoBack").click(function () {
            location.href = "<%=this.Teach%>";
        });
    });
    function gotoClassTaskList() {
        location.href = "ClassTaskList.aspx" + window.location.search;
    }
    function gotoTaskArrange() {
        location.href = "TaskArrange.aspx" + window.location.search;
    }
</script>
