// JavaScript Document
function list_sen(){
	this.img_bo="img/shenyin.png";
	this.content="";
	this.img_yu="";
	this.src="";
}

list_sen.prototype={
	init:function(){
		var html="";
		html+="<div class='sen_list' data-src='"+this.src+"'><a href='javascript:void(0)'>";
        html+="<img class='img_bo' src='img/shenyin.png' alt=''>";
        html+="<p>"+this.content+"</p>";
        html+="<img class='img_yu' src='"+this.img_yu+"' alt=''>";
        html+="</a></div>";
        $(".main1 .section1").append(html);     
	}
}


$(function(){

	var data=[{"content":"My feet hurt1.","img_yu":"img/yu1.png","src":"video/new1.mp3"},
	{"content":"My feet hurt2.","img_yu":"img/yu2.png","src":"video/new2.mp3"},
	{"content":"Sandy has feet. Squidward hasfeet.Sandy has feet.","img_yu":"img/yu3.png","src":"video/new1.mp3"},
	{"content":"My feet hurt1.","img_yu":"img/yu1.png","src":"video/new2.mp3"},
	{"content":"My feet hurt2.","img_yu":"img/yu2.png","src":"video/new1.mp3"},
	{"content":"Sandy has feet. Squidward hasfeet.Sandy has feet.","img_yu":"img/yu3.png","src":"video/new2.mp3"}];

	var sen_list1=new list_sen();
	for(var i=0;i<data.length;i++){
		sen_list1.content=data[i].content;
		sen_list1.img_yu=data[i].img_yu;
		sen_list1.src=data[i].src;
		sen_list1.init();
	}
	setTimeout(function(){ 
		let img_bo_height=(-$(".img_bo").height()/2)+"px";
		let img_yu_height=(-$(".img_yu").height()/2)+"px";
		$(".sen_list .img_bo").css("margin-top",img_bo_height);
		$(".sen_list .img_yu").css("margin-top",img_yu_height);
	},10); 
	
});


