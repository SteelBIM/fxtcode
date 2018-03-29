var CAS = { Define: {},
    RndVar: function () { return "CAS_" + Math.random().toString().replace(".", ""); },
    API: function (args) {
        var apiurl = CAS.Define.aipmain + args.api.replace(/\./ig, "/") + ".ashx";
        var vdata = args.data;
        //vdata = $.extend({},{ userid: CAS.Define.userid, cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid, companyid: CAS.Define.companyid }, vdata);
        $.ajax({ dataType: "json", url: apiurl, type: args.type, data: vdata, async: args.wait ? false : true
            , success: function (data) {
                //调试信息
                if (data && data.returntype != 1) {
                    if (data.returntype == -99) //-99 未登录重新登录
                    {
                        ReLogin();
                    } else {
                    }
                }
                else {
                    //处理json中值为Null的，都改为""，方便给页面控件赋值时不显示为"null" kevin
                    function handlejson(o) {
                        if (o == null) return o;
                        if (o.length > 0) {
                            for (var i = 0; i < o.length; i++) {
                                //log(o[i])
                                o[i] = handlejson(o[i]);
                            }
                        }
                        else if (o) {
                            for (var item in o) {
                                if (o[item] == null) {
                                    o[item] = "";
                                }
                            }
                        }
                        return o;
                    }

                    if (data && data.data && typeof data.data == "object")
                        data.data = handlejson(data.data);
                }
                if (args.callback) { args.callback(data); }
            }
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (XMLHttpRequest.responseText != "")
                    log("错误:" + XMLHttpRequest.responseText);
            }, complete: function (XHR, TS) { XHR = null; }
        });
    }
};
var init = {
    JsonToDate: function (t, format) { //格式日期 如：jsonToDate(date,"yyyy-MM-dd"):2012-08-14
        if (t == "") return t;
        try {
            var obj = {};
            if ("object" == typeof (t)) {
                obj = t;
            }
            else {
                obj = eval('new ' + (t.replace(/\//g, '')));
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
    },
    //千分位
    Commafy: function (n) {
        if (n == null) return "";
        if (isNaN(n)) return n;
        n = n.toString();
        var re = /\d{1,3}(?=(\d{3})+$)/g;
        var n1 = n.replace(/^(\d+)((\.\d+)?)$/, function (s, s1, s2) { return s1.replace(re, "$&,") + s2; });
        return n1;
    },
    GetQuery: function (name, url) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = null;
        if (url)
            r = url.substr(1).match(reg);
        else
            r = window.location.search.substr(1).match(reg);
        if (r != null) return decodeURI(r[2]); return null;
    },
    ajax: function (args) {//type:POST or GET;data:args=args or {args:args} dataType:json or other
        $.ajax({
            type: args.type,
            url: args.url,
            data: args.data,
            dataType: args.dataType == null ? "json" : args.dataType,
            success: function (data) {
                args.callback(data);
            }
        });
    },
    urlargshandle: function (args) {
        args = args.replace(/#/g, "№");
        args = args.replace(/&/g, "□");
        return args;
    }, trim: function (args, t) {
        if (args.length == 0) { return ""; }
        else {
            args.replace(/t$/ig, "");
        }
    }
};
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
$.ajaxSetup({ cache: false });

$.fn.extend({
    //自动完成楼盘
    casautoproject: function (args) {
        var tmpvalue = "";
        var $this = $(this);
        //处理数据
        function _return(data) {
            if (tmpvalue == $this.val()) return;
            if (data && data.projectid) {
                $("#" + args.objprojectid).val(data.projectid);
                if (data.projectname) $("#" + args.objprojectname).val(data.projectname);
            }
            tmpvalue = $this.val();
        }
        $(this).casautocomplete({
            fieldformats: [{ field: "projectname"}],
            fieldresult: "projectname", //返回的字段
            data: "api.autoprice", //API地址或者json数据
            options: { extraParams: { type: "dropdown", cityid: CAS.Define.cityid, companyid: CAS.Define.companyid} }, //扩展选项
            callback: function (event, data, formatitem) {//回调函数
                _return(data);
                if (args && args.callback) args.callback(data);
            }
        });
        //处理找不到的情况
        $this.bind("change", function () {
            if (args && args.callback) {
                args.callback(null);
            }
            _return(null);
        });
        return this;
    },
    //自动完成 
    casautocomplete: function (args) {

        var $this = $(this); 
        if (null != $this.unautocomplete) {
            $this.unautocomplete();
        }
        var options = {
            scrollHeight: 250,
            max: 200,
            matchContains: true,
            dataType: "json",
            //keycallback: function () { keycallback() },
            formatItem: function (row, i, max) {
                var formats = [];
                if (args.fieldformats) {
                    var len = args.fieldformats.length;
                    for (var i = 0; i < len; i++) {
                        var begin = args.fieldformats[i].begin;
                        if (!begin) begin = "";
                        var end = args.fieldformats[i].end;
                        if (!end) end = "";
                        formats.push(begin + row[args.fieldformats[i].field] + end);
                    }
                    return formats.join("");
                }
            },
            formatMatch: function (row, i, max) {
                if (args.fieldmatchs) {
                    var s = "";
                    var len = args.fieldmatchs.length;
                    for (var i = 0; i < len; i++) {
                        s += row[args.fieldmatchs[i]] + " ";
                    }
                    return s;
                }
            },
            formatResult: function (row) {
                return row[args.fieldresult];
            }
        }
        options = $.extend({}, options, args.options);
        if ($.fn.autocomplete) {
            $this.autocomplete(args.data, options).result(
                function (event, data, formatted) {
                    args.callback(event, data, formatted);
                });
        }
        else {
            $this.autocomplete(args.data, options).result(
                function (event, data, formatted) {
                    args.callback(event, data, formatted);
                });
        }
    },
    //取消自动完成
    casunautocomplete: function () {
        var $this = $(this);
        if ($.fn.unautocomplete) {
            $this.unautocomplete();
        }
        else {
            $this.unautocomplete();
        }
    }
});

String.prototype.trimEnd = function (args) {
    if (this.length == 0) { return this; }
    else {
        return this.replace(/^(\s|\u00A0)+|(\s|\u00A0)+$/g, "");
    }
}