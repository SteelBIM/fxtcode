function gotoTop(min_height) {
    //预定义返回顶部的html代码，它的css样式默认为不显示
    var gotoTop_html = "";
    gotoTop_html += "<div class='gotoTop' id='gotoTop' title='返回顶部'>";
    gotoTop_html += "<a href='javascript:void(0)'>返回顶部</a>";
    gotoTop_html += "</div>";

    //将返回顶部的html代码插入页面上 
    $(".mainB").append(gotoTop_html);
    $("#gotoTop").click(
    //定义返回顶部点击向上滚动的动画
		function () {
		    $('html,body').animate({ scrollTop: 0 }, 700);
			});
    //获取页面的最小高度，无传入值则默认为150像素
    min_height ? min_height = min_height : min_height = 150;
    //为窗口的scroll事件绑定处理函数
    $(window).scroll(function () {
        //获取窗口的滚动条的垂直位置
        var s = $(window).scrollTop();
        //当窗口的滚动条的垂直位置大于页面的最小高度时，让返回顶部元素渐现，否则渐隐
        if (s > min_height) {
            $("#gotoTop").fadeIn(100);

        } else {
            $("#gotoTop").fadeOut(200);
        }
    });
}
	gotoTop();


