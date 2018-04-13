//错点分析页面js
var TaskID = Common.QueryString.GetValue("TaskID");
var TaskTitle = unescape(Common.QueryString.GetValue("TaskTitle"));
var ClassName = unescape(Common.QueryString.GetValue("ClassName"));
var OnTime = Common.QueryString.GetValue("OnTime");
var includeLate=0;//统计是否包含补交
var mp3Obj = new Array();//存储各个跟读课文大题的音频集合
var longMp3Cnt = 0;//课文索引
var cnt = new Array();//存储各个跟读课文大题对应的循环播放小题的计数
var Csstype = Common.QueryString.GetValue("Csstype");//判断是否来自云平台
var loadDialog, urldialog;
var canClick = 0;
$(function () {
    $("#aFirst").click(function () {
        location.href = "ClassTaskList.aspx" + (Csstype != "undefined" ? "?Csstype=cloudHeader" : "");
    });
    //页头布置作业选中效果
    $("#aClassTaskList").addClass("on");
    Init();
});

function Init() {
    //无按时交作业学生，仅有补交
    if (OnTime == "0") {
        //选项框选中
        includeLate = 1;
    }
    $(".titH").bind("click", function () {
        GetReport();
    });
    loadData();
}

function hrefHtml() {
    window.location.href = "ClassTaskDetail.aspx?TaskID=" + TaskID + "&ClassName=" + escape(ClassName) + (Csstype != "undefined" ? '&Csstype=cloudHeader' : '');
}

function JumpStuList(questionID, questionModel) {
    var taskTitle = escape(TaskTitle);
    var classname = escape(ClassName);
    window.location.href = "ClassTaskOfStu.aspx?TaskID=" + TaskID + "&TaskTitle=" + taskTitle + "&ClassName=" + classname
        + "&QuestionID=" + questionID + "&QuestionModel=" + questionModel + "&IncludeLate=" + includeLate + "&IsReport=0"
        + (Csstype != "undefined" ? '&Csstype=cloudHeader' : '');
}
//跟读课文循环一遍后回到第一个播放
function Recycle() {
    var nowMp3Obj = mp3Obj[this.ArrayIndex];
    cnt[this.ArrayIndex]++;
    if (cnt[this.ArrayIndex] == nowMp3Obj.Mp3List.length) {
        cnt[this.ArrayIndex] = 0;
        nowMp3Obj.CurrentMp3 = 0;
        var mp3url = nowMp3Obj.Mp3List[nowMp3Obj.CurrentMp3];
        nowMp3Obj.PlayerObj.jPlayer("setMedia", { mp3: mp3url })
    }
}

function loadData() {
    loadDialog = art.dialog({
        id: 'loading',
        opacity: .1,
        padding: '0',
        lock: true,
        content: '<img style="width:305px;height:304px;align:center" src="../App_Themes/images/Loading.gif" />'
    });
    $(".aui_close").hide();

    var obj = { TaskID: TaskID, IncludeLate: includeLate }
    $.post("?action=AnalysisWrong&rand=" + Math.random(), obj, function (result) {
        result = eval("(" + result + ")");
        if (result.Success) {
            var wLevel = 0;//不存在大于20%的 / 存在大于20%但不存在大于50%的 / 存在大于50%的
            var wLevelText = '';
            var noContainText = '';//本次作业不包含的知识点
            var arrayKnowledge = [], arrayWrongRate = [];
            for (var i = 0; i < result.Data.length; i++) {
                arrayKnowledge.push(result.Data[i].KnowldgeName);
                arrayWrongRate.push(result.Data[i].WrongRate);
                
                if (result.Data[i].WrongRate > 20) {
                    if (wLevel < 1) {
                        wLevel = 1;
                        wLevelText = result.Data[i].KnowldgeName;
                    } else if (wLevel == 1) {
                        wLevelText += "," + result.Data[i].KnowldgeName;
                    }
                } else if (result.Data[i].WrongRate > 50) {
                    if (wLevel < 2) {
                        wLevel = 2;
                        wLevelText = result.Data[i].KnowldgeName;
                    } else {
                        wLevelText += "," + result.Data[i].KnowldgeName;
                    }
                }
            }
            $('#columnDiv').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: '错误点分布图'
                },
                xAxis: {
                    categories: arrayKnowledge,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    title: {
                        text: '错误率 (%)'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr>' +
                        '<td style="padding:0"><b>{point.y}%</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [{
                    name: '知识点',
                    data: arrayWrongRate
                }]
            });
            var remarkText = '';
            if (wLevel < 1) {
                remarkText = "<a onclick='clickA(this)' class='" + (includeLate == 1 ? "on" : "") + "'>包含补交作业学生成绩</a><h5>作业结果简评：</h5><span><p>亲爱的老师，您好！该班级本次作业表现良好，错误占比均不高。</p></span>";
            } else {
                var wrongItems = '';
                if (wLevelText.split(",").length == 1) {
                    wrongItems = wLevelText;
                } else {
                    var arrayWLevel = wLevelText.split(",");
                    for (var j = 0; j < arrayWLevel.length; j++) {
                        if (j == 0) {
                            wrongItems = arrayWLevel[j];
                        }
                        else if (j == arrayWLevel.length - 1) {
                            wrongItems += "和" + arrayWLevel[j];
                        } else {
                            wrongItems += "、" + arrayWLevel[j];
                        }
                    }
                }
                remarkText = "<a onclick='clickA(this)' class='" + (includeLate == 1 ? "on" : "") + "'>包含补交作业学生成绩</a><h5>作业结果简评：</h5><span><p>亲爱的老师，您好！该班级本次作业的错误点主要集中在" + wrongItems + "。</p></span>";
            }
            
            $(".remarkDiv").html(remarkText);        
            loadDialog.close();
        }
        else {
            alert(result.Message);
            if (result.Message == "没有学生按时交作业！") {
                window.location.replace("../Default.aspx");
            }
        }
    });
}

//获取高频错题
function GetReport()
{
    if (canClick == 0) {     
        loadDialog = art.dialog({
            id: 'loading',
            opacity: .1,
            padding: '0',
            lock: true,
            content: '<img style="width:305px;height:304px;align:center" src="../App_Themes/images/Loading.gif" />'
        });
        $(".aui_close").hide();

        $.post("?action=GetQuestion&rand=" + Math.random(), { TaskID: TaskID, IncludeLate: includeLate }, function (data) {
            data = eval("(" + data + ")");
            if (data.Success) {
                canClick = 1;
                if (data.Data.SubjectID == 1)
                    $(".main").addClass("chineseS");
                else if (data.Data.SubjectID == 2) {
                    $(".main").addClass("mathS");
                }
                $(".conts").html(data.Data.QuestionHTML);
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
                        $(mp3[i]).KingsunMp3Player({
                            swfPath: "../Scripts/Plugins/jplayer/",
                            mp3: $(mp3[i]).attr("mp3url"),
                            module: $(mp3[i]).attr("module"),//模式：0-正常（大喇叭）
                            OnlyOne: true
                        });
                    }
                }
                loadDialog.close();
            }
            else {
                alert(data.Message);
            }
        });
    }
}

//点击包含补交作业按钮
function clickA(obj) {
    if ($(obj).attr("class") == "on") {
        $(obj).attr("class", "");
        includeLate = 0;
    } else {
        $(obj).attr("class", "on");
        includeLate = 1;
    }
    loadData();
}

//全屏显示
function FullScreen(url) {
    urldialog = art.dialog({
        id: 'answerUrl',
        opacity: 0.7,
        background: '#000',
        padding: '0',
        lock: true,
        content: '<img id="clockBtn" style="align:center" src="' + url + '" />'
    });
    $("#clockBtn").bind("click", function () { urldialog.close(); urldialog = null; });
}