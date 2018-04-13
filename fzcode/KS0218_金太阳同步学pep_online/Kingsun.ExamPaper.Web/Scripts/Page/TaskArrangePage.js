/// <reference path="../jquery.json-2.4.js" />
/// <reference path="../../App_Themes/js/jquery.cookie.js" />
/// <reference path="../Common.js" />

//布置作业页面js

//按F5键，清空缓存
document.body.onkeydown = function (event) {
    event = event ? event : window.event;
    //F5键
    if (event.keyCode == 116) {
        if (confirm("确认刷新吗？刷新会清空已选的题目哦！")) {
            clearCookie(1);
        } else {
            if (event.preventDefault) {
                event.preventDefault();
            } else {
                event.returnValue = false;
            }
        }
    }
    // 将退格键屏蔽
    if (event.keyCode == 8) {
        if (event.preventDefault) {
            event.preventDefault();
        } else {
            event.returnValue = false;
        }
    }
};

var classArray = new Array();//存储选择的班级
var unitArray = new Array();//存储选择的单元
var questionArray = new Array();//存储选择的题目
var qCnt = 0, qTime = 0, selBookID = 0, matchUnitID = 0, lastUnitName = "";

var classGradeID = 0;//班级所在年级ID
var gradeID = 0;//下拉框选择年级ID
var bookReel = 0, currentTerm = 0;
var parentEdition;
var Subject = 0;
$(function () {
    $(".topDiv").hide();
    $(".contDiv").hide();
    $(".selC").hide();
    $(".cloosePart").hide();
    $("#defaultPageClass").hide();
    //加载单元列表的滚动条
    $("#unitList").niceScroll({ touchbehavior: false, autohidemode: false, cursorcolor: "#fbc579", cursoropacitymax: 1, cursorwidth: 10 });
    //布置作业选择班级的展开与收缩
    //初始化
    $(".selC").hide();
    $(".slideWrap a.prev").hide();
    $(".slideWrap a.next").hide();
    $(".cloosePart").hide();
    if ($("#ulClass li").length <= 10) {
        $(".unfold").hide();
    }
    var now = new Date();
    now.setDate(now.getDate() + 1);//当前日期加一天
    $("#deadline").val(Common.GetDate(now) + " 23:59:59");
    autoSetWeek();

    Common.CheckIndexOf();

    //获取班级列表
    $.post("?action=GetClassList&rand=" + Math.random(), function (result) {
        if (result) {
            result = eval("(" + result + ")");//JSON.parse
            if (result.Success) {
                toggleDefaultPage(0);
                bookReel = result.Data.CurrentTerm;
                currentTerm = bookReel;
                Subject = result.Data.Subject;
                autoChoose('#selBookReel', bookReel);
                //加载班级
                var listClass = result.Data.ClassList;
                $.each(listClass, function (index, value) {
                    $("#ulClass").append('<li><a onclick="chooseClass(this)" href="javascript:void(0)" classid="' + value.ClassID
                        + '" gradeid="' + value.GradeID + '" isEmpty="' + value.IsEmpty + '">' + value.ClassName + '</a></li>');
                });
                if ($("#ulClass li").length > 10) {
                    $(".unfold").show();
                }
                loadCookie();
                //Common.AutoPosition();
            }
            else {
                //alert(result.Message);
                toggleDefaultPage(1);
            }
            Common.AutoPosition();
        }
    });
    $(".unfold").bind("click", function () {
        if ($("#ulClass li").length > 10) {
            $("#ulClass").addClass("on");
            $(this).hide();
            $(".shrink").show();
        } else {
            $("#ulClass").removeClass("on");
            $(this).show();
            $(".shrink").hide();
        }
        $('#unitList').getNiceScroll().resize();
    });
    $(".shrink").bind("click", function () {
        if ($("#ulClass li").length > 10) {
            $("#ulClass").removeClass("on");
            $(this).hide();
            $(".unfold").show();
        } else {
            $("#ulClass").addClass("on");
            $(this).show();
            $(".unfold").hide();
        }
        $('#unitList').getNiceScroll().resize();
    });
    //选择年级
    $("#selGrade").click(function () {
        //当所选年级与班级所在年级不一致时
        if (gradeID != $("#selGrade select").val()) {
            if (confirm("您确定要更换年级吗？当前选择的题目将会被清空！")) {
                clearCookie(0);
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
                clearCookie(0);
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

//切换无班级的缺省页
function toggleDefaultPage(flag) {
    if (flag == 1) {
        $(".topDiv").hide();
        $(".contDiv").hide();
        $(".selC").hide();
        $(".cloosePart").hide();
        $("#defaultPageClass").show();
        $(".span").html('您还没有班级哦，请联系学校管理员创建班级吧！');
    } else {
        $("#defaultPageClass").hide();
        $(".topDiv").show();
        $(".contDiv").show();

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
//选择班级
function chooseClass(obj) {
    if ($(obj).attr("isEmpty") == "1") {
        alert("此班级没有学生，无法选择！");
        return;
    }
    var selGradeID = parseInt($(obj).attr("gradeid"));
    //取消选中班级
    if ($(obj).attr("class") == "on") {
        $(obj).removeClass("on");
    } else {//选中班级
        if (classGradeID == 0) {
            classGradeID = selGradeID;
            gradeID = selGradeID;
            $(obj).addClass("on");
            autoChoose('#selGrade', gradeID);
            loadBookUnitQuestion();
        } else if (classGradeID == selGradeID) {
            $(obj).addClass("on");
        } else {
            if (confirm("当前已选择" + Common.GetChineseNum(classGradeID - 1) + "年级，选择" + Common.GetChineseNum(selGradeID - 1) + "年级则放弃" + Common.GetChineseNum(classGradeID - 1) + "年级班级！")) {
                clearCookie(1);
                $("#ulClass li a").removeClass("on");
                $(obj).addClass("on");
                classGradeID = selGradeID;
                gradeID = selGradeID;
                autoChoose('#selGrade', gradeID);
                loadBookUnitQuestion();
            }
        }
    }
    saveClass();
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
    $.post("?action=GetBooks&rand=" + Math.random(), { GradeID: gradeID, BookReel: bookReel, UnitID: matchUnitID }, function (result) {
        if (result) {
            result = eval("(" + result + ")");//JSON.parse
            if (result.Success) {
                parentEdition = result.Data.ParentEdition;
                $("#spParentEdition").text(parentEdition.EditionName);
                if (result.Message == "") {
                    lastUnitName = result.Data.LastUnitName;
                    //加载课本
                    loadBooks(result.Data.BookList, result.Data.BookID);
                    //加载单元
                    loadUnits(result.Data.UnitList);
                    //加载题目
                    loadQuestions(result.Data.QuestionList, matchUnitID);
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
                $("#ulClass li a.on").removeClass("on");
                clearCookie(1);
                classGradeID = 0;
                bookReel = currentTerm;
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
function loadBooks(listBook, defaultBookID) {
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
        if (Common.Cookie.getcookie("BookID") > 0) {
            markBook(Common.Cookie.getcookie("BookID"));
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
            $.post("?action=GetUnits&rand=" + Math.random(), { BookID: $(this).attr("bookID"), LastUnitName: lastUnitName }, function (result) {
                if (result) {
                    result = eval("(" + result + ")");//JSON.parse
                    if (result.Success) {
                        matchUnitID = result.Data.MatchUnitID;
                        selBookID = result.Data.BookID;
                        loadUnits(result.Data.UnitList);
                        loadQuestions(result.Data.QuestionList, matchUnitID);
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
function loadUnits(listUnit) {
    if (listUnit.length > 0) {
        var unitListHtml = '';
        var isMatch = false;//标记是否找到匹配单元项
        $.each(listUnit, function (index, value) {
            if ((value.KeyWord + " " + value.UnitName) == lastUnitName) {
                matchUnitID = value.UnitID;
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
            matchUnitID = listUnit[0].UnitID;
            lastUnitName = listUnit[0].KeyWord + " " + listUnit[0].UnitName;
            $('#unitList li a[unitid="' + matchUnitID + '"]').addClass("on");
        }
        $("#unitList").html(unitListHtml);
        //markAllUnit(matchUnitID);
        chooseUnit($('#unitList li a[unitid="' + matchUnitID + '"]'));
    } else {
        $("#unitList").html("");
        $(".cloosePart").hide();
    }
}
//选择单元
function chooseUnit(obj) {
    $("#unitList li a").removeClass("on");
    $(obj).addClass("on");
    matchUnitID = $(obj).attr("unitid");
    lastUnitName = $(obj).attr("keyword") + " " + $(obj).attr("unitname");
    $("#unitName h4").html(lastUnitName + "<span>（已选0项）</span>");
    markAllUnit(matchUnitID);
    $.post("?action=GetQuestions&UnitID=" + matchUnitID + "&rand=" + Math.random(), { UnitID: matchUnitID }, function (result) {
        if (result) {
            result = eval("(" + result + ")");//JSON.parse
            if (result.Success) {
                loadQuestions(result.Data.QuestionList, $(obj).attr("unitid"));
            } else {
                $(".cloosePart").hide();
                alert(result.Message);
            }
        }
    });
}
//加载题目
function loadQuestions(listQuestion, unitid) {
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
                + '<table><tr><td>' + (value.QuestionTitle[0] == "*" ? value.QuestionTitle.substring(1) : value.QuestionTitle) + '</td></tr></table></a>'
                + '<a class="preview">预览</a></li>';
            if (index == listQuestion.length - 1) {
                qStr += '</ul></div><div class="clearfix"></div>';
            }
        });
        $("#chooseQues").html(qStr);
        Common.AutoPosition();
        markQuestions(unitid);
        $(".cloosePart").show();
        autoPositionBar();
        //选择题目
        $(".partBox li a.aS").bind("click", function () {
            if ($(this).parent("li").attr("class") == "on") {
                $(this).parent("li").removeClass("on");
                saveUnitAndQuestion($(this).parent("li"), 0);
            } else {
                if (qCnt >= 20) {
                    alert("已选20个大题,再多学生就负担不了咯~");
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
            //分学科跳转
            if (Subject == 1) {
                location.href = "../ChineseModels/DoQuestion.aspx?AccessType=1&QuestionID=" + $(this).parent("li").attr("questionid")
                    + "&Round=" + Common.Cookie.getQueRound("UnitID" + $(this).parent("li").attr("unitid"), $(this).parent("li").attr("questionid"))
                 + (window.location.search ? "&Csstype=cloudHeader" : "");
            } else if (Subject == 2) {
                location.href = "../MathModels/DoQuestion.aspx?AccessType=1&QuestionID=" + $(this).parent("li").attr("questionid")
                    + "&Round=" + Common.Cookie.getQueRound("UnitID" + $(this).parent("li").attr("unitid"), $(this).parent("li").attr("questionid"))
                 + (window.location.search ? "&Csstype=cloudHeader" : "");
            } else {
                location.href = "../QuestionModels/DoQuestion.aspx?AccessType=1&QuestionID=" + $(this).parent("li").attr("questionid")
                    + "&Round=" + Common.Cookie.getQueRound("UnitID" + $(this).parent("li").attr("unitid"), $(this).parent("li").attr("questionid"))
                 + (window.location.search ? "&Csstype=cloudHeader" : "");
            }
        });
    } else {
        $(".cloosePart").hide();
    }
}


//保存班级到classArray
function saveClass() {
    classArray = [];
    var ulclass = $("#ulClass li a.on");
    if (ulclass) {
        if (ulclass.length == 0) {
            classGradeID = 0;
            clearCookie(1);
            $(".selC").hide();
            $(".cloosePart").hide();
            Common.AutoPosition();
        } else {
            for (var i = 0; i < ulclass.length; i++) {
                classArray.push({ CID: $(ulclass[i]).attr("classid"), CName: $(ulclass[i]).text() });
            }
        }
    } else {
        classGradeID = 0;
        clearCookie(1);
        $(".selC").hide();
        $(".cloosePart").hide();
        Common.AutoPosition();
    }

}

//缓存单元和题目
function saveUnitAndQuestion(obj, isAdd) {
    var qModel = $(obj).attr("questionModel");//M1和M2为跟读题，默认1次
    var tmpTime = parseInt($(obj).attr("questionTime"));
    var tmpRound = 1;
    var ques = { QID: $(obj).attr("questionid"), UID: $(obj).attr("unitid"), Round: tmpRound };
    var unitIndex = -1;
    for (var i = 0; i < unitArray.length; i++) {
        if (unitArray[i].UID == ques.UID) {
            unitIndex = i;
            break;
        }
    }
    if (isAdd == 1) {
        //添加题目时，累计单元已选项
        questionArray = Common.Cookie.insertQue("UnitID" + ques.UID, ques);
        Common.Cookie.setcookie("UnitID" + ques.UID, $.toJSON(questionArray));
        qCnt++;
        qTime += tmpRound * tmpTime;
        if (unitIndex == -1) {
            unitArray.push({ UID: ques.UID });
            unitIndex = unitArray.length - 1;
        }
    } else {
        tmpRound = Common.Cookie.getQueRound("UnitID" + ques.UID, ques.QID);
        //移除题目时，减少单元已选项
        questionArray = Common.Cookie.deleteQue("UnitID" + ques.UID, ques.QID);
        qCnt--;
        qTime -= tmpRound * tmpTime;
        if (questionArray.length == 0) {
            unitArray = Common.DeleteArray(unitArray, unitIndex);
            Common.Cookie.delcookie("UnitID" + ques.UID);
        }
        else {
            Common.Cookie.setcookie("UnitID" + ques.UID, $.toJSON(questionArray));
        }
    }
    markSingleUnit(questionArray.length, ques.UID);
    markTotalData();
    //更新缓存
    saveCookie();
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
//标记当前单元
function markSingleUnit(count, unitid) {
    $("#unitName h4 span").html("（已选" + count + "项）");
    if (count > 0) {
        $('#unitList li span[unitid="' + unitid + '"]').html("已选" + count + "项");
    } else {
        $('#unitList li span[unitid="' + unitid + '"]').html("");
    }
}
//标记总时长和总项数
function markTotalData() {
    $(".titSp").html("总时长" + (qTime / 60) + "分钟 共选择" + qCnt + "项");
}
//遍历标记每个单元
function markAllUnit(unitid) {
    questionArray = Common.Cookie.getcookieArray("UnitID" + unitid);
    for (var i = 0; i < unitArray.length; i++) {
        $('#unitList li span[unitid="' + unitArray[i].UID + '"]').html("已选" + Common.Cookie.getcookieArray("UnitID" + unitArray[i].UID).length + "项");
    }
    $("#unitName h4 span").html("（已选" + questionArray.length + "项）");

    markTotalData();
}
//遍历标记每个题目
function markQuestions(unitid) {
    questionArray = Common.Cookie.getcookieArray("UnitID" + unitid);
    for (var i = 0; i < questionArray.length; i++) {
        $('.partBox li[questionid="' + questionArray[i].QID + '"]').attr("class", "on");
    }
}

//从Cookie中加载上次数据
function loadCookie() {
    //判断是否切换了学科
    if (Common.Cookie.getcookie("TaskSub") != Subject) {
        clearCookie(1);
    } else {
        var tmpDeadline = Common.Cookie.getcookie("Deadline");
        if (tmpDeadline) {
            $("#deadline").val(tmpDeadline);
            autoSetWeek();
        }
        classArray = Common.Cookie.getcookieArray("ClassList");
        if (classArray.length > 0) {
            for (var i = 0; i < classArray.length; i++) {
                $('#ulClass li a[classid="' + classArray[i].CID + '"]').addClass("on");
            }
            if (Common.Cookie.getcookie("GradeID")) {
                gradeID = Common.Cookie.getcookie("GradeID");
                classGradeID = gradeID;
                autoChoose('#selGrade', gradeID);
            }
            if (Common.Cookie.getcookie("BookReel")) {
                bookReel = Common.Cookie.getcookie("BookReel");
                autoChoose('#selBookReel', bookReel);
            }
            if (Common.Cookie.getcookieArray("UnitList")) {
                unitArray = Common.Cookie.getcookieArray("UnitList");
            }
            if (Common.Cookie.getcookie("QuestionCount")) {
                qCnt = parseInt(Common.Cookie.getcookie("QuestionCount"));
            }
            if (Common.Cookie.getcookie("QuestionTimes")) {
                qTime = parseInt(Common.Cookie.getcookie("QuestionTimes"));
            }
            if (Common.Cookie.getcookie("LastUnitID")) {
                matchUnitID = Common.Cookie.getcookie("LastUnitID");
                if (Common.Cookie.getcookieArray("UnitID" + matchUnitID)) {
                    questionArray = Common.Cookie.getcookieArray("UnitID" + matchUnitID);
                    if (questionArray.length > 0) {
                        loadBookUnitQuestion();
                    }
                }
            }
        }
    }
}


//将页面数据存储Cookie（班级、课本、单元、题目）
function saveCookie() {
    Common.Cookie.setcookie("TaskSub", Subject);
    //缓存交作业时间
    if ($("#deadline").val() != "") {
        Common.Cookie.setcookie("Deadline", $("#deadline").val());
    } else {
        Common.Cookie.delcookie("Deadline");
    }
    //缓存班级列表
    if (classArray.length > 0) {
        Common.Cookie.setcookie("ClassList", $.toJSON(classArray));
    } else {
        Common.Cookie.delcookie("ClassList");
    }
    //缓存单元列表
    if (unitArray.length > 0) {
        Common.Cookie.setcookie("UnitList", $.toJSON(unitArray));
    } else {
        Common.Cookie.delcookie("UnitList");
    }
    //缓存选择的年级
    if (gradeID > 0) {
        Common.Cookie.setcookie("GradeID", gradeID);
    } else {
        Common.Cookie.delcookie("GradeID");
    }
    //缓存选择的册别
    if (bookReel > 0) {
        Common.Cookie.setcookie("BookReel", bookReel);
    } else {
        Common.Cookie.delcookie("BookReel");
    }
    //缓存最新选择的课本
    if (selBookID > 0) {
        Common.Cookie.setcookie("BookID", selBookID);
    } else {
        Common.Cookie.delcookie("BookID");
    }
    //缓存最近选择的单元
    if (matchUnitID > 0) {
        Common.Cookie.setcookie("LastUnitID", matchUnitID);
    } else {
        Common.Cookie.delcookie("LastUnitID");
    }
    //缓存总耗时（秒）
    if (qTime > 0) {
        Common.Cookie.setcookie("QuestionTimes", qTime);
    } else {
        Common.Cookie.delcookie("QuestionTimes");
    }
    //缓存总题数
    if (qCnt > 0) {
        Common.Cookie.setcookie("QuestionCount", qCnt);
    } else {
        Common.Cookie.delcookie("QuestionCount");
    }
}
//清除缓存
function clearCookie(isClearClass) {
    if (isClearClass == 1) {
        Common.Cookie.delcookie("TaskSub");
        Common.Cookie.delcookie("Deadline");
        Common.Cookie.delcookie("ClassList");
        classArray = [];
    }
    Common.Cookie.delcookie("TaskTitle");
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
    unitArray = [];
    questionArray = [];
    selBookID = 0;
    matchUnitID = 0;
    qTime = 0;
    qCnt = 0;
}

function nextStep() {
    saveCookie();
    if (qCnt == 0) {
        alert("还没有选择题目哦！");
    } else {
        location.href = "TaskBasket.aspx" + window.location.search;
    }
}