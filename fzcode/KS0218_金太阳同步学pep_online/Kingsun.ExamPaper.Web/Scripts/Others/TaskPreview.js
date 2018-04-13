var mp3Obj = new Array();//存储各个跟读课文大题的音频集合
var longMp3Cnt = 0;//课文索引
var cnt = new Array();//存储各个跟读课文大题对应的循环播放小题的计数

var delObj, loadDialog,createDialog;
var unitArray = new Array();//存储选择的单元
var questionArray = new Array();//存储选择的题目
var classTaskID = '', notEdit = 0;
var classGradeID = 0;
var classPackName = '';
var backurl = escape(Common.QueryString.GetValue("backurl"));//记录返回到优教学课时的返回路径

$(function () {
    $("#mainTitle,.navDiv").hide();
    
    classTaskID = Common.QueryString.GetValue("ClassTaskID");
    notEdit = Common.QueryString.GetValue("NotEdit");
    classPackName = unescape(Common.QueryString.GetValue("TaskName"));
    if (classTaskID == "undefined" || classTaskID == "") {
        alert("未获取到选择的题目信息，请返回课时页重试！");
        return;
    }
    if (notEdit == "undefined" || notEdit == "" || notEdit != "1") {
        //判断作业是否已经发布
        //$.post("?action=GetClassTask", { ClassTaskID: classTaskID }, function (data) {
        //    data = eval("(" + data + ")");
        //    if (data.Success) {
        //        if (data.Data == "1") {
        //            notEdit = 1;
        //            $(".addExer,.setHW").hide();
        //        } else {
        //            notEdit = 0;
        //        }
        //        //获取题目
        //        GetClassTaskQue(classTaskID, notEdit);
        //    } else {
        //        alert(data.Message);
        //    }
        //});
        notEdit = 0;
    } else {
        notEdit = 1;
        $(".aReturnY,.addExer,.setHW").hide();
        
        //GetClassTaskQue(classTaskID, notEdit);
    }
    //获取题目
    GetClassTaskQue(classTaskID, notEdit);

    loadDialog = art.dialog({
        id: 'loading',
        opacity: .1,
        padding: '0',
        lock: true,
        content: '<img style="width:305px;height:304px;align:center" src="../App_Themes/images/Loading.gif" />'
    });
    $(".aui_close").hide();
});

function GetClassTaskQue(classTaskID,notEdit) {
    //获取作业题目
    $.post("?action=GetStuReport", { ClassTaskID: classTaskID, NotEdit: notEdit }, function (data) {
        var result = eval("(" + data + ")");
        if (result.Success) {
            loadDialog.close();
            $(".content").html(result.Data.QuestionHTML);
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
                $(mp3[i]).parent().find(".mp3Cont em.num").html($(mp3[i]).attr("num"));
                $(mp3[i]).parent().find(".mp3Cont em.state").html($(mp3[i]).attr("state"));
            }
            AutoPosition();
        }
        else {
            if (result.Message == "作业中题目为空！") {
                alert(result.Message);
                location.replace("TaskArrange.aspx?ClassTaskID=" + classTaskID + "&TaskName=" + escape(classPackName) + "&backurl=" + backurl);
            } else {
                alert(result.Message);
                loadDialog.close();
            }
        }
    });
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
//返回到选择题目页
function backToArrange() {
    if (classTaskID == '' || classTaskID == "undefined") {
        alert("未获取到选择的题目信息，请返回课时页重试！")
        return;
    }
    location.replace("TaskArrange.aspx?ClassTaskID=" + classTaskID + "&TaskName=" + escape(classPackName) + "&backurl=" + backurl);
}

function updateClassTaskQues(obj, questionid, round) {
    $.post("?action=UpdateClassTaskQues", { ClassTaskID: classTaskID, QuestionID: questionid, Round: round }, function (data) {
        if (data) {
            var result = eval("(" + data + ")");
            if (result.Success) {
                $(obj).parent().attr("round", round);
                $(obj).parent().find("b").html("（至少跟读" + round + "遍）");
            } else {
                alert(result.Message);
            }
            AutoPosition();
        }
    });
}
//跟读次数减1
function decrease(obj, questionid, unitid) {
    var round = parseInt($(obj).parent().attr("round"));
    if (round <= 1) {
        round = 1;
    } else {
        round--;
        updateClassTaskQues(obj, questionid, round);
    }
}
//跟读次数加1
function increase(obj, questionid, unitid) {
    var round = parseInt($(obj).parent().attr("round"));
    if (round >= 9) {
        round = 9;
    } else {
        round++;
        updateClassTaskQues(obj, questionid, round);
    }
}

//删除题目
function delQuestion(obj, questionid) {
    delObj = obj;
    var dialogHTML = '<div class="alertMsg"><span><h4>提示</h4><p>确定要删除此题吗？</p></span>'
            + '<div><a class="btn1" onclick="closeDelDialog(1,\'' + questionid + '\')">确定</a>'
            + '<a class="btn2 on" onclick="closeDelDialog(0)">取消</a></div></div>';
    delDialog = art.dialog({
        id: 'IsDelQ',
        opacity: .1,
        padding: 0,
        lock: true,
        content: dialogHTML
    });
    $(".aui_close").hide();//隐藏弹窗的关闭按钮
}

function closeDelDialog(isDel, questionid) {
    delDialog.close();

    if (isDel == 1) {
        $.post("?action=DeleteClassTaskQues", { ClassTaskID: classTaskID, QuestionID: questionid }, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    //删除完全部题目，页面跳转
                    if (result.Data == "0")
                    {
                        alert("题目被清空，将跳转到布置作业页面！");
                        backToArrange();
                    }
                    var currentBox = $(delObj).parent().parent();//td<<tr<<tbody<<table<<div
                    //当一个Unit块下的最后一个题目都删除完后，整个单元块都移除
                    if ($(currentBox).parent().find(".box").length == 1) {
                        $(currentBox).parent().remove();
                    } else {
                        $(currentBox).remove();
                        $.each($(".contBox .content .module h4 label"), function (index, value) {
                            $(value).html(index + 1);
                        });
                    }
                } else {
                    alert(result.Message);
                }
                AutoPosition();
            }
        });
    }
}
//保存并返回优教学
function backToYJX() {
    if (backurl != "undefined" && backurl != "") {
        location.href = unescape(backurl);
    } else {
        alert("无法返回创建课时页面！");
    }
}

function addTask() {
    //获取班级列表
    $.post("?action=GetClassList&rand=" + Math.random(), function (result) {
        if (result) {
            result = eval("(" + result + ")");//JSON.parse
            if (result.Success) {
                var listClass = result.Data.ClassList;
                var liHtml = '';
                $.each(listClass, function (index, value) {
                    liHtml += '<a class="choose" onclick="chooseClass(this)" href="javascript:void(0)" classid="' + value.ClassID
                        + '" gradeid="' + value.GradeID + '" isEmpty="' + value.IsEmpty + '"><em>&nbsp;</em>' + value.ClassName + '</a>';
                });

                var now = new Date();
                now.setDate(now.getDate() + 1);//当前日期加一天
                var d=Common.GetDate(now) + " 23:59:59";
                var week=Common.GetWeekday(new Date(d.replace(/\-/g, "\/")), "周");

                var dialogHTML = '<div class="KSWin"><div class="chooseClass"><h2>班级选择</h2><table><tbody>'
					+ '<tr><td class="tr vt" width="100">班级：</td><td class="vt" colspan="2"><p class="selectPan">'
					+ liHtml
					+ '</p></td></tr>'
                    + '<tr><td class="tr">收作业时间：</td><td width="190"><div class="input-date">'
                    + '<input name="deadline" type="text" id="deadline" class="single-text normal date" '
                    + 'onfocus="WdatePicker({dateFmt:\'yyyy-MM-dd HH:mm:ss\',minDate: \'new Date()\'}),autoSetWeek()" value=\''+d+'\'/>'
                    + '</div></td><td><b id="spWeek">' + week + '</b></td></tr>'
				    + '<tr><td class="tr">作业名称：</td><td colspan="2">'
                    + '<p>' + classPackName + '</p>'
                    + '<p class="pt10"><input type="text" class="KSinput" id="txtTaskTitle" value="本作业有什么特色呢？为它取个名儿吧！" onfocus="changMsg()" onblur="showMsg()"/></p></td></tr>'
					//+ '<input type="text" class="KSinput" value="" id="txtTaskTitle"/></td></tr>'
				    + '<tr><td colspan="3"><a class="KSWinBtn fl left" onclick="confirmAddTask()">发布</a><a class="KSWinBtn fl right" onclick="closeDialog();">取消</a></td></tr>'
			        + '</tbody></table></div><h2></h2></div>';
                createDialog = art.dialog({
                    id: 'KSDialog',
                    height: 400,
                    opacity: .1,
                    padding: 0,
                    lock: true,
                    content: dialogHTML
                });
                $(".aui_close").hide();//隐藏弹窗的关闭按钮
            }
            else {
                alert("没有班级信息，请先设置班级信息！");
            }
        }
    });
}

function changMsg() {
    if ($("#txtTaskTitle").val() == "本作业有什么特色呢？为它取个名儿吧！") {
        $("#txtTaskTitle").val("");
    }
}

function showMsg() {
    if ($("#txtTaskTitle").val() == "") {
        $("#txtTaskTitle").val("本作业有什么特色呢？为它取个名儿吧！");
    }
}

function closeDialog() {
    createDialog.close();
}

function confirmAddTask() {
    var tasktitle = Common.TrimSpace($("#txtTaskTitle").val());
    var deadline = $("#deadline").val();
    if (tasktitle == '' || tasktitle == '本作业有什么特色呢？为它取个名儿吧！') {
        alert("作业名称不能为空！");
        return;
    } else if (!checkDeadline()) {
        alert("收作业时间不能为空，且必须在三个小时以后！");
        $("#deadline").focus();
        return;
    }
    var classids = '',classArray=$(".selectPan a.selected");
    if (classArray.length > 0) {
        for (var i = 0; i < classArray.length ; i++) {
            classids += "," + $(classArray[i]).attr("classid");
        }
        classids = classids.substring(1);
    } else {
        alert("请选择班级！");
        return;
    }
    tasktitle = classPackName +' '+ tasktitle;
    $.post("?action=AddTask", { ClassTaskID: classTaskID, TaskTitle: tasktitle, Deadline: deadline, ClassIDs: classids }, function (data) {
        if (data) {
            var result = eval("(" + data + ")");
            if (result.Success) {
                alert("布置成功！");
                createDialog.close();
                backToYJX();
                //location.replace("TaskPreview?ClassTaskID=" + classTaskID + "&NotEdit=1");
            } else {
                alert(result.Message);
            }
        }
    });
}

//选择班级
function chooseClass(obj) {
    if ($(obj).attr("isEmpty") == "1") {
        alert("此班级没有学生，无法选择！");
        return;
    }
    var selGradeID = parseInt($(obj).attr("gradeid"));
    //取消选中班级
    if ($(obj).attr("class") == "selected") {
        $(obj).removeClass("selected");
    } else {//选中班级
        if (classGradeID == 0) {
            classGradeID = selGradeID;
            $(obj).addClass("selected");
        } else if (classGradeID == selGradeID) {
            $(obj).addClass("selected");
        } else {
            if (confirm("当前已选择" + Common.GetChineseNum(classGradeID - 1) + "年级，选择" + Common.GetChineseNum(selGradeID - 1) + "年级则放弃" + Common.GetChineseNum(classGradeID - 1) + "年级班级！")) {
                $(".selectPan a").removeClass("selected");
                $(obj).addClass("selected");
                classGradeID = selGradeID;
            }
        }
    }
}

//选择时间自动获取星期
function autoSetWeek() {
    if ($("#deadline").val() == "") {
        $("#spWeek").html("");
    } else {
        var date = new Date($("#deadline").val().replace(/\-/g, "\/"));
        $("#spWeek").html(Common.GetWeekday(date, "周"));
    }
}

//校验截止日期是否不为空且在3个小时以后
function checkDeadline() {
    if ($("#deadline").val() == "") {
        return false;
    }
    var date = new Date($("#deadline").val().replace(/\-/g, "\/"));
    if (date.getTime() - (new Date()).getTime() <= 10800000) {
        return false;
    } else {
        return true;
    }
}

function AutoPosition() {
    var height = $("#mainBody").height();
    var Height = $(window).height();
    var otherHeight = $(".header").height() + $("#footer").height() ;
    if (height < (Height - otherHeight)) {
        $("#footer").css("position", "fixed");
    } else {
        $("#footer").css("position", "relative");
    }
}