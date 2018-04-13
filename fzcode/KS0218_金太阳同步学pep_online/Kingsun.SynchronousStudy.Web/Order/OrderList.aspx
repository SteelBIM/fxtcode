<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderList.aspx.cs" Inherits="Kingsun.AppLibrary.Web.OrderManager.OrderList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>购买清单</title>

    <script src="../Scripts/Common/jquery-easyui/jquery.min.js"></script>

    <script src="../Scripts/Common/jquery.json-2.4.js"></script>

    <script src="../Scripts/Common/jquery-easyui/jquery.easyui.min.js"></script>

    <script src="../Scripts/Common/easyuiCommon.js"></script>

    <link href="../Scripts/Common/jquery-easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/Common/Common.js"></script>

    <link href="../Scripts/Common/jquery-easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/Common/Constant.js"></script>
    <!--日期选择-->
    <script src="../Scripts/Common/DateSelect/daterangepicker.jQuery.js"></script>
    <script src="../Scripts/Common/DateSelect/jquery-ui-1.7.1.custom.min.js" type="text/javascript"></script>
    <link href="../Scripts/Common/DateSelect/ui.daterangepicker.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/Common/DateSelect/jquery-ui-1.7.1.custom.css" rel="stylesheet" type="text/css" />
    <!--日期选择-->
    <script src="../Scripts/Management/OrderManagement.js" type="text/javascript"></script>
    <script src="../Scripts/Init/orderlistPage.js" type="text/javascript"></script>
    <script type="text/javascript">
        var orderlist;
        $(function () {
            orderlist = new OrderListPage();
            orderlist.Init();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="divcontent">
            <table id="tbfeesetting">
            </table>
            <div id="tb">
                <a href="javascript:void(0)" id="exportExcel" class="easyui-linkbutton" data-options="iconCls:'icon-add'">导出EXCEL</a>
                <select id="selectEdition" style="width: 120px;">

                </select>
                <select id="selectPayway" class="easyui-combobox" style="width: 120px;">
                    <option value="0">支付方式</option>
                    <option value="1">支付宝支付</option>
                    <option value="2">微信支付</option>
                    <option value="5">苹果支付</option>
                </select>
                <input type="text" id="txtdate" value="选择时间段" />
                <input type="text" id="searchkey" class="easyui-validatebox" value="" placeholder="请输入查询的手机号"
                    title="请输入查询的手机号" />
                <input type="radio" name="ispay" value="3" checked="checked" />
                <span>全部</span>
                <input type="radio" name="ispay" value="1" />
                <span>支付成功</span>
                <input type="radio" name="ispay" value="0" />
                <span>未支付</span> <a href="javascript:void(0)" id="searchbtn" class="easyui-linkbutton"
                    data-options="iconCls:'icon-search'">搜索</a>
            </div>
        </div>
    </form>
</body>
</html>
