// JavaScript Document
var my_audio = document.getElementById("my_audio");
var p_all=$(".progress").width()-20;
var num=1;
$(function () {
	
	
	my_audio.addEventListener('ended', function(){
			$(".button img").attr("src","/images/work/details/bofang.png");
			$(".contros .time").css("display","block");
			$(".aud1 .show").css("display","block");
		});	 
		
});

	//暂停或播放
	function playPause()
	{ 	
		if(my_audio.paused)
		{
			my_audio.play();
			$(".button img").attr("src","");
			$(".contros .time").css("display","none");
			$(".aud1 .show").css("display","none");
		}
		else
		{
			my_audio.pause();
			$(".button img").attr("src","/images/work/details/bofang.png");
			$(".contros .time").css("display","block");
			$(".aud1 .show").css("display","block");
		}
		
		//alert(my_audio.duration);
	}		
	//audio播放的时候实时获取当前播放时间
	

		my_audio.ontimeupdate = function()
		{
			//获取当前播放时间	
			document.getElementById("now_time").innerHTML = timeFormat(my_audio.currentTime);
			//当前的长度	
			now_long=my_audio.currentTime/my_audio.duration*p_all;
			$(".bar").css({width:now_long});
			var btn_l=now_long+'px';
			$(".btn").css({left:btn_l});
			//
			addListenTouch();
			
		};

		
	//页面一旦加入就获取audio的总时间
		my_audio.onprogress = function()
		{		
			document.getElementById("all_time").innerHTML = timeFormat(my_audio.duration);
			//总的长度
		};	
	//时间转化
		function myFunction() 
		{
		// 显示 id="now_time" 的 span 元素中音频的播放位置 
			
		}	
	// Time format converter - 00:00//时间格式转换器- 00:00
		var timeFormat = function(seconds){
			var m = Math.floor(seconds/60)<10 ? "0"+Math.floor(seconds/60) : Math.floor(seconds/60);
			var s = Math.floor(seconds-(m*60))<10 ? "0"+Math.floor(seconds-(m*60)) : Math.floor(seconds-(m*60));
			return m+":"+s;
		};	
	//手动拉拽进度条的部分
	function addListenTouch(){
		//var speed=$('.had-play');
		var btn=document.getElementById("btn");
		document.getElementById("btn").addEventListener("touchstart", touchStart, false);
		document.getElementById("btn").addEventListener("touchmove", touchMove, false);
		document.getElementById("btn").addEventListener("touchend", touchEnd, false);
		}
	function touchStart(e){
		e.preventDefault();
		var touch=e.touches[0];
		startX=touch.pageX;
		my_audio.pause();
//		$(".button img").attr("src","img/work/details/bofang.png");
//		$(".contros .time").css("display","block");
//		$(".aud1 .show").css("display","block");
		document.getElementById("all_time").innerHTML = timeFormat(my_audio.duration);	
		}
	function touchMove(e){//滑动
		e.preventDefault();
		var touch=e.touches[0];
		x=touch.pageX-startX//滑动的距离
		//btn.style.webkitTransform='translate('+0+'px,'+y+'px)';
		var widthBar=now_long+x;
		//
		$(".bar").css({width:widthBar});
		if(widthBar<p_all)
			{
			//	
			$("#btn").css({left:widthBar+10+'px'});
			$("#bar").css({width:widthBar});	
			}//不让进度条超出页面
		//
		var yu=widthBar/p_all*my_audio.duration;
		document.getElementById("now_time").innerHTML = timeFormat(yu);
		}
	function touchEnd(e){//手指离开屏幕
		e.preventDefault();
		now_long=parseInt(btn.style.left);
		var touch=e.touches[0];
		var dragPaddingLeft=btn.style.left;
		var change=dragPaddingLeft.replace("px","");
		numDragpaddingLeft=parseInt(change);
		var currentTime=(numDragpaddingLeft/(p_all-20)*my_audio.duration);
		my_audio.play();
		$(".button img").attr("src","");
//		$(".contros .time").css("display","none");
//		$(".aud1 .show").css("display","none");
		document.getElementById("all_time").innerHTML = timeFormat(my_audio.duration);	
		my_audio.currentTime=currentTime;
		}			