//////////////////////////////////////////////
/////常用常量定义
//////////////////////////////////////////////
var Constant = Constant || {};
Constant.do_Url = '/Handler.ashx';
Constant.serviceKey = 'KingsunMetaDataService';
Constant.code_Url = "http://metadata.kingsun.cn/Service/";
Constant.WebStartTime = "2015-06-01 08:00:00";  ////[根据需求]系统部署时间，数据统计使用
//Constant.file_Url = "http://119.145.5.77:8029/"; //资源文件服务器地址
Constant.file_AppUrl = "http://filebackup.kingsun.cn/"; //资源文件服务器地址
Constant.file_Url = "http://file.kingsun.cn/";
///////////////////////////////////////////////
////start///分页所用常亮参数
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

////读取对应教材章节目录  //Stage Grade Subject Booklet Edition
Common.GetStandardCatalog = function (Stage, Grade, Subject, Booklet, Edition, callback) {
    var obj = {
        Stage: Stage,
        Grade: Grade,
        Subject: Subject,
        Booklet: Booklet,
        Edition: Edition
    };
    Common.CodeAjax("StandardCatalog.sun", obj, function (data) {
        if (typeof callback == "function") {
            callback(data);
            return;
        }
    });
}


////读取服务器知识点数据，根据ID字符串---资源已经选择的数据
Common.GetKnowledgeName = function (CodeName, callback) {
    var obj = {
        CodeName: CodeName  ////以名称字段存放ID字符串
    };
    Common.CodeAjax("KnowledgeName.sun", obj, function (data) {
        if (typeof callback == "function") {
            callback(data);
            return;
        }
    });
}

////读取服务器教材目录，根据ID字符串---资源已经选择的数据
Common.GetStandardCatalogName = function (FolderName, callback) {
    var obj = {
        FolderName: FolderName  ////以名称字段存放ID字符串
    };
    Common.CodeAjax("StandardCatalogName.sun", obj, function (data) {
        if (typeof callback == "function") {
            callback(data);
            return;
        }
    });
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
Common.Cookie.getfzcookie = function (cookiename) {
    var cookie = $.cookie(cookiename);
    return cookie;
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
//获取单元题目缓存
Common.Cookie.getcookieArray = function (cookiename) {
    var listArray = [];
    var tmpList = Common.Cookie.getcookie(cookiename);
    if (tmpList) {
        listArray = eval("(" + tmpList + ")");
    }
    return listArray;
}
//获取题目
Common.Cookie.getQueRound = function (cookiename, val) {
    var qRound = 1;
    var listArray = [];
    var tmpList = Common.Cookie.getcookieArray(cookiename);
    if (tmpList) {
        listArray = tmpList;
        for (var i = 0; i < listArray.length; i++) {
            if (listArray[i].QID == val) {
                qRound = listArray[i].Round;
                break;
            }
        }
    }
    return qRound;
}
//更新缓存中的题目次数
Common.Cookie.updateQue = function (cookiename, val, round) {
    var listArray = [];
    var tmpList = Common.Cookie.getcookieArray(cookiename);
    if (tmpList) {
        listArray = tmpList;
        for (var i = 0; i < listArray.length; i++) {
            if (listArray[i].QID == val) {
                listArray[i].Round = round;
                break;
            }
        }
        Common.Cookie.setcookie(cookiename, $.toJSON(listArray));
    }
}
//插入选择题目到单元题目缓存
Common.Cookie.insertQue = function (cookiename, item) {
    var listArray = [];
    var tmpList = Common.Cookie.getcookieArray(cookiename);
    if (tmpList) {
        listArray = tmpList;
    }
    listArray.push(item);
    return listArray;
}
//从单元题目缓存删除选择题目（判断QID）
Common.Cookie.deleteQue = function (cookiename, val) {
    var listArray = [], delIndex = -1;
    var tmpList = Common.Cookie.getcookieArray(cookiename);
    if (tmpList) {
        listArray = tmpList;
        if (listArray.length > 0) {
            for (var i = 0; i < listArray.length; i++) {
                if (listArray[i].QID == val) {
                    delIndex = i;
                    break;
                }
            }
            listArray = Common.DeleteArray(listArray, delIndex);
        }
    }
    return listArray;
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// End ////Cookie 管理 Start 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// start ////过滤文本框中输入的特殊字符
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.ValidateTxt = function (txtValue) {
    var forbidChar = new Array("@", "#", "$", "%", "^", "&", "*", "……", "￥", "×", "\"", "<", ">", "\\", "/");
    for (var i = 0; i < forbidChar.length; i++) {
        if (txtValue.indexOf(forbidChar[i]) >= 0) {
            return "您输入的信息: " + txtValue + " 中含有非法字符: " + forbidChar[i] + " 请更正！";
        }
    }
    return "";
}

Common.HtmlEncode = function (str) {
    if (str == null) return "";
    return str.toString().replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/\"/g, "&quot;");
}
//把字符串进行HTML反编码
Common.HtmlDecode = function (str) {
    if (str == null) return "";
    return str.toString().replace(/\&amp\;/g, '\&').replace(/\&gt\;/g, '\>').replace(/\&lt\;/g, '\<').replace(/\&quot\;/g, '\"');
}
//过滤字符串中的多余空格
Common.TrimSpace = function (str) {
    var reg = /^\s+|\s+$/g;//先去除两端的空格
    str = str.replace(reg, '');;
    reg = /\s+/g;//然后去除中间的多余空格
    str = str.replace(reg, " ");
    return str;
}
//获取文本光标位置
Common.getCursortPosition = function (ctrl) {
    var CaretPos = 0;	// IE Support
    if (document.selection) {
        ctrl.focus();
        var Sel = document.selection.createRange();
        Sel.moveStart('character', -ctrl.value.length);
        CaretPos = Sel.text.length;
    }
        // Firefox support
    else if (ctrl.selectionStart || ctrl.selectionStart == '0')
        CaretPos = ctrl.selectionStart;
    return (CaretPos);
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// End ////过滤文本框中输入的特殊字符
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// start ////时间格式内容转换为字符型 eg：yyyy年MM月dd日 hh时mm分ss秒
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.FormatTime = function (time, format) {
    if (!time) {
        return "";
    }
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
        return unescape(this.data[key] == "null" ? "" : this.data[key]);
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
    var regexp = /^1[3|4|5|7|8][0-9]\d{8}$/
    return regexp.test(phone);
}

//验证是否有拼音
Common.Validate.IsPinYin = function (content) {
    var strRegex = "[a-zA-Zāáǎàēéěèīíǐìōóǒòūúǔùǖǘǚǜ]{1,6}";
    var reg = new RegExp(strRegex, 'g');
    var cReg = new RegExp("[A-Z]+");
    var htmlReg = new RegExp("(?=<).*?(?=>)", 'g');
    var htmlList = content.match(htmlReg);
    content = content.split(htmlReg);
    var newContent = '';
    var h = 0;
    for (var c = 0; c < content.length; c++) {
        if (reg.test(content[c])) {
            var list = content[c].match(reg);
            var temp = [];
            var json = {};
            //去重
            for (var i = 0; i < list.length; i++) {
                if (!json[list[i]]) {
                    temp.push(list[i]);
                    json[list[i]] = 1;
                }
            }
            for (var i = 0; i < temp.length; i++) {
                if (temp[i] != "br" && temp[i] != "nbsp" && temp[i] != "u" && temp[i] != "n" && !cReg.test(temp[i])) {
                    content[c] = content[c].replace(new RegExp(temp[i], "g"), '<font style="font-family:GB Pinyinok-C">' + temp[i] + '</font>');
                }
            }
        }
        if (content[c].substring(0, 1) == '>') {
            content[c] = htmlList[h] + content[c];
            h++;
        }
        newContent += content[c];
    }
    return newContent;
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
//art提示框
Common.DialogMsg = function (type, msg) {
    art.dialog({
        id: 'msg',
        icon: type,
        content: msg
    }).time(3);
}
////关闭art弹窗 ID=弹窗ID
Common.closeDialog = function (ID) {
    art.dialog({ id: ID }).close();
    //art.dialog({ id: ID }).time(3);
}
//关闭所有art弹窗
Common.closeAllDialog = function () {
    var list = art.dialog.list;
    for (var i in list) {
        list[i].close();
    };
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

//获取格式为yyyy-MM-dd的日期字符串
Common.GetDate = function (time) {
    return time.getFullYear() + "-" + ((time.getMonth() >= 9 ? "" : "0") + (time.getMonth() + 1)) + "-" + ((time.getDate() >= 10 ? "" : "0") + time.getDate());
}
//将格式为yyyy-MM-dd HH:mm:ss的字符串转换为日期
Common.StringToDate = function (timestr) {
    return new Date(timestr.replace(/-/g, "/ "));
    //var str1 = timestr.split(' ');
    //var str2 = str1[1].split(':');
    //return new Date((new Date(str1[0]).getTime()) + str2[0] * 3600000 + str2[1] * 60000 + str2[2] * 1000);
}
//获取星期，pre为“周”或“星期”
Common.GetWeekday = function (time, pre) {
    var week;
    switch (time.getDay()) {
        case 1:
            week = "一";
            break;
        case 2:
            week = "二";
            break;
        case 3:
            week = "三";
            break;
        case 4:
            week = "四";
            break;
        case 5:
            week = "五";
            break;
        case 6:
            week = "六";
            break;
        default:
            week = "日";
            break;
    }
    return pre + week;
}
//通过阿拉伯数字获取中文大写数字
Common.GetChineseNum = function (num) {
    var numText = "";
    switch (num) {
        case 1: numText = "一"; break;
        case 2: numText = "二"; break;
        case 3: numText = "三"; break;
        case 4: numText = "四"; break;
        case 5: numText = "五"; break;
        case 6: numText = "六"; break;
        case 7: numText = "七"; break;
        case 8: numText = "八"; break;
        case 9: numText = "九"; break;
        default: numText = "一"; break;
    }
    return numText;
}
//控制内容过少时，页脚置底
Common.AutoPosition = function () {
    var height = $("#mainBody").height();
    var Height = $(window).height();
    var otherHeight = $(".header").height() + $(".navDiv").height() + $("#footer").height() + 140;
    if (height < (Height - otherHeight)) {
        $("#footer").css("position", "fixed");
    } else {
        $("#footer").css("position", "relative");
    }
}

Common.DeleteArray = function (inArray, delIndex) {
    var len = inArray.length, outArray = [];

    if (delIndex < 0 || delIndex >= len) return inArray;
    if (delIndex == 0) { inArray.shift(); return inArray };
    if (delIndex == len - 1) { inArray.pop(); return inArray };

    outArray = inArray.slice(0, delIndex);
    outArray = outArray.concat(inArray.slice(delIndex + 1));

    return outArray;
}
//在IE8下，js数组没有indexOf方法;
//在使用indexOf方法前，执行一下下面的js, 原理就是如果发现数组没有indexOf方法，会添加上这个方法。
Common.CheckIndexOf = function () {
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
//添加事件处理程序
Common.addHandler = function (element, type, handler) {
    if (element.addEventListener) {//DOM2
        element.addEventListener(type, handler, false);
    } else if (element.attachEvent) {//IE
        element.attachEvent("on" + type, handler);
    } else {//DOM0级
        element["on" + type] = handler;
    }
}
//移除之前添加的事件处理程序
Common.removeHandler = function (element, type, handler) {
    if (element.removeEventListener) {//DOM2
        element.removeEventListener(type, handler, false);
    } else if (element.detachEvent) {//IE
        element.detachEvent("on" + type, handler);
    } else {//DOM0级
        element["on" + type] = null;
    }
}

Common.onlineAsk = function () {
    window.open("http://crm2.qq.com/page/portalpage/wpa.php?uin=4001118180&aty=1&a=0&curl=&ty=1");
}
//匹配分式，并改为上下显示
Common.MatchFenShi = function (str) {
    str = Common.MatchKuoHao(str);
    var regex = new RegExp(/(\d+\/\d+)|(\（\）\/\（\）)/g);
    if (regex.test(str)) {
        var list = str.match(regex);
        for (var i = 0; i < list.length; i++) {
            str = str.replace(list[i], '<span class="MathJye"><table class="table" style="margin-right:1px" cellpadding="0" cellspacing="0"><tbody>'
                        + '<tr><td class="fsline">' + (list[i].split("/"))[0].replace("（）", "（   ）") + '</td></tr>'
                        + '<tr><td>' + (list[i].split("/"))[1].replace("（）", "（   ）") + '</td></tr></tbody></table></span>');
        }
        str = str.replace("{", "").replace("}", "");
    }
    return str;
}

//匹配括号
Common.MatchKuoHao = function (str) {
    var regex = new RegExp(/（\s+）/g);
    if (regex.test(str)) {
        var list = str.match(regex);
        for (var i = 0; i < list.length; i++) {
            str = str.replace(list[i], '（）');
        }
    }
    return str;
}

//计算小题得分
Common.GetMinQuestionScore = function (count) {
    var score = 0;
    if (count > 0) {
        score = 100 / count;
    }
    return score;
}

//计算大题得分
Common.GetParentQueScore = function (minQueCount, rightCount) {
    var score = 0;
    if (minQueCount == rightCount) {
        score = 100;
    } else {
        score = 100 * rightCount / minQueCount;
    }
    return score;
}