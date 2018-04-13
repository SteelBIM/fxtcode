// JavaScript Document
$(function () {
    //$(".share").click(function(){
    //    $(".box").css("display","block");
    //    $(".shadow").css("display","block");
    //});
    
    //$(".box .cancel").click(function(){
    //    $(".box").css("display","none");
    //    $(".shadow").css("display","none");
    //});
    bofang();
});

function bofang(){
	var videos=$(".content video");
	var audios=$(".content audio");
	videos[0].addEventListener('play', function() {
		audios[0].pause();	
		$(".button img").attr("src","/images/work/details/bofang.png");
		$(".contros .time").css("display","block");
		$(".aud1 .show").css("display","block");
	}, false);
	audios[0].addEventListener('play', function() {
		videos[0].pause();
		$(".button img").attr("src","");
		$(".contros .time").css("display","none");
		$(".aud1 .show").css("display","none");
	}, false);
}
//function bofang(){
//	var videos=$(".content video");
//	var that=0;
//	for(var i=0;i<videos.length;i++){
//		var a=videos[i];
//		a.index=i;
//		a.addEventListener('play', function() {
//			    for(var k=0;k<videos.length;k++){
//			    	if(this.index==k){
//			    		
//			    		videos[k].play();
//			    	}
//			    	else{
//			    		videos[k].pause();	
//			    	}
//			    }	
//				
//		}, false);	
//	
//	}
//}
