// JavaScript Document

$(function(){
	
	$(".classList li a").bind("click",function(){
		ChangeClass(this);
		})
	
	$(".ulList li").bind("click",function(){
		ChangeNav(this);
		})
		
	/*布置作业*/
	/*班级切换*/
	$("#list1 li a").bind("click",function(){
		ChangeClass1(this);
		})
	})

//classList——班级切换
function ChangeClass(obj){
	$(".classList li a").removeClass("on");
	$(obj).addClass("on");
	}
function ChangeClass1(obj){
	if($(obj).attr("class")=="on"){
		$(obj).removeClass("on");
		}else{
			$(obj).addClass("on");
			}
	
	}
//ulList——条件菜单切换
function ChangeNav(obj){
	$(".ulList li").removeClass("on");
	$(obj).addClass("on");
	}