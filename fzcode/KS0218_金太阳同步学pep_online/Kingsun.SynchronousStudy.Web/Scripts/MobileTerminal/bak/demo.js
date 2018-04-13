// JavaScript Document
var identifying;//验证码 
var winWidth = 0;//屏幕宽度
var winHeight = 0;//屏幕高度
var h_h = 0;//页头高度
var f_h = 0;//页脚高度
var maxH = 0;//内容区最大高度
var number = 2;//添加年级的变量

$(function () {
    init();
    select1();
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
//切换年级
function dianji1(i) {

    $(".single-select" + i).find(".select-items").toggle();




    $(".single-select" + i + " .select-items li").click(function (e) {

        var nameS = "";
        $(this).parent().find("li").removeClass("selected");
        $(this).addClass("selected");
        nameS = $(this).html();
        $(this).parent().parent().parent().find(".select-tit span").addClass("on");
        $(this).parent().parent().parent().find(".select-tit span").html(nameS);

    });
}

/*左划删除效果*/
function init() {

    // 设定每一行的宽度=屏幕宽度+按钮宽度
    $(".line-scroll-wrapper").width($(".line-wrapper").width() + $(".line-btn-delete").width() + $(".quanxuan img").width() + 15);

    // 设定常规信息区域宽度=屏幕宽度
    $(".line-normal-wrapper").width(window.screen.width * 0.95);
    // 设定文字部分宽度（为了实现文字过长时在末尾显示...）
    $(".line-normal-msg").width($(".line-normal-wrapper").width() - 280);

    // 获取所有行，对每一行设置监听
    var lines = $(".line-normal-wrapper");
    var len = lines.length;
    var lastX, lastXForMobile;

    // 用于记录被按下的对象
    var pressedObj; // 当前左滑的对象
    var lastLeftObj; // 上一个左滑的对象

    // 用于记录按下的点
    var start;

    // 网页在移动端运行时的监听
    for (var i = 0; i < len; ++i) {
        lines[i].addEventListener('touchstart', function (e) {
            lastXForMobile = e.changedTouches[0].pageX;
            pressedObj = this; // 记录被按下的对象 

            // 记录开始按下时的点
            var touches = event.touches[0];
            start = {
                x: touches.pageX, // 横坐标
                y: touches.pageY // 纵坐标
            };
        });

        lines[i].addEventListener('touchmove', function (e) {
            // 计算划动过程中x和y的变化量
            var touches = event.touches[0];
            delta = {
                x: touches.pageX - start.x,
                y: touches.pageY - start.y
            };

            // 横向位移大于纵向位移，阻止纵向滚动
            if (Math.abs(delta.x) > Math.abs(delta.y)) {
                event.preventDefault();
            }
        });

        $(".Html5 .dianji .bianji").click(function (e) {
            $(lastLeftObj).animate({
                marginLeft: "0"
            }, 500); // 右滑
            lastLeftObj = null;
        });

        lines[i].addEventListener('touchend', function (e) {
            if (lastLeftObj && pressedObj != lastLeftObj) { // 点击除当前左滑对象之外的任意其他位置
                $(lastLeftObj).animate({
                    marginLeft: "0"
                }, 500); // 右滑
                lastLeftObj = null; // 清空上一个左滑的对象
            }
            var diffX = e.changedTouches[0].pageX - lastXForMobile;
            if (diffX < -150) {
                $(pressedObj).animate({
                    marginLeft: "-57px"
                }, 500); // 左滑
                lastLeftObj && lastLeftObj != pressedObj &&
                    $(lastLeftObj).animate({
                        marginLeft: "0"
                    }, 500); // 已经左滑状态的按钮右滑
                lastLeftObj = pressedObj; // 记录上一个左滑的对象
            } else if (diffX > 150) {
                if (pressedObj == lastLeftObj) {
                    $(pressedObj).animate({
                        marginLeft: "0"
                    }, 500); // 右滑
                    lastLeftObj = null; // 清空上一个左滑的对象
                }
            }
        });
    }

    // 网页在PC浏览器中运行时的监听
    for (var i = 0; i < len; ++i) {
        $(lines[i]).bind('mousedown', function (e) {
            lastX = e.clientX;
            pressedObj = this; // 记录被按下的对象
        });

        $(lines[i]).bind('mouseup', function (e) {
            if (lastLeftObj && pressedObj != lastLeftObj) { // 点击除当前左滑对象之外的任意其他位置
                $(lastLeftObj).animate({
                    marginLeft: "0"
                }, 500); // 右滑
                lastLeftObj = null; // 清空上一个左滑的对象
            }
            var diffX = e.clientX - lastX;
            if (diffX < -150) {
                $(pressedObj).animate({
                    marginLeft: "-57px"
                }, 500); // 左滑
                lastLeftObj && lastLeftObj != pressedObj &&
                    $(lastLeftObj).animate({
                        marginLeft: "0"
                    }, 500); // 已经左滑状态的按钮右滑
                lastLeftObj = pressedObj; // 记录上一个左滑的对象
            } else if (diffX > 150) {
                if (pressedObj == lastLeftObj) {
                    $(pressedObj).animate({
                        marginLeft: "0"
                    }, 500); // 右滑
                    lastLeftObj = null; // 清空上一个左滑的对象
                }
            }
        });
    }
}

/*全选删除效果*/
function select1() {
    $(".line-btn-delete a").click(function (e) {
        $(".box3").css("display", "block");
    });

    $(".header .dianji .chuangjian").click(function (e) {
        $(".line-wrapper .quanxuan").css("display", "block");
        $(".chuangjian").css("display", "none");
        $(".chuangjian1").css("display", "block");
        $(".footer .yaoqing").css("display", "none");
        $(".footer .shanchu").css("display", "block");
    });
    $(".header .dianji .chuangjian1").click(function (e) {
        $(".line-wrapper .quanxuan").css("display", "none");
        $(".chuangjian").css("display", "block");
        $(".chuangjian1").css("display", "none");
        $(".footer .yaoqing").css("display", "block");
        $(".footer .shanchu").css("display", "none");
    });

    /*选择角色*/
    $(".Html12 ul li .a1").click(function () {
        $(".Html12 ul li .a2 img").attr("src", "images/icon4@2x.png");
        $(".Html12 ul li .a1 img").attr("src", "images/icon1@2x.png");
    });
    $(".Html12 ul li .a2").click(function () {
        $(".Html12 ul li .a2 img").attr("src", "images/icon2@2x.png");
        $(".Html12 ul li .a1 img").attr("src", "images/icon3@2x.png");
    });

    //选择班级修改	
    $(".Html4 .bao dl dt").click(function (e) {

        if (this.cho == 1) {
            $(this).css("background", "#fff");
            $(this).css("color", "#000");
            $(this).css("border", "1px solid #DCDCDC");
            this.cho = 2;
        }
        else {
            $(this).css("background", "#88C9EF");
            $(this).css("color", "#fff");
            $(this).css("border", "1px solid #88C9EF");
            this.cho = 1;
        }
    });

    //更多按钮功能实现
    $(".Html4 .banji a").click(function (e) {
        if (this.up == 1) {
            $(this).parent().find("dl").css("overflow", "hidden");
            $(this).parent().find("dl").css("height", "117px");
            $(this).html("更多");
            this.up = 2;
        }
        else {

            $(this).parent().find("dl").css("overflow", "auto");
            $(this).parent().find("dl").css("height", "auto");
            $(this).html("收起");
            this.up = 1;
        }
        findDimensions();
        resizeImg();
    });
}



function qiehuan(j) {
    $(".Html4 .bao ul li").removeClass("on");
    $(".Html4 .bao ul .li" + j).addClass("on");
    $(".Html4 .bao .banji div").css("display", "none");
    $(".Html4 .bao .banji" + j).css("display", "block");

}
