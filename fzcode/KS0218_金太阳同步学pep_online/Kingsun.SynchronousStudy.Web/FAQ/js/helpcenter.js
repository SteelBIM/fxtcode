// JavaScript Document

$(function(){
	
   $(".question ul li").click(function(){
	   var num=$(this).index();
	   $(".question").css("display","none");
	   $(".answer").css("display","none");
	   $(".answer"+(num+1)).css("display","block");
	   
   });
   $(".answer a").click(function(){
	   $(".answer").css("display","none");
	   $(".question").css("display","block"); 
   });
	
});

