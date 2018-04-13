/// <reference path="../jquery.json-2.4.js" />
/// <reference path="../../App_Themes/js/jquery.cookie.js" />
/// <reference path="../Common.js" />

//布置作业页面js
document.body.onkeydown = function (event) {
    event = event ? event : window.event;
    // 将退格键屏蔽
    if (event.keyCode == 8) {
        if (event.preventDefault) {
            event.preventDefault();
        } else {
            event.returnValue = false;
        }
    }
};

var unitArray = new Array();//存储选择的单元
var questionArray = new Array();//存储选择的题目
var qCnt = 0, qTime = 0, selBookID = 0, lastUnitID = 0, lastUnitName = "";

var mod_ed = 0, gradeID = 0, bookReel = 0, classTaskID = '';
var parentEdition;
var classPackName = '';
var backurl = escape(Common.QueryString.GetValue("backurl"));//记录返回到优教学课时的返回路径
var subject = 0;
$(function () {
    $("#mainTitle,.navDiv").hide();
    //加载单元列表的滚动条
    $("#unitList").niceScroll({ touchbehavior: false, autohidemode: false, cursorcolor: "#fbc579", cursoropacitymax: 1, cursorwidth: 10 });
    //布置作业选择班级的展开与收缩
    //初始化
    $(".slideWrap a.prev").hide();
    $(".slideWrap a.next").hide();
    $(".cloosePart").hide();
    Common.CheckIndexOf();

    initData();
    
    Common.AutoPosition();
    
    //选择年级
    $("#selGrade").click(function () {
        //当所选年级与班级所在年级不一致时
        if (gradeID != $("#selGrade select").val()) {
            if (confirm("您确定要更换年级吗？当前选择的题目将会被清空！")) {
                clearQuestion();
                gradeID = $("#selGrade select").val();
                loadBookUnitQuestion();
            } else {
                autoChoose('#selGrade', gradeID);
            }
        }
    });
    //选择册别
    $("#selBookReel").click(function () {
        //当所选册别与当前学期册别不一致时
        if (bookReel != $("#selBookReel select").val()) {
            if (confirm("您确定要更换册别吗？当前选择的题目将会被清空！")) {
                clearQuestion();
                bookReel = $("#selBookReel select").val();
                loadBookUnitQuestion();
            } else {
                autoChoose('#selBookReel', bookReel);
            }
        }
    });

    $(window).resize(function () {
        autoPositionBar();
    });
});

//状态栏的动态效果
function autoPositionBar() {
    var wH = $(window).height();//浏览器高度
    var wW = $(window).width(); //浏览器宽度
    var w = (wW - 880) / 2;
    if ($(".cloosePart").height() > wH) {
        $("#statusBar .cont").css({ "position": "fixed", "bottom": "0", "left": w + "px" });
        $(window).bind("scroll", function () {
            var sTop = $(window).scrollTop();
            var divTop = $("#mainBody .main .cloosePart .btmD").offset().top; //容器Y坐标
            //判断页面滚动到某处时的效果
            if (sTop > divTop - wH + 104) {
                $("#statusBar .cont").css({ "position": "", "bottom": "", "left": "" });
            } else {
                $("#statusBar .cont").css({ "position": "fixed", "bottom": "0", "left": w + "px" });
            }
        })
    }
}

//自动选择下拉框
function autoChoose(elementName, compareVal) {
    $(elementName + ' select option').removeAttr("selected");
    $(elementName + ' select option[value="' + compareVal + '"]').attr("selected", "selected");
    $(elementName + ' ul li').removeClass("selected");
    for (var i = 0; i < $(elementName + ' select option').length; i++) {
        if ($(elementName + ' select option:eq(' + i + ')').val() == compareVal) {
            $(elementName + ' .select-tit span').html($(elementName + ' ul li:eq(' + i + ')').html());
            $(elementName + ' ul li:eq(' + i + ')').addClass("selected");
            break;
        }
    }
}
//加载课本、单元和题目
function loadBookUnitQuestion() {
    $.post("?action=GetBooks&rand=" + Math.random(), { GradeID: gradeID, BookReel: bookReel, UnitID: lastUnitID, ED: mod_ed, ClassTaskID: classTaskID }, function (data) {
        if (data) {
            var result = eval("(" + data + ")");//JSON.parse
            if (result.Success) {
                parentEdition = result.Data.ParentEdition;
                $("#spParentEdition").text(parentEdition.EditionName);
                if (result.Message == "") {
                    selBookID = result.Data.BookID;
                    lastUnitName = result.Data.LastUnitName;
                    lastUnitID = result.Data.LastUnitID;
                    subject = result.Data.Subject;
                    //加载课本
                    loadBooks(result.Data.BookList, result.Data.BookID);
                    //加载单元
                    loadUnits(result.Data.UnitList, result.Data.QuestionList,result.Data.MarkObject);
                    //加载题目
                    //loadQuestions(result.Data.QuestionList, lastUnitID);
                } else {
                    $("#selBook img").attr("src", "");
                    $("#selBook .info").html("");
                    $("#ulBook").html("");
                    $("#unitList").html("");
                    $(".selC").show();
                    $(".cloosePart").hide();
                    Common.AutoPosition();
                    alert(result.Message);
                }
            } else {
                clearQuestion();
                $("#selBook img").attr("src", "");
                $("#selBook .info").html("");
                $("#ulBook").html("");
                $("#unitList").html("");
                $(".selC").hide();
                $(".cloosePart").hide();
                Common.AutoPosition();
                alert(result.Message);
            }
        }
    });
}
//加载课本
function loadBooks(listBook,defaultBookID) {
    if (listBook.length > 0) {
        if (listBook.length <= 3) {
            $(".slideWrap a.prev").hide();
            $(".slideWrap a.next").hide();
        } else {
            $(".slideWrap a.prev").show();
            $(".slideWrap a.next").show();
        }
        var ulBookHtml = '';
        $.each(listBook, function (index, value) {
            ulBookHtml += '<li><a href="javascript:void(0)" bookID="'
                + value.BookID + '" bookCover="' + value.BookCover + '" bookName="' + value.BookName + '" editionName="' + value.EditionName
                + '" ><img src="' + value.BookCover + '" alt="' + value.EditionName + '"/><span>' + value.EditionName + '</span></a></li>';
        });
        $("#ulBook").html(ulBookHtml);
        //标记选中课本
        if (selBookID > 0) {
            markBook(selBookID);
        } else {
            markBook(defaultBookID > 0 ? defaultBookID : listBook[0].BookID);
        }
        $(".selC").show();
        //选择课本切换效果
        $(".slideWrap").slide({ mainCell: "ul.bd", effect: "leftLoop", vis: 3, scroll: 1, autoPlay: false })
        //课本选中点击效果
        $("#ulBook li a").bind("click", function () {
            $("#ulBook li a").removeClass("on");
            $(this).addClass("on");
            $("#selBook img").attr("src", $(this).attr("bookCover"));
            $("#selBook .info").html(parentEdition.EditionName + " " + $(this).attr("editionName") + " " + $(this).attr("bookName"));
            //切换课本时重新加载单元和题目
            $.post("?action=GetUnits&rand=" + Math.random(), { BookID: $(this).attr("bookID"), LastUnitName: lastUnitName, ClassTaskID: classTaskID }, function (data) {
                if (data) {
                    var result = eval("(" + data + ")");//JSON.parse
                    if (result.Success) {
                        lastUnitID = result.Data.LastUnitID;
                        selBookID = result.Data.BookID;
                        loadUnits(result.Data.UnitList, result.Data.QuestionList, result.Data.MarkObject);
                        //loadQuestions(result.Data.QuestionList, lastUnitID);
                    } else {
                        $("#unitList").html("");
                        $(".cloosePart").hide();
                        alert(result.Message);
                    }
                }
            });
        });
    } else {
        $("#selBook img").attr("src", "");
        $("#selBook .info").html("");
        $("#ulBook").html("");
        $("#unitList").html("");
        $(".selC").show();
        $(".cloosePart").hide();
    }
}
//加载单元
function loadUnits(listUnit, listQuestion, markObj) {
    if (listUnit.length > 0) {
        var unitListHtml = '';
        var isMatch = false;//标记是否找到匹配单元项
        $.each(listUnit, function (index, value) {
            if (value.UnitID == lastUnitID) {
                //lastUnitID = value.UnitID;
                isMatch = true;
                unitListHtml += '<li><a class="on" unitid="' + value.UnitID + '" keyword="' + value.KeyWord + '" unitname="' + value.UnitName
                + '" onclick="chooseUnit(this)" href="javascript:void(0)">' + (value.KeyWord + ' ' + value.UnitName)
                + '</a><span unitid="' + value.UnitID + '"></span></li>';
            } else {
                unitListHtml += '<li><a unitid="' + value.UnitID + '" keyword="' + value.KeyWord + '" unitname="' + value.UnitName
                + '" onclick="chooseUnit(this)" href="javascript:void(0)">' + (value.KeyWord + ' ' + value.UnitName)
                + '</a><span unitid="' + value.UnitID + '"></span></li>';
            }
        });
        if (!isMatch) {
            lastUnitID = listUnit[0].UnitID;
            lastUnitName = listUnit[0].KeyWord + " " + listUnit[0].UnitName;
            //$('#unitList li a[unitid="' + lastUnitID + '"]').addClass("on");
        }
        $('#unitList li a[unitid="' + lastUnitID + '"]').addClass("on");
        $("#unitList").html(unitListHtml);
        $("#unitName h4").html(lastUnitName + "<span>（已选0项）</span>");
        loadQuestions(listQuestion, markObj);
        //chooseUnit($('#unitList li a[unitid="' + lastUnitID + '"]'));
    } else {
        $("#unitList").html("");
        $(".cloosePart").hide();
    }
}
//选择单元
function chooseUnit(obj) {
    $("#unitList li a").removeClass("on");
    $(obj).addClass("on");
    lastUnitID = $(obj).attr("unitid");
    lastUnitName = $(obj).attr("keyword") + " " + $(obj).attr("unitname");
    $("#unitName h4").html(lastUnitName + "<span>（已选0项）</span>");
    $.post("?action=GetQuestions&UnitID=" + lastUnitID + "&rand=" + Math.random(), { UnitID: lastUnitID, ClassTaskID: classTaskID }, function (data) {
        if (data) {
            var result = eval("(" + data + ")");//JSON.parse
            if (result.Success) {
                loadQuestions(result.Data.QuestionList, result.Data.MarkObject);
            } else {
                $(".cloosePart").hide();
            }
        }
    });
}
//加载题目
function loadQuestions(listQuestion,markObj) {
    if (listQuestion.length > 0) {
        var section = "";
        var qStr = '';
        $.each(listQuestion, function (index, value) {
            if (section == "") {
                section = value.Section;
                qStr += '<div class="partC"><h5>' + (section.indexOf("*") >= 0 ? "" : section) + '</h5><ul class="partBox">';
            } else if (section != value.Section) {
                section = value.Section;
                qStr += '</ul></div><div class="clearfix"></div>'
                    + '<div class="partC"><h5>' + (section.indexOf("*") >= 0 ? "" : section) + '</h5><ul class="partBox">';
            }
            qStr += '<li questionid="' + value.QuestionID + '" unitid="' + value.UnitID
                + '" questionModel="' + value.QuestionModel + '" questionTime="' + value.QuestionTime + '">'
                + '<a class="aS"><em>&nbsp;</em>'
                + '<table><tr><td>' + value.QuestionTitle + '</td></tr></table></a>'
                + '<a class="preview">预览</a></li>';
            if (index == listQuestion.length - 1) {
                qStr += '</ul></div><div class="clearfix"></div>';
            }
        });
        $("#chooseQues").html(qStr);
        Common.AutoPosition();
        markAll(markObj);
        $(".cloosePart").show();
        autoPositionBar();
        //选择题目
        $(".partBox li a.aS").bind("click", function () {
            if ($(this).parent("li").attr("class") == "on") {
                $(this).parent("li").removeClass("on");
                saveUnitAndQuestion($(this).parent("li"), 0);
            } else {
                if (qCnt >= 100) {
                    alert("已选择100个大题！再多学生就负担不了咯~");
                    return;
                }
                $(this).parent("li").addClass("on");
                saveUnitAndQuestion($(this).parent("li"), 1);
            }
        });
        //预览题目
        $(".partBox li a.preview").bind("click", function () {
            if ($(this).parent("li").attr("class") == "on") {
            } else {
                if (qCnt >= 100) {
                    alert("已选择100个大题！再多学生就负担不了咯~");
                    return;
                }
                $(this).parent("li").addClass("on");
                saveUnitAndQuestion($(this).parent("li"), 1);
            }
            if (saveClassTask() != "") {
                if (subject == 1) {
                    location.href = "../ChineseModels/DoQuestion.aspx?AccessType=1&QuestionID=" + $(this).parent("li").attr("questionid") + "&Round=1&ClassTaskID=" + classTaskID + "&TaskName=" + escape(classPackName) + "&backurl=" + backurl;
                } else if (subject == 2) {
                    location.href = "../MathModels/DoQuestion.aspx?AccessType=1&QuestionID=" + $(this).parent("li").attr("questionid") + "&Round=1&ClassTaskID=" + classTaskID + "&TaskName=" + escape(classPackName) + "&backurl=" + backurl;
                } else {
                    location.href = "../QuestionModels/DoQuestion.aspx?AccessType=1&QuestionID=" + $(this).parent("li").attr("questionid") + "&Round=1&ClassTaskID=" + classTaskID + "&TaskName=" + escape(classPackName) + "&backurl=" + backurl;
                }
            } else {
                alert("保存课时信息失败，请重试！");
            }
        });
    } else {
        $(".cloosePart").hide();
    }
}

//缓存单元和题目
function saveUnitAndQuestion(obj, isAdd) {
    if (isAdd == 1) {
        $.post("?action=AddQuestion", { QuestionID: $(obj).attr("questionid"), ClassTaskID: classTaskID, Round: 1, UnitID: lastUnitID }, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    classTaskID = result.Data.ClassTaskID;//获取新创建的ClassTaskID
                    markAll(result.Data.MarkObject);
                } else {
                    $(obj).parent("li").removeClass("on");
                }
            }
        });
    } else {
        $.post("?action=DeleteQuestion", { QuestionID: $(obj).attr("questionid"), ClassTaskID: classTaskID, UnitID: lastUnitID }, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    markAll(result.Data.MarkObject);
                } else {
                    $(obj).parent("li").addClass("on");
                }
            }
        });
    }
    
}

//标记课本
function markBook(bookID) {
    if (bookID) {
        selBookID = bookID;
        var tmpBook = $('#ulBook li a[bookID="' + selBookID + '"]');
        $(tmpBook).addClass("on");
        $("#selBook img").attr("src", $(tmpBook).attr("bookCover"));
        $("#selBook .info").html(parentEdition.EditionName + " " + $(tmpBook).attr("editionName") + " " + $(tmpBook).attr("bookName"));
    }
}
//遍历标记每个单元和题目
function markAll(markObj) {
    if (markObj) {
        qCnt = 0;
        qTime = 0;
        unitArray = markObj.UnitList;
        for (var i = 0; i < unitArray.length; i++) {
            qCnt += unitArray[i].QCount;
            qTime += unitArray[i].QTime;
            $('#unitList li span[unitid="' + unitArray[i].UnitID + '"]').html("已选" + unitArray[i].QCount + "项");
        }
        if (markObj.QuestionList) {
            questionArray = markObj.QuestionList;
            $("#unitName h4 span").html("（已选" + questionArray.length + "项）");
            for (var i = 0; i < questionArray.length; i++) {
                $('.partBox li[questionid="' + questionArray[i].QuestionID + '"]').attr("class", "on");
            }
        } else {
            $("#unitName h4 span").html("（已选0项）");
        }
        $(".titSp").html("总时长" + (qTime / 60) + "分钟 共选择" + qCnt + "项");
    } else {
        $('#unitList li span[unitid="' + lastUnitID + '"]').html("");
        $("#unitName h4 span").html("（已选0项）");
        $(".titSp").html("总时长0分钟 共选择0项");
    }
}

function initData() {
    classTaskID = Common.QueryString.GetValue("ClassTaskID");
    classPackName = unescape(Common.QueryString.GetValue("TaskName"));
    if (classTaskID == "undefined" || classTaskID == "") {
        classTaskID = "";
        mod_ed = Common.QueryString.GetValue("ED");
        gradeID = Common.QueryString.GetValue("GD");
        bookReel = Common.QueryString.GetValue("BR");
        autoChoose('#selGrade', gradeID);
        autoChoose('#selBookReel', bookReel);
        loadBookUnitQuestion();
    } else {
        $.post("?action=GetClassTask", { ClassTaskID: classTaskID }, function (data) {
            if (data) {
                var result = eval("(" + data + ")");//JSON.parse
                if (result.Success) {
                    var classTask = result.Data.ClassTask;
                    mod_ed = classTask.MOD_ED;
                    gradeID = classTask.GradeID;
                    bookReel = classTask.BookReel;
                    lastUnitID = classTask.LastUnitID;
                    selBookID = classTask.BookID;
                    autoChoose('#selGrade', gradeID);
                    autoChoose('#selBookReel', bookReel);
                    loadBookUnitQuestion();
                } else {
                    alert("获取课时信息失败！");
                }
            }
        });
    }
}

//清除所选题目
function clearQuestion() {
    $.post("?action=ClearQuestion", { ClassTaskID: classTaskID });
}

function nextStep() {
    if (qCnt == 0) {
        alert("还没有选择题目哦！");
    } else {
        if (saveClassTask() != "") {
            location.href = "TaskPreview.aspx?ClassTaskID=" + classTaskID + "&TaskName=" + escape(classPackName) + "&backurl=" + backurl;
        } else {
            alert("保存课时信息失败，请重试！");
        }
        //$.post("?action=SaveClassTask", { ED: mod_ed, GradeID: gradeID, BookReel: bookReel, BookID: selBookID, UnitID: lastUnitID, ClassTaskID: classTaskID }, function (data) {
        //    if (data) {
        //        var result = eval("(" + data + ")");//JSON.parse
        //        if (result.Success) {
        //            location.href = "TaskPreview.aspx?ClassTaskID=" + classTaskID;
        //        } else {
        //            alert("保存课时信息失败，请重试！");
        //        }
        //    }
        //});
    }
}

function saveClassTask() {
    $.post("?action=SaveClassTask", { ED: mod_ed, GradeID: gradeID, BookReel: bookReel, BookID: selBookID, UnitID: lastUnitID, ClassTaskID: classTaskID });
    return classTaskID;
}