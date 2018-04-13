/// <reference path="../Common.js" />
//学生作业列表页面js

var TotalPages = 0, TotalCount = 0, PageIndex = 1, TaskState = 2, Subject = 3;
var taskSum = 0;
var isSkip = false;
$(function () {
    $(".phoneMsg,.topSelect,.bottomList,.defaultPage").hide();

    if (Common.Validate.IsInt(Common.QueryString.GetValue("PageIndex"))) {
        PageIndex = parseInt(Common.QueryString.GetValue("PageIndex"));
    }
    if (Common.Validate.IsInt(Common.QueryString.GetValue("TaskState"))) {
        TaskState = parseInt(Common.QueryString.GetValue("TaskState"));
    }
    if (Common.Validate.IsInt(Common.QueryString.GetValue("Subject"))) {
        Subject = parseInt(Common.QueryString.GetValue("Subject"));
    }
    if (Common.Validate.IsInt(Common.QueryString.GetValue("taskSum"))) {
        taskSum = parseInt(Common.QueryString.GetValue("taskSum"));
        if (taskSum != 0 && taskSum != "0") {
            $("#em_Total").show().html(taskSum);
        }
    }
    switch (Subject) {
        case 1:
            $(".Chinese").parent().attr("class", "selected bradiusL");
            break;
        case 2:
            $(".Math").parent().attr("class", "selected bradiusL");
            break;
        case 3:
        default:
            $(".English").parent().attr("class", "selected bradiusL");
            break;
    }
    if (TaskState == 2) {
        $("#spTaskState").attr("class", "taskB");
    } else {
        $("#spTaskState").attr("class", "taskB unfinished");
    }

    CheckSecurityPhone();

    $("#selSemester").click(function () {
        gotoPage(PageIndex);
    });

    $("#sp_WrongQueList").click(function () {
        var subid = 3;
        $.each($(".ulSide li"), function () {
            if ($(this).attr("class") == "selected" || $(this).attr("class") == "selected bradiusL") {
                subid = $(this).attr("num");
            }
        });
        location.href = "StuWrongQue.aspx?SubID=" + subid; //跳转参数
    });
});

function skipBind() {
    isSkip = true;
    $(".phoneMsg").hide();
    $(".topSelect,.bottomList").show();
    gotoPage(PageIndex);
}

//验证手机号码
function CheckSecurityPhone() {
    if (!isSkip) {
        $.post("?action=CheckSecurityPhone&Rand=" + Math.random(), null, function (result) {
            if (result) {
                result = eval("(" + result + ")");//JSON.parse
                if (result.Success) {
                    $(".topSelect,.bottomList").show();
                    gotoPage(PageIndex);
                } else {
                    Common.addHandler(document.getElementById("txtPhone"), "keyup", checkPhone);
                    $(".phoneMsg").show();
                    Common.AutoPosition();

                    //取消个人中心点击 重新绑定点击弹出提示
                    //$("#aUserConter").unbind("click");
                    //$("#aUserConter").bind("click", function () { alert("请先完善手机号"); });
                }
            } else {
                $(".phoneMsg").show();
                Common.AutoPosition();

                //取消个人中心点击 重新绑定点击弹出提示
                //$("#aUserConter").unbind("click");
                //$("#aUserConter").bind("click", function () { alert("请先完善手机号"); });
            }
        });
    } else {
        $(".phoneMsg").hide();
        $(".topSelect,.bottomList").show();
        gotoPage(PageIndex);
    }
}

//点击手机输入框
function clickPhone(obj) {
    if (Common.TrimSpace($(obj).val()) == "请输入手机号") {
        $(obj).val("");
    }
    $(obj).css("color", "#000");
}
//移开手机输入框
function blurPhone(obj) {
    if (Common.TrimSpace($(obj).val()) == "") {
        $(obj).val("请输入手机号");
        $(obj).css("color", "#888");
    }
}
//实时检测号码的输入
function checkPhone() {
    //alert(1);
    if (Common.Validate.IsMobileNo($("#txtPhone").val())) {
        $("#getCode").attr("class", "getCode");//获取验证码按钮可点击
    } else {
        $("#getCode").attr("class", "getCode on");//获取验证码按钮不可点击
    }
}
//点击验证码输入框
function clickCode(obj) {
    if (Common.TrimSpace($(obj).val()) == "请输入短信验证码") {
        $(obj).val("");
    }
    $(obj).css("color", "#000");
}
//移开验证码输入框
function blurCode(obj) {
    if (Common.TrimSpace($(obj).val()) == "") {
        $(obj).val("请输入短信验证码");
        $(obj).css("color", "#888");
    }
}
//发送验证码
function sendVerifyCode() {
    if ($("#getCode").attr("class") == "getCode on") {
        return;
    }
    $("#getCode").attr("class", "getCode on");//获取验证码按钮不可点击

    var phone = Common.TrimSpace($("#txtPhone").val());
    if (Common.Validate.IsMobileNo(phone)) {
        $.post("?action=SendVerifyCode&Rand=" + Math.random(), { SecurityPhone: phone }, function (result) {
            if (result) {
                result = eval("(" + result + ")");//JSON.parse
                alert(result.Message);

                clock();

            } else {
                alert("验证码发送失败，请重试！");
                $("#getCode").attr("class", "getCode");//获取验证码按钮可点击
            }
        });
    } else {
        alert("请输入合法的手机号！");
        $("#getCode").attr("class", "getCode");//获取验证码按钮可点击
    }

}

//计时器
function clock() {
    var validCode = true;
    var time = 60;
    //获取验证码清除绑定事件 直到倒计时结束添加绑定事件
    $("#getCode").unbind("click");
    //验证码倒计时
    if (validCode) {
        validCode = false;
        $("#getCode").attr("class", "getCode on");
        var t = setInterval(function () {
            time--;
            $("#getCode").html(time + "秒后重新获取");
            if (time == 0) {
                clearInterval(t);
                $("#getCode").html("获取验证码");
                validCode = true;
                $("#getCode").attr("class", "getCode");
                $("#getCode").bind("click", sendVerifyCode);

                var securityPhone = Common.TrimSpace($("#txtPhone").val());
                $.post("?action=ClearCodeCookie&Rand=" + Math.random(), { SecurityPhone: securityPhone }, function (result) {

                });
            }
        }, 1000)
    }
}

//提交手机信息
function confirmPhone() {
    var verifyCode = Common.TrimSpace($("#txtCode").val());
    var securityPhone = Common.TrimSpace($("#txtPhone").val());
    if (Common.Validate.IsMobileNo(securityPhone) && verifyCode != "") {
        $.post("?action=VerifyAndBind&Rand=" + Math.random(), { SecurityPhone: securityPhone, VerifyCode: verifyCode }, function (result) {
            if (result) {
                result = eval("(" + result + ")");//JSON.parse
                if (result.Success) {
                    alert("绑定成功!");
                    location.reload();
                } else {
                    alert(result.Message);
                    $("#txtCode").focus();
                }
            } else {
                alert("验证码校验失败，请检查后重试！");
                $("#txtCode").focus();
            }
        });
    } else {
        alert("请输入合法的手机号和验证码！");
    }
}

//获取任务列表
function getTaskList() {
    $("#ulTaskList").html("");
    var obj = {
        Semester: $("#selSemester select").val(), StuTaskState: TaskState,
        PageIndex: PageIndex, PageSize: 8, SubjectID: Subject
    };
    $.post("?action=GetStuTaskList&Rand=" + Math.random(), obj, function (result) {
        if (result) {
            result = eval("(" + result + ")");//JSON.parse
            if (result.Success) {
                if (PageIndex > result.Data.TotalPages) { //最后一页最后一条数据提交后，返回作业列表 显示当前最后一页的数据
                    PageIndex = result.Data.TotalPages;
                    getTaskList();
                }
                else {
                    $(".defaultPage").hide();
                    $(".bottomList").show();
                    var liHtml = '';
                    TotalCount = result.Data.TotalCount;
                    TotalPages = result.Data.TotalPages;
                    if (TotalCount > 0) {
                        if (obj.StuTaskState == 2) {
                            $("#em_Total").show();
                            $("#em_Total").html(TotalCount);
                        }
                        $.each(result.Data.StuTaskList, function (index, value) {
                            //liHtml += getStuTaskHtml(value.TaskID, value.TaskTitle, value.TaskType, value.Deadline, value.DoDate, value.SubmitDate, value.StuScore, value.StuTaskID, value.StuTaskState, value.RemainTime);
                            liHtml += getStuTaskHtml(value.CatalogID, value.CatalogName, value.BookName, "", "", "", "", "", "", "",value);
                        });
                        $("#ulTaskList").html(liHtml);
                        initPages();
                        Common.AutoPosition();
                    } else {
                        //$("#ulTaskList").html('');
                        //$(".topSelect").hide();
                        if ($("#selSemester select").val() == "" || $("#selSemester select").val() == null) {
                            $("#selSemester").hide();
                        }
                        $(".bottomList").hide();
                        $(".defaultPage").show();
                        if (TaskState == 2) {
                            $("#em_Total").html("0").hide();
                            $("#defaultTxt").html("没有未完成的学习任务啦~");
                        } else {
                            $("#defaultTxt").html("还没有完成过学习任务哦~");
                        }
                    }
                }
            } else {
                TotalCount = 0;
                TotalPages = 0;
                if ($("#selSemester select").val() == "" || $("#selSemester select").val() == null) {
                    $("#selSemester").hide();
                }
                $(".bottomList").hide();
                $(".defaultPage").show();
                if (TaskState == 2) {
                    $("#em_Total").html("0").hide();
                    $("#defaultTxt").html("没有未完成的学习任务啦~");
                } else {
                    //$("#defaultTxt").html("老师还没有布置学习任务哦~");
                    $("#defaultTxt").html("老师还没有布置学习任务哦~");
                }
            }
        }
    });
}

//初始化分页控件
function initPages() {
    var pageHtml = '<a title="上一页" class="pre" id="aPre" onclick="prePage()">&lt;</a>';
    if (TotalPages <= 10) {
        for (var i = 0; i < TotalPages; i++) {
            if (PageIndex == i + 1) {
                pageHtml += '<a class="cur" onclick="gotoPage(' + (i + 1) + ')">' + (i + 1) + '</a>';
            } else {
                pageHtml += '<a onclick="gotoPage(' + (i + 1) + ')">' + (i + 1) + '</a>';
            }
        }
    } else {
        if (PageIndex <= 8) {
            for (var i = 0; i < 8; i++) {
                if (PageIndex == i + 1) {
                    pageHtml += '<a class="cur" onclick="gotoPage(' + (i + 1) + ')">' + (i + 1) + '</a>';
                } else {
                    pageHtml += '<a onclick="gotoPage(' + (i + 1) + ')">' + (i + 1) + '</a>';
                }
            }
            pageHtml += '<b>...</b>';
            pageHtml += '<a onclick="gotoPage(' + TotalPages + ')">' + TotalPages + '</a>';
        } else if (PageIndex > 8 && PageIndex < TotalPages) {
            for (var i = 0; i < 6; i++) {
                pageHtml += '<a onclick="gotoPage(' + (i + 1) + ')">' + (i + 1) + '</a>';
            }
            pageHtml += '<b>...</b>';
            pageHtml += '<a class="cur" onclick="gotoPage(' + PageIndex + ')">' + PageIndex + '</a>';
            pageHtml += '<b>...</b>';
            pageHtml += '<a onclick="gotoPage(' + TotalPages + ')">' + TotalPages + '</a>';
        } else {
            for (var i = 0; i < 8; i++) {
                pageHtml += '<a onclick="gotoPage(' + (i + 1) + ')">' + (i + 1) + '</a>';
            }
            pageHtml += '<b>...</b>';
            pageHtml += '<a class="cur" onclick="gotoPage(' + TotalPages + ')">' + TotalPages + '</a>';
        }
    }
    pageHtml += '<a title="下一页" class="next" id="aNext" onclick="nextPage()">&gt;</a><input id="inPage" type="text" /><a onclick="goPage()">Go</a>'
    $("#tdPage").html(pageHtml);
}
//上一页
function prePage() {
    if (PageIndex > 1) {
        gotoPage(PageIndex - 1);
    }
}
//下一页
function nextPage() {
    if (PageIndex < TotalPages) {
        gotoPage(PageIndex + 1);
    }
}
//Go跳转
function goPage() {
    if (Common.Validate.IsInt($("#inPage").val())) {
        var tmpVal = parseInt($("#inPage").val());
        if (tmpVal < 1 || tmpVal > TotalPages) {
            $("#inPage").val("");
        } else if (tmpVal != PageIndex) {
            gotoPage(tmpVal);
        }
    } else {
        $("#inPage").val("")
    }
}
//跳转到指定页
function gotoPage(pageindex) {
    PageIndex = pageindex;
    if ($("#spTaskState").attr("class") == "taskB unfinished") {
        TaskState = 4;
        $("#divTaskList").removeClass().attr("class", "bottomList finished")
    } else {
        TaskState = 2;
        $("#divTaskList").removeClass().attr("class", "bottomList unfinished")
    }

    getTaskList();
}


//获取每条学生任务显示HTML文本
function getStuTaskHtml(taskID, tasktitle, tasktype, deadline, dodate, submitdate, stuscore, stutaskid, stutaskstate, RemainTime,value) {
    var tTypeText = '', tStateText = '', tTipsText = '', classHtml = '', scoreHtml = '', emClass = '';
    if (tasktype == 3) {
        tTypeText = '作业';
        emClass = 'task';
    } else if (tasktype == 2) {
        tTypeText = '课堂';
        emClass = 'classroom';
    } else {
        tTypeText = '预习';
        emClass = 'preView';
        if (value.ParentID != 0) {
            emClass = "";
        }
    }
    //判断任务状态
    var endDate = '';
    if (deadline != "") {
        endDate = Common.StringToDate(deadline);
    }
    var sumbitHtml = '';
    //已截止，未完成
    if (stutaskstate == 3) {
        tStateText = '已截止';
        tTipsText = '赶紧补做吧！';
        if (dodate) {
            classHtml = '<a class="first" onclick="goStuTask(\'' + taskID + '\',\'' + stutaskid + '\',0,\'' + tasktype + '\')">Go</a>';
        } else {
            classHtml = '<a class="first toDo" onclick="goStuTask(\'' + taskID + '\',\'' + stutaskid + '\',1,\'' + tasktype + '\')">Go</a>';
        }
    }
        //已完成
    else if (stutaskstate == 2) {
        tTipsText = '完成啦！';

        classHtml = '<a class="first" onclick="checkStuTask(\'' + stutaskid + '\',\'' + tasktype + '\')">查看详情</a>';
        if (tasktype == 3) {
            if (stuscore >= 0) {
                scoreHtml = '<em class="score">' + parseInt(stuscore) + '分</em>';
            } else {
                scoreHtml = '<em class="noscore">&nbsp;</em>';
            }
        }
        var submitDate = Common.StringToDate(submitdate);
        sumbitHtml = '<p class="f12">提交时间：<label class="datetime">' + Common.GetWeekday(submitDate, '周') + '</label>&nbsp;&nbsp;<label class="datetime">' + submitdate + '</label></p>';
    }
        //已做未完成
    else if (stutaskstate == 1) {
        tTipsText = '还没完成哦，加油！';
        classHtml = '<a class="first" onclick="goStuTask(\'' + taskID + '\',\'' + stutaskid + '\',1,\'' + tasktype + '\')">Go</a>';
    }
        //未做
    else {
        if (tasktype == 3) {
            tTipsText = '作业来啦！';
        } else if (tasktype == 1) {
            tTipsText = '预习资料来啦！';
        }
        classHtml = '<a class="first toDo" onclick="goStuTask(\'' + taskID + '\',\'' + stutaskid + '\',0,\'' + tasktype + '\')">Go</a>';
    }

    return '<li onclick="goStuTask(\'' + taskID + '\',\'' + stutaskid + '\',0,\'' + tasktype + '\')" class="' + emClass + '"><em>' + "&nbsp;" + '</em><span class="leftLi"><p>' //<i>' + tStateText + '</i>
                + '<label class="time">' + tasktitle + '&nbsp;</label>'
                + '<i>' + tTipsText + '</i></p>'
                //+ (endDate == "" ? "" : '<p class="f12">截止时间：<label class="datetime">' + (endDate == "" ? "" : Common.GetWeekday(endDate, '周')) + '</label>&nbsp;&nbsp;<label class="datetime">' + deadline + '</label></p>')
                //+ (TaskState == 4 ? "" : ('<p class="f12">截止时间：' + (RemainTime == "0" ? '<label class="deadline">已截止</label>' : '还有<label class="datetime">' + RemainTime + '</label>') + '</p>'))
                + sumbitHtml
                + '</span><span class="rightLi">' + scoreHtml + classHtml + '</span></li>';


    //return '<li class="' + emClass + '"><em>' + "&nbsp;" + '</em><span class="leftLi"><i>' + tStateText + '</i>'
    //            + '<p><label class="time">' + tasktitle + '&nbsp;</label>'
    //            + '<i>' + tTipsText + '</i></p>'
    //            //+ (endDate == "" ? "" : '<p class="f12">截止时间：<label class="datetime">' + (endDate == "" ? "" : Common.GetWeekday(endDate, '周')) + '</label>&nbsp;&nbsp;<label class="datetime">' + deadline + '</label></p>')
    //            + (TaskState == 4 ? "" : ('<p class="f12">截止时间：' + (RemainTime == "0" ? '<label class="deadline">已截止</label>' : '还有<label class="datetime">' + RemainTime + '</label>') + '</p>'))
    //            + sumbitHtml
    //            + '</span><span class="rightLi">' + scoreHtml + classHtml + '</span></li>';
}

//选择科目获取任务列表
function gotoTaskList(subjectid) {
    if (subjectid != Subject) {
        Subject = subjectid;
        $(".ulSide li").removeClass("selected");
        switch (Subject) {
            case 1:
                $(".Chinese").parent().attr("class", "selected");
                break;
            case 2:
                $(".Math").parent().attr("class", "selected");
                break;
            case 3:
            default:
                $(".English").parent().attr("class", "selected");
                break;
        }
        PageIndex = 1;
        TaskState = 2;
        $("#spTaskState").attr("class", "taskB");
        CheckSecurityPhone();
        //location.href = "StuTaskList.aspx?Subject=" + subjectid;
    }
}

//Go，做作业
function goStuTask(taskID, stuTaskID, isDo, tasktype) {
    if (checkFlash()) {
        //if (tasktype == 3) {
        //    if (Subject == 1) {
        //        location.href = "../ChineseModels/DoQuestion.aspx?AccessType=3&StuTaskID=" + stuTaskID + "&PageIndex=" + PageIndex + "&TaskState=" + TaskState + "&Subject=" + Subject;
        //    } else if (Subject == 2) {
        //        location.href = "../MathModels/DoQuestion.aspx?AccessType=3&StuTaskID=" + stuTaskID + "&PageIndex=" + PageIndex + "&TaskState=" + TaskState + "&Subject=" + Subject;
        //    } else {
        //        location.href = "../QuestionModels/DoQuestion.aspx?AccessType=3&StuTaskID=" + stuTaskID + "&PageIndex=" + PageIndex + "&TaskState=" + TaskState + "&Subject=" + Subject;
        //    }
        //}
        //else if (tasktype == 1) {
        //    //更新学生任务信息
        //    $.post("?action=UpdateStuTask&Rand=" + Math.random(), { StuTaskID: stuTaskID }, function (data) {
        //        data = eval("(" + data + ")");
        //        if (data.Success) {
        //            location.href = "../Student/StuPreviewForWeb.aspx?StuTaskID=" + stuTaskID + "&PageIndex=" + PageIndex + "&Subject=" + Subject;
        //        }
        //        else {
        //            alert(data.Message);
        //        }
        //    });
        //}
        if (isDo == 1) {
            pageherf(stuTaskID, tasktype, taskID);
        } else {
            //更新学生任务信息
            $.post("?action=UpdateStuTask&Rand=" + Math.random(), { StuTaskID: stuTaskID, TaskID: taskID }, function (data) {
                data = eval("(" + data + ")");
                if (data.Success) {
                    pageherf(stuTaskID, tasktype, taskID);
                }
                else {
                    alert(data.Message);
                }
            });
        }
    } else {
        alert("请先下载并安装Flash插件，才能正常做题哦！");
    }
}

function pageherf(stuTaskID, taskType, taskID) {
    if (taskType == 3) {
        if (Subject == 1) {
            location.href = "../ChineseModels/DoQuestion.aspx?AccessType=3&StuTaskID=" + stuTaskID + "&PageIndex=" + PageIndex + "&TaskState=" + TaskState + "&Subject=" + Subject + "&TaskID=" + taskID;
        } else if (Subject == 2) {
            location.href = "../MathModels/DoQuestion.aspx?AccessType=3&StuTaskID=" + stuTaskID + "&PageIndex=" + PageIndex + "&TaskState=" + TaskState + "&Subject=" + Subject + "&TaskID=" + taskID;
        } else {
            location.href = "../QuestionModels/DoQuestion.aspx?AccessType=3&StuTaskID=" + stuTaskID + "&PageIndex=" + PageIndex + "&TaskState=" + TaskState + "&Subject=" + Subject + "&TaskID=" + taskID;
        }
    } else if (taskType == 1) {
        location.href = "../Student/StuPreviewForWeb.aspx?StuTaskID=" + stuTaskID + "&PageIndex=" + PageIndex + "&Subject=" + Subject;
    }
}

//检查作业报告
function checkStuTask(stuTaskID, tasktype) {
    if (tasktype == 3) {
        var taskSum;
        taskSum = $("#em_Total").html();
        location.href = "StuReport.aspx?StuTaskID=" + stuTaskID + "&PageIndex=" + PageIndex + "&TaskState=" + TaskState + "&Subject=" + Subject + "&taskSum=" + taskSum;
    }
    else if (tasktype == 1) {
        location.href = "../Student/StuPreviewForWeb.aspx?StuTaskID=" + stuTaskID + "&PageIndex=" + PageIndex;
    }
}
/**
 * 检测浏览器是否安装了flash;
 */
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