// JavaScript Document
var winWidth=0;//屏幕宽度
var winHeight=0;//屏幕高度
var h_h=0;//页头高度
var f_h=0;//页脚高度
var maxH=0;//内容区最大高度

var popup = function popup(aaa) {
    var black = document.createElement("div");
    var black1 = document.createElement("div");
    var black2 = document.createElement("h2");
    var black3 = document.createElement("div");
    var black4 = document.createElement("a");
    var black5 = document.createTextNode(aaa);
    var black6 = document.createTextNode("确定");
    black.className = 'zong';
    black1.className = 'zhezhao';
    black3.className = 'hezi';
    black4.appendChild(black6);
    black2.appendChild(black5);
    black.appendChild(black1);
    black.appendChild(black3);
    black3.appendChild(black2);
    black3.appendChild(black4);
    document.body.appendChild(black);
    black1.onclick = function () {
        black.parentNode.removeChild(black);

    }
    black4.onclick = function () {
        black.parentNode.removeChild(black);

    }
};

$(function(){
	findDimensions();
	resizeImg();
	$("#inp1").blur(function(){
		 if($("#inp1").val()!=""){
			$(".joinclass .content .get").css("background-color","#FF7387"); 
		 }
		 else{
			$(".joinclass .content .get").css("background-color","#FFB9C3");  
		 }
		});
	$("#inp2").blur(function(){
		 if($("#inp2").val()!=""){
			$(".joinclass .content .queding").css("background-color","#1A9EE8");
		 }
		 else{
			 $(".joinclass .content .queding").css("background-color","#8CCEF3"); 
		 }
		});
	//$(".joinclass .content .queding").click(function(){
	//	$(".joinclass .content").css("display","none");
	//	$(".joinclass .content1").css("display","block");
	//	$(".joinclass .content2").css("display","none");
	//});  
	$(".joinclass .content1 .confirm").click(function(){
		$(".joinclass .content").css("display","none");
		$(".joinclass .content1").css("display","none");
		$(".joinclass .content2").css("display","block");
	});
	  
	/* 加入班级的选择班级*/
	$(".content1 .chose ul li").click(function(){
		$(".content1 .chose ul li").removeClass("on");
		$(this).addClass("on");
	});
});


function resizeImg(){	
	//计算缩放比例  
	//ratio = w/720;
	$(".main").height(maxH);
}

//函数：获取并计算屏幕尺寸
function findDimensions()  
{ 
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
	if (document.documentElement && document.documentElement.clientHeight && document.documentElement.clientWidth) 
	{ 
		winHeight = document.documentElement.clientHeight; 
		winWidth = document.documentElement.clientWidth; 
	} 
	h_h=$(".header").height();//页头高度
	f_h=$(".footer").height();//页脚高度
	//自动调节主体内容高度
	maxH=winHeight-h_h-f_h;
	$(".main").height(maxH);
}


 