//////////////////////////////////////////////
/////常用常量定义
//////////////////////////////////////////////
var Constant = Constant || {};
Constant.teach = 'SunnyTeach/';
Constant.do_Url = '/' + Constant.teach + 'Handler/Handler.ashx';
Constant.serviceKey = 'Kingsun.SunnyTeach.Web';
Constant.host = "http://" + window.location.host + "/";
//Constant.code_mod_Url = "http://192.168.3.200:2342/"; // MOD基础数据地址
//Constant.resource_Url = "http://192.168.3.201:8899/"; //资源网地址 开发
//Constant.code_mod_Url = "http://183.47.42.221:8027/"; // MOD基础数据地址
Constant.code_mod_Url = "http://metadata.kingsun.cn/Service/"; // MOD基础数据地址
//Constant.code_mod_Url = "http://192.168.3.97:1155/"; // MOD基础数据地址
//Constant.classInfo_Url = "http://183.47.42.221:8001/";
Constant.classInfo_Url = "http://localhost:4463/";
Constant.resource_Url = "http://183.47.42.221:1777/"; //资源池地址 开发
//Constant.resource_Url = "http://192.168.3.97:1155/"; //资源池地址 开发
Constant.file_Url = "http://file.kingsun.cn/"; //资源文件服务器地址
Constant.Ossfile_Url = "https://tbxcdn.kingsun.cn/SynchronousStudy/UpLoadFile/";
Constant.Videofile_Url = "https://video.kingsun.cn/";
///////////////////////////////////////////////
////start///分页所用常亮参数
///////////////////////////////////////////////
Constant.PageSize = 10;
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

Common.CodeAjax_City = function (sevice, data, callback) {
    var sendValues = { t: data };
    var url = Constant.code_mod_Url + sevice + "?rand=" + Math.random();
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


Common.GetStrLength = function (str) {
    if (str == null) return 0;
    if (typeof str != "string") {
        str += "";
    }
    return str.replace(/[^x00-xff]/g, "01").length;
}

// 资源库数据调用方法
Common.ResourceAjax = function (sevice, data, callback) {
    var sendValues = { t: data };
    var url = Constant.resource_Url + sevice + "?rand=" + Math.random();

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
        beforeSend: function () {
            //alert("正在加载资源数据");
            //$("#loading").html("<img src='' />");
        },
        success: function (response) {
            $("#loading").empty();
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

// 基础数据MOD数据调用方法
Common.CodeAjax = function (sevice, data, callback) {
    var sendValues = { t: data };
    var url = Constant.code_mod_Url + sevice + "?rand=" + Math.random();
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
//////////////通过基础条件获取教材数据/////////////////
Common.GetSBListByStages = function (Stage, Grade, Subject, Booklet, Edition, callback) {
    var obj = {
        Stage: Stage,
        Grade: Grade,
        Subject: Subject,
        Booklet: Booklet,
        Edition: Edition
    };
    Common.CodeAjax("GetSBListByStages.sun", obj, function (data) {
        if (typeof callback == "function") {
            callback(data);
            return;
        }
    });
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
////读取对应教材章节目录  //BookId
Common.GetCatalogByBookId = function (BookId, callback) {
    var obj = {
        BookId: BookId
    };
    Common.CodeAjax("GetCatalogByBookId.sun", obj, function (data) {
        if (typeof callback == "function") {
            callback(data);
            return;
        }
    });
}

////读取对应教材章节目录(包含有已经删除的记录)  //BookId
Common.GetALLCatalogByBookId = function (BookId, callback) {
    var obj = {
        BookId: BookId
    };
    Common.CodeAjax("GETALLCATALOGBYBOOKID.sun", obj, function (data) {
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


//获取地区信息
//Common.GetPathByAreaID = function (ID, callback) {
//    var obj = {
//        ID: ID  ////以名称字段存放ID字符串
//    };
//    Common.CodeAjax("GetPathByAreaID.sun", obj, function (data) {
//        if (typeof callback == "function") {
//            callback(data);
//            return;
//        }
//    });
//}

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
//    var strCookie = document.cookie;
//    var arrCookie = strCookie.split("; ");
//    var ck;
//    for (var i = 0; i < arrCookie.length; i++) {
//        var arr = arrCookie[i].split("=");
//        if (cookiename == arr[0]) {
//            return unescape(arr[1]);
//            break;
//        }
//    }
//    return "";

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

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// start ////字符串长度判断
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.DataLength = function (fData) {
    var intLength = 0
    for (var i = 0; i < fData.length; i++) {
        if ((fData.charCodeAt(i) < 0) || (fData.charCodeAt(i) > 255))
            intLength = intLength + 2
        else
            intLength = intLength + 1
    }
    return intLength
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
        return unescape(this.data[key]);
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

Common.District = Common.District || {};
/////////////通过条件获取学校列表/////////////////////
Common.District.GetSchool = function (districtID, townID, keyword, callback) {
    var obj = {
        DistrictID: districtID,
        TownsID: townID,
        SchoolName: keyword
    };
    Common.CodeAjax("do.school", obj, function (data) {
        if (typeof callback == "function") {
            callback(data);
            return;
        }
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

Common.AjaxMod = function (Modurl,serviceKey, funcName, data, callback) {
    var request = new Common.Request();
    request.RID = Common.GUID();
    request.SKEY = serviceKey;
    request.Pack.ID = request.RID;
    request.Pack.Function = funcName;
    request.Pack.Data = $.toJSON(data);
    request.Pack = $.toJSON(request.Pack);
    var sendValues = { Form: $.toJSON(request) };
    var url = Modurl + "?rand=" + Math.random();
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
        dataType: "jsonp",
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

//////////////////////////////////////////////////
////////根据学段、年级、学科、版本、册别 获取资源列表信息
////////////////////////////////////////////////// 
Common.AjaxResource = function (sevice, data, callback) {
    var sendValues = { t: data };
    var url = Constant.resource_Url + sevice + "?rand=" + Math.random();
//    var url = "http://192.168.3.70:8001/" + sevice + "?rand=" + Math.random();
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