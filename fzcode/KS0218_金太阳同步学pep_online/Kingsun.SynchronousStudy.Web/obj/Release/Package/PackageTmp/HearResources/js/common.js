// JavaScript Document
var my_audio=new Audio();
$(function() {

	$(".main1 .sen_list").on("click",function(){
		
		if($(this).hasClass("on")){
			if(my_audio.paused){
				my_audio.play();
				$(this).find(".img_bo").attr("src","img/shenyin.gif");
			}
			else if (my_audio.played) {
				my_audio.pause();
				$(this).find(".img_bo").attr("src","img/shenyin.png");			
			}
		}
		else{			
			my_audio.src = $(this).data('src');
			my_audio.load();
			my_audio.play();			
			$(".main1 .sen_list .img_bo").attr("src","img/shenyin.png");
			$(this).find(".img_bo").attr("src","img/shenyin.gif");	    
		}
		$(".main1 .sen_list").removeClass("on");
		$(this).addClass("on");				
	});
	$(".main1 .text_list").click(function() {
		
		if($(this).hasClass("on")){
			if(my_audio.paused){
				my_audio.play();
				$(this).find(".img_bo").attr("src","img/shenyin.gif");
			}
			else if (my_audio.played) {
				my_audio.pause();
				$(this).find(".img_bo").attr("src","img/shenyin.png");			
			}
		}
		else{			
			my_audio.src = $(this).data('src');
			my_audio.load();
			my_audio.play();
			$(".main1 .text_list .img_bo").attr("src","img/shenyin.png");
		    $(this).find(".img_bo").attr("src","img/shenyin.gif");
		}
		$(".main1 .text_list").removeClass("on");
		$(this).addClass("on");				
	});
	my_audio.addEventListener('ended', function() {
		$(".main1 .sen_list").removeClass("on");
		$(".main1 .text_list").removeClass("on");
		$(".main1 .sen_list .img_bo").attr("src","img/shenyin.png");
		$(".main1 .text_list .img_bo").attr("src","img/shenyin.png");
	});

});


