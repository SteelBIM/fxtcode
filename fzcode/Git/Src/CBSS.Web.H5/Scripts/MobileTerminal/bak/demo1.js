// JavaScript Document
var identifying;//验证码 
var winWidth = 0;//屏幕宽度
var winHeight = 0;//屏幕高度
var h_h = 0;//页头高度
var f_h = 0;//页脚高度
var maxH = 0;//内容区最大高度
var number = 2;//添加年级的变量

$(function () {

    findDimensions();
    resizeImg();
});



function resizeImg() {
    //计算缩放比例  
    //ratio = w/720;
    $(".main").height(maxH);
}

//函数：获取并计算屏幕尺寸
function findDimensions() {
    //获取窗口宽度 
    if (window.innerWidth)
        winWidth = window.innerWidth;
    else if ((document.body) && (document.body.clientWidth))
        winWidth = document.body.clientWidth;
    //获取窗口高度 
    if (window.innerHeight)
        winHeight = window.innerHeight;
    else if ((document.body) && (document.body.clientHeight))
        winHeight = document.body.clientHeight;
    //通过深入Document内部对body进行检测，获取窗口大小 
    if (document.documentElement && document.documentElement.clientHeight && document.documentElement.clientWidth) {
        winHeight = document.documentElement.clientHeight;
        winWidth = document.documentElement.clientWidth;
    }
    h_h = $(".header").height();//页头高度
    f_h = $(".footer").height();//页脚高度
    //自动调节主体内容高度
    maxH = winHeight - h_h - f_h;
    $(".main").height(maxH);
}
/*班级学习情况的班级选择*/
function chosegrade() {

    //模块完成情况页面书籍选择控件
    if ($(".head img").attr("src") == "../../AppTheme/images/shou2.png") {
        $(".toolwin1").css("display", "none");
        $(".maskLayer1").css("display", "none");
        $(".head img").attr("src", "../../AppTheme/images/la2.png");
        $(".head span").html($(".timelist1 .on span").html());

    }
    else {

        $(".toolwin1").css("display", "block");
        $(".maskLayer1").css("display", "block");
        $(".head img").attr("src", "../../AppTheme/images/shou2.png");

    }
}
/*报告列表的班级选择*/
function chosegrade1() {
    if ($(".head img").attr("src") == "../../AppTheme/images/shou2.png") {
        $(".toolwin").css("display", "none");
        $(".maskLayer").css("display", "none");
        $(".head img").attr("src", "../../AppTheme/images/la2.png");
        $(".head span").html($(".timelist .on").html());
        $(".head span").attr("classid", $(".timelist .on").attr("id"));
        changeCalendar();
    }
    else {

        $(".toolwin").css("display", "block");
        $(".maskLayer").css("display", "block");
        $(".head img").attr("src", "../../AppTheme/images/shou2.png");
    }
}

//报告列表选择班级按钮功能实现
function changeClass() {
    //报告列表选择班级按钮功能实现
    $(".toolwin ul li").click(function (e) {
        $(".toolwin ul li").removeClass("on");
        $(this).addClass("on");

    });
    //日历页面班级选择控件
    $(".toolwin .to_ul a").click(function (e) {
        $(".toolwin").css("display", "none");
        $(".maskLayer").css("display", "none");
        $(".head img").attr("src", "../../AppTheme/images/la2.png");
        $(".head span").html($(".timelist .on").html());
        $(".head span").attr("classid", $(".timelist .on").attr("id"));
        changeCalendar();
    });
}


$(function () {

    //班级学习情况选择班级按钮功能实现
    $(".toolwin1 ul li").click(function (e) {
        if ($(this).hasClass("on")) {
            $(this).removeClass("on");
        }
        else if ($(this).hasClass("wu")) {
            return;
        }
        else {
            $(".toolwin1 ul li").removeClass("on");
            $(this).addClass("on");
        }
    });

    //日历页面班级选择控件
    $(".maskLayer").click(function (e) {
        $(".toolwin").css("display", "none");
        $(".maskLayer").css("display", "none");
        $(".head img").attr("src", "../../AppTheme/images/la2.png");
        $(".head span").html($(".timelist .on").html());
        $(".head span").attr("classid", $(".timelist .on").attr("id"));
        changeCalendar();
    });

    //模块完成情况页面书籍选择控件
    $(".maskLayer1").click(function (e) {
        $(".toolwin1").css("display", "none");
        $(".maskLayer1").css("display", "none");
        $(".head img").attr("src", "../../AppTheme/images/la2.png");
        $(".head span").html($(".timelist1 .on span").html());

    });



    /*获取当前是星期几*/
    var date = new Date();
    var datee = date.getDay();
    switch (datee) {
        case 0:
            $(".html1 .nr p .span1").html("周日");
            $(".html2  p .span1").html("周日");
            break;
        case 1:
            $(".html1 .nr p .span1").html("周一");
            $(".html2  p .span1").html("周一");
            break;
        case 2:
            $(".html1 .nr p .span1").html("周二");
            $(".html2  p .span1").html("周二");
            break;
        case 3:
            $(".html1 .nr p .span1").html("周三");
            $(".html2  p .span1").html("周三");
            break;
        case 4:
            $(".html1 .nr p .span1").html("周四");
            $(".html2  p .span1").html("周四");
            break;
        case 5:
            $(".html1 .nr p .span1").html("周五");
            $(".html2  p .span1").html("周五");
            break;
        case 6:
            $(".html1 .nr p .span1").html("周六");
            $(".html2  p .span1").html("周六");
            break;
    }


    ///*计算显示完成量*/
    //var amount = $(".i_num").html();
    //var amount1 = $(".em_num").html();
    //var pastime1 = $(".dd_s");
    //var interval = setInterval(function () {
    //    var widthline = Math.round(amount) / Math.round(amount1) * 100;
    //    pastime1.css("width", widthline + "%");

    //}, 500);
    ChangeSpWidth();
});

function changeCalendar() {
    var myCalendar = new SimpleCalendar('#calendar');
    var strjson = "";
    //Common.QueryString.GetValue("EditionID")
    var obj = { EditionID: 21, UserID: "1130228176", ClassID: $("#span").attr("classid") }
    $.post("../../Handler/LR_WeChatHandler.ashx?queryKey=getreporttime", obj, function (data) {
        if (data) {
            var result = JSON.parse(data);
            if (result.Success) {
                strjson = JSON.parse(result.data);
                for (var i = 0; i < strjson.length; i++) {
                    myCalendar.addMark(strjson[i].Time, "上学");
                }
            }
        }
    });
}

//改变完成度显示
function ChangeSpWidth() {
    /*计算显示完成量*/
    var amount = $(".i_num").html();
    var amount1 = $(".em_num").html();
    var pastime1 = $(".dd_s");
    var interval = setInterval(function () {
        var widthline = Math.round(amount) / Math.round(amount1) * 100;
        pastime1.css("width", widthline + "%");

    }, 500);
}


