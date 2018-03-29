(function ($) {
    $.extend({
        extendAjax: function (data, fun, templatecache) {
            data = jQuery.extend({
                data: "",
                url: "",
                type: "get",
                dataType: "json",
                dataToJSON: false,
                remote: true,
                cache: true
            }, data);
            $.ajax({
                type: data.type,
                data: data.data,
                url: data.url,
                dataType: data.dataType,
                cache: false,
                context: { data: data },
                beforeSend: function (XHR) {
                },
                success: function (tmpldata) {
                    if (data.dataToJSON)
                        tmpldata = $.parseJSON(tmpldata);
                    fun(tmpldata);
                },
                error: function (XmlHttpRequest, textStatus, errorThrown) {
                    alert($.parseJSON(XmlHttpRequest.responseText).Message);
                    //var dataError = $.parseJSON(XmlHttpRequest.responseText);
                }
            });

        }
    });
})(jQuery);
Date.prototype.format = function (format) {
    /* 
    * eg:format="yyyy-MM-dd hh:mm:ss"; 
    */
    var o = {
        "M+": this.getMonth() + 1, // month  
        "d+": this.getDate(), // day  
        "h+": this.getHours(), // hour  
        "m+": this.getMinutes(), // minute  
        "s+": this.getSeconds(), // second  
        "q+": Math.floor((this.getMonth() + 3) / 3), // quarter  
        "S": this.getMilliseconds()
        // millisecond  
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4
                        - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1
                            ? o[k]
                            : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}