// JavaScript Document
/*资源左侧菜单点击选中和失去选择效果*/
$(function () {
    changeTaskCss();
    showInfo();
    showtextarea();
});
function changeTaskCss(){
	$(".topSelect span a.part").click(function(){
		$(this).parent().addClass("unfinished");
		});
	$(".topSelect span a.all").click(function(){
		$(this).parent().removeClass("unfinished");
		});
	}

/*展开内容简介*/
function showInfo() {
    $(".queList ol li p .queInfo").click(function () {
        $(".queList ol li .commendList").not($(this).parent().parent().find(".commendList")).slideUp();
        $(this).parent().parent().find(".commendList").slideToggle();
    });
}
/*回复文本框的显隐以及文本框text-indent的动态改变*/
function showtextarea() {
    $(".discussList li p a.answerClick").click(function () {
        var textareaBox = $(this).parent().parent().next(".commendTextarea");
        textareaBox.slideToggle();
        var bl = textareaBox.css("display");
        if (bl = "block") {
            var strlength = textareaBox.find(".studentName").width();
            textareaBox.find("textarea").css("text-indent", strlength + 'px');
        }
    });
}


 
		
		