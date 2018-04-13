// JavaScript Document
var identifying;//验证码 
var winWidth = 0;//屏幕宽度
var winHeight = 0;//屏幕高度
var h_h = 0;//页头高度
var f_h = 0;//页脚高度
var maxH = 0;//内容区最大高度
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


