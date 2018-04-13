<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Coupon.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.Order.Coupon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Scripts/Common/jquery-easyui/jquery.min.js"></script>

    <script src="../Scripts/Common/jquery.json-2.4.js"></script>

    <script src="../Scripts/Common/jquery-easyui/jquery.easyui.min.js"></script>

    <script src="../Scripts/Common/easyuiCommon.js"></script>

    <link href="../Scripts/Common/jquery-easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/Common/Common.js"></script>

    <link href="../Scripts/Common/jquery-easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/Common/Constant.js"></script>
    <script src="../Scripts/Management/OrderManagement.js"></script>
    <script src="../Scripts/Init/CouponPage.js"></script>
    <script>
        var monitor;
        $(function () {
            monitor = new MonInit();
            monitor.Init();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="mon">
        </div>
    </form>
</body>
</html>
