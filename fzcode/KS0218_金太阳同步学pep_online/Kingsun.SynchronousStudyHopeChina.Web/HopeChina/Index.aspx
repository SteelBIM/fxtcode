<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Kingsun.SynchronousStudyHopeChina.Web.HopeChina.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>同步学平台</title>
    <script type="text/javascript" src="/AppTheme/js/jquery-1.11.2.min.js"></script>

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
                                <li><a href="ModuleManagement/ModuleList.aspx" title="模块列表" class="collapsed" target="iframe1" id="TBL0201" style="display: none;">模块列表</a></li>
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
        <script type="text/javascript" src="/AppTheme/js/frame.js"></script>
    </form>
</body>
<link href="/AppTheme/css/frame.css" rel="stylesheet" />
<script type="text/javascript" src="/AppTheme/js/jquery.cookie.js"></script>
<script type="text/javascript" src="/AppTheme/js/jquery.json-2.4.js"></script>
<script type="text/javascript" src="/AppTheme/js/Common.js"></script>
<script type="text/javascript" src="/AppTheme/js/frame.js"></script>
</html>
