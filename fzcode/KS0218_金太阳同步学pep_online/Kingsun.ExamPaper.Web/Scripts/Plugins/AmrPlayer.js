var RongIMLib;
var Current;

(function (RongIMLib) {
    var RongIMVoice = (function () {
        function RongIMVoice() {
        }
        /**
        * 初始化声音库
        */
        RongIMVoice.init = function (current, amr) {
            Current = current;
            $(current).after('<div class="mp3player"></div>');
            $(current).appendTo($(current).next());
            $(current).after('<div class="mp3Cont big"><em class="state"></em><em class="num good"></em><a class="play"></a><a class="pause" style="display:none;"></a></div>');
            if (this.isIE) {
                var div = document.createElement("div");
                div.setAttribute("id", "flashContent");
                document.body.appendChild(div);
                var script = document.createElement("script");
                script.src = "../Scripts/Plugins/swfobject.js";
                var header = document.getElementsByTagName("head")[0];
                header.appendChild(script);
                setTimeout(function () {
                    var swfVersionStr = "11.4.0";
                    var flashvars = {};
                    var params = {};
                    params.quality = "high";
                    params.bgcolor = "#ffffff";
                    params.allowScriptAccess = "always";
                    params.allowfullscreen = "true";
                    var attributes = {};
                    attributes.id = "player";
                    attributes.name = "player";
                    attributes.align = "middle";
                    swfobject.embedSWF("../Scripts/Plugins/player.swf", "flashContent", "1", "1", swfVersionStr, null, flashvars, params, attributes);
                }, 200);
            }
            else {
                var list = ["../Scripts/Plugins/pcmdata.min.js", "../Scripts/Plugins/libamr-min.js"];
                for (var i = 0, len = list.length; i < len; i++) {
                    var script = document.createElement("script");
                    script.src = list[i];
                    document.head.appendChild(script);
                }
            }
            this.isInit = true;

            //开始播放
            $(current).next("div").find(".play").click(function () {
                var r = new XMLHttpRequest();
                r.open("GET", amr, false);
                r.overrideMimeType('audio/amr');
                r.send(null);
                var blob = binaryToBlob(r.responseText);
                blob.name = blob.fileName = url.substring(amr.lastIndexOf('/') + 1);
                blob.fileType = "audio/amr"; //"image/octet-stream";

                RongIMVoice.play(blob);
            });
            //停止
            $(current).next("div").find(".pause").click(function () {
                RongIMVoice.stop();
            });
        };
        /**
        * 开始播放声音
        * @param data {string} amr 格式的 base64 码
        * @param duration {number} 播放大概时长 用 data.length / 1024
        */
        RongIMVoice.play = function (data) {
            $(Current).parent().find(".play").css("display", "block").addClass("on");
            $(Current).css("display", "none");
            this.checkInit("play");
            var me = this;
            if (me.isIE) {
                me.thisMovie().doAction("init", data);
            }
            else {
                me.palyVoice(data);
                //me.onCompleted(duration);
            }
        };
        /**
        * 停止播放声音
        */
        RongIMVoice.stop = function () {
            this.checkInit("stop");
            var me = this;
            if (me.isIE) {
                me.thisMovie().doAction("stop");
            }
            else {
                if (me.element) {
                    me.element.stop();
                }
            }
            $(this).parent().find(".pause").css({ "display": "inline-block", "*display": "inline", "_display": "inline" });
            $(this).css("display", "none");
        };
        /**
        * 播放声音时调用的方法
        */
        RongIMVoice.onprogress = function () {
            this.checkInit("onprogress");
        };
        RongIMVoice.checkInit = function (postion) {
            if (!this.isInit) {
                throw new Error("RongIMVoice not initialized,postion:" + postion);
            }
        };
        RongIMVoice.thisMovie = function () {
            return eval("window['player']");
        };
        RongIMVoice.onCompleted = function (duration) {
            var me = this;
            var count = 0;
            var timer = setInterval(function () {
                count++;
                me.onprogress();
                if (count >= duration) {
                    clearInterval(timer);
                }
            }, 1000);
            if (me.isIE) {
                me.thisMovie().doAction("play");
            }
        };
        RongIMVoice.base64ToBlob = function (base64Data, type) {
            var mimeType;
            if (type) {
                mimeType = { type: type };
            }
            base64Data = base64Data.replace(/^(.*)[,]/, '');
            var sliceSize = 1024;
            var byteCharacters = atob(base64Data);
            var bytesLength = byteCharacters.length;
            var slicesCount = Math.ceil(bytesLength / sliceSize);
            var byteArrays = new Array(slicesCount);
            for (var sliceIndex = 0; sliceIndex < slicesCount; ++sliceIndex) {
                var begin = sliceIndex * sliceSize;
                var end = Math.min(begin + sliceSize, bytesLength);
                var bytes = new Array(end - begin);
                for (var offset = begin, i = 0; offset < end; ++i, ++offset) {
                    bytes[i] = byteCharacters[offset].charCodeAt(0);
                }
                byteArrays[sliceIndex] = new Uint8Array(bytes);
            }
            return new Blob(byteArrays, mimeType);
        };
        RongIMVoice.palyVoice = function (blob) {
            var reader = new FileReader();// blob = this.base64ToBlob(base64Data, "audio/amr"), me = this;
            reader.onload = function () {
                var samples = new AMR({
                    benchmark: true
                }).decode(reader.result);
                me.element = AMR.util.play(samples);
            };
            reader.readAsBinaryString(blob);
        };
        RongIMVoice.isIE = /Trident/.test(navigator.userAgent);
        RongIMVoice.isInit = false;
        return RongIMVoice;
    })();
    RongIMLib.RongIMVoice = RongIMVoice;
    //兼容AMD CMD
    if ("function" === typeof require && "object" === typeof module && module && module.id && "object" === typeof exports && exports) {
        module.exports = RongIMVoice;
    }
    else if ("function" === typeof define && define.amd) {
        define("RongIMVoice", [], function () {
            return RongIMVoice;
        });
    }
})(RongIMLib || (RongIMLib = {}));
