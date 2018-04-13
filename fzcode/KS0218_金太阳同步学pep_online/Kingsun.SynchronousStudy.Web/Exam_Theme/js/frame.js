// jquery全屏框架

/* 初始化加载函数*/
$(function () {
    //初始化宽度、高度   
    setSize("head", "foot", "left", "splitBar", "mainbody");
    //当文档窗口发生改变时 触发   
    $(window).resize(function () {
        setSize("head", "foot", "left", "splitBar", "mainbody");
    });

    $(".menuUl li ul").each(function () {
        $(this).children("li").last().addClass("last");
    })

    /*菜单相关 第一级--------------------*/
    $(".menuUl li a.collapsed").click(function () {
        $(".menuUl li ul").slideUp(500); //关闭所有展开的子项
        $(".menuUl li a.collapsed").removeClass("collapsed").addClass("collapsed");
        $(".menuUl li a.expanded").removeClass("expanded").addClass("collapsed");
        $(this).next().slideDown(1000); //展开当前子项
        $(this).removeClass("collapsed").addClass("expanded");
    });

    $(".nav ul li a").click(function () {
        var projectName = $(this).text();
        $(".leftMenu h3").html(projectName);
    })

    $(".menuUl li ul li a").click(function () {
        var parentValue = $(this).parent().parent().prev().text();
        var curValue = $(this).text();
        var projectName = $(".leftMenu h3").text();
        var str = '<a href="index.html"><em class="homeIco"></em><span>桌面</span></a>';
        str = '';
        str += '<a class="first"><em></em><span>' + projectName + '</span></a>';
        str += '<a><em class="itemIco"></em><span>' + parentValue + '</span></a><a><em class="itemIco"></em><span>' + curValue + '</span></a>';
        $("#divnev").html(str);
        $(".menuUl li ul li a.cur").removeClass("cur");
        $(this).addClass("cur");
    })
    $("a").bind("focus", function () { if (this.blur) this.blur(); });
});
/*
设置各区域的宽高度，包括四个参数;要求各区块结构有固定的ID
topObj： 顶栏容器的ID
botObj： 底栏容器的ID
leftObj：左侧栏容器的ID
barObj： 分隔栏容器的ID
*/
function setSize(topObj,botObj,leftObj,barObj,mainObj){
	var aW=$(window).width();
	var aH=$(window).height();
	
	var tH=$("#"+topObj).height();
	var bH=$("#"+botObj).height();
	var lW=$("#"+leftObj).width();
	var bW=$("#"+barObj).width();
	
    $("#"+mainObj).width(aW-lW-bW-1).height(aH-tH-bH-30);   
    $("#"+leftObj).height(aH-tH-bH);  
    $("#"+barObj).height(aH-tH-bH); 
    $("#iframe1").height(aH-tH-bH-33); 
}
/*切换分隔栏*/
var isOpen=true;
function switchBar() {
    if (isOpen) {
        //$("#path").animate({ "left": 0 });
        $("#left").animate({ 'width': 0 });
        $(".mainbody").animate({ 'padding-left': 0 });
        $("#splitBar").animate({ 'left': 0 }).children().eq(0).attr('title', '展开侧栏');
        $("#splitBar").children().eq(0).css("background-position", "0 bottom");
        isOpen = false;
    }
    else {
        //$("#path").animate({ "left": 200 });
        $(".mainbody").animate({ 'padding-left': 210 });
        $("#left").animate({ 'width': 200 });
        $("#splitBar").animate({ 'left': 200 }).children().eq(0).attr('title', '隐藏侧栏');
        $("#splitBar").children().eq(0).css("background-position", "0 top");
        isOpen = true;
    }
}
//浏览器版本号判断函数，因为jq.browser在以后的版本可能被取消，所以用这种方式来最保险
function browserDetect(){
  var sUA = navigator.userAgent.toLowerCase();
  var sIE = sUA.indexOf("msie");
  var sOpera = sUA.indexOf("opera");
  var sMoz = sUA.indexOf("gecko");
  if (sOpera != -1) return "opera";
  if (sIE != -1){
    nIeVer = parseFloat(sUA.substr(sIE + 5));
    if (nIeVer >= 7) return "ie7.0";
    else if (nIeVer >= 6) return "ie6.0";
    else if (nIeVer >= 5.5) return "ie5.5";
    else if (nIeVer >= 5 ) return "ie5.0";    
  }
  if (sMoz != -1)  return "moz";
  return "other";
}