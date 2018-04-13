// JavaScript Document
var winWidth=0;//屏幕宽度
var winHeight=0;//屏幕高度
var h_h=0;//页头高度
var f_h=0;//页脚高度
var maxH=0;//内容区最大高度

$(function(){
	listtiao();
	unittiao();
	recordtiao();
	reporthuan();
	findDimensions();
	resizeImg();
	
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

/*班级列表跳转页面*/
function listtiao(){
	$(".classlist ul li").click(function(){
		if($(this).hasClass("noclick")){
			return;	
		}
		$(this).find("a").attr("href","volume.html");
	});
}
/*册别单元跳转页面*/
function unittiao(){
	$(".volume ul li").click(function(){
		if($(this).hasClass("noclick")){
			return;	
		}
		$(this).find("a").attr("href","speecheval.html");
	});
}
/*口语评测模块跳转页面*/
function recordtiao(){
	$(".speecheval ul li").click(function(){
		if($(this).hasClass("noclick")){
			return;	
		}
		$(this).find("a").attr("href","record.html");
	});
}

function reporthuan(){
	$(".record .ul1 li").click(function(){
		var num=$(this).index();
		$(".record .ul1 li").removeClass("on");
		$(this).addClass("on");
		$(".record .content").css("display","none");
		$(".record .content"+(num+1)).css("display","block");
	});
}

/*切换册别*/
function dianji1(i){
		
		$(".single-select"+i).find(".select-items").toggle();					
	$(".single-select"+i+" .select-items li").click(function(e){

			var nameS = ""; 									   
			$(this).parent().find("li").removeClass("selected");
			$(this).addClass("selected");
			nameS = $(this).html();
			$(this).parent().parent().parent().find(".select-tit span").addClass("on");
			$(this).parent().parent().parent().find(".select-tit span").html(nameS);		 
		
});
}



 