// JavaScript Document

$(function () {
   
    
	/*领取弹框*/
    $(".wrap .banner .order,.video .order").click(function () {
		$(".wrap .box").css("display","block");
		$(".wrap .shadow1").css("display","block");
	});
	
	 /*点击弹窗消失*/
	$(".box .close").click(function(){
		$(".box").css("display","none");
		$(".wrap .shadow1").css("display","none");
		$(".box input").val("");
	});
	$(".box1 .close").click(function(){
		$(".box1").css("display","none");
		$(".wrap .shadow1").css("display","none");
		$(".box input").val("");
	});
	$(".box1 .sour").click(function () {
	    $(".box1").css("display", "none");
	    $(".wrap .shadow1").css("display", "none");
	    $(".box input").val("");
	});
	/*切换电视*/
	$(".video ul li").click(function(){
		var num=$(this).index();
		$(".video ul li").removeClass("on");
		$(this).addClass("on");
		$(".video .vider1 video").attr("src", "images/video" + (num + 1) + ".mp4");
		$(".video .vider1 video").attr("poster", "images/video" + (num + 1) + ".png");
		$(".video p").css("display","none");
		$(".video .p"+(num+1)).css("display","block");
	});
	
	/*鼠标经过事件该表图片*/
	$(".thinking ul li").mouseover(function(){
	  var num=$(this).index();
	  $(".thinking ul li").removeClass("on");
	  $(this).addClass("on");
	  $(".thinking img").attr("src","images/image"+(num+1)+".png");
	});
});
