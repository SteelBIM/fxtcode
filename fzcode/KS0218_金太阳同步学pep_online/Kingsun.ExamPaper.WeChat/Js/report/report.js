// JavaScript Document
var winWidth = 0;//屏幕宽度
var winHeight = 0;//屏幕高度
var h_h = 0;//页头高度
var f_h = 0;//页脚高度
var maxH = 0;//内容区最大高度
var a;

$(function () {
    listtiao();
    unittiao(); 
    reporthuan();
    findDimensions();
    resizeImg();
    bofang();
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

/*班级列表跳转页面*/
function listtiao() {
    $(".classlist ul li").click(function () {
        if ($(this).hasClass("noclick")) {
            return;
        }
        $(this).find("a").attr("href", "Volume.aspx");
    });
}
/*册别单元跳转页面*/
function unittiao() {
    $(".volume ul li").click(function () {
        if ($(this).hasClass("noclick")) {
            return;
        }
        $(this).find("a").attr("href", "SpeechEval.aspx");
    });
} 

function reporthuan() {
    $(".record .ul1 li").click(function () {
        var num = $(this).index();
        $(".record .ul1 li").removeClass("on");
        $(this).addClass("on");
        $(".record .content").css("display", "none");
        $(".record .content" + (num + 1)).css("display", "block");
    });
}

/*切换册别*/
function dianji1(i) { 
    $(".single-select" + i).find(".select-items").toggle();
    $(".single-select" + i + " .select-items li").click(function (e) {

        var nameS = "";
        $(this).parent().find("li").removeClass("selected");
        $(this).addClass("selected");
        nameS = $(this).html();
        
        $(this).parent().parent().parent().find(".select-tit span").addClass("on");
        $(this).parent().parent().parent().find(".select-tit span").html(nameS);
        alert($(this).attr("id"));
        $("#ul_Unit").html(""); 
    });
}

var yinpin = new Array();
function bofang() {
    var my = document.getElementById('audio1');
    var j = 1;
    var curr = 0; // 当前播放的视频
    $(".content1 ul li a").click(function () { 
        $(".hide").html("");
        $(".hide").html(this.attributes['url'].nodeValue);
        yinpin = $(".hide").html().split(";");
        var i = $(this).parent().parent().index() + 1;
        $(".content1 ul li a").css("background", "url(/images/report/play.png) no-repeat");
        $(".content1 ul li a").css("background-size", "100%");
        if (i == a) {
            j++;
            if (j % 2 == 0) {
                curr = 0;
                my.load();

            }
            if (j % 2 == 1) {
                play();
                $(this).css("background", "url(/images/report/pasue.png) no-repeat");
                $(this).css("background-size", "100%");
            }
        }
        else {
            j = 1;
            curr = 0;
            a = i;
            my.addEventListener('ended', play);
            play();
            $(this).css("background", "url(/images/report/pasue.png) no-repeat");
            $(this).css("background-size", "100%");

        }

    });
    $(".content2 ul li a").click(function () {
        $(".hide").html("");
        $(".hide").html(this.attributes['url'].nodeValue);
        yinpin = $(".hide").html().split(";");
        var i = $(this).parent().parent().index() + 1;
        $(".content2 ul li a").css("background", "url(/images/report/play.png) no-repeat");
        $(".content2 ul li a").css("background-size", "100%");
        if (i == a) {
            j++;
            if (j % 2 == 0) {
                curr = 0;
                my.load();

            }
            if (j % 2 == 1) {
                play();
                $(this).css("background", "url(/images/report/pasue.png) no-repeat");
                $(this).css("background-size", "100%");
            }
        }
        else {
            j = 1;
            curr = 0;
            a = i;
            my.addEventListener('ended', play);
            play();
            $(this).css("background", "url(/images/report/pasue.png) no-repeat");
            $(this).css("background-size", "100%");

        }

    });
    $(".record .ul1 li").click(function () {
        j = 1;
        curr = 0;
        a = 0;
        my.load();
        $(".content1 ul li a").css("background", "url(/images/report/play.png) no-repeat");
        $(".content1 ul li a").css("background-size", "100%");
        $(".content2 ul li a").css("background", "url(/images/report/play.png) no-repeat");
        $(".content2 ul li a").css("background-size", "100%");
    });
    function play(e) {
        var vLen = yinpin.length; // 播放列表的长度
        my.src = yinpin[curr];
        my.load(); // 如果短的话，可以加载完成之后再播放，监听 canplaythrough 事件即可
        my.play();

        curr++;
        if (curr > vLen) {
            curr = 0;
            j = 2;
            //my.load();
            $(".content1 ul li a").css("background", "url(/images/report/play.png) no-repeat");
            $(".content1 ul li a").css("background-size", "100%");
            $(".content2 ul li a").css("background", "url(/images/report/play.png) no-repeat");
            $(".content2 ul li a").css("background-size", "100%");
        }// 播放完了，重新播放
    }
    /*my.addEventListener('ended', function() {	
			$(".content1 ul li a").css("background","url(images/play.png) no-repeat");
		    $(".content1 ul li a").css("background-size","100%");
			$(".content2 ul li a").css("background","url(images/play.png) no-repeat");
		    $(".content2 ul li a").css("background-size","100%");
		}, false);*/
}





