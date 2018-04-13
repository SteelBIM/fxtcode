// JavaScript Document
var identifying;//验证码 
var winWidth = 0;//屏幕宽度
var winHeight = 0;//屏幕高度
var h_h = 0;//页头高度
var f_h = 0;//页脚高度
var maxH = 0;//内容区最大高度
var number = 2;//添加年级的变量
var niannum = "一年级";//选择年级变量 
var classnum = new Array();

//
var demo = function () {
    this.getArry = function () {
        var classnum2 = classnum;
        return classnum2;
    }
}

$(function () {
    init();
    for (var i = 6; i > 0; i--) {
        qiehuan1(i);
        jishu();
    }
    classnum.sort(paixu);
    shengcheng();
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
    h_h1 = $(".head_back").height();
    //自动调节主体内容高度
    maxH = winHeight - h_h - f_h - h_h1;
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



    //删除数组中的元素的构建函数
    Array.prototype.indexOf = function (val) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == val) return i;
        }
        return -1;
    };
    Array.prototype.remove = function (val) {
        var index = this.indexOf(val);
        if (index > -1) {
            this.splice(index, 1);
        }
    };

    //选择班级修改	
    $(".Html4 .bao dl dt").click(function (e) {



        if ($(this).hasClass("on1")) {
            $(this).removeClass("on1");
            var b = $(this).html();
            classnum.remove((niannum + b));
            classnum.sort(paixu);
            shengcheng();
            jishu();

        }
        else {
            if (classnum.length < 10) {

                $(this).addClass("on1");
                var a = $(this).html();
                classnum.push((niannum + a));
                classnum.sort(paixu);
                shengcheng();
                jishu();
            } else {
                popup("一次添加的班级不能超过十个");

                return;
            }
        }


    });
}

$(function () {
    $(".black_overlay").click(function () {
        $(this).parent().css("display", "none");
    });

});
//切换班级和年级的效果
function qiehuan1(j) {
    $(".Html4 .bao ul li").removeClass("on");
    $(".Html4 .bao ul .li" + j).addClass("on");

    niannum = $(".Html4 .bao ul .li" + j + " span").html();
    $(".Html4 .bao dl dt").removeClass("on1");
    for (var i = 0; i < classnum.length; i++) {
        for (var k = 1; k < 22; k++) {
            var c = classnum[i];
            if (c == (niannum + k + "班")) {
                $(".Html4 .bao dl .dt" + k).addClass("on1");
            }
        }
    }
}

//计算选择的个数
function jishu() {
    var dd = 0;
    for (var i = 0; i < classnum.length; i++) {
        for (var k = 1; k < 19; k++) {
            var c = classnum[i];
            if (c == (niannum + k + "班")) {
                dd++;

            }
        }
    }
    switch (niannum) {
        case "一年级":
            $(".li1 p b").html(dd);
            break;
        case "二年级":
            $(".li2 p b").html(dd);
            break;
        case "三年级":
            $(".li3 p b").html(dd);
            break;
        case "四年级":
            $(".li4 p b").html(dd);
            break;
        case "五年级":
            $(".li5 p b").html(dd);
            break;
        case "六年级":
            $(".li6 p b").html(dd);
            break;
    }
    $(".Html4 .p2 span").html(classnum.length);
}
//生成选择的年级班级列表
function shengcheng() {

    $(".ul2").html("");
    var html1 = "";
    for (var i = 0; i < classnum.length; i++) {
        var c = classnum[i];
        var li = "<li class=\"newli\">" + c + "</li>";
        html1 += li;
    }
    $(".ul2").html(html1);

}

//对选择的班级年级数组进行排序
function paixu(value1, value2) {
    var a, b;
    var nianji1 = value1.substr(0, 3);
    var nianji2 = value2.substr(0, 3);
    var banji1 = parseInt(value1.substr(3));
    var banji2 = parseInt(value2.substr(3));
    /*var banji3=banji1.substr(0,banji1.length-1);
	var banji4=banji2.substr(0,banji2.length-1);*/
    switch (nianji1) {
        case "一年级":
            a = 1;
            break;
        case "二年级":
            a = 2;
            break;
        case "三年级":
            a = 3;
            break;
        case "四年级":
            a = 4;
            break;
        case "五年级":
            a = 5;
            break;
        case "六年级":
            a = 6;
            break;
    }
    switch (nianji2) {
        case "一年级":
            b = 1;
            break;
        case "二年级":
            b = 2;
            break;
        case "三年级":
            b = 3;
            break;
        case "四年级":
            b = 4;
            break;
        case "五年级":
            b = 5;
            break;
        case "六年级":
            b = 6;
            break;
    }
    if (a < b) {

        return -1;

    } else if (a > b) {

        return 1;

    } else {
        if (banji1 < banji2) {

            return -1;

        } else if (banji1 > banji2) {

            return 1;

        } else {

            return 0;

        }
        return 0;

    }
}



function qiehuan(j) {
    $(".Html4 .bao ul li").removeClass("on");
    $(".Html4 .bao ul .li" + j).addClass("on");
    $(".Html4 .bao .banji div").css("display", "none");
    $(".Html4 .bao .banji" + j).css("display", "block");

}
