<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stargame.aspx.cs" Inherits="Kingsun.SynchronousStudyHopeChina.Web.H5.stargame" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>开始报名</title>
    <link rel="stylesheet" type="text/css" href="css/reset.css" />
    <link rel="stylesheet" type="text/css" href="css/startSkin.css"/>
     <script src="js/jquery-1.10.2.min.js"></script>
    <script src="../AppTheme/js/jquery.json-2.4.js"></script>
    <script src="../AppTheme/js/jquery.cookie.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/Common/easyuiCommon.js"></script>
    <script src="../Scripts/Management/UserInfoManagement.js"></script>
    <script src="js/popupControl.js"></script>
    <script src="js/JsBridge.js"></script>
    <script type="text/javascript">
        $(function () {
            var data = localStorage["UserId"];
            var udata = JSON.parse(data);
            var userid = udata.UserId;
          
            var result = UserinfoManage.QueryUserinfoById(userid);
            //先判断是否已超过比赛时间
            var error = Common.QueryString.GetValue("error");
            if (error == 'timeout') {
                $("#btnMatch").html("查看成绩");
                $("#btnMatch").on("click", function () {
                    Match(2, userinfo.Userid, userinfo.Grade); //已交卷
                });

            }


            if (result.Success) {
                userinfo = result.Data;

                $("#pname").text(userinfo.Username);

                var text = "";
                switch ($.trim(userinfo.Period)) {
                    case "F1":
                        text = "小学1~2年级(F)";
                        break;
                    case "F2":
                        text = "小学3~4年级(F)";
                        break;
                    case "F3":
                        text = "小学5~6年级(F)";
                        break;
                    case "F4":
                        text = "初中(F)";
                        break;
                    case "F5":
                        text = "高中(F)";
                        break;
                    case "F6":
                        text = "大学(F)";
                        break;
                    default:
                        text = "错误";
                        break;
                }
                $("#grouptype").text(text);

                if (userinfo.IsFinish) {
                    $("#btnMatch").html("查看成绩");
                    $("#btnMatch").on("click", function() {
                        Match(2, userinfo.Userid, userinfo.Grade); //已交卷
                    });

                } else {
                    if (userinfo.Grade != null && userinfo.Grade != 0 && userinfo.Grade != undefined) {
                        $("#btnMatch").html("继续比赛");
                        $("#btnMatch").on("click", function() {
                            Match(1, userinfo.Userid, userinfo.Grade); //已有成绩
                        });
                    } else {
                        $("#btnMatch").html("开始比赛");
                        $("#btnMatch").on("click", function() {
                            Match(0, userinfo.Userid, null); //未有成绩
                        });
                    }
                }
            } else {

            }

        });

        function Match(type, userid, articleid) {
            var data;
            if (type == 0) {
                //send message to native
                data = {
                    "data": {
                        "UserId": userid

                    }
                };
                window.WebViewJavascriptBridge.callHandler(
                    'startMatch', data, function(responseData) {

                    }
                );
            } else if (type == 1) {
                //send message to native
                data = {
                    "data": {
                        "UserId": userid,
                        "Articleid": articleid

                    }
                };
                window.WebViewJavascriptBridge.callHandler(
                    'continueMatch', data, function(responseData) {

                    }
                );
            } else {
                data = {
                    "data": {
                        "UserId": userid
                    }
                };
                window.WebViewJavascriptBridge.callHandler(
                    'inquireResult', data, function (responseData) {

                    }
                );
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="skell">
            <div class="header">
                <a class="return" onclick="Finish()"></a>
                <span><a onclick="Finish()">关闭</a></span>
            </div>
            <div class="content">
                <img src="images/leaf.png" />
                <p class="p1" id="pname"></p>
                <p class="p2" id="grouptype"></p>
            </div>
            <a class="signBtn" id="btnMatch">开始比赛</a>
            <div class="placeholder"></div>

        </div>

    </form>
</body>
</html>
