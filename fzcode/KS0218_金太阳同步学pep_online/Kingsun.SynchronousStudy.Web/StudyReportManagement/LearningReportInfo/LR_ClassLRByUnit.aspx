<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LR_ClassLRByUnit.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.StudyReportManagement.LearningReportInfo.LR_ClassLRByUnit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>学习报告--班级学习情况</title>
    <script type="text/javascript" src="../../AppTheme/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../../AppTheme/js/Common.js"></script>
    <script type="text/javascript" src="../../Scripts/MobileTerminal/mobile.js"></script>
    <script type="text/javascript" src="../../Scripts/MobileTerminal/demo1.js"></script>
    <script type="text/javascript" src="../../Scripts/MobileTerminal/LR_ClassLRByUnitInit.js"></script>
    <link href="../../AppTheme/css/WeChat.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <div class="main">
        <div class="head1">
            <h2>班级学习情况</h2>
        </div>
        <div class="html2">
            <div class="nr1">
                <div class="content-block content-block1">
                    <input id="demo1" type="text" readonly="readonly" value="" />
                    <input id="value1" type="hidden" />
                    <a href="#"></a>
                </div>
                <div class="content-block content-block2">
                    <input id="demo2" type="text" readonly="readonly" value="Module 1/Unit 1" />
                    <input id="value2" type="hidden" />
                    <a href="#"></a>
                </div>
                <ul id="ul_ClassInfo">
                    <%-- <li><a href="hapdub.html"><span>趣配音</span>
                        <div class="percentage"><span class="dd_s"></span></div>
                        <b><i class="i_num">20</i>/<em class="em_num">40</em>
                            <img src="../images/xiugai.png" alt="" /></b>
                    </a></li>--%>
                    <%--<li><a href="opinion.html"><span>说说看</span><div class="percentage"><span></span></div>
                        <b><i>30</i>/<em>40</em><img src="../images/xiugai.png" alt="" /></b></a></li>--%>
                </ul>
            </div>
            <div class="html1" id="div_LR">
                <div class="no_nr" id="divNClass" style="display: none;">
                    <img src="../../AppTheme/images/xie.png" alt="" />
                    <p>请根据您布置的内容选择相应的目录</p>
                </div>
            </div>
            <p id="divlr">以上数据为学生学习人数（单位：人）</p>
        </div>
    </div>
</body>
<script src="../../Scripts/MobileTerminal/LArea.js"></script>
<script>
    var lr;
    $(function () {
        lr = new LR_ClassLRByUint();
        lr.Init();
    });

    var area1 = new LArea();
    var area2 = new LArea();
    var provs_data = <%=provs_data%>
    //级联-1
    area1.init({
        'trigger': '#demo1',
        'valueTo': '#value1',
        'keys': {
            id: 'id',
            name: 'name'
        },
        'type': 1,
        'data': [provs_data],
        'change': function () {
            var obj = { BookID: $("#value1").val() };
            $.post("../../Handler/LR_WeChatHandler.ashx?queryKey=getmoduleinfo", obj, function (data) {
                if (data) {
                    var result = JSON.parse(data);//eval("(" + data + ")");
                    if (result.Success) {
                        city_data = [];
                        city_data = eval("(" + result.data + ")");

                        $('#demo2').val(city_data[0].name);
                        $('#value2').val(city_data[0].id);
                        area2.setGearTooth(city_data);
                        //lr.ChangeSel($("#value1").val()); //选择Book后加载数据
                        lr.ChangeSel(city_data[0].id);

                        topOne();
                    }
                }
            });
        }
    });

    $('#demo1').val(provs_data[0].name);
    $('#value1').val(provs_data[0].id);


    //级联-2
    area2.init({
        'trigger': '#demo2',
        'valueTo': '#value2',
        'keys': {
            id: 'id',
            name: 'name'
        },
        'type': 2,
        'data': [],
        'change': function () {
            topOne();
            //area2.setGearTooth(city_data);///修改数据源
            lr.ChangeSel($("#value2").val()); //选择Unit后加载数据
        }
    });

    //初始根据年级册第一个选项加载Unit
    if (provs_data[0] != undefined) {
        var obj = { BookID: provs_data[0].id };
        $.post("../../Handler/LR_WeChatHandler.ashx?queryKey=getmoduleinfo", obj, function (data) {
            if (data) {
                var result = JSON.parse(data);//eval("(" + data + ")");
                if (result.Success) {
                    city_data = [];
                    city_data = eval("(" + result.data + ")");
                    $('#demo2').val(city_data[0].name);
                    $('#value2').val(city_data[0].id);
                    area2.setGearTooth(city_data);
                    //初始加载Unit完成之后 继续读取完成度
                    lr.ChangeSel($("#value2").val()); //选择Unit后加载数据
                }
            }
        });
    }
    function topOne() {
        $(".gear")[1].style["-webkit-transform"] = 'translate3d(0px,0em,0px)';
        $(".gear")[1].setAttribute('top', '0em');
    }
</script>
</html>
