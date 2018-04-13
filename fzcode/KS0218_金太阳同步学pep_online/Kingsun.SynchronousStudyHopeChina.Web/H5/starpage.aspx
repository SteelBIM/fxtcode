<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="starpage.aspx.cs" Inherits="Kingsun.SynchronousStudyHopeChina.Web.H5.starpage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../AppTheme/js/jquery.json-2.4.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/Management/UserInfoManagement.js"></script>
    <script src="js/JsBridge.js"></script>
    <title>中转页面</title>
    <%-- ReSharper disable once UsageOfPossiblyUnassignedValue --%>
    <script type="text/javascript">
//        $(function () {
//            startWebPage('{UserId:"1"}');
//        });
//
//        function startWebPage(data, responseData) {
//            console.log(data);
//            var url;
//            if (data != undefined) {
//                url = "";
//                //var udata = JSON.parse(data); 
//                //var userid = udata.UserId;
//                var result = UserinfoManage.StarPageByUserId(1);
//                if (result.Success) {
//                    var dataresult = result.Data;
//                    switch ((dataresult - 0)) {
//                    case 0:
//                        url = "beforeactivity.aspx"; //未到报名时间
//                        break;
//                    case 1:
//                        url = "registration.aspx?userid=" + userid + ""; //比赛未开始 未报名
//                        break;
//                    case 2:
//                    case 3:
//                        url = "registfinish.aspx?error=0"; //已报名
//                        break;
//                    case 4:
//                        url = "registfinish.aspx?error=1"; //未报名
//                        break;
//                    case 5:
//                        url = "stargame.aspx?userid=" + userid + "";
//                        break;
//                    case 6:
//                        url = "registfinish.aspx?error=2";
//                        break;
//                    default:
//                        url = "registfinish.aspx?error=1"; //无状态
//                        break;
//                    }
//                    //打开页面
//                    window.location.href = url;
//                    //responseCallback({ 'Success' : true, 'data' : url, 'Message' : ""  });
//                }
//            }
//        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
