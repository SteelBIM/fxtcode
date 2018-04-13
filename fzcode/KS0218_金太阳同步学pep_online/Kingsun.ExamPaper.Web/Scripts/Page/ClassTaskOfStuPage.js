//错误学生列表页面和学生报告页面

var TaskTitle = "";
var ClassName = "";
var TaskID = "";
//页头
var headHtml = "";
//学生作业详情
var StuTaskHtml = "";
var obj;
var mp3Obj = new Array();//存储各个跟读课文大题的音频集合
var longMp3Cnt = 0;//课文索引
var cnt = new Array();//存储各个跟读课文大题对应的循环播放小题的计数
var loadDialog;
var stuID = '';
var Csstype = Common.QueryString.GetValue("Csstype");//判断是否来自云平台
$(function () {
    //页头布置作业选中效果
    $("#aClassTaskList").addClass("on");
    Init();
});

//页面初始化方法
function Init(){

    //判断页面显示作业报告还是错误学生名单
    var IsReport = Common.QueryString.GetValue("IsReport");//1：作业报告；0：错误学生名单
    TaskTitle = unescape(Common.QueryString.GetValue("TaskTitle"));        
        
    headHtml = '<ul class="catalog">' +
            '<li><a href="ClassTaskList.aspx' + (Csstype != "undefined" ? '?Csstype=cloudHeader' : '') + '">首页</a></li>' +
            '<li><em class="emImg"></em></li>';
    ClassName = unescape(Common.QueryString.GetValue("ClassName"));
    TaskID = Common.QueryString.GetValue("TaskID");
    
    if (IsReport == "1") {
        //获取url参数
        var TrueName = unescape(Common.QueryString.GetValue("TrueName"));
        var StuID = Common.QueryString.GetValue("StuID");
        var StuTaskID = Common.QueryString.GetValue("StuTaskID");
        //TaskID = Common.QueryString.GetValue("TaskID");
        ShowStuReport(StuID, TrueName,StuTaskID);
    }
    else if (IsReport == "0") {
        //获取url参数
        //ClassName = unescape(Common.QueryString.GetValue("ClassName"));
        var QuestionID = Common.QueryString.GetValue("QuestionID");
        //TaskID = Common.QueryString.GetValue("TaskID");
        var QuestionModel = Common.QueryString.GetValue("QuestionModel");
        var includeLate = Common.QueryString.GetValue("IncludeLate");
        ShowStuList(TaskID, QuestionID, QuestionModel,includeLate);
    }
    
    Common.AutoPosition();
}

//作业报告页面
function ShowStuReport(StuID, TrueName, StuTaskID) {   
    document.title = "优作业——学生成绩详情";
    //页头(学生姓名)
    headHtml += '<li><a href="ClassTaskDetail.aspx?TaskID=' + TaskID + "&ClassName=" + escape(ClassName) + (Csstype != "undefined" ? '&Csstype=cloudHeader' : '') + '">' + TaskTitle + '</a></li>' +
                '<li><em class="emImg"></em></li>' +
                '<li><a href="javascript:void(0)">' + TrueName + '</a></li></ul>';
    $(".headH").html(headHtml);

    //学生姓名div隐藏
    document.getElementById("stulist").style.display = "none";

    //展示学生报告
    GetStuReport(StuID, StuTaskID);
    Common.AutoPosition();
}

//错误学生名单页面
function ShowStuList(TaskID, QuestionID, QuestionModel, includeLate)    
{
    var ontime = 0;
    if (includeLate == 0)
    {
        ontime = 1;
    }
    document.title = "优作业——错误学生名单";
    //页头(错点分析+错误学生名单)
    headHtml += '<li><a href="ClassTaskDetail.aspx?TaskID=' + TaskID + "&ClassName=" + escape(ClassName) + (Csstype != "undefined" ? '&Csstype=cloudHeader' : '') + '">' + TaskTitle + '</a></li>' +
                '<li><em class="emImg"></em></li>'+
                '<li><a href="AnalysisWrong.aspx?TaskID=' + TaskID + "&TaskTitle=" + escape(TaskTitle) + "&ClassName=" + escape(ClassName)
                + "&OnTime=" + ontime + (Csstype != "undefined" ? '&Csstype=cloudHeader' : '') + '">错点分析</a></li>' +
                '<li><em class="emImg"></em></li>'+
                '<li><a href="javascript:void(0)">错误学生名单</a></li></ul>';
    $(".headH").html(headHtml);
    //初始化滚动条
    $(".stuUl").niceScroll({ touchbehavior: false, autohidemode: false, cursorcolor: "#fbc579", cursoropacitymax: 1, cursorwidth: 10 });

    //学生报告隐藏
    document.getElementById("report").style.display = "none";
    document.getElementById("reportlist").style.display = "none";

    var stuListHtml = '';
    //展示错题学生姓名列表
    obj = { TaskID: TaskID, QuestionID: QuestionID, QuestionModel: QuestionModel, IncludeLate: includeLate }
    $.post("?action=GetStuList", obj, function (data) {
        data = eval("(" + data + ")");
        if (data.Success) {
            for (var i = 0; i < data.Data.length; i++) {
                stuListHtml += '<li><a href="javascript:void(0)" id="' + data.Data[i].UserID + '" onclick=StuReportHtml(' + "'" + data.Data[i].UserID + "',this,'" + data.Data[i].StuTaskID + "'" + ')>' + data.Data[i].TrueName + '</a></li>';
            }
            $(".stuUl").html(stuListHtml);
        }
        else {
            alert(data.Message);
        }
    });
}

//显示学生作业报告
function StuReportHtml(StudentID, curobj, StuTaskID) {  
    if (document.getElementById(StudentID).className == "on") {
        //再次点击姓名报告隐藏
        $(curobj).removeClass("on");
        //学生报告隐藏
        document.getElementById("report").style.display = "none";
        document.getElementById("reportlist").style.display = "none";
    }
    else {
        //点击姓名按钮状态更改及显示作业报告
        $(curobj).parent().parent().find("a").removeClass("on");
        $(curobj).addClass("on");
        GetStuReport(StudentID, StuTaskID);
    }
    Common.AutoPosition();
}

//获取学生作业报告
function GetStuReport(StudentID, StuTaskID)
{
    loadDialog = art.dialog({
        id: 'loading',
        opacity: .1,
        padding: '0',
        lock: true,
        content: '<img style="width:305px;height:304px;align:center" src="../App_Themes/images/Loading.gif" />'
    });
    $(".aui_close").hide();

    StuTaskHtml = "";
    var subjectID = 3;
    //获取学生作业详情
    obj = { StuTaskID: StuTaskID }
    $.post("?action=GetStuTask", obj, function (info) {
        info = eval("(" + info + ")");
        if (info.Success) {
            //var stutaskinfo = eval("(" + info + ")");
            subjectID = info.Data.SubjectID;
            stuID = info.Data.StudentID;
            //展示学生作业详情
            var submitDate = info.Data.SubmitDate == null ? "尚未提交作业" : info.Data.SubmitDate.substr(0, 16).replace(/-/g, ".");
            var title = info.Data.TaskTitle;
            if (title.length > 36) {
                title = title.substring(0, 32) + " ...... " + title.substring(title.length - 2, title.length);
            }
            StuTaskHtml += '<div class="hDiv"><h5 title="' + info.Data.TaskTitle + '">' + title + '</h5><span>作业提交时间：' + submitDate + '</span></div>'
                + '<ul class="ulS">'
                + '<li class="li1"><em class="emS"></em>' + (info.Data.SubmitDate == null ? "本次测试尚未提交哦~！" : "本次测试完成！") + '</li>'
            if (info.Data.SubmitDate != null) {
                StuTaskHtml += '<li class="li2">成绩：' + (info.Data.StuScore >= 0 ? ('<b>' + Math.floor(info.Data.StuScore) + '</b>') : '<em class="noneScore">&nbsp;</em>') + '</li>'
                + '<li class="li3">用时：' + Math.ceil(info.Data.SpendTime / 60) + '分钟</li>'
            }
            StuTaskHtml += '</ul>';
            document.getElementById("report").style.display = "block";
            document.getElementById("reportlist").style.display = "block";
            $(".titDiv").html(StuTaskHtml);

            if (info.Data.SubmitDate != null) {
                //获取学生作业报告
                obj = { StudentID: StudentID, StuTaskID: StuTaskID }
                $.post("?action=GetStuReport", obj, function (data) {
                    var result = eval("(" + data + ")");
                    if (result.Success) {
                        if (subjectID == 1) {
                            $(".main").addClass("chineseS");
                        }
                        else if (subjectID == 2)
                        {
                            $(".main").addClass("mathS");
                        }
                        $(".content").html(result.Data.QuestionHTML);
                        var listWidth = [], w = 0, currentParent = "";
                        $.each($(".Y2 .shell ol li"), function (index) {
                            if (index == 0) {
                                w = $(this).width();
                                currentParent = $(this).parent().attr("id");
                            }
                            if (currentParent != $(this).parent().attr("id")) {
                                listWidth.push(w);
                                w = $(this).width();
                                currentParent = $(this).parent().attr("id");
                            } else {
                                if (w < $(this).width()) {
                                    w = $(this).width();
                                }
                            }
                            if (index == $(".Y2 .shell ol li").length - 1) {
                                listWidth.push(w);
                            }

                        });
                        $.each($(".Y2 .shell ol"), function (index, value) {
                            $(value).css("width", listWidth[index] + 5);
                        });
                        $(".Y2 .shell ol li").css("display", "block");
                        var mp3 = $(".mp3player");
                        if (mp3) {
                        } else {
                            alert("没有获取到声音哦");
                        }
                        for (var i = 0; i < mp3.length; i++) {
                            if ($(mp3[i]).attr("mp3url").split(";").length > 1) {
                                longMp3Cnt++;
                                cnt[longMp3Cnt - 1] = 0;
                                var s = $(mp3[i]).attr("mp3url");
                                mp3Obj[longMp3Cnt - 1] = $(mp3[i]).KingsunMp3Player({
                                    swfPath: "../Scripts/Plugins/jplayer/",  //flash路径
                                    mp3: $(mp3[i]).attr("mp3url"),  //mp3播放路径
                                    module: $(mp3[i]).attr("module"),//模式：0-正常（大喇叭）
                                    OnlyOne: true,   //是否只允许一个播放
                                    AutoNext: true,
                                    EndFun: Recycle,
                                    ArrayIndex: longMp3Cnt - 1
                                });
                            }
                            else {
                                //初始化音频播放
                                var url = $(mp3[i]).attr("mp3url");
                                //if (url.substring(url.length - 3) == "amr") {
                                //    RongIMLib.RongIMVoice.init(mp3[i],url);
                                //} else {
                                    //初始化音频播放
                                    $(mp3[i]).KingsunMp3Player({
                                        swfPath: "../Scripts/Plugins/jplayer/",
                                        mp3: url,
                                        module: $(mp3[i]).attr("module"),//模式：0-正常（大喇叭）
                                        OnlyOne: true
                                    });
                                //}
                            }
                            $(mp3[i]).parent().find(".mp3Cont em.num").html($(mp3[i]).attr("num"));
                            $(mp3[i]).parent().find(".mp3Cont em.state").html($(mp3[i]).attr("state"));
                        }
                        $(".setScore .scoreBtn").bind("click", function () {
                            $(this).parent().find("p").toggle();
                            $(this).toggleClass("nextStyle");
                        })
                        if ($(".setScore p")) {
                            $(".setScore .sureSet").bind("click", function () {
                                $(this).parent().hide();
                                $(this).parent().parent().find(".scoreBtn").removeClass("nextStyle");
                            })
                        }
                    }
                    else {
                        alert(result.Message);
                    }
                    Common.AutoPosition();
                    loadDialog.close();
                });               
            }
            else {
                $(".content").html("");
            }
        }
        else {
            alert(info.Message);
        }        
        
    });
}

//转换时间格式
function TransferTime(time,format) {
    if (!time) {
        return "尚未提交作业";
    }
    if (format == undefined || format == "") {
        format = "yyyy.MM.dd hh:mm";
    }
    var date = new Date(parseInt(time.substring(6, time.length - 2)))
    return date.format(format);
}

//跟读课文循环一遍后回到第一个播放
function Recycle(){
    var nowMp3Obj = mp3Obj[this.ArrayIndex];
    cnt[this.ArrayIndex]++;
    if (cnt[this.ArrayIndex] == nowMp3Obj.Mp3List.length) {
        cnt[this.ArrayIndex] = 0;
        nowMp3Obj.CurrentMp3 = 0;
        var mp3url = nowMp3Obj.Mp3List[nowMp3Obj.CurrentMp3];
        nowMp3Obj.PlayerObj.jPlayer("setMedia", { mp3: mp3url })
    }
}

//修改分数
function UpdateScore(questionID, StuTaskID,parentID) {
    //获取分数
    var score = $("#txt"+questionID).val();    
    if (score == "")
    {
        alert("请输入批改的分数");
        return;
    }
    if (score < 0 || score > 100)
    {
        alert("请输入正确范围内的分数");
        return;
    }
    //上传分数
    obj = { QuestionID: questionID, ParentID:parentID,StuTaskID: StuTaskID, StuScore: score,StudentID:stuID };
    $.post("?action=UpdateScore", obj, function (data) {
        data = eval("(" + data + ")");
        if (data.Success) {
            document.getElementById('txt' + questionID).value="";
            $("#none" + questionID).attr("class", "scoreBox");
            $("#none" + questionID).attr("id", "score" + questionID);
            $("#score" + questionID).html(score);
            if (parentID != "")
            {
                if (data.Data.QScore != -1)
                {
                    $("#none" + parentID).attr("class", "scoreBox");
                    $("#none" + parentID).attr("id", "score" + parentID);
                    $("#score" + parentID).html(data.Data.QScore);
                }
            }
            if (data.Data.TScore != -1) {
                $(".li2").html("你的成绩是：<b>" + data.Data.TScore + "</b>");
            }
        } else {
            alert(data.Message);
        }
    });
}

//全屏显示
function FullScreen(url) {
    var urldialog = art.dialog({
        id: 'answerUrl',
        opacity: 0.7,
        background: '#000',
        padding: '0',
        lock: true,
        content: '<img id="clockBtn" style="align:center" src="' + url + '" />'
    });
    $("#clockBtn").bind("click", function () { urldialog.close(); });
}
