$(function(){
	autoFixed();
})
$(window).resize(function(){
	autoFixed();
})
function autoFixed() {
    var tH, fH, allH, otherH;
    allH = $(window).height();
    tH = $("#header").height();
    otherH=30+10;
    var rate=1200/797;
    var rate1=714/797;
    var rate2=53/797;
    var finalH=allH-tH-otherH;
    var finalW=finalH*rate
    $(".wrap").css({"height":allH,"overflow":"auto"});
    $(".mainBody").css({"height":finalH,"width":finalW});
    $(".mainBody .leftNav").css({"height":finalH*rate1,"top":finalH*rate2});
  }