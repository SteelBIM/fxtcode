/// <reference path="../jquery.json-2.4.js" />
/// <reference path="../../App_Themes/js/jquery.cookie.js" />
/// <reference path="../Common.js" />

//作业篮页面js
var taskDialog, loadDialog, delDialog;
var delObj;
var classArray = new Array();//存储选择的班级
var unitArray = new Array();//存储选择的单元
var questionArray = new Array();//存储选择的题目
var qCnt = 0, qTime = 0, selBookID = 0;
var deadline;

var unitids = '', questionids = '', classids = '';
var Subject = 0;
//读取Cookie
function readCookie() {
    if (Common.Cookie.getcookie("Deadline")) {
        deadline = Common.Cookie.getcookie("Deadline");
    }
    classArray = Common.Cookie.getcookieArray("ClassList");
    unitArray = Common.Cookie.getcookieArray("UnitList");
    if (classArray.length > 0 && unitArray.length > 0) {
        for (var i = 0; i < unitArray.length; i++) {
            unitids += "," + unitArray[i].UID;
            questionArray = Common.Cookie.getcookieArray("UnitID" + unitArray[i].UID);
            for (var j = 0; j < questionArray.length; j++) {
                questionids += ",'" + questionArray[j].QID + "'";
            }
        }
        if (unitids != '') {
            unitids = unitids.substring(1);
        }
        if (questionids != '') {
            questionids = questionids.substring(1);
        } else {
            alert("没有选择题目，将跳转到布置作业页面！");
            location.replace("TaskArrange.aspx" + window.location.search);
        }
        selBookID = Common.Cookie.getcookie("BookID");
        if (Common.Cookie.getcookie("QuestionCount")) {
            qCnt = parseInt(Common.Cookie.getcookie("QuestionCount"));
        }
        if (Common.Cookie.getcookie("QuestionTimes")) {
            qTime = parseInt(Common.Cookie.getcookie("QuestionTimes"));
        }
    } else {
        alert("没有选择题目，将跳转到布置作业页面！");
        location.replace("TaskArrange.aspx" + window.location.search);
    }
}

$(function () {
    $("#aTaskArrange").addClass("on");
    Common.CheckIndexOf();
    readCookie();

    $.post("?action=GetUnitQuestions&rand=" + Math.random(), { UnitIDs: unitids, QuestionIDs: questionids }, function (result) {
        if (result) {
            result = eval("(" + result + ")");//JSON.parse
            if (result.Success) {
                var listQuestions = result.Data.QuestionList;
                Subject = result.Data.Subject;
                var unitname = '';
                var unitHtml = '';
                var taskTitle = "";
                var questionTitle = "";
                $.each(listQuestions, function (index, value) {
                    if (unitname != value.KeyWord + "  " + value.UnitName) {
                        if (unitname != '') {
                            unitHtml += '</div>';
                            taskTitle += ",";
                        }
                        unitname = value.KeyWord + "  " + value.UnitName;
                        taskTitle += value.KeyWord;
                        unitHtml += '<div class="unitBox"><h4>' + unitname + '</h4>';
                    }
                    questionTitle = (value.QuestionTitle[0] == "*" ? value.QuestionTitle.substring(1) : value.QuestionTitle);
                    unitHtml += '<div class="box" questionid="' + value.QuestionID + '"><table><tr><td width="445"><span class="hwMsg" title="'
                        + questionTitle + '">' + (value.Section.indexOf("*") >= 0 ? "" : value.Section) + ' ' + (questionTitle.length > 33 ? value.QuestionTitle.substring(0, 30)
                        + "......" : questionTitle) + '</span></td>';
                    unitHtml += '<td width="115">' + (value.QuestionTime / 60) + '</td>';
                    if (value.QuestionModel == "M1" || value.QuestionModel == "M2") {
                        var tmpRound = Common.Cookie.getQueRound("UnitID" + value.UnitID, value.QuestionID);
                        unitHtml += '<td width="140"><span class="numSp">'
                        + '<a class="left" href="javascript:void(0)" onclick="decrease(this,' + value.QuestionTime + ',\''
                       + value.QuestionID + '\',\'' + value.UnitID + '\')">-</a>'
                        + '<input disabled="disabled" type="text" value="' + tmpRound + '"/>'
                        + '<a class="right" href="javascript:void(0)" onclick="increase(this,' + value.QuestionTime + ',\''
                        + value.QuestionID + '\',\'' + value.UnitID + '\')">+</a></span></td>';
                        unitHtml += '<td width="135">' + ((value.QuestionTime * tmpRound) / 60) + '</td>';
                    } else {
                        unitHtml += '<td width="140"></td>';
                        unitHtml += '<td width="135">' + (value.QuestionTime / 60) + '</td>';
                    }
                    unitHtml += '<td width="135"><a class="pvw" href="javascript:preview(\'' + value.UnitID + '\',\'' + value.QuestionID + '\')">预览</a>'
                        + '<a class="del" href="javascript:void(0)" onclick="delQuestion(this,' + value.QuestionTime + ',\''
                        + value.QuestionID + '\',\'' + value.UnitID + '\')" >删除</a></td>';
                    unitHtml += '<td></td>';
                    unitHtml += '</tr></table></div>';
                    if (index == listQuestions.length - 1) {
                        unitHtml += '</div>';
                    }
                });
                $("#mainContent .listHead").after(unitHtml);
                //判断是否有缓存过TaskTitle
                if (Common.Cookie.getcookie("TaskTitle")) {
                    taskTitle = Common.Cookie.getcookie("TaskTitle");
                } else {
                    taskTitle = "[" + Common.GetDate(new Date()) + "]" + taskTitle;
                }
                $("#spTaskTitle").val(taskTitle);
                //收作业时间
                if (deadline) {
                    $("#deadline").val(deadline);
                    autoSetWeek();
                }
                //作业对象：班级
                var classNames = "";
                for (var i = 0; i < classArray.length; i++) {
                    classids += "," + classArray[i].CID;
                    classNames += "," + classArray[i].CName;
                }
                classids = classids.substring(1);
                $("#className").html(classNames.substring(1));
                markTotalData();
                Common.AutoPosition();
            } else {
                alert(result.Message);
            }
        }
    });
});
//单题预览
function preview(unitid, questionid) {
    if (Subject != Common.Cookie.getcookie("TaskSub")) {
        alert("已切换学科，将跳转到布置作业页面！");
        location.replace("TaskArrange.aspx" + window.location.search);
    } else {
        //分学科跳转
        if (Subject == 1) {
            location.href = "../ChineseModels/DoQuestion.aspx?AccessType=2&QuestionID=" + questionid
            + "&Round=" + Common.Cookie.getQueRound("UnitID" + unitid, questionid) + (window.location.search ? "&Csstype=cloudHeader" : "");
        } else if (Subject == 2) {
            location.href = "../MathModels/DoQuestion.aspx?AccessType=2&QuestionID=" + questionid
            + "&Round=" + Common.Cookie.getQueRound("UnitID" + unitid, questionid) + (window.location.search ? "&Csstype=cloudHeader" : "");
        } else {
            location.href = "../QuestionModels/DoQuestion.aspx?AccessType=2&QuestionID=" + questionid
            + "&Round=" + Common.Cookie.getQueRound("UnitID" + unitid, questionid) + (window.location.search ? "&Csstype=cloudHeader" : "");
        }
    }
}
//总体预览
function previewAll() {
    if (Subject != Common.Cookie.getcookie("TaskSub")) {
        alert("已切换学科，将跳转到布置作业页面！");
        location.replace("TaskArrange.aspx" + window.location.search);
    } else {
        var listQuestion = $(".box");//获取所有题目，按顺序存储
        var questionIDs = '', rounds = '', sorts = '';
        for (var i = 0; i < listQuestion.length; i++) {
            questionIDs += "," + $(listQuestion[i]).attr("questionid") + "_"
                + ($(listQuestion[i]).find("input").val() ? $(listQuestion[i]).find("input").val() : 1);
        }
        if (questionIDs != '') {
            questionIDs = questionIDs.substring(1);
            $.post("?action=GoTaskPreview", { QuestionIDs: questionIDs }, function (result) {
                if (result) {
                    result = eval("(" + result + ")");//JSON.parse
                    if (result.Success) {
                        location.href = "TaskPreview.aspx" + window.location.search;
                    } else {
                        alert(result.Message);
                    }
                }
            });
        } else {
            alert("尚未选择题目，请先选择！");
            location.href = 'TaskArrange.aspx' + window.location.search;
        }
    }
}
//下载作业
function DownloadTask() {
    if (Subject != Common.Cookie.getcookie("TaskSub")) {
        alert("已切换学科，将跳转到布置作业页面！");
        location.replace("TaskArrange.aspx" + window.location.search);
    } else {
        var listQuestion = $(".box");//获取所有题目，按顺序存储
        var questionIDs = '', rounds = '', sorts = '';
        for (var i = 0; i < listQuestion.length; i++) {
            questionIDs += "," + $(listQuestion[i]).attr("questionid") + "_"
                + ($(listQuestion[i]).find("input").val() ? $(listQuestion[i]).find("input").val() : 1);
        }
        if (questionIDs != '') {
            questionIDs = questionIDs.substring(1);
            $.post("?action=DownloadTask", { QuestionIDs: questionIDs }, function (result) {
                if (result) {
                    result = eval("(" + result + ")");//JSON.parse
                    if (result.Success) {
                        window.open("TaskPrint.aspx?Rand=" + Math.random() + "&UserID=" + result.Data + "&TaskTitle=" + escape(Common.TrimSpace($("#spTaskTitle").val())), "_blank");
                        //location.href = "TaskBasket.aspx?IsPrint=1&TaskTitle=" + escape(Common.TrimSpace($("#spTaskTitle").val()));
                    } else {
                        alert(result.Message);
                    }
                }
            });
        } else {
            alert("尚未选择题目，请先选择！");
            location.href = 'TaskArrange.aspx' + window.location.search;
        }
    }
}

function FocusTitle(obj) {
    $(obj).select();
}
//修改作业名称
function ChangeTitle(obj) {
    var taskTitle = Common.TrimSpace($(obj).val());
    $(obj).val(taskTitle);
    var errMsg = Common.ValidateTxt(taskTitle);
    if (errMsg != "") {
        alert(errMsg);
    } else if (taskTitle.length > 52) {
        alert("作业名称长度不能超过52个字符！");
    } else {
        Common.Cookie.setcookie("TaskTitle", taskTitle);
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
//自动获取周几
function autoSetWeek() {
    Common.Cookie.setcookie("Deadline", $("#deadline").val());
    if ($("#deadline").val() == "") {
        $("#spWeek").html("");
    } else {
        var date = new Date($("#deadline").val().replace(/\-/g, "\/"));
        $("#spWeek").html(Common.GetWeekday(date, "周"));
    }
}
//标记总时长和总项数
function markTotalData() {
    $(".titSp").html("已选题目_" + qCnt + "_项共耗时_" + (qTime / 60) + "_分钟");
}
//跟读次数减1
function decrease(obj, time, questionid, unitid) {
    if (Subject != Common.Cookie.getcookie("TaskSub")) {
        alert("已切换学科，将跳转到布置作业页面！");
        location.replace("TaskArrange.aspx" + window.location.search);
    } else {
        var rObj = $(obj).parent().find("input");//跟读次数
        var tObj = $(obj).parent().parent().next();//单题总耗时
        var round = $(rObj).val();
        if (round <= 1) {
            round = 1;
        } else {
            round--;
            qTime -= time;
        }
        $(rObj).val(round);
        $(tObj).html(round * (time / 60));
        Common.Cookie.updateQue("UnitID" + unitid, questionid, round);
        markTotalData();
    }
}
//跟读次数加1
function increase(obj, time, questionid, unitid) {
    if (Subject != Common.Cookie.getcookie("TaskSub")) {
        alert("已切换学科，将跳转到布置作业页面！");
        location.replace("TaskArrange.aspx" + window.location.search);
    } else {
        var rObj = $(obj).parent().find("input");//跟读次数
        var tObj = $(obj).parent().parent().next();//单题总耗时
        var round = $(rObj).val();
        if (round >= 9) {
            round = 9;
        } else {
            round++;
            qTime += time;
        }
        $(rObj).val(round);
        $(tObj).html(round * (time / 60));
        Common.Cookie.updateQue("UnitID" + unitid, questionid, round);
        markTotalData();
    }
}
//删除题目
function delQuestion(obj, time, questionid, unitid) {
    if (Subject != Common.Cookie.getcookie("TaskSub")) {
        alert("已切换学科，将跳转到布置作业页面！");
        location.replace("TaskArrange.aspx" + window.location.search);
    } else {
        delObj = obj;
        var dialogHTML = '<div class="alertMsg"><span><h4>提示</h4><p>确定要删除此题吗？</p></span>'
                + '<div><a class="btn1" onclick="closeDelDialog(1,\'' + time + '\',\'' + questionid + '\',\'' + unitid + '\')">确定</a>'
                + '<a class="btn2 on" onclick="closeDelDialog(0)">取消</a></div></div>';
        delDialog = art.dialog({
            id: 'IsDelQ',
            opacity: .1,
            padding: 0,
            lock: true,
            content: dialogHTML
        });
        $(".aui_close").hide();//隐藏弹窗的关闭按钮
        //art.dialog({
        //    id: 'IsDelQ',
        //    opacity: .1,
        //    width: 200,
        //    height: 60,
        //    padding: 0,
        //    lock: true,
        //    content: '确定要删除此题吗？',
        //    button: [
        //        {
        //            name: '确定',
        //            callback: function () {

        //            }
        //        }, {
        //            name: '取消'
        //        }
        //    ]
        //});
        //$(".aui_close").hide();//隐藏弹窗的关闭按钮
    }
}
function closeDelDialog(isDel, time, questionid, unitid) {
    delDialog.close();
    if (isDel == 1) {
        questionArray = Common.Cookie.deleteQue("UnitID" + unitid, questionid);
        if (questionArray.length == 0) {
            for (var i = 0; i < unitArray.length; i++) {
                if (unitArray[i].UID == unitid) {
                    unitArray = Common.DeleteArray(unitArray, i);
                    break;
                }
            }
            Common.Cookie.delcookie("UnitID" + unitid);
        }
        else {
            Common.Cookie.setcookie("UnitID" + unitid, $.toJSON(questionArray));
        }
        var currentBox = $(delObj).parent().parent().parent().parent().parent();//td<<tr<<tbody<<table<<div
        //当一个Unit块下的最后一个题目都删除完后，整个单元块都移除
        if ($(currentBox).parent().find(".box").length == 1) {
            $(currentBox).parent().remove();
        } else {
            $(currentBox).remove();
        }
        qCnt--;
        qTime -= time;
        markTotalData();
        saveCookie();
    }
}

function saveCookie() {
    //缓存总题数
    if (qCnt > 0) {
        Common.Cookie.setcookie("QuestionCount", qCnt);
    } else {
        clearCookie();
        alert("题目被清空，将跳转到布置作业页面！");
        location.replace("TaskArrange.aspx" + window.location.search);
    }
    //缓存交作业时间
    if ($("#deadline").val() != "") {
        Common.Cookie.setcookie("Deadline", $("#deadline").val());
    } else {
        Common.Cookie.delcookie("Deadline");
    }
    //缓存单元列表
    if (unitArray.length > 0) {
        Common.Cookie.setcookie("UnitList", $.toJSON(unitArray));
    } else {
        Common.Cookie.delcookie("UnitList");
    }
    //缓存总耗时（秒）
    if (qTime > 0) {
        Common.Cookie.setcookie("QuestionTimes", qTime);
    } else {
        Common.Cookie.delcookie("QuestionTimes");
    }
}
function clearCookie(isClearClass) {
    Common.Cookie.delcookie("TaskTitle");
    Common.Cookie.delcookie("Deadline");
    Common.Cookie.delcookie("ClassList");
    Common.Cookie.delcookie("ClassList");
    unitArray = Common.Cookie.getcookieArray("UnitList");
    for (var i = 0; i < unitArray.length; i++) {
        Common.Cookie.delcookie("UnitID" + unitArray[i].UID);
    }
    Common.Cookie.delcookie("UnitList");
    Common.Cookie.delcookie("GradeID");
    Common.Cookie.delcookie("BookReel");
    Common.Cookie.delcookie("BookID");
    Common.Cookie.delcookie("LastUnitID");
    Common.Cookie.delcookie("QuestionTimes");
    Common.Cookie.delcookie("QuestionCount");
}
//调整作业
function ModifyTask() {
    if (Subject != Common.Cookie.getcookie("TaskSub")) {
        alert("已切换学科，将跳转到布置作业页面！");
        location.replace("TaskArrange.aspx" + window.location.search);
    } else {
        saveCookie();
        location.replace("TaskArrange.aspx" + window.location.search);
    }
}

//发布作业
function AddTask() {
    if (Subject != Common.Cookie.getcookie("TaskSub")) {
        alert("已切换学科，将跳转到布置作业页面！");
        location.replace("TaskArrange.aspx" + window.location.search);
    } else {
        var listQuestion = $(".box");//获取所有题目，按顺序存储
        if (listQuestion.length == 0) {
            alert("没有选择题目，将跳转到布置作业页面！");
            location.replace("TaskArrange.aspx" + window.location.search);
        } else {
            var taskTitle = Common.TrimSpace($("#spTaskTitle").val());
            var errMsg = Common.ValidateTxt(taskTitle);
            if (errMsg != "") {
                alert(errMsg);
                $("#spTaskTitle").focus();
                return;
            }
            if (taskTitle.length == 0) {
                alert("请填写作业名称！");
                $("#spTaskTitle").focus();
            } else if (taskTitle.length > 52) {
                alert("作业名称长度不能超过52个字符！");
                $("#spTaskTitle").focus();
            } else if (!checkDeadline()) {
                alert("收作业时间不能为空，且必须在三个小时以后！");
                $("#deadline").focus();
            } else {
                var dialogHTML = '<div class="alertMsg"><span><h4>提示</h4><p>确定布置作业吗？</p></span>'
                + '<div><a class="btn1 on" onclick="closeAddTaskDialog(1)">确定</a>'
                + '<a class="btn2" onclick="closeAddTaskDialog(0)">取消</a></div></div>';
                taskDialog = art.dialog({
                    id: 'IsAddTask',
                    opacity: .1,
                    padding: 0,
                    lock: true,
                    content: dialogHTML
                });
                $(".aui_close").hide();//隐藏弹窗的关闭按钮
            }
        }
    }
}

function closeAddTaskDialog(isAdd) {
    taskDialog.close();
    if (isAdd == 1) {
        confirmAddTask();
    }
}

function confirmAddTask() {
    if (Subject != Common.Cookie.getcookie("TaskSub")) {
        alert("已切换学科，将跳转到布置作业页面！");
        location.replace("TaskArrange.aspx" + window.location.search);
    } else {
        loadDialog = art.dialog({
            id: 'loading',
            opacity: .1,
            padding: '0',
            lock: true,
            content: '<img style="width:305px;height:304px;align:center" src="../App_Themes/images/Loading.gif" />'
        });
        $(".aui_close").hide();

        var listQuestion = $(".box");//获取所有题目，按顺序存储
        var questionIDs = '', rounds = '', sorts = '';
        for (var i = 0; i < listQuestion.length; i++) {
            questionIDs += "," + $(listQuestion[i]).attr("questionid");
            rounds += "," + ($(listQuestion[i]).find("input").val() ? $(listQuestion[i]).find("input").val() : 1);
            sorts += "," + (i + 1);
        }

        $.post("?action=AddTask&rand=" + Math.random(), {
            TaskTitle: Common.TrimSpace($("#spTaskTitle").val()), Deadline: $("#deadline").val(), TaskCount: qCnt, RequireTime: qTime,
            ClassIDs: classids, QuestionIDs: questionIDs.substring(1), Rounds: rounds.substring(1), Sorts: sorts.substring(1), SubjectID: Subject
        }, function (result) {
            if (result) {
                result = eval("(" + result + ")");//JSON.parse
                if (result.Success) {
                    clearCookie();
                    var createDate = result.Data;
                    //$.post("?action=PushNewTaskMsg&rand=" + Math.random(), { CreateDate: createDate });
                    location.replace("ClassTaskList.aspx" + window.location.search);
                } else {
                    if (result.Message == "Sub") {
                        alert("已切换学科，布置失败，将跳转到布置作业页面！");
                        location.replace("TaskArrange.aspx" + window.location.search);
                    } else {
                        alert(result.Message);
                        loadDialog.close();
                    }
                }
            }
        });
    }
}