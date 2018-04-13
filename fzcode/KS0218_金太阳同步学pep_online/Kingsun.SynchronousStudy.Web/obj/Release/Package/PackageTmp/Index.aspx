<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>同步学平台</title>
    <script type="text/javascript" src="AppTheme/js/jquery-1.11.2.min.js"></script>

    <script type="text/javascript">
        var indexPage;
        $(function () {
            //$("#iframe1").removeAttr("style"); //ie,ff均支持
            //$("#iframe1").removeAttr("style"); //ie,ff均支持
            //$("#iframe1").attr("style", "");
            $(".menuUl li a.collapsed").next().slideDown(1000); //展开当前子项
            //$(".menuUl li a.collapsed").removeClass("collapsed").addClass("expanded");
            SetMenu('<%= menuList %>');
            $("#aClose").click(function () {
                $.get("/Loginout.aspx?rand=" + Math.random(), function (data) {
                    if (data == "LogOutSucceed") {
                        window.parent.location.reload();
                    }
                    else {
                        alert(data);
                    }
                });
            });
        });
        function SetMenu(menu) {
            if (menu != null && menu != "") {
                var menuCodeList = menu.split(',');
                var menuPageList = document.getElementsByTagName('a');
                for (i = 0; i < menuPageList.length; i++) {
                    for (j = 0; j < menuCodeList.length; j++) {
                        if (menuPageList[i].id == menuCodeList[j]) {
                            menuPageList[i].style.display = "block";
                            break;
                        }
                    }
                }
            }
        }
    </script>
</head>
<body onload="SetMenu('<%= menuList %>')">
    <form id="form1" runat="server">
        <div id="head">
            <h1>
                <a href="Index.aspx" title="同步学平台">同步学平台</a></h1>
            <div class="toptools">
                <div class="">
                    <b></b><span></span>
                </div>
                <div class="adminUser">
                    <b></b><span>
                        <%=info %></span>
                </div>
                <ul class="toolbar">
                    <li><b class="li_02"></b><%--<a href="javascript:void(0)" id="aClose">退出</a>--%>
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="aClose_Click">退出</asp:LinkButton>
                    </li>
                </ul>
            </div>
            <div class="nav">
                <ul>
                </ul>
            </div>
        </div>
        <div class="mainbody">
            <div id="left">
                <div class="leftMenu">
                    <h3></h3>
                    <ul class="menuUl">
                        <li>
                            <a href="javascript:void(0)" title="应用管理" class="collapsed" id="TBX" style="display: none;" target="iframe1">应用管理</a>
                            <ul>
                                <li><a href="ApplicationManagement/APPManagement.aspx" title="版本列表" class="collapsed" target="iframe1" id="TBX01" style="display: none;">版本列表</a></li>
                            </ul>
                        </li>
                        <li>
                            <a href="javascript:void(0)" title="模块管理" class="collapsed" target="iframe1" id="TBX0101" style="display: none;">模块管理</a>
                            <ul>
                                <li><a href="ModuleManagement/ModuleList.aspx" title="模块列表" class="collapsed" target="iframe1" id="TBX0201" style="display: none;">模块列表</a></li>
                            </ul>
                        </li>
                        <li>
                            <a href="javascript:void(0)" title="课程管理" class="collapsed" target="iframe1" id="TBX0301" style="display: none;">课程管理</a>
                            <ul>
                                <li><a href="CourseManagement/CourseManageMent.aspx" title="课程列表" class="collapsed" target="iframe1" id="TBX0401" style="display: none;">课程列表</a></li>
                            </ul>
                        </li>

                        <li>
                            <a href="javascript:void(0)" title="用户管理" class="collapsed" target="iframe1" id="TBX0501" style="display: none;">用户管理</a>
                            <ul>
                                <li><a href="UserManagement/UserInfo.aspx" title="用户列表" class="collapsed" target="iframe1" id="TBX0102" style="display: none;">用户列表</a></li>
                                <li><a href="UserManagement/UserStatistics.aspx" title="用户记录" class="collapsed" target="iframe1" id="TBX0103" style="display: none;">用户记录</a></li>
                            </ul>
                        </li>
                        <li>
                            <a href="javascript:void(0)" title="用户反馈" class="collapsed" target="iframe1" id="TBX0302" style="display: none;">用户反馈</a>
                            <ul>
                                <li><a href="FeedbackManagement/Feedback.aspx" title="反馈列表" class="collapsed" target="iframe1" id="TBX0202" style="display: none;">反馈列表</a></li>
                            </ul>
                        </li>
                        <li>
                            <a href="javascript:void(0)" title="活动管理" class="collapsed" target="iframe1" id="TBX07" style="display: none;">活动管理</a>
                            <ul>
                                <li><a href="ManageMent/VideoDetailsList.aspx" title="活动视频" class="collapsed" target="iframe1" id="TBX0702" style="display: none;">活动视频</a></li>
                            </ul>
                        </li>
                        <li>
                            <a href="javascript:void(0)" title="消息管理" class="collapsed" target="iframe1" id="TBX08" style="display: none;">消息管理</a>
                            <ul>
                                <li><a href="MessageManagement/MessagePush.aspx" title="消息列表" class="collapsed" target="iframe1" id="TBX0801" style="display: none;">消息列表</a></li>
                                <li><a href="MessageManagement/AddMessagePush.aspx" title="新增消息" class="collapsed" target="iframe1" id="TBX0802" style="display: none;">新增消息</a></li>
                            </ul>
                        </li>
                        <li>
                            <a href="javascript:void(0)" title="运营统计" class="collapsed" target="iframe1" id="TBX09" style="display: none;">运营统计</a>
                            <ul>
                                <li><a href="OperationStatistics/VideoStatistics.aspx" title="趣配音" class="collapsed" target="iframe1" id="TBX0901" style="display: none;">趣配音</a></li>
                                <li><a href="OperationStatistics/HearResourcesStatistics.aspx" title="说说看" class="collapsed" target="iframe1" id="TBX0902" style="display: none;">说说看</a></li>
                            </ul>
                        </li>
                        <li>
                            <a href="javascript:void(0)" title="订单管理" class="collapsed" target="iframe1" id="TBX10" style="display: none;">订单管理</a>
                            <ul>
                                <li><a href="Order/OrderList.aspx" title="订单管理" class="collapsed" target="iframe1" id="TBX1001" style="display: none;">订单管理</a></li>

                            </ul>
                        </li>
                        <li>
                            <a href="javascript:void(0)" title="套餐管理" class="collapsed" target="iframe1" id="TBX11" style="display: none;">套餐管理</a>
                            <ul>
                                <li><a href="FeeManager/FeeSetting.aspx" title="价格设置" class="collapsed" target="iframe1" id="TBX1101" style="display: none;">价格设置</a></li>

                            </ul>
                        </li>
                        <li>
                            <a href="javascript:void(0)" title="优惠卷管理" class="collapsed" target="iframe1" id="TBX12" style="display: none;">优惠卷管理</a>
                            <ul>
                                <li><a href="Order/Coupon.aspx" title="优惠卷管理" class="collapsed" target="iframe1" id="TBX1201" style="display: none;">优惠卷管理(旧)</a></li>
                                <li><a href="Order/CouponList.aspx" title="优惠卷管理" class="collapsed" target="iframe1" id="TBX1201" style="display: none;">优惠卷管理</a></li>
                                <%--   <li><a href="Order/TestCouponList.aspx" title="优惠卷管理Test" class="collapsed" target="iframe1" id="TBX1201" style="display: none;">优惠卷管理Test</a></li> --%>
                            </ul>
                        </li>
                        <li>
                            <a href="javascript:void(0)" title="希望之星" class="collapsed" target="iframe1" id="A1" style="display: none;">希望之星</a>
                            <ul>
                                <li><a href="http://183.47.42.221:9001/HopeChina/UserInfoManageMent.aspx" title="用户管理" class="collapsed" target="iframe1" id="A2" style="display: none;">用户管理</a></li>
                                <li><a href="http://183.47.42.221:9001/HopeChina/ArticleManageMent.aspx" title="文章管理" class="collapsed" target="iframe1" id="A3" style="display: none;">文章管理</a></li>
                            </ul>
                        </li>
                        <li>
                            <a href="javascript:void(0)" title="期末试卷管理" class="collapsed" target="iframe1" id="ExamPaper" style="display: none;">期末试卷管理</a>
                            <ul>
                                <li><a href="ExamPaperManagement/bookmanager.aspx" title="教材管理" class="collapsed" target="iframe1" id="ExamPaper01" style="display: none;">教材管理</a></li>
                                <li><a href="ExamPaperManagement/catalogmanager.aspx" title="目录管理" class="collapsed" target="iframe1" id="ExamPaper02" style="display: none;">目录管理</a></li>
                                <li><a href="ExamPaperManagement/questionmanager.aspx" title="题目管理" class="collapsed" target="iframe1" id="ExamPaper03" style="display: none;">题目管理</a></li>
                                <li><a href="ExamPaperManagement/AssignmentNumbersList.aspx" title="交卷人数管理" class="collapsed" target="iframe1" id="ExamPaper04" style="display: none;">交卷人数管理</a></li>

                            </ul>
                        </li>
                        <li>
                            <a href="javascript:void(0)" title="口语直播管理" class="collapsed" target="iframe1" id="SpokenBroadcas" style="display: none;">口语直播管理</a>
                            <ul>
                                <li><a href="SpokenBroadcasManagement/CourseList.aspx" title="课程课时管理" class="collapsed" target="iframe1" id="SP01" style="display: none;">课程课时管理</a></li>
                                <li><a href="SpokenBroadcasManagement/CourseCount.aspx" title="课程统计" class="collapsed" target="iframe1" id="SP04" style="display: none;">课程统计</a></li>
                                <li><a href="SpokenBroadcasManagement/UserAppointCount.aspx" title="预约用户统计" class="collapsed" target="iframe1" id="SP02" style="display: none;">预约用户统计</a></li>
                                <li><a href="SpokenBroadcasManagement/ClassUserCount.aspx" title="上课用户统计" class="collapsed" target="iframe1" id="SP03" style="display: none;">上课用户统计</a></li>
                                <li><a href="SpokenBroadcasManagement/GlobalSet.aspx" title="全局设置" class="collapsed" target="iframe1" id="SP06" style="display: none;">全局设置</a></li>
                                <li><a href="SpokenBroadcasManagement/OtherSourceUserCount.aspx" title="其它来源用户统计" class="collapsed" target="iframe1" id="SP07" style="display: none;">其它来源用户统计</a></li>
                            </ul>
                        </li>
                        <li>
                            <a href="javascript:void(0)" title="配音比赛管理" class="collapsed" target="iframe1" id="VoiceGame" style="display: none;">配音比赛管理</a>
                            <ul>
                                <li><a href="InterestDubbingGame/GameUserList.aspx" title="比赛用户查询" class="collapsed" target="iframe1" id="VoiceGame01" style="display: none;">比赛用户查询</a></li>
                                <li><a href="InterestDubbingGame/GameStat.aspx" title="配音赛事统计" class="collapsed" target="iframe1" id="VoiceGame02" style="display: none;">配音赛事统计</a></li>
                                <li><a href="InterestDubbingGame/PushList.aspx" title="推送列表" class="collapsed" target="iframe1" id="VoiceGame03" style="display: none;">推送列表</a></li>
                                <li><a href="InterestDubbingGame/DubbingImportExcel.aspx" title="趣配音导入" class="collapsed" target="iframe1" id="VoiceGame04" style="display: none;">推送列表</a></li>
                                <li><a href="InterestDubbingGame/GameUserClassList.aspx" title="报名用户查询" class="collapsed" target="iframe1" id="VoiceGame05" style="display: none;">报名用户查询</a></li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="splitBar">
                <a href="#" title="隐藏侧栏" onclick="switchBar();"></a>
            </div>
            <div id="path">
            </div>
            <div id="main">
                <iframe src="ApplicationManagement/APPManagement.aspx" name="iframe1" id="iframe1" frameborder="0"></iframe>
            </div>
        </div>
        <div id="foot">
        </div>
        <script type="text/javascript" src="AppTheme/js/frame.js"></script>
    </form>
</body>
<link href="AppTheme/css/frame.css" rel="stylesheet" />
<script type="text/javascript" src="AppTheme/js/jquery.cookie.js"></script>
<script type="text/javascript" src="AppTheme/js/jquery.json-2.4.js"></script>
<script type="text/javascript" src="AppTheme/js/Common.js"></script>
<script type="text/javascript" src="AppTheme/js/frame.js"></script>
</html>
