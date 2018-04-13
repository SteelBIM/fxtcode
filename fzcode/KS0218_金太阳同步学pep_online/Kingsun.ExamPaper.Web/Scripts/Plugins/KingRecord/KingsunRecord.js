/*方直科技音乐播放器V1.0
听说作业web端定制开发
*/
/// <reference path="jquery-1.4.1.js" />
/// <reference path="../jplayer/jquery.jplayer.min.js" />
var KSRecord;

$.fn.KingsunRecord = function (Settings) {
    var _Options = {
        swfPath: ""
        , Text: "" //以竖线分隔开
        , AutoNext: false
        , RecordEnd: null//录音完成之后的事件
    };
    var Current = this;
    this.Options = $.extend(_Options, Settings);
    this.KingRecord = null;
    this.TextArr = this.Options.Text.split('|');
    this.CurrentIndex = 0;
    this.HasMicro = true;
    this.AllowMicro = false;
    //初始化
    this.InitRecord = function () {
        if (!this.Options.swfPath) {
            return;
        }
        var html = '<div class="kingRecord_cont" ><a class="off" href="javascript:void(0)"></a> <a class="on" href="javascript:void(0)"></a><em></em></div>'
        html += ' <div class="kingRecord_obj show" > <div id="record1" ></div></div>';
        $(this).html(html);

        this.Options.ContentDiv = "record1";
        var flashvars = {};
        var params = {
            wmode: "transparent",
            allowScriptAccess: "always"
        };
        var attributes = {

        };
        swfobject.embedSWF(this.Options.swfPath, this.Options.ContentDiv, "220", "140", "9.0.0", "expressInstall.swf", flashvars, params, attributes);
        this.KingRecord = swfobject.getObjectById(this.Options.ContentDiv);

        $(this).find(".off").click(function () {
            $(this).hide();
            $(this).parent().find(".on").css("display", "block");
            Current.RecordOn(Current.TextArr[Current.CurrentIndex]);
            Current.CurrentIndex = Current.CurrentIndex + 1;
        });
        $(this).find(".on").click(function () {
            $(this).hide();
            $(this).parent().find(".off").css("display", "block");
            Current.RecordOff();
            if (Current.CurrentIndex > Current.TextArr.length - 1) {
                Current.CurrentIndex = 0;
            }
        });
    }

    this.RecordOn = function (text) {
        if (Current.KingRecord) {
            var result1 = Current.KingRecord.setSampleText(text);
            var result2 = Current.KingRecord.startRecord();
        }
    }

    this.RecordOff = function () {
        if (Current.KingRecord) {
            var result = Current.KingRecord.stopRecord();
        }
    }

    this.GetScore = function () {
        return Current.KingRecord.getScore();
    }

    this.GetReplayPath = function () {
        return Current.KingRecord.getReplayPath();
    }

    //新增接口：设置用户id，参数String类型，可以是用户账号
    this.SetUserID = function (userid) {
        if (Current.KingRecord) {
            Current.KingRecord.setUserID(userid);
        }
    }

    function checkFlash() {
        var hasFlash = false;
        var swf = null;
        //document.all为IE下，document.getElementsByTagName("*")为非IE
        if (document.all || document.getElementsByTagName("*")) {
            try {
                swf = new ActiveXObject('ShockwaveFlash.ShockwaveFlash');
                if (swf) {
                    hasFlash = true;
                }
            }
            catch (e) {
                //catch不能做处理，而且必须要捕捉;
                //否则在firefox,下，ActiveXObject会出错，下面的代码不会再去执行
            }
            if (!swf) {
                //navigator首字母必须是小写，大写是错误的
                if (navigator.plugins && navigator.plugins.length > 0) {
                    var swf = navigator.plugins["Shockwave Flash"];
                    if (swf) {
                        hasFlash = true;
                    }
                }
            }
        }
        return hasFlash;
    }

    this.MicroReady = function () {
        if (checkFlash()) {
            if (Current.KingRecord) {
                try {
                    if (Current.KingRecord.openSecurityPanel()) {
                        KSRecord.HasMicro = true;
                    }
                } catch (e) {
                    return false;
                }
            }
        } else {
            window.location.href = "Direction.html";
        }
        return true;
    }

    this.InitRecord();

    KSRecord = Current;
    KSRecord.RecordEnd = function (data) {
        if (data) {
            if (Current.Options.RecordEnd) {
                Current.Options.RecordEnd(data);
            }
            //alert(data.lines[0].score);
        }
    };
}
//录音过程中检测返回信息
function errorCallBack(data) {
    console.log("错误码:" + data);
    switch (data) {        
        case -61001:
            alert("没有麦克风");
            break;
        case -61002://（新增）
            alert("没有可用的麦克风，可能被占用");
            break;
        case -10001:
            alert("服务器通讯错误");
            break;
        case -7://原-10002
            alert("服务器连接失败");
            break;
        case -10010:
            alert("服务器连接超时");
            break;
        case -12010:
            alert("安全沙箱冲突2010");
            break;
        case -12048:
            alert("安全沙箱冲突2048");
            break;
        case -20001:
            alert("服务器验证错误");
            break;
        case -20007:
            alert("评测超时");
            break;
        case -30002:
            alert("说话时间超过限制");
            break;
        case -30003:
            alert("数据压缩错误");
            break;
            //case -50013:
            //    alert("本地socket错误");
            //    break;
        case -52001:
            alert("Opus编码错误");
            break;
        case -61001:
            alert("启动录音失败");
            break;
        case -1001://原-61002
            alert("录音异常");
            break;
        case -62001:
            alert("识别异常");
            break;
        default:
            parent.ShowResultWithoutResult();
            break;
    }
}
function recordComplete() {
    if (KSRecord) {
        var result = KSRecord.KingRecord.getScore();
        if (KSRecord.RecordEnd) {
            KSRecord.RecordEnd(eval("(" + result + ")"));
        }
    }
}
//录音开始前检测是否有麦克风
function noMicrophone() {
    alert("没有麦克风");
    KSRecord.HasMicro = false;
}

function ShowFlashInfo(data) {
    //alert(data);//返回"成功开始录音"，"成功结束录音"
}

//用户拒绝麦克风访问时
function refuseRecord() {
    alert("请选择“允许”，并勾选“记住”哦！");
}

//用户允许麦克风访问时
function allowRecord() {
    KSRecord.AllowMicro = true;
}