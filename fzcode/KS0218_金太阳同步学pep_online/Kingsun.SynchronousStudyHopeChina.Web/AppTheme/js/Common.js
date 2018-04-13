//////////////////////////////////////////////
/////常用常量定义
//////////////////////////////////////////////
var Constant = Constant || {};
Constant.do_Url = '/Handler.ashx';
Constant.code_Url = "http://localhost:5176/";
Constant.file_Url = "http://183.47.42.221:8029/";
///////////////////////////////////////////////
////start///分页所用常量参数
///////////////////////////////////////////////
Constant.PageSize = 15;
///////////////////////////////////////////////
////end///分页所用常亮参数
///////////////////////////////////////////////

var Common = Common || {};
Common.GUID = function () {
    var result, i, j;
    result = '';
    for (j = 0; j < 32; j++) {
        if (j == 8 || j == 12 || j == 16 || j == 20)
            result = result + '-';
        i = Math.floor(Math.random() * 16).toString(16).toUpperCase();
        result = result + i;
    }
    return result
}
Common.RequestCore = function (core) {
    if (core) {
        this.ID = core.ID;
        this.Function = core.Function;
        this.Data = core.Data;
    }
    else {
        this.ID = "";
        this.Function = "";
        this.Data = "";
    }
}
Common.Request = function (request) {
    if (request) {
        this.RID = request.RID;
        this.SKEY = request.SKEY;
        this.Pack = request.Pack;
        this.Ticket = Common.Cookie.getcookie(Constant.serviceKey);
    }
    else {
        this.RID = "";
        this.SKEY = "";
        this.Pack = new Common.RequestCore();
        this.Ticket = Common.Cookie.getcookie(Constant.serviceKey);
    }
}

Common.Ajax = function (serviceKey, funcName, data, callback) {
    var request = new Common.Request();
    request.RID = Common.GUID();
    request.SKEY = serviceKey;
    request.Pack.ID = request.RID;
    request.Pack.Function = funcName;
    request.Pack.Data = $.toJSON(data);
    request.Pack = $.toJSON(request.Pack);
    var sendValues = { Form: $.toJSON(request) };
    var url = Constant.do_Url + "?rand=" + Math.random();
    var async_Sign = true;
    if (typeof callback == "function") {
        async_Sign = true;
    } else {
        async_Sign = false;
    }
    var obj = null;
    $.ajax({
        type: "POST",
        url: url,
        data: sendValues,
        async: async_Sign,
        success: function (response) {
            try {
                response = eval("(" + response + ")")
            }
            catch (exception) {
                //alert("返回数据格式错误！");
            }
            if (async_Sign) {
                callback(response);
            } else {
                obj = response;
            }
        },
        error: function (request, status, error) {
            //alert("请求失败！提示：" + status + error);
        }
    });
    return obj;
}

Common.DirectAjax = function (directURL, funcName, data, callback) {
    var request = new Common.Request();
    request.RID = Common.GUID();
    request.SKEY = "";
    request.Pack.ID = request.RID;
    request.Pack.Function = funcName;
    request.Pack.Data = data;
    request.Pack = $.toJSON(request.Pack);
    var sendValues = { Form: $.toJSON(request) };
    var url = directURL + "?rand=" + Math.random();
    var async_Sign = true;
    if (typeof callback == "function") {
        async_Sign = true;
    } else {
        async_Sign = false;
    }
    var obj = null;
    $.ajax({
        type: "POST",
        url: url,
        data: sendValues,
        async: async_Sign,
        success: function (response) {
            try {
                response = eval("(" + response + ")")
            }
            catch (exception) {
                //alert("返回数据格式错误！");
            }
            if (async_Sign) {
                callback(response);
            } else {
                obj = response;
            }
        },
        error: function (request, status, error) {
            //alert("请求失败！提示：" + status + error);
        }
    });
    return obj;
}
Common.CodeAjax = function (sevice, data, callback) {
    var sendValues = { t: data };
    var url = Constant.code_Url + sevice + "?rand=" + Math.random();
    var async_Sign = true;
    if (typeof callback == "function") {
    } else {
        //////不支持同步
        return null;
    }
    var obj = null;
    $.ajax({
        type: "POST",
        url: url,
        data: sendValues,
        dataType: "jsonp",
        async: true,
        success: function (response) {
            if (async_Sign) {
                callback(response);
            } else {
                obj = response;
            }
        },
        error: function (request, status, error) {
        }
    });
    return obj;
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// Start ////Cookie 管理 Start 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.Cookie = Common.Cookie || {};
Common.Cookie.getcookie = function (cookiename) {
    var strCookie = document.cookie;
    var arrCookie = strCookie.split("; ");
    var ck;
    for (var i = 0; i < arrCookie.length; i++) {
        var arr = arrCookie[i].split("=");
        if (cookiename == arr[0]) {
            return unescape(arr[1]);
            break;
        }
    }
    return "";
}
Common.Cookie.setcookie = function (cookiename, val, day) {
    if (day) { day = day; } else { day = 0; }
    if (day == 0) { document.cookie = cookiename + "=" + escape(val) + ";path=/"; }
    else {
        var expires = new Date();
        expires.setTime(expires.getTime() + (1000 * 24 * 3600 * day));
        document.cookie = cookiename + "=" + escape(val) + ";path=/;expires=" + expires.toGMTString();
    }
}

Common.Cookie.delcookie = function (cookiename) {
    var expires = new Date();
    expires.setTime(expires.getTime() - (1000 * 24 * 3600 * 365));
    document.cookie = cookiename + "=;path=/;expires=" + expires.toGMTString();
}
Common.Cookie.delallcookie = function () {
    var strCookie = document.cookie;
    var arrCookie = strCookie.split("; ");
    for (var i = 0; i < arrCookie.length; i++) {
        var arr = arrCookie[i].split("=");
        Common.delcookie(arr[0]);
    }
    return 0;
};
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// End ////Cookie 管理 Start 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// start ////过滤文本框中输入的特殊字符
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.ValidateTxt = function (txtValue) {
    var forbidChar = new Array("@", "#", "$", "%", "^", "&", "*", "……", "￥", "×", "\"", "<", ">");
    for (var i = 0; i < forbidChar.length; i++) {
        if (txtValue.indexOf(forbidChar[i]) >= 0) {
            return "您输入的信息: " + txtValue + " 中含有非法字符: " + forbidChar[i] + " 请更正！";
        }
    }
    return "";
}

Common.HtmlEncode = function (str) {
    if (str == null) return "";
    return str.toString().replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/\"/g, "&#34;").replace(/\'/g, "&#39;");
}
//把字符串进行HTML反编码
Common.HtmlDecode = function (str) {
    if (str == null) return "";
    return str.toString().replace(/\&amp\;/g, '\&').replace(/\&gt\;/g, '\>').replace(/\&lt\;/g, '\<').replace(/\&quot\;/g, '\'').replace(/\&\#39\;/g, '\'');
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// End ////过滤文本框中输入的特殊字符
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// start ////时间格式内容转换为字符型 eg：yyyy年MM月dd日 hh时mm分ss秒
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.FormatTime = function (time, format) {
    if (format == undefined || format == "") {
        format = "yyyy年MM月dd日 hh时mm分";
    }
    var date = new Date(parseInt(time.substring(6, time.length - 2)))
    return date.format(format);
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
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// end ////时间格式内容转换为字符型 eg：yyyy年MM月dd日 hh时mm分ss秒
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// start ////获取url参数
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.QueryString = {
    data: {},
    Initial: function () {
        var aPairs, aTmp;
        var queryString = new String(window.location.search);
        queryString = queryString.substr(1, queryString.length); //remove   "?"     
        aPairs = queryString.split("&");
        for (var i = 0; i < aPairs.length; i++) {
            aTmp = aPairs[i].split("=");
            this.data[aTmp[0]] = aTmp[1];
        }
    },
    GetValue: function (key) {
        return this.data[key];
    }
}
Common.QueryString.Initial();
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// end ////获取url参数
///////////////////////////////////////////////////////////////////////


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// start ////公共验证函数
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.Validate = Common.Validate || {};
////是否为数字
Common.Validate.IsNumber = function (str) {
    var reg = /^\d+(\.\d+)?$/;
    return reg.test(str);
};
//是否为整数
Common.Validate.IsInt = function (str) {
    var reg = /^-?\d+$/;
    return reg.test(str);
}
////是否版本号
Common.Validate.IsVersion = function (str) {
    var reg = /^[1-9]{1}\.\d{1,2}$/;
    return reg.test(str);
};
///正则验证是否是正确的URL地址
Common.Validate.IsURL = function (str_url) {
    var strRegex = "^((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?";
    var re = new RegExp(strRegex);
    //re.test()
    if (re.test(str_url)) {
        return (true);
    } else {
        return (false);
    }
}
Common.Validate.IsMobileNo = function (phone) {
    var regexp = /^1[3|4|5|8][0-9]\d{8}$/
    return regexp.test(phone);
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// end ////公共验证函数
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.showMsg = function (msg) {
    $.messager.show({
        title: '系统提示',
        msg: msg,
        showType: 'show'
    });
}


Common.CodeData = Common.CodeData || {};
//////////////////////////////////////////////////
////////根据gbcode获取代码表数据,可用逗号分隔，一次获取多个基础数据
//////////////////////////////////////////////////
Common.CodeData.InitData = function (gbcode, callback) {
    var codeArray = gbcode.split(",");
    var sendValues = [];
    $.each(codeArray, function (index) {
        var code = codeArray[index];
        if (Common.CodeData[code]) {

        } else {
            if (window.localStorage) {
                /////如果支持localStorage，尝试从localStorage读取
                var codeData = window.localStorage.getItem("CodeData_" + code);
                if (codeData) {
                    Common.CodeData[code] = eval(codeData);
                    return;
                }
            }
            sendValues.push(code);
        }
    });
    if (sendValues.length > 0) {
        Common.CodeAjax("do.jsonp", sendValues.join(","), function (data) {
            $.each(sendValues, function (index) {
                var valueKey = sendValues[index];
                if (data[valueKey]) {
                    Common.CodeData[valueKey] = data[valueKey];
                    if (window.localStorage) {
                        window.localStorage.setItem("CodeData_" + valueKey, $.toJSON(data[valueKey]));
                    }
                }
            });
            if (typeof callback == "function") {
                callback();
            }
        });
    } else {
        if (typeof callback == "function") {
            callback();
        }
    }
};
//////////////////////////////////////////////////
////////根据gbcode获取代码表数据
//////////////////////////////////////////////////
Common.CodeData.GetData = function (gbcode) {
    if (Common.CodeData[gbcode]) {
        return Common.CodeData[gbcode];
    } else {
        if (window.localStorage) {
            var codeData = window.localStorage.getItem("CodeData_" + gbcode);
            if (codeData) {
                Common.CodeData[gbcode] = eval(codeData);
                return Common.CodeData[gbcode];
            }
        }
        return null;
    }
}


Common.GetVideoUrl = function (fileID) {
    var picurl = Constant.file_Url + "Preview.ashx";
    var url = "";
    $.ajax({
        type: "POST",
        url: picurl,
        data: { fileID: fileID },
        dataType: "jsonp",
        async: false,
        success: function (msg) {
            msg = eval(msg)
            if (msg.CanPreview) {
                url = msg.URL;
            }
        },
        error: function (e, x) {
            return "";
        }
    });

    return url;
}



Common.is_weixn = function () {

    var ua = navigator.userAgent.toLowerCase();
    var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
    for (var v = 0; v < Agents.length; v++) {
        if (ua.indexOf(Agents[v].toLowerCase()) > 0) {
            if (ua.match(/MicroMessenger/i) == "micromessenger") {
                return true;
            } else {
                return false;
            }
        }
    }
    return true;


}

Common.is_pc = function () {

    var ua = navigator.userAgent.toLowerCase();
    var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
    for (var v = 0; v < Agents.length; v++) {
        if (ua.indexOf(Agents[v].toLowerCase()) > 0) {
            return false;
        }
    }
    return true;


}

Common.tips = function (content, time) {
    return artDialog({
        id: 'Tips',
        title: false,
        cancel: false,
        fixed: true,
        lock: true
    })
    .content('<div style="padding: 0 1em;">' + content + '</div>')
    .time(time || 1);
};
