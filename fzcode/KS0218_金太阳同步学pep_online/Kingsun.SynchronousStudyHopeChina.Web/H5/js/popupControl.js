// JavaScript Document
$(function () {



});

function popupShow() {
    $(".popupGroup").show();
    var h = $(document).height();
    $(".shadow").css("height", h);
}

function popupHide() {
    $(".popupGroup").hide();
}

//选择分组
function selectGroup(obj) {
    var str = $(obj).text();
    $(".group").html(str);
    switch ($.trim(str)) {
        case "小学1-2年级（F）": $("#txtGroup").val("F1");
            break;
        case "小学3-4年级（F）": $("#txtGroup").val("F2");
            break;
        case "小学5-6年级（F）": $("#txtGroup").val("F3");
            break;
        case "初中（F）": $("#txtGroup").val("F4");
            break;
        case "高中（F）": $("#txtGroup").val("F5");
            break;
        case "大学": $("#txtGroup").val("F6");
            break;
        default: $("#txtGroup").val("");
    }

    $(".popupGroup").hide();
}

//控制是否按钮样式
function btnState(obj) {
    $(obj).toggleClass("choosed");
    $(obj).siblings().removeClass("choosed");
}