<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="action.aspx.cs" Inherits="WebAppTest.action" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        var json = '[{ "cfileid": "96_81256696", "cvalue": "4,431" }, { "cfileid": "96_96697685", "cvalue": "0" }, { "cfileid": "unitprice", "cvalue": "21,100" }, { "cfileid": "96_49748848", "cvalue": "0" }, { "cfileid": "96_50961446", "cvalue": "0" }, { "cfileid": "96_38202788", "cvalue": "扣除" }, { "cfileid": "96_91888345", "cvalue": "692,679" }, { "cfileid": "96_56197129", "cvalue": "0" }, { "cfileid": "buildingarea", "cvalue": "60.00" }, { "cfileid": "96_96587585", "cvalue": "180" }, { "cfileid": "96_10379898", "cvalue": "1,899" }, { "cfileid": "netprice", "cvalue": "402,302" }, { "cfileid": "96_20643213", "cvalue": "1,265,820" }, { "cfileid": "96_78665802", "cvalue": "1,195,557" }, { "cfileid": "96_57961930", "cvalue": "402,302" }, { "cfileid": "96_84233488", "cvalue": "70,443" }, { "cfileid": "96_8305482", "cvalue": "其他" }, { "cfileid": "96_7758090", "cvalue": "0" }, { "cfileid": "tax", "cvalue": "863,698" }, { "cfileid": "96_41340468", "cvalue": "深圳" }, { "cfileid": "96_7676504", "cvalue": "常规" }, { "cfileid": "96_26062161", "cvalue": "100,576" }, { "cfileid": "landarea", "cvalue": "0.00" }, { "cfileid": "96_36186371", "cvalue": "692,679" }, { "cfileid": "fullname", "cvalue": "彩天名苑金兰轩10层10N" }, { "cfileid": "96_17958585", "cvalue": "个人非住宅物业" }, { "cfileid": "96_22293697", "cvalue": "633" }, { "cfileid": "96_17108195", "cvalue": "63,300" }, { "cfileid": "96_46565203", "cvalue": "0" }, { "cfileid": "96_1471896", "cvalue": "1,195,557" }, { "cfileid": "96_24836215", "cvalue": "1697.20%" }, { "cfileid": "96_74805409", "cvalue": "全额征收" }, { "cfileid": "totalprice", "cvalue": "1,266,000"}]';
        console.log($.parseJSON(json));
        var search = location.search;

        var re = /注册:@\S+/ig;
        var str = "注册:scht|测试";
        console.log(re.test(str));

        $(function () {
            $("#btnclick").click(function () {
                alert(location.href);

            });

            $("#btnAddIntroductiong").click(function () {
                var len = $("#introul li").length + 1;
                var lihtml = '<li class="ml5 sbdblue pd5">' +
                         '<div class="mb3"><span>标题</span><input /></div>' +
            '<div class="mb3"><span style=" vertical-align:top">照片</span><img src="" /><span class="uploadImg" id="upspan' + len + '"></span></div>' +
            '<div class="mb3"><span style=" vertical-align:top">内容</span><textarea class="w200 h100"></textarea></div></li>';
                $("#introul").append(lihtml);
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <button id="btnclick">点击</button>
    <button id="btnAddIntroductiong">点击
    点击</button>
    <ul id="introul">
                            <li class="ml5 sbdblue pd5">
                                <div class="mb3"><span>标题</span><input /></div>
                                <div class="mb3"><span style=" vertical-align:top">照片</span><img src="/upload/365/OA/2013/10/10/57323c80fa104ce89c87.png" /><span class="uploadImg" id="upspan1"></span></div>
                                <div class="mb3"><span style=" vertical-align:top">内容</span><textarea class="w200 h100"></textarea></div>
                            </li>
                        </ul>
    </div>
    </form>
</body>
</html>
