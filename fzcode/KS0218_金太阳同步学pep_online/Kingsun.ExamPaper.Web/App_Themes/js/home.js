// JavaScript Document
$(function () {
    showInfo();
    showtextarea();
});

/*展开内容简介*/
function showInfo() {
    $(".queList ol>li>p a").click(function () {
        $(".queList ol>li .commendList").not($(this).parent().parent().find(".commendList")).slideUp();
        $(".queList ol>li>p a").not($(this)).removeClass("down");
        $(this).parent().parent().find(".commendList").slideToggle();
        $(this).toggleClass("down");
    });
}

/*回复文本框的显隐以及文本框text-indent的动态改变*/
function showtextarea() {
    $(".commendList li a.answerClick").click(function () {
        var textareaBox, parentW, userW;
        textareaBox = $(this).parent().parent().find(".commendTextarea");
        $(".commendList li .commendTextarea").not(textareaBox).slideUp();
        textareaBox.slideToggle();
        userW = textareaBox.find(".studentName").width();
        var bl = textareaBox.css("display");
        if (bl == 'block') {
            textareaBox.find(".answerText").css("text-indent", userW + "px");
            textareaBox.find(".answerText").focus();
        }
    });
}