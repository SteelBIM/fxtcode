/*方直科技音乐播放器V1.0
听说作业web端定制开发
*/
/// <reference path="jquery-1.4.1.js" />
/// <reference path="jplayer/jquery.jplayer.min.js" />
$.fn.KingsunMp3Player = function (Settings) {
    var defaults = {
        swfPath: null,
        title: "",
        mp3: "",
        module: 0,  //模式：0-正常（大喇叭），1-录音，2-小喇叭
        OnlyOne: false,
        AutoNext: false,
        EndFun: null,
        ArrayIndex: null,   //20150915钟伟鹏添加：当有多个跟读课文的大题时，存储该大题的索引
        UserID: "",         //20150930钟伟鹏添加；原因：云知声添加了新接口，设置用户ID，需要提供此参数
        ksRecord: null, //云知声录音控件
        content: "", //音频对应的内容
        StartRecord: null,  //开始录音时的事件
        StartProgress: null,    //开始进度条
        UpdateProgress: null,   //实时更新进度条
        EndProgress: null,  //结束进度条
        IsClickMicro: true,  //麦克风是否可点击

    };
    defaults = $.extend(defaults, Settings);
    if (!defaults.mp3 || !defaults.swfPath) {
        return;
    }
    var playInterval, recordTimeout, progressInterval;
    var isStory = Common.QueryString.GetValue("type") == 'story';
    var progressValue = 0;
    var Current = this;
    Current.ArrayIndex = defaults.ArrayIndex;
    Current.PlayerObj = null;
    Current.Module = defaults.module;
    Current.Mp3List = defaults.mp3.split(";");
    Current.ContentList = defaults.content.split(";");
    Current.DuringList = [];//每个音频持续时长
    Current.CurrentMp3 = 0;
    Current.Defaults = defaults;
    Current.UserID = defaults.UserID;
    Current.KsRecord = defaults.ksRecord;
    Current.StartRecord = defaults.StartRecord;
    Current.StartProgress = defaults.StartProgress;
    Current.UpdateProgress = defaults.UpdateProgress;
    Current.EndProgress = defaults.EndProgress;
    Current.IsClickMicro = defaults.IsClickMicro;


    Current.DuringList[Current.CurrentMp3] = 0;//重置音频时长,计时放在此处是因为加载页面就需要播放一遍.hlw加
    playInterval = setInterval(function () {
        Current.DuringList[Current.CurrentMp3] += 0.1;
    }, 100);
    this.InitPlayer = function () {
        $(this).after('<div class="mp3player"></div>');
        $(this).appendTo($(this).next());
        this.PlayerObj = $(this).jPlayer({
            ready: function () {
                $(this).jPlayer("setMedia", {
                    mp3: Current.Mp3List[Current.CurrentMp3]
                });
                if (!isStory) {//非课文,只播放不跟读
                    $(this).jPlayer("play");
                } else {//课文,播放并自动进入跟读程序
                    $(this).next("div").find(".micro").trigger("click");
                }
            },
            ended: function () {
                clearInterval(playInterval);
                if (isStory) {//跟读课文播放完自动开始录音,流程和跟读单词不同.
                    Current.End();
                }
            },//
            supplied: "mp3",
            wmode: "window",
            swfPath: defaults.swfPath
        });
        for (var i = 0; i < Current.Mp3List.length; i++) {
            Current.DuringList.push(0);
        }
        if (Current.Module == 0) {//正常模式
            if (Common.QueryString.GetValue("type") == 'story') {//课文模式
                $(this).after('<div class="mp3Cont big"><em class="state"></em><em class="num good"></em><a class="play"></a><a class="pause" style="display:none;"></a></div>');
            } else {
                $(this).after('<div class="mp3Cont big"><em class="state"></em><em class="num good"></em><a class="micro"></a><a class="play" style="display:none;"><a class="pause" style="display:none;"></a></div>');//不是跟读课文,不管什么模式,都显示录音按钮 hlw
            }


        }
        else if (Current.Module == 1) {//录音模式
            $(this).after('<div class="mp3Cont big"><em class="state"></em><em class="num good"></em><a class="micro"></a><a class="play" style="display:none;"><a class="pause" style="display:none;"></a></div>');
        }
        else {//小喇叭模式，报告中跟读单词播放
            $(this).after('<div class="mp3Cont small"><em class="state"></em><em class="num good"></em><a class="play"></a><a class="pause" style="display:none;"></a></div>');
        }
        //hlw

        if (isStory) {
            $(this).next("div").find(".micro").click(function () {//开始跟读流程
                if (Current.IsClickMicro && Current.KsRecord.HasMicro) {
                    if (Current.StartRecord) {
                        Current.Defaults.StartRecord(Current.CurrentMp3);
                    }
                    Current.PlayerObj.jPlayer("play");
                    $(this).parent().find(".play").css("display", "block").addClass("on");//显示“播放中”按钮
                    $(this).css("display", "none");//隐藏“麦克风”按钮
                    Current.DuringList[Current.CurrentMp3] = 0;//重置音频时长
                    playInterval = setInterval(function () {
                        Current.DuringList[Current.CurrentMp3] += 0.1;
                    }, 100);
                }
            });
        } else {
            $(this).next("div").find(".micro").click(function () {//开始跟读流程
                if (Current.IsClickMicro && Current.KsRecord.HasMicro) {
                    if (Current.StartRecord) {
                        Current.Defaults.StartRecord(Current.CurrentMp3);
                    }
                    //  Current.PlayerObj.jPlayer("play"); //hlw 注释 



                    $(this).parent().find(".play").css("display", "block").addClass("on");//显示“播放中”按钮  hlw注释
                    $(this).css("display", "none");//隐藏“麦克风”按钮  hlw注释               
                    Current.Record();
                }
            })
        }
        ;
        $(this).next("div").find(".play").click(function () {//点击“播放中”按钮，回到初始状态
            if (Current.Module == 1) {
                if (playInterval) {
                    clearInterval(playInterval);
                }

                Current.PlayerObj.jPlayer("stop");
                $(this).parent().find(".micro").css("display", "block");
                Current.Mp3Again();
            } else {
                if (defaults.OnlyOne) {
                    // $(".pause").click();
                    $(".pause").change();
                }
                Current.PlayerObj.jPlayer("play");
                $(this).parent().find(".pause").css({ "display": "inline-block", "*display": "inline", "_display": "inline" });
            }
            $(this).css("display", "none");
        });
        //    $(this).next("div").find(".pause").click(function () {//点击“暂停录音”按钮，回到初始状态
        $(this).next("div").find(".pause").change(function () {//根据需求此处禁用点击事件,故暂用change替代
            if (Current.Module == 1) {
                if (progressInterval) {
                    clearInterval(progressInterval);
                    Current.EndProgress();
                    progressValue = 0;
                }
                if (recordTimeout) {
                    clearTimeout(recordTimeout);
                }
                Current.PlayerObj.jPlayer("stop");
                $(this).parent().find(".micro").css("display", "block");
                Current.Mp3Again();
            } else {
                Current.PlayerObj.jPlayer("stop");
                $(this).parent().find(".play").css({ "display": "inline-block", "*display": "inline", "_display": "inline" });
            }
            $(this).css("display", "none");
        });

    };
    //点击播放
    this.ClickPlay = function () {//hlw 
        Current.PlayerObj.jPlayer("play");
        Current.DuringList[Current.CurrentMp3] = 0;//重置音频时长
        playInterval = setInterval(function () {
            Current.DuringList[Current.CurrentMp3] += 0.1;
        }, 100);
    }

    this.Play = function () {
        $(Current).next("div").find(".play").click();
        //Current.PlayerObj.jPlayer("play")
    };
    //暂停
    this.Pause = function () {
        //  $(Current).next("div").find(".pause").click();hlw注释
        $(Current).next("div").find(".pause").change();
    };
    //停止
    this.Stop = function () {
        if (playInterval) {
            clearInterval(playInterval);
        }
        if (recordTimeout) {
            clearTimeout(recordTimeout);
        }
        if (progressInterval) {
            clearInterval(progressInterval);
            Current.EndProgress();
            progressValue = 0;
        }
        Current.PlayerObj.jPlayer("stop");
        if (Current.Module == 1) {
            $(Current).parent().find(".micro").css("display", "block");
            $(Current).parent().find(".play").css("display", "none");
        } else {
            $(Current).parent().find(".play").css("display", "block");
            $(Current).parent().find(".micro").css("display", "none");
        }
        $(Current).parent().find(".pause").css("display", "none");
    };
    //录音中
    this.Record = function () {
        var recordDurring = 0;
        if (playInterval) {
            clearInterval(playInterval);
            recordDurring = Current.DuringList[Current.CurrentMp3] * 1.5;
            //if (Current.DuringList[Current.CurrentMp3] <= 3) {//hlw注释
            //    Current.DuringList[Current.CurrentMp3] = 3;
            //}
            //else {
            //    Current.DuringList[Current.CurrentMp3] = 1.5 * Current.DuringList[Current.CurrentMp3];
            //}
            if ($("#mp3player").find("audio")) {//通过h5获取音频时长
                var h5Audioduration = $("#mp3player").find("audio")[0].duration;
                if (recordDurring < h5Audioduration * 1.5) {
                    recordDurring = h5Audioduration * 1.5;
                }
            }

            if (recordDurring <= 3) {//hlw加
                recordDurring = 3;
            }
        }
        Current.PlayerObj.jPlayer("stop");
        $(Current).parent().find(".play").css("display", "none");
        $(Current).parent().find(".pause").css("display", "block");
        $("em .state").html("录音中");
        Current.StartProgress();
        Current.KsRecord.RecordOn(Current.ContentList[Current.CurrentMp3]);//开始录音
        progressInterval = setInterval(function () {
            progressValue += 0.1;
            // Current.UpdateProgress((progressValue * 100) / Current.DuringList[Current.CurrentMp3]);//实时更新进度条
            Current.UpdateProgress((progressValue * 100) / (recordDurring+0.7));//实时更新进度条 hlw
        }, 100);
        recordTimeout = setTimeout(function () {
            Current.KsRecord.RecordOff();
            Current.EndProgress();
            clearInterval(progressInterval);
            progressValue = 0;

        }, (
      //  Current.DuringList[Current.CurrentMp3] * 1000 + 100
          recordDurring * 1000 + 1000//hlw加
        ));//延时0.1秒，保证进度条走完
    };
    this.NextRecord = function (autoNext) {
        if (Current.Mp3List.length == 1 || Current.CurrentMp3 == (Current.Mp3List.length - 1)) {
            Current.Pause();
            Current.CurrentMp3 = 0;
            Current.Mp3Again();
        } else {
            Current.CurrentMp3 = Current.CurrentMp3 + 1;
            Current.PlayerObj.jPlayer("setMedia", { mp3: Current.Mp3List[Current.CurrentMp3] });
            if (autoNext) {
                Current.PlayerObj.jPlayer("play");
                $(Current).parent().find(".play").css("display", "block");
                $(Current).parent().find(".pause").css("display", "none");
                if (Current.DuringList[Current.CurrentMp3] == 0) {
                    playInterval = setInterval(function () {
                        Current.DuringList[Current.CurrentMp3] += 0.1;
                    }, 100);
                } else {
                    playInterval = null;
                }
            }
        }
    };
    //下一个mp3
    this.NextMp3 = function (autoNext) {
        Current.CurrentMp3 = Current.CurrentMp3 + 1;
        var mp3url = Current.Mp3List[Current.CurrentMp3];
        Current.Stop();
        Current.PlayerObj.jPlayer("setMedia", { mp3: mp3url });
        if (autoNext) {
            Current.Play();
        }
    };
    this.End = function () {
        if (Current.Module == 1) {
            Current.Record();
        } else {
            if (Current.Mp3List.length == 1 || Current.CurrentMp3 == (Current.Mp3List.length - 1)) {
                Current.Stop();
            }
            else if (Current.Mp3List.length > 1) {
                Current.NextMp3(Current.Defaults.AutoNext);
            }
            if (Current.Defaults.EndFun) {
                Current.Defaults.EndFun(Current.ArrayIndex);
            }
        }
    };

    this.Mp3Again = function () {
        //Current.CurrentMp3 = 0;
        var mp3url = Current.Mp3List[Current.CurrentMp3];
        Current.PlayerObj.jPlayer("setMedia", { mp3: mp3url });
        if (Current.Defaults.autoNext) {
            Current.Play();
        }
    };
    //从上一句开始播放——20151202钟伟鹏新增
    this.BackPlay = function (isLast) {
        if (Current.CurrentMp3 > 0 && !isLast) {
            Current.CurrentMp3 = Current.CurrentMp3 - 1;
        }
        var mp3url = Current.Mp3List[Current.CurrentMp3];
        Current.PlayerObj.jPlayer("setMedia", { mp3: mp3url });
    };

    this.InitPlayer();
    return Current;
}

$(function () {
    $(".Mp3Player").KingsunMp3Player();
});