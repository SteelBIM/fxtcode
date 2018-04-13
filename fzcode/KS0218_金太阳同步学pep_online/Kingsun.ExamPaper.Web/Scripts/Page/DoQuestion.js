/// <reference path="../Common.js" />
/// <reference path="../Plugins/jplayer/jquery.jplayer.min.js" />
/// <reference path="../Plugins/KingsunMp3Player.js" />
/// <reference path="../Client.js" />
/// <reference path="../../App_Themes/js/artDialog/artDialog.js" />
//学生做作业页面js

var AccessType = parseInt(Common.QueryString.GetValue("AccessType"));
var StuTaskID;
var TaskID;
var QuestionID;
var PageIndex = 1;
var TaskState = 0;
var TaskQuestionList = [];
var QIndex = -1;
var Mp3Obj = null, BackMp3Obj = null;
var submitDialog, helpDialog;
var autoScrollHeight = 0;
var catalogId = 0;
var stuAnswerList = [];
var type = Common.QueryString.GetValue("type");

var classTaskID = Common.QueryString.GetValue("ClassTaskID");//是否来自课时
var classPackName = unescape(Common.QueryString.GetValue("TaskName"));//课时标题
var backurl = escape(Common.QueryString.GetValue("backurl"));//记录返回到优教学课时的返回路径
var Subject = parseInt(Common.QueryString.GetValue("Subject"));
var IsDoubleScreen = 0;
var Csstype = Common.QueryString.GetValue("Csstype");//判断是否来自云平台
var hasSubmit = false;

$(function () {
    $(".mp3Cont em.num").html("");
    autoSetPosition(1, IsDoubleScreen);
    //   switch (AccessType)   hlw修改
    switch (3) {
        case 1:
        case 2:
            GetPreviewQuestions();
            break;

        case 3:
            GetStuTaskQuestions();
            break;
        case 4:
            GetStuWrongQuestions();
            break;

        case 5:
            GetWrongQueDo();
            break;
        case 6:
            //GetPreviewQuestions();
            break;
        default:
            break;
    }

    $("#divrecord").KingsunRecord({
        swfPath: "../Scripts/Plugins/KingRecord/YunzhishengForWeb.swf",
        Text: "",//hlw       
        RecordEnd: function (value) {
            self.frames[0].EndRecord(value);
        }
    });
    if (Common.Validate.IsInt(Common.QueryString.GetValue("PageIndex"))) {
        PageIndex = parseInt(Common.QueryString.GetValue("PageIndex"));
    }
    if (Common.Validate.IsInt(Common.QueryString.GetValue("TaskState"))) {
        TaskState = parseInt(Common.QueryString.GetValue("TaskState"));
    }
    $(window).resize(function () {
        autoSetPosition(0, IsDoubleScreen);
    });
});
//预览——获取指定题目信息
function GetPreviewQuestions() {
    if (Common.QueryString.GetValue("QuestionID")) {
        var qRound = Common.QueryString.GetValue("Round");
        QuestionID = Common.QueryString.GetValue("QuestionID");
        $.post("?action=GetPreviewQuestions&Rand=" + Math.random(), { QuestionID: QuestionID }, function (result) {
            TaskQuestionList = [];
            if (result) {
                result = eval("(" + result + ")");//JSON.parse
                if (result.Success) {
                    $.each(result.Data, function (index, value) {
                        var tmpurl = value.QuestionModel + ".aspx?AccessType=" + AccessType + "&QuestionID=" + value.QuestionID
                            + "&Round=" + qRound + "&QIndex=" + index + "&Sub=" + value.QuestionModel[0];
                        TaskQuestionList.push({ Url: tmpurl, QuestionTitle: value.QuestionTitle, Round: qRound, QuestionModel: value.QuestionModel });
                    });
                    QIndex = 0;
                    //默认到第一个未做的题目
                    loadQuestion(QIndex);
                } else {
                    alert(result.Message);
                    backList();
                }
            } else {
                alert("未获取到信息哦！");
                backList();
            }
        });
    }
}
//做作业——获取学生作业题目
function GetStuTaskQuestions() {
    if (Common.QueryString.GetValue("CatalogId")) {
        catalogId = Common.QueryString.GetValue("CatalogId");

        $.post("?action=GetStuTaskQuestions&Rand=" + Math.random(), { catalogId: catalogId }, function (result) {
            TaskQuestionList = [];
            if (result) {
                result = eval("(" + result + ")");//JSON.parse
                if (result.Success) {
                    $.each(result.Data, function (index, value) {
                        if (value.IsDo == 0 && QIndex < 0) {
                            QIndex = index;
                        }
                        var tmpurl = value.QuestionModel + ".aspx?AccessType=" + AccessType + "&StuTaskID=" + value.StuTaskID + "&IsDo=" + value.IsDo
                                + "&QuestionID=" + value.QuestionID + "&Round=" + value.Round + "&QIndex=" + index + "&Sub=" + value.QuestionModel[0] + "&ParentID=" + value.ParentID + "&CatalogId=" + catalogId + "&type=" + type;
                        //var tmpurl = value.QuestionModel + ".aspx?AccessType=" + 3 + "&IsDo=" + value.IsDo
                        //       + "&QuestionID=" + value.QuestionID + "&Round=" + value.Round + "&QIndex=" + index +  "&ParentID=" + value.ParentID;
                        TaskQuestionList.push({ Url: tmpurl, QuestionTitle: value.QuestionTitle, Round: value.Round, QuestionModel: value.QuestionModel });
                    });
                    if (QIndex < 0) {
                        //  QIndex = TaskQuestionList.length - 1; hlw
                        QIndex = 0;
                    }
                    //默认到第一个未做的题目
                    loadQuestion(QIndex);
                } else {
                    alert(result.Message);
                    backList();
                }
            } else {
                alert("未获取到信息哦！");
                backList();
            }
        });
    }
}
//错题重做——获取学生作业错题
function GetStuWrongQuestions() {
    if (Common.QueryString.GetValue("StuTaskID")) {
        StuTaskID = Common.QueryString.GetValue("StuTaskID");
        TaskID = Common.QueryString.GetValue("TaskID");
        $.post("?action=GetStuWrongQuestions&Rand=" + Math.random(), { StuTaskID: StuTaskID, TaskID: TaskID }, function (result) {
            TaskQuestionList = [];
            if (result) {
                result = eval("(" + result + ")");//JSON.parse
                if (result.Success) {
                    var tempParentID = '';
                    $.each(result.Data, function (index, value) {
                        var tmpurl = '';
                        if (value.QuestionModel == "M18" && value.ParentID != '' && value.ParentID != null) {
                            if (value.ParentID != tempParentID) {
                                tempParentID = value.ParentID;
                                tmpurl = value.QuestionModel + ".aspx?AccessType=" + AccessType + "&StuTaskID=" + value.StuTaskID + "&IsDo=" + value.IsDo
                                            + "&QuestionID=" + value.ParentID + "&Round=" + value.Round + "&QIndex=" + index + "&Sub=" + value.QuestionModel[0] + "&ParentID=" + value.ParentID;
                                TaskQuestionList.push({ Url: tmpurl, QuestionTitle: value.QuestionTitle, Round: value.Round, QuestionModel: value.QuestionModel });
                            }
                        } else {
                            tmpurl = value.QuestionModel + ".aspx?AccessType=" + AccessType + "&StuTaskID=" + value.StuTaskID + "&IsDo=" + value.IsDo
                                    + "&QuestionID=" + value.QuestionID + "&Round=" + value.Round + "&QIndex=" + index + "&Sub=" + value.QuestionModel[0] + "&ParentID=" + value.ParentID;
                            TaskQuestionList.push({ Url: tmpurl, QuestionTitle: value.QuestionTitle, Round: value.Round, QuestionModel: value.QuestionModel });
                        }
                    });
                    QIndex = 0;
                    //默认到第一个未做的题目
                    loadQuestion(QIndex);
                } else {
                    alert(result.Message);
                    backList();
                }
            } else {
                alert("未获取到信息哦！");
                backList();
            }
        });
    }
}

//错题集做题，获取题目  AccessType=5
function GetWrongQueDo() {
    if (Common.QueryString.GetValue("UnitID") && Common.QueryString.GetValue("QuestionID")) {
        var UnitID = Common.QueryString.GetValue("UnitID");
        var QuestionID = (Common.QueryString.GetValue("QuestionID") == "undefined" ? "" : Common.QueryString.GetValue("QuestionID"));
        $.post("?action=GetWrongQueDo&Rand=" + Math.random(), { QuestionID: QuestionID, UnitID: UnitID }, function (result) {
            TaskQuestionList = [];
            if (result) {
                result = eval("(" + result + ")");//JSON.parse
                if (result.Success) {
                    var tempParentID = '';
                    $.each(result.Data, function (index, value) {
                        //if (value.QuestionModel == "M18" && value.ParentID != '') {
                        //    if (value.ParentID != tempParentID) {
                        //        tempParentID = value.ParentID;
                        //        tmpurl = value.QuestionModel + ".aspx?AccessType=" + AccessType + "&StuTaskID=" + value.StuTaskID + "&IsDo=" + value.IsDo
                        //                    + "&QuestionID=" + value.ParentID + "&Round=" + value.Round + "&QIndex=" + index + "&Sub=" + value.QuestionModel[0];
                        //        TaskQuestionList.push({ Url: tmpurl, QuestionTitle: value.QuestionTitle, Round: value.Round, QuestionModel: value.QuestionModel });
                        //    }
                        //} else {
                        tmpurl = value.QuestionModel + ".aspx?AccessType=" + AccessType + "&StuTaskID=" + value.StuTaskID + "&IsDo=" + value.IsDo
                                + "&QuestionID=" + value.QuestionID + "&Round=" + value.Round + "&QIndex=" + index + "&Sub=" + value.QuestionModel[0];
                        TaskQuestionList.push({ Url: tmpurl, QuestionTitle: value.QuestionTitle, Round: value.Round, QuestionModel: value.QuestionModel });
                        //}
                    });
                    QIndex = 0;
                    //默认到第一个未做的题目
                    loadQuestion(QIndex);
                } else {
                    alert(result.Message);
                    backList();
                }
            } else {
                alert("未获取到信息哦！");
                backList();
            }
        });
    }
}



//返回上一页
function backList() {
    window.location.href = "StuTaskList.aspx";
    //if (classTaskID != "" && classTaskID != "undefined") {
    //    location.href = "../Others/TaskArrange.aspx?ClassTaskID=" + classTaskID + "&TaskName=" + escape(classPackName) + "&backurl=" + backurl;
    //} else {
    //    switch (AccessType) {
    //        case 1://预览：返回布置作业
    //            location.href = "../Teacher/TaskArrange.aspx" + (Csstype != "undefined" ? "?Csstype=cloudHeader" : "");
    //            break;
    //        case 2://预览：返回作业篮
    //            location.href = "../Teacher/TaskBasket.aspx" + (Csstype != "undefined" ? "?Csstype=cloudHeader" : "");
    //            break;
    //        case 3://做作业：返回任务列表
    //            location.href = "../Student/StuTaskList.aspx?PageIndex=" + PageIndex + "&TaskState=" + TaskState + "&Subject=" + Subject;
    //            break;
    //        case 4://错题重做：返回作业报告
    //            location.href = "../Student/StuReport.aspx?StuTaskID=" + StuTaskID + "&PageIndex=" + PageIndex + "&TaskState=" + TaskState + "&Subject=" + Subject;
    //            break;
    //        case 5://错题集：返回返回错题集
    //            location.href = "../Student/StuWrongQue.aspx?dBid=" + Common.QueryString.GetValue("dBid") + "&UnitID="
    //                            + Common.QueryString.GetValue("UnitID") + "&SubID=" + Common.QueryString.GetValue("SubID");
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
//打开跟读题帮助
function openHelpDialog() {
    //切换时停止播放
    if (Mp3Obj) {
        Mp3Obj.Stop();
    }
    //暂停录音回放
    if ($("#aSmall").hasClass("on")) {
        BackMp3Obj.jPlayer("stop");
        $("#aSmall").removeClass("on");
    }

    var dialogHtml = '<div class="tipCont" id="helpDiv"><em style="background: url(../App_Themes/images/close_bg.png) center center no-repeat; position: absolute; cursor: pointer; right: 0px; width: 30px; height: 30px; margin-top: -10px; margin-right: -10px;" onclick="closeDialog(helpDialog)"></em>'
        + '<dl>'
            + '<dt>录音流程：</dt>'
            + '<dd><span>点击</span><em class="em1">&nbsp;</em><span>开始跟读</span><em class="em">&nbsp;</em><span>原音播放</span><em class="em">&nbsp;</em><span>自动开始录音</span><em class="em">&nbsp;</em><span>录音结束显示本次录音得分；</span></dd>'
            + '<dt>温馨提示：</dt>'
            + '<dd><span>原音播放和录音过程可点击</span><em class="em2">&nbsp;</em><span>按钮暂停；</span></dd>'
            + '<dd><span>录音结束后可点击</span><em class="em3">&nbsp;</em><span>听自己的声音；</span></dd>'
            + '<dd><span>读完规定次数后，还可以继续跟读哦，最终成绩取最高分；</span></dd>'
            + '<dt><span>注意：</span></dt>'
            + '<dd><span>一定要读完规定次数哦，若没有完成，不会有最终成绩。</span></dd>'
            + '<dt><span>若不小心拒绝了麦克风，按F5键刷新点“允许”哦！</span></dt>'
        + '</dl>'
    + '</div>';
    helpDialog = art.dialog({
        id: 'IsHelp',
        opacity: .1,
        padding: 0,
        lock: true,
        content: dialogHtml
    });
    $(".aui_close").hide();//隐藏弹窗的关闭按钮
}
//提交作业
function submitTask(autoNext,again) {
    //切换时停止播放
    if (Mp3Obj) {
        Mp3Obj.Stop();
    }
    if (BackMp3Obj) {
        if ($("#aSmall").hasClass("on")) {
            BackMp3Obj.jPlayer("stop");
            $("#aSmall").removeClass("on");
        }
    }

    $.post("/SpokenTest/SubmitAnswer", { queList: JSON.stringify(stuAnswerList) }, function (data) {
        if (data == null || data.length == 0) {
            alert("提交成绩出错,请重试");
            return;
        }
        if (autoNext) {//从服务端获取下一节内容            
            window.location.href = data;
        } else if (again) {//再做一次
            window.location.reload();
        } else {
            alert("太棒了,提交成功!");
            //$(".returnA").click();//返回目录页
        }
    });
    hasSubmit = true;//只要有提交操作就视为提交
}

function CheckSubmit() {
    if (hasSubmit)
        return true;
    else
        return confirm("当前模块尚未完成,确定要离开吗?");
}

function closeDialog(dialogObj) {
    dialogObj.close();
}

//关闭提交作业询问窗口
function closeSubmitDialog(isSubmit) {
    closeDialog(submitDialog);
    if (isSubmit == 1) {
        confirmSubmitTask();
    } else {
        history.go(0);
    }
}
//确认提交作业
function confirmSubmitTask() {
    var dialog = art.dialog({
        id: 'loading',
        opacity: .1,
        padding: '0',
        lock: true,
        content: '<img style="width:305px;height:304px;align:center" src="../App_Themes/images/Loading.gif" />'
    });
    $(".aui_close").hide();//隐藏弹窗的关闭按钮
    $.post("?action=SubmitTask&Rand=" + Math.random(), { StuTaskID: StuTaskID }, function (result) {
        TaskQuestionList = [];
        if (result) {
            result = eval("(" + result + ")");//JSON.parse
            if (result.Success) {
                if (result.Data != "") {
                    alert(result.Data);
                    location.replace("../Student/StuTaskList.aspx?StuTaskID=" + StuTaskID + "&PageIndex=" + PageIndex + "&TaskState=" + TaskState + "&Subject=" + Subject);
                } else {
                    if (confirm("提交成功！是否查看作业报告？")) {
                        location.replace("../Student/StuReport.aspx?StuTaskID=" + StuTaskID + "&PageIndex=" + PageIndex + "&TaskState=" + TaskState + "&Subject=" + Subject);
                    } else {
                        location.replace("../Student/StuTaskList.aspx?StuTaskID=" + StuTaskID + "&PageIndex=" + PageIndex + "&TaskState=" + "&Subject=" + Subject);
                    }
                }
            } else {
                alert(result.Message);
                dialog.close();
            }
        }
    });
}
//上一题
function prevQuestion() {
    $("#prevA").hide();
    if (QIndex > 0) {
        QIndex--;
        loadQuestion(QIndex);
    }
}
//下一题
function nextQuestion() {
    $(".yu").html("");
    $("#nextA").hide();
    if (type == 'story') {
        $($(".btmC")[0]).css("display", "none");
        $($(".btmC")[1]).css("display", "block");
        $(".accomplishment").css("display", "none");
        $("#iframe1").attr("src", "Result.aspx");
        return;
    }
    if (QIndex < TaskQuestionList.length - 1) {
        QIndex++;
        loadQuestion(QIndex);
    } else {
        $($(".btmC")[0]).css("display", "none");
        $($(".btmC")[1]).css("display", "block");
        $(".accomplishment").css("display", "none");
        $("#iframe1").attr("src", "Result.aspx");
    }
}
//加载题目
function loadQuestion(qindex) {
    //做作业时检测作业是否被老师撤销
    if (AccessType == 3) {
        $.post("?action=CheckIsUndo&Rand=" + Math.random(), { StuTaskID: StuTaskID }, function (result) {
            if (result) {
                result = eval("(" + result + ")");//JSON.parse
                if (result.Success) {
                    loadQ(qindex);
                } else {
                    alert(result.Message);
                    location.href = "../Student/StuTaskList.aspx?PageIndex=" + PageIndex + "&TaskState=" + TaskState + "&Subject=" + Subject;
                }
            }
        });
    } else {
        loadQ(qindex);
    }
}
function loadQ(qindex) {
    $(".solution").hide();
    if (TaskQuestionList[qindex].QuestionModel == "M1" || TaskQuestionList[qindex].QuestionModel == "M2") {
        $("#helpA").show();
    } else {
        $("#helpA").hide();
    }
    $(".titBox").hide();
    $(".titCont").hide();
    $(".mp3Cont em.num").html("");
    //切换时停止播放
    if (Mp3Obj) {
        Mp3Obj.Stop();
    }
    //切换题目时移除回放按钮
    InitBackPlay("");

    if (TaskQuestionList[qindex].QuestionTitle[0] == "*") {
        TaskQuestionList[qindex].QuestionTitle = TaskQuestionList[qindex].QuestionTitle.substring(1);
    }
    var tmpTitle = TaskQuestionList[qindex].QuestionTitle;
    if (TaskQuestionList[qindex].QuestionModel == "M1" || TaskQuestionList[qindex].QuestionModel == "M2") {
        if (tmpTitle.length > 32) {
            tmpTitle = tmpTitle.substring(0, 29) + "...";
        }
        //  tmpTitle += "<b>听读" + TaskQuestionList[qindex].Round + "遍</b>" ; hlw

    } else {
        if (tmpTitle.length > 36) {
            tmpTitle = tmpTitle.substring(0, 33) + "...";
        }
    }

    // $("#topicTitle").html(tmpTitle).attr("title", tmpTitle);
    $(".accomplishment").html((qindex + 1) + "/" + TaskQuestionList.length);
    if (type == 'story') {
        $(".accomplishment").css("display", "none");
    }
    $("#iframe1").attr("src", TaskQuestionList[qindex].Url);

    if (TaskQuestionList.length == 1) {
        if (AccessType == 3) {
            $("#submitA").show();
        }
        $("#prevA").hide();
        $("#nextA").hide();
    } else {
        if (qindex == 0) {
            $("#prevA").hide();
            //  $("#nextA").show();
        } else if (qindex == TaskQuestionList.length - 1) {

            $("#submitA").show();

            $("#prevA").show();
            $("#nextA").hide();
        } else {
            $("#prevA").show();
            //$(stuAnswerList).each(function (index, value) {
            //    if (this.QuestionID == TaskQuestionList[qindex].QuestionID) {//本次有做题记录显示下一题按钮
            //        $("#nextA").show();
            //        return false;
            //    }
            //})            
            //$("#nextA").show();
        }
    }
}
function showNextA() {
    $("#nextA").show();
}

function showSubmit() {
    $("#submitA").show();
}
//自适应屏幕大小
function autoSetPosition(isInit, isDoubleScreen) {
    IsDoubleScreen = isDoubleScreen;
    var windowHeight = $(window).height();
    var iframeHeight = document.getElementById('iframe1').contentWindow.document.documentElement.scrollHeight;
    var contCHeight = iframeHeight + 40;//音频控件多出的高度：40px
    var boxHeight = contCHeight + 154; //上、下栏高度共：154px;
    boxHeight = boxHeight > 600 ? boxHeight : 600;//最小高度为600px
    var setYH = 0;//上、下间隔
    if (windowHeight < boxHeight) {
        setYH = 10;
    } else {
        setYH = (windowHeight - boxHeight) / 2
    }
    autoScrollHeight = 66 + setYH / 2;

    if (isInit == 1) {//初次加载(没有内容)时，固定iframe最小高度:426px (620-154-40)
        $(document.getElementById('iframe1')).css("height", "426px");
        $('.box').css({ "position": "absolute", "top": setYH + "px" });
    }
    else {
        if (windowHeight <= 620) {//页面高度不足620（600+10+10）时
            iframeHeight = 426;
        } else {
            if (windowHeight < boxHeight) {//做题框的高度大于页面高度时，iframe的高度：页面高度-154-40-10*2
                iframeHeight = windowHeight - 214;
            } else if (iframeHeight < 426) {
                iframeHeight = 426;
            }
        }

        if (isDoubleScreen == 0) {//一屏
            $(document.getElementById('iframe1')).css("height", iframeHeight + "px");
            $('.M', document.getElementById('iframe1').contentWindow.document).css("height", iframeHeight + "px");
            $('.box').css({ "position": "absolute", "top": setYH + "px" });
        } else {//上下分屏
            var stemHeight = $('.M .stem', document.getElementById('iframe1').contentWindow.document).height();
            var problemHeight = $('.M .problem', document.getElementById('iframe1').contentWindow.document).height();
            if (stemHeight > (iframeHeight - 10) / 2) {//上屏高度大于整体的一半时，固定高度
                if (problemHeight > (iframeHeight - 10) / 2) {
                    stemHeight = (iframeHeight - 10) / 2;
                    problemHeight = (iframeHeight - 10) / 2;
                } else {
                    stemHeight = iframeHeight - 10 - problemHeight;
                }
            } else {
                problemHeight = iframeHeight - 10 - stemHeight;
            }

            $(document.getElementById('iframe1')).css("height", iframeHeight + "px");
            $('.M .stem', document.getElementById('iframe1').contentWindow.document).css("height", stemHeight + "px");
            $('.M .problem', document.getElementById('iframe1').contentWindow.document).css("height", problemHeight + "px");
            $('.box').css({ "position": "absolute", "top": setYH + "px" });
        }
    }
}

//录音题音频加载
function InitRecord(Mp3Url, QContent, IsMicro) {
    //检测QQ浏览器是否为极速模式
    if (Client.ua.indexOf("QQ") >= 0 && Client.browser.chrome > 0&&false) {//false,现qq浏览器不需要任何处理了
        art.dialog({
            id: 'QQDetect',
            opacity: .1,
            width: 300,
            lock: true,
            cancelVal: '取消',
            cancel: function () {
                backList();
            },
            content: '<span style="font-size:20px;color:red;"><b>请先切换为“兼容模式”哦！</b></span><img style="align:center" src="../App_Themes/images/QQBrowserConfig.png" />'
        });
        $(".aui_close").hide();//隐藏弹窗的关闭按钮
    } else {

        $("#divrecord").css({ "z-index": "9999", top: "0px", left: "25%" });
        $("#record1").css({ width: "230px", height: "140px" });

        KSRecord.KingRecord = swfobject.getObjectById("record1");
        var tmpInterval = setInterval(function () {
            try {
                if (KSRecord.MicroReady()) {
                    $(".li1").show();
                    clearInterval(tmpInterval);
                }
            } catch (e) {

            }
        }, 1000);

        //检测页面是否已加载播放控件
        if (Mp3Obj) {
            Mp3Obj.jPlayer("destroy");
            $(".li1").html('<div class="mp3player" id="mp3player"></div>');
        }
        //初始化音频播放
        Mp3Obj = $("#mp3player").KingsunMp3Player({
            swfPath: "../Scripts/Plugins/jplayer/",
            mp3: Mp3Url,
            content: QContent,
            module: 1,
            ksRecord: KSRecord,
            AutoNext: false,
            OnlyOne: true,
            StartRecord: function (cIndex) {
                $(".titBox").hide();
                //$(".titCont").hide();
                InitBackPlay("");
                if (self.frames[0].hideScore) {
                    self.frames[0].hideScore(cIndex);
                }
                if (self.frames[0].showOn) {
                    self.frames[0].showOn();
                }
            },
            StartProgress: function () {
                $(".progressBar span").attr("style", "width:0");
                $(".ongoing").show();
            },
            UpdateProgress: function (progress) {
                $(".progressBar span").attr("style", "width:" + progress + "%");
            },
            EndProgress: function () {
                $(".ongoing").hide();
                $(".progressBar span").attr("style", "width:0");
            },
            IsClickMicro: IsMicro
        });
        if (Mp3Url) {

        } else {
            alert("此题没有声音哦");
        }
    }
}
//听力题音频加载
function InitPlay(Mp3Url) {
    $("#divrecord").css({ "z-index": "9999", top: "200px", left: "200px" });
    $("#record1").css({ width: "200px", height: "200px" });
    $(".li1").show();
    //检测页面是否已加载播放控件
    if (Mp3Obj) {
        Mp3Obj.jPlayer("destroy");
        $(".li1").html('<div class="mp3player" id="mp3player"></div>');
    }
    //初始化音频播放
    Mp3Obj = $("#mp3player").KingsunMp3Player({
        swfPath: "../Scripts/Plugins/jplayer/",
        mp3: Mp3Url,
        module: 0,
        OnlyOne: true,
        EndFun: function () {
            if (self.frames[0].autoFocus) {
                self.frames[0].autoFocus();
            }
        }
    });
}
//加载音频回放按钮
function InitBackPlay(Mp3Url) {
    $(".li2").show();
    if (Mp3Url) {
        var tmpBackUrl = Mp3Url.split(";");
        var backindex = 0;
        if (BackMp3Obj) {
            //移除绑定的时间以及清除录音回放控件
            Common.removeHandler(document.getElementById("aSmall"), "click", backClickHandler);
            BackMp3Obj.jPlayer("destroy");
        }
        //初始化回放按钮
        BackMp3Obj = $("#backplayer").jPlayer({
            ready: function () {
                $(this).jPlayer("setMedia", {
                    mp3: tmpBackUrl[backindex]
                });
            },
            ended: function () {
                if (backindex < tmpBackUrl.length - 1) {
                    backindex = backindex + 1;
                    $(this).jPlayer("setMedia", {
                        mp3: tmpBackUrl[backindex]
                    }).jPlayer("play");
                }
                else {
                    backindex = 0;
                    $(this).jPlayer("setMedia", {
                        mp3: tmpBackUrl[backindex]
                    });
                    $("#aSmall").removeClass("on");
                }
            },
            supplied: "mp3",
            wmode: "window",
            swfPath: "../Scripts/Plugins/jplayer/"
        });
        //回放按钮点击事件
        Common.addHandler(document.getElementById("aSmall"), "click", backClickHandler);
        //$(".li2").show();
        $("#aSmall").attr("class", "");
    } else {
        if (BackMp3Obj) {
            //移除绑定的时间以及清除录音回放控件
            if ($("#aSmall").hasClass("on")) {
                BackMp3Obj.jPlayer("stop");
                $("#aSmall").removeClass("on");
            }
            Common.removeHandler(document.getElementById("aSmall"), "click", backClickHandler);
            BackMp3Obj.jPlayer("destroy");
        }
        $(".li2").hide();
    }
}
//回放按钮点击事件句柄
function backClickHandler() {
    if ($("#aSmall").hasClass("on")) {
        BackMp3Obj.jPlayer("stop");
        $("#aSmall").removeClass("on");
    }
    else {
        //检测是否播放原音
        $("#aSmall").addClass("on");
        BackMp3Obj.jPlayer("play");
    }
}

function ShowResultWithoutResult() {
    ShowScore(0, 1, 1);
    $(".titBox").show();
    $(".titCont").show();
    $("#aSmall").attr("class", "loading");
    $(".li2").show();
    $(".pause").change();
    InitBackPlay("");
}
//显示跟读结果
function ShowResult(score, readRound, requireRound, backUrl) {
    //if (score == 0) {
    //    $(".titBox").attr("class", "titBox tbx4");
    //  //  $(".titBox span").html("没有声音哦~");
    //} else if (score < 60) {
    //    $(".titBox").attr("class", "titBox tbx4");
    //   // $(".titBox span").html("不要灰心哦~");
    //} else if (score < 80) {
    //    $(".titBox").attr("class", "titBox tbx2");
    //   // $(".titBox span").html(score + "分");
    //} else if (score < 100) {
    //    $(".titBox").attr("class", "titBox tbx3");
    //   // $(".titBox span").html(score + "分");
    //} else {
    //    $(".titBox").attr("class", "titBox tbx1");
    //   // $(".titBox span").html(score + "分");
    //}

    //if (readRound < requireRound) {
    //    $(".titCont").html("<em></em><span><p>还差<b>" + (requireRound - readRound) + "</b>遍</p><p>加油哦！</p></span>");
    //    $(".mp3Cont em.num").html(readRound);
    //} else {
    //    $(".titCont").html("<em></em><span><p>完成啦！</p></span>");
    //    $(".mp3Cont em.num").html("√");
    //}
    ShowScore(score, readRound, requireRound);
    $(".titBox").show();
    $(".titCont").show();
    $("#aSmall").attr("class", "loading");
    $(".li2").show();
    showNextA();
    InitBackPlay(backUrl);
}
//错题模式下显示正确答案
//answerHtml格式：<span class="w">(1)book</span><span class="r">(2)book</span><span class="w">(3)book</span>
function ShowAnswer(isRight, answerHtml) {
    //切换时停止播放
    if (Mp3Obj) {
        Mp3Obj.Stop();
    }
    //切换题目时移除回放按钮
    InitBackPlay("");
    var aHtml = '<div class="conts ' + (isRight ? 'good' : 'bad') + '"><div class="boxS"><table><tr><td><em></em></td><td>';
    if (isRight) {
        aHtml += '<span>恭喜你，答对了！</span>';
    } else {
        aHtml += '<span class="first">正确答案:</span>' + answerHtml;
    }
    aHtml += '</td></tr></table></div></div>';
    $(".solution").html(aHtml);
    $(".li1").hide();
    $(".solution").show();
}

function SetStars(star) {//完成模块后设置左边菜单对应的星星数
    $("#menu .on .xing").css("background-image", "url(/App_Themes/images/star" + star.toString() + ".png");
}

function HidePlay() {
    $(".li1").hide();
    $(".li2").hide();
    $("#divrecord").css({ "z-index": "-1", top: "0px", left: "0px" });
    $("#record1").css({ width: "1px", height: "1px" });
}

//显示跟读结果
function ShowScore(score, readRound, requireRound) {
    $(".titBox span").html("");
    if (score < 40) {
        $(".titBox").attr("class", "titBox tbx1");
        $("#audio").attr("src","sounds/1.mp3");
        //$(".titBox span").html("没有声音哦~");
    } else if (40 <= score && score < 60) {
        $(".titBox").attr("class", "titBox tbx2");
        $("#audio").attr("src", "sounds/2.mp3");
        //  $(".titBox span").html("不要灰心哦~");
    } else if (60 <= score && score < 80) {
        $(".titBox").attr("class", "titBox tbx3");
        $("#audio").attr("src", "sounds/3.mp3");
        //$(".titBox span").html(score + "分");
    } else if (80 <= score && score < 90) {
        $(".titBox").attr("class", "titBox tbx4");
        $("#audio").attr("src", "sounds/4.mp3");
        // $(".titBox span").html(score + "分");
    } else {
        $(".titBox").attr("class", "titBox tbx5");
        $("#audio").attr("src", "sounds/5.mp3");
        //  $(".titBox span").html(score + "分");
    }
    $(".titCont").html("<em></em><span><p>完成啦！</p></span>");
    // $(".mp3Cont em.num").html("√");
    $(".yu").html("");
    var yu = 0;
    yu = YuCount(score);
    for (var j = 0; j < yu; j++) {
        $(".yu").append("<img src='/App_Themes/images/yu.png' >");
    }

    $(".titBox").show();
    $(".titCont").show();
    $("#aSmall").attr("class", "loading");
    $(".li2").show();
}
//计算小鱼数
function YuCount(score) {
    var yu = 0;
    if (score < 40) {
        yu = 1;
    } else if (40 <= score && score < 60) {
        yu = 2;
    } else if (60 <= score && score < 80) {
        yu = 3;
    } else if (80 <= score && score < 90) {
        yu = 4;
    } else if (score >= 90) {
        yu = 5;
    }
    return yu;
}
//显示跟读结果
function ShowBackPlayer(backUrl) {
    $("#aSmall").attr("class", "");
    InitBackPlay(backUrl);
}