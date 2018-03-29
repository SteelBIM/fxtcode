<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test2.aspx.cs" Inherits="WebSpiderRule.Old.test2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="jquery-1.8.3.min.js"></script>	
    <script type="text/javascript">
        $.ajax({
            type: "post",
            url: "http://wx.17gp.com/Inquiry/QueryHouseByGuid",
            data: { houseGuid: "3346e809-8bc7-4ae7-90ba-0941faa205a1" },
            dataType: 'json',
            success: function (data) {
                var strHtml = '';
                $(data).each(function (i) {
                    strHtml += '<li><span>参考均价：<i>' + this.price + ' 元/平米</i><font>（' + this.quality + '）</font></span></li>';
                });
                if (data == null || data.length == 0) {
                    strHtml += '<li><span><i>楼盘均价：</i> <b>暂无均价</b></span></li>';
                }
                $('#houseSearchResult').html(strHtml);
                SetHistoryHouseItem("", "东西湖区", "跃进小区", "3346e809-8bc7-4ae7-90ba-0941faa205a1");
            },
            error: function () {

            }
        });



        //$.ajax({
        //    type: "post",
        //    url: "http://www.17gp.com//Home/AjaxGetPropertyList",
        //    data: { city: "", area: "", keyword: '上', count: count },
        //    dataType: 'json',
        //    success: function (data) {
        //    }
        //});


        //var url = "http://evs.worldunion.cn/QueryPriceManagement/AutoPrice.ashx?ram=" + new Date();

        //$.ajax({
        //    type: "POST",
        //    url: "http://evs.worldunion.cn/QueryPriceManagement/AutoPrice.ashx?ram=" + new Date(),
        //    data: { conid: 1109, CityId: 1, SDate: '2016-07-26', EDate: '2016-08-26', type: "auto_getConInfo" },
        //    dataType: "xml",            
        //    success: function (data) {

        //    }
        //});
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         121212
    </div>
    </form>
</body>
</html>
