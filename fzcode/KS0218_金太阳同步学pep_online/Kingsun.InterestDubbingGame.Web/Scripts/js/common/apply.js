// JavaScript Document

$(function () {
	$(".share").click(function(){
		//$(".box").css("display","block");
		//$(".shadow").css("display","block");
	});
	$(".xinxi_nr").click(function(){
		$(".box2").css("display","block");
		$(".shadow").css("display","block");
	});
	$(".box .cancel").click(function(){
		$(".box").css("display","none");
		$(".shadow").css("display","none");
	});
	$(".box1 .close").click(function () {
	    $(".box1").css("display", "none");
	    $(".shadow").css("display", "none");
	});
	$(".box2 .close").click(function(){
		$(".box2").css("display","none");
		$(".shadow").css("display","none");
	});
	$(".inp2").bind('input',function(){
		vercode();
	});
	$(".tishi input").click(function(){
		panduan();
	});
	
	
});
function onlyNum(){
  if(!(event.keyCode==46)&&!(event.keyCode==8)&&!(event.keyCode==37)&&!(event.keyCode==39))
  if(!((event.keyCode>=48&&event.keyCode<=57)||(event.keyCode>=96&&event.keyCode<=105)))
  event.returnValue=false;
}

function vercode(){
	if($("#inp2").val().length==6){
		$(".confirm").addClass("on");
	}
	else{
		$(".confirm").removeClass("on");
	}
}	

function panduan(){
	if($(".tishi input").is(':checked')){
		$(".tishi1").css("display","none");
	}
	else{
		$(".tishi1").css("display","block");
	}
}
	

	