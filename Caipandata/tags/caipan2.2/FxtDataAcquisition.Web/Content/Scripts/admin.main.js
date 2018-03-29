//选择主题及初始化主题逻辑
(function () {
    $(".color-panel .color-mode ul li").click(function () {
        var color = $(this).attr("data-style");
        $.cookie('currentTheme', color, { expires: 7, path: '/' });
    });
    var currentTheme = $.cookie('currentTheme');
    if (currentTheme != null && currentTheme) {
        $('#style_color').attr("href", "/assets/css/style_" + currentTheme + ".css");
    }
})();

//新菜单根据Url决定逻辑
(function () {
    var locationHref = window.location.href;
    $(".page-sidebar>ul>li>a").each(function () {
        var attr1 = $(this).attr("href");
        var attr2 = String(attr1).substr(0, attr1.length - 5);
        //特殊处理(住宅)
        if (locationHref.indexOf("House/Build/") > 0 || locationHref.indexOf("House/House/") > 0) {
            if (attr1 == "/House/Project/Index") {
                $(this).parent().addClass("active");
                $(this).append("<span class='selected'></span>");
            }
            return false;
        }

        //特殊处理(商业)
        if (locationHref.indexOf("Business/BusinessCircle/") > 0 || locationHref.indexOf("Business/BusinessBuild/") > 0 || locationHref.indexOf("Business/BusinessFloor/") > 0 || locationHref.indexOf("Business/BusinessHouse/") > 0 ) {
            if (attr1 == "/Business/BusinessStreet/Index") {
                $(this).parent().addClass("active");
                $(this).append("<span class='selected'></span>");
            }
            return false;
        }

        //特殊处理(办公)
        if (locationHref.indexOf("Office/OfficeBuilding/") > 0 || locationHref.indexOf("Office/OfficeHouse/") > 0 || locationHref.indexOf("Office/OfficeSubArea/") > 0) {
            if (attr1 == "/Office/OfficeProject/Index") {
                $(this).parent().addClass("active");
                $(this).append("<span class='selected'></span>");
            }
            return false;
        }

        //特殊处理(工业)
        if (locationHref.indexOf("Industry/IndustryBuilding/") > 0 || locationHref.indexOf("Industry/IndustryHouse/") > 0 || locationHref.indexOf("Industry/IndustrySubArea/") > 0) {
            if (attr1 == "/Industry/IndustryProject/Index") {
                $(this).parent().addClass("active");
                $(this).append("<span class='selected'></span>");
            }
            return false;
        }

        if (locationHref.indexOf(attr1) > 0 || locationHref.indexOf(attr2) > 0) {
            $(this).parent().addClass("active");
            $(this).append("<span class='selected'></span>");

            $("#navigation .page-title span").html($(this).text());
            $("#navigation .page-title small").html($(this).attr("title") || "");
            $("#navigation .breadcrumb li:eq(1) span").html($(this).text());
            $("#navigation .breadcrumb li:eq(1) i").remove();
            $("#navigation .breadcrumb li:eq(2)").remove();

            var title = $(this).text();
            if ($.trim(document.title) != "") {
                title += " - " + document.title;
            }
            document.title = title;
            return false;
        }
    });
})();

(function () {
    var isIE8Or9 = false;

    if (window.ActiveXObject) {
        var ua = navigator.userAgent.toLowerCase();
        var ie = ua.match(/msie ([\d.]+)/)[1];
        if (ie == 8.0 || ie == 9.0) {
            isIE8Or9 = true;
        }

        if (ie == 6.0) {
            alert("您的浏览器版本是IE6，在本系统中不能达到良好的视觉效果，建议你升级到IE8及以上！");
        }
    }

    if (!isIE8Or9) {
        //alert("您的浏览器版本不是IE8或IE9，在本系统中不能达到良好的视觉效果，建议你升级到IE8以上！")
    }
})();

$("#checkall").click(function () {
    var ischecked = this.checked;
    $("input:checkbox[name='ids']").each(function () {
        this.checked = ischecked;
    });

    $.uniform.update(':checkbox');
});

$("#delete").click(function () {
    var len = $("table.table>tbody").find("input:checked").length;
    if (len == 0) {
        alert("请选择要删除的数据");
        return false;
    }
    var message = "你确定要删除勾选的记录吗?";
    if ($(this).attr("message")) {
        message = $(this).attr("message") + "，" + message;
    }

    if (confirm(message)) {
        $("#mainForm").submit();
    }

});

$(function () {
    var url = window.location.href;
    var firstModule = ["/Authorize/", "/Land/", "/House/", "/Business/", "/Office/", "/Industry/", "/Human/", "/Company/"];

    //一级菜单
    if (url.indexOf(firstModule[1]) > 0) {
        $("ul.navbar-nav").find("li").eq(1).addClass("active").find("a").append("<span class=\"selected\"></span>");
    }

    if (url.indexOf(firstModule[2]) > 0) {
        $("ul.navbar-nav").find("li").eq(2).addClass("active").find("a").append("<span class=\"selected\"></span>");
    }
    if (url.indexOf(firstModule[3]) > 0) {
        $("ul.navbar-nav").find("li").eq(3).addClass("active").find("a").append("<span class=\"selected\"></span>");
    }
    if (url.indexOf(firstModule[4]) > 0) {
        $("ul.navbar-nav").find("li").eq(4).addClass("active").find("a").append("<span class=\"selected\"></span>");
    }
    if (url.indexOf(firstModule[5]) > 0) {
        $("ul.navbar-nav").find("li").eq(5).addClass("active").find("a").append("<span class=\"selected\"></span>");
    }
    if (url.indexOf(firstModule[6]) > 0) {
        $("ul.navbar-nav").find("li").eq(6).addClass("active").find("a").append("<span class=\"selected\"></span>");
    }
    if (url.indexOf(firstModule[7]) > 0) {
        $("ul.navbar-nav").find("li").eq(7).addClass("active").find("a").append("<span class=\"selected\"></span>");
    }

    $("#control").click(function () {
        var container = $(".page-container");
        var value = $("#condi").css("display");
        if (value == "none") {
            $("#condi").css("display", "block");
            $("#control").text("收起∧");
            container.removeClass("sidebar-closed");

        } else {
            $("#condi").css("display", "none");
            $("#control").text("展开∨");
            container.addClass("sidebar-closed");
        }
    });

});


(function ($) {
    $.fn.extend({
        loading: function () {
            $(this).prev(".over").show();
            $(this).show();
        },
        unloading: function() {
            $(this).prev(".over").hide();
            $(this).hide();
        }
    });
})(jQuery);


