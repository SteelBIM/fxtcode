// JavaScript Document
function list_text(){
	this.img_bo="img/shenyin.png";
	this.title="";
	this.img_yu="";
	this.content="";
	this.src="";
}

list_text.prototype={
	init:function(){
		var html="";
		html+="<div class='text_list' data-src='"+this.src+"'>";
        html+="<div class='text_title'>";
        html+="<a href='javascript:void(0)'>";
        html+="<img class='img_bo' src='img/shenyin.png' alt=''>";
        html+="<h2>"+this.title+"</h2>";
        html+="<img class='img_yu' src='"+this.img_yu+"' alt=''>";
        html+="</a>";
        html+="</div>";        	    
        html+="<p>"+this.content+"</p>";
        html+="</div>";
        $(".main1 .section2").append(html);     
	}
}

$(function(){

	var data=[{"title":"My feet hurt1.","src":"video/new1.mp3","img_yu":"img/yu1.png","content":"If not to the sun for smiling, warm is still in the sun there, but wewill laugh more confident calm; if turned to found his own shadow, appropriate escape, the sun will be through the heart,warm each place behind the corner; if an outstretch-ed palm cannot fall butterfly, thenclenched waving arms, given power; if I can't have bright smile, it will face to thesunshine, and sunshine smile together, in full bloom."}];

	var sen_list2=new list_text();
	for(var i=0;i<data.length;i++){
		sen_list2.title=data[i].title;
		sen_list2.content=data[i].content;
		sen_list2.img_yu=data[i].img_yu;
		sen_list2.src=data[i].src;
		sen_list2.init();
	}
	setTimeout(function(){ 
		let img_bo_height=(-$(".img_bo").height()/2)+"px";
		let img_yu_height=(-$(".img_yu").height()/2)+"px";
		$(".text_list .img_bo").css("margin-top",img_bo_height);
		$(".text_list .img_yu").css("margin-top",img_yu_height);
	},10); 
	

});


