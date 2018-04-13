var Common = Common || {};
var Constant = Constant || {};
Constant.code_Url = "http://metadata.kingsun.cn/Service/";

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

Common.GetMetaData = function (gbcode, callback) {
    var codeArray = gbcode.split(",");
    Common.CodeAjax("do.jsonp", codeArray.join(","), function (data) {
        if (typeof callback == "function") {
            callback(data);
        }
    });
}