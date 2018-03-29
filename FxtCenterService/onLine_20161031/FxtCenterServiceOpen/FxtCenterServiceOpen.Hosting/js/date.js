var DateHelper = {
    JsonToDate: function (t, format) { //格式日期 如：jsonToDate(date,"yyyy-MM-dd"):2012-08-14
        if (t == "") return t;
        try {
            var obj = {};
            if ("object" == typeof (t)) {
                obj = t;
            }
            else {
                if (/\//g.test(t)) {
                    obj = eval('new ' + (t.replace(/\//g, '')));
                } else {
                    obj = t.xmlDateToJavascriptDate();
                }
            }
            var now = new Date();
            if (now.getFullYear() == obj.getFullYear() && now.getMonth() == obj.getMonth() && format.indexOf("hh:mm") > -1 && (now.getDate() == obj.getDate() || now.getDate() - 1 == obj.getDate())) {
                if (now.getDate() == obj.getDate()) {
                    if (obj.getHours() >= 6 && obj.getHours() < 12) {
                        return obj.format("上午 hh:mm");
                    } else if (obj.getHours() >= 12 && obj.getHours() < 18) {
                        return obj.format("下午 hh:mm");
                    } else {
                        return obj.format("晚上 hh:mm");
                    }
                } else {
                    return obj.format("昨天 hh:mm");
                }
            } else {
                return obj.format(format);
            }
        } catch (e) { return t; }
    }
}

String.prototype.xmlDateToJavascriptDate = function (xmlDate) {
        // It's times like these you wish Javascript supported multiline regex specs
    var re = /^([0-9]{4,})-([0-9]{2})-([0-9]{2})T([0-9]{2}):([0-9]{2}):([0-9]{2})(\.[0-9]+)?(Z|([+-])([0-9]{2}):([0-9]{2}))?$/;
        var match = this.match(re);
        if (!match)
            return null;
        var all = match[0];
        var year = match[1]; var month = match[2]; var day = match[3];
        var hour = match[4]; var minute = match[5]; var second = match[6];
        var milli = match[7];
        var z_or_offset = match[8]; var offset_sign = match[9];
        var offset_hour = match[10]; var offset_minute = match[11];

        if (offset_sign) { // ended with +xx:xx or -xx:xx as opposed to Z or nothing
            var direction = (offset_sign == "+" ? 1 : -1);
            hour = parseInt(hour) + parseInt(offset_hour) * direction;
            minute = parseInt(minute) + parseInt(offset_minute) * direction;
        }
        var utcDate = Date.UTC(year, Number(month) - 1, day, hour, minute, second, (milli || 0));
        return new Date(utcDate);
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
        "S": this.getMilliseconds(), //millisecond
        "w": this.getDay()
    }
    if (format == "chs") {
        var date2 = new Date();
        var date3 = date2.getTime() - this.getTime()
        var days = Math.floor(date3 / (24 * 3600 * 1000))
        var leave1 = date3 % (24 * 3600 * 1000)
        var hours = Math.floor(leave1 / (3600 * 1000))
        var leave2 = leave1 % (3600 * 1000)
        var minutes = Math.floor(leave2 / (60 * 1000))
        var leave3 = leave2 % (60 * 1000)
        var seconds = Math.round(leave3 / 1000)
        format = (days > 365) ? Math.round(days / 365) + "年前" :
        (
            (days > 30 && days < 365) ? Math.round(days / 30) + "月前" :
            (
                (days > 0 && days < 31) ? days + "天前" :
                (
                    (hours > 0) ? hours + "小时前" :
                    (
                        (minutes > 0) ? minutes + "分钟前" : "1分钟前"
                    )
                )
            )
        );
    }
    else {
        if (/(y+)/.test(format))
            format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(format))
                format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    }
    return format;
}