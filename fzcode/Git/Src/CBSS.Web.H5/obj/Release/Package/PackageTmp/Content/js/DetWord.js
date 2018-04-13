// JavaScript Document
function list_sen(){
	this.score="86.5";
	this.time="2018-1-16 10:58:00";
	this.word_all="";
	this.word_correct="";
	this.word="";
}

list_sen.prototype={
	init:function(){
		$(".score").html(this.score);
		$(".time").html(this.time);
		$(".word_all").html(this.word_all);
		$(".word_correct").html(this.word_correct);
		var html="";
		for(var i=0;i<this.word_all;i++){
			if(this.word[i].state==0){
				html+='<li><span class="li_left">'+this.word[i].ranswers+'</span><span class="li_my wrong">'+this.word[i].myanswers+'</span></li>';
			}
			else{
				html+='<li><span class="li_left">'+this.word[i].ranswers+'</span><span class="li_my correct">'+this.word[i].myanswers+'</span></li>';
			}
		}
		
        $(".main .section ul").append(html);     
	}
}

$(function(){

	var data=[{"word_all":2,"word_correct":1,"word":"[{'ranswers':'project','myanswers':'pro','state':0},{'ranswers':'overtime','myanswers':'overtime','state':1}]"}];

	var sen_list1=new list_sen();	
		sen_list1.word_all=data[0].word_all;
		sen_list1.word_correct=data[0].word_correct;
		sen_list1.word=eval('(' + data[0].word + ')');
		sen_list1.init();	
});


