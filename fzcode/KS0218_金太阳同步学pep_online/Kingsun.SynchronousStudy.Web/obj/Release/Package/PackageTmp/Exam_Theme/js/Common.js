$(function () {
    $(window).resize(function () {
        $('#tbdatagrid').datagrid('resize', {
            width: $(window).width() - 20,
            height: $(window).height() - 55
        }).datagrid('resize', {
            width: $(window).width() - 20,
            height: $(window).height() - 55
        });
    });

    $("#searchKey").css("color", "gray").focus(function () {
        var value = $.trim($(this).val());
        var title = $.trim($(this).attr("title"));
        if (value == title) {
            $(this).val("").css("color", "black");
        }
    }).blur(function () {
        var value = $.trim($(this).val());
        var title = $.trim($(this).attr("title"));
        if (!value || value == title) {
            $(this).val(title).css("color", "gray");
        }
    }).keyup(function (e) {
        var key = e.which;
        if (key == 13) {
            $("#searchBtn").click();
        }
    });

});

function FormatTime(time, format) {
    if (!time) {
        return "";
    }
    if (format == undefined || format == "") {
        format = "yyyy年MM月dd日 hh时mm分";
    }
    var date = new Date(parseInt(time.substring(6, time.length - 2)))
    return date.format(format);
}
//在IE8下，js数组没有indexOf方法;
//在使用indexOf方法前，执行一下下面的js, 原理就是如果发现数组没有indexOf方法，会添加上这个方法。
function CheckIndexOf() {
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function (elt) {
            var len = this.length >>> 0;

            var from = Number(arguments[1]) || 0;
            from = (from < 0)
                 ? Math.ceil(from)
                 : Math.floor(from);
            if (from < 0)
                from += len;

            for (; from < len; from++) {
                if (from in this &&
                    this[from] === elt)
                    return from;
            }
            return -1;
        };
    }
}
Date.prototype.format = function (format) {
    var o =
    {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}

