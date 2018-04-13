
var LR_ClassListInit = function () {
    var Current = this;
    this.UserID = window.Common.QueryString.GetValue("UserID");
    this.EditionID = window.Common.QueryString.GetValue("EditionID");
    this.ClassID = window.Common.QueryString.GetValue("ClassID");
    this.Time = window.Common.QueryString.GetValue("Time");

    this.Init = function () {
        Current.GetLearningReport();

        //Current.GetGradeName();
        Current.GetGradeState();
    };

    function getGradeNum(e) {
        switch (e) {
            case "一年级上册":
                $("#li1 p").css("display", "none");
                $("#li1").removeClass("wu");
                break;
            case "一年级下册":
                $("#li2 p").css("display", "none");
                $("#li2").removeClass("wu");
                break;
            case "二年级上册":
                $("#li3 p").css("display", "none");
                $("#li3").removeClass("wu");
                break;
            case "二年级下册":
                $("#li4 p").css("display", "none");
                $("#li4").removeClass("wu");
                break;
            case "三年级上册":
                $("#li5 p").css("display", "none");
                $("#li5").removeClass("wu");
                break;
            case "三年级下册":
                $("#li6 p").css("display", "none");
                $("#li6").removeClass("wu");
                break;
            case "四年级上册":
                $("#li7 p").css("display", "none");
                $("#li7").removeClass("wu");
                break;
            case "四年级下册":
                $("#li8 p").css("display", "none");
                $("#li8").removeClass("wu");
                break;
            case "五年级上册":
                $("#li9 p").css("display", "none");
                $("#li9").removeClass("wu");
                break;
            case "五年级下册":
                $("#li10 p").css("display", "none");
                $("#li10").removeClass("wu");
                break;
            case "六年级上册":
                $("#li11 p").css("display", "none");
                $("#li11").removeClass("wu");
                break;
            case "六年级下册":
                $("#li12 p").css("display", "none");
                $("#li12").removeClass("wu");
                break;
            default:
                break;
        }
    }

    //根据学习报告启用下拉按钮
    this.GetGradeState = function () {
        var obj = { ClassID: Current.ClassID, EditionID: Current.EditionID };
        $.post("../../Handler/LR_WeChatHandler.ashx?queryKey=GetGradeInfo", obj, function (data) {
            if (data) {
                var result = JSON.parse(data);
                if (result.Success) {
                    var str = JSON.parse(result.data);
                    for (var i = 0; i < str.length; i++) {
                        getGradeNum(str[i].GradeName);
                    }
                }
            }
        });
    }

    ////修改显示为X年级下册
    //this.GetGradeName = function () {
    //    var obj = { ClassID: Current.ClassID, EditionID: Current.EditionID };
    //    $.post("../../Handler/LR_WeChatHandler.ashx?queryKey=getclassinfobyclassid", obj, function (data) {
    //        if (data) {
    //            var result = JSON.parse(data);
    //            if (result.Success) {
    //                var str = JSON.parse(result.data);
    //                $("#span").html(str[1].GradeName);
    //                $("#span").attr("bookid", str[1].BookID);
    //            }
    //        }
    //    });
    //}

    //获取当前日期学习数据 
    this.GetLearningReport = function () {
        var obj = { UserID: Current.UserID, ClassID: Current.ClassID, EditionID: Current.EditionID, Time: Current.Time };
        $.post("../../Handler/LR_WeChatHandler.ashx?queryKey=QueryLearningReport", obj, function (data) {
            if (data) {
                var result = JSON.parse(data);
                if (result.Success) {
                    var classhtml = "";
                    var mc = JSON.parse(result.ModuleConfig);
                    for (var i = 0; i < mc.length; i++) {
                        if (i == 0) {
                            if (result.stuNum > 0) {
                                classhtml += "<div class=\"nr1\">";
                                classhtml += "<p id=\"" + mc[i].id + "\">" + mc[i].name + "</p>";
                                classhtml += "<ul>";
                                classhtml += "<li><a href=\"#\"><span>趣配音</span><div class=\"percentage\"><span class=\"dd_s\"></span></div><b><i class=\"i_num\">" + result.stuNum + "</i>/<em class=\"em_num\">" + result.classStuNum + "</em><img src=\"../../AppTheme/images/xiugai.png\" alt=\"\"/></b></a></li>";
                                classhtml += "</ul>";
                                classhtml += "</div>";
                            } else {
                                classhtml += "<div class=\"nr3\">";
                                classhtml += "<p id=\"" + mc[i].id + "\">" + mc[i].name + "</p>";
                                classhtml += " <h2>暂无记录</h2>";
                                classhtml += "</div>";
                            }
                        } else {
                            classhtml += "<div class=\"nr2\">";
                            classhtml += "<p id=\"" + mc[i].id + "\">" + mc[i].name + "</p>";
                            classhtml += "<a id=\"" + mc[i].id + "\" onclick=\"return GetModuleStuNum(this.id)\">点击查看报告<img src=\"../../AppTheme/images/seebao.png\" alt=\"\"/></a>";
                            classhtml += "</div>";
                        }
                    }
                    $("#ht").html(classhtml);
                    $("#span").html(result.GradeName);
                    $("#span").attr("bookid", result.BookID);
                }
            }
        });
    };
};

function GetModuleStuNum(id) {
    var obj = { ClassID: window.Common.QueryString.GetValue("ClassID"), Time: window.Common.QueryString.GetValue("Time"), BookID: $("#span").attr("bookid"), Fid: id };
    $.post("../../Handler/LR_WeChatHandler.ashx?queryKey=getmoduleinfobybookid", obj, function (data) {
        if (data) {
            var result = JSON.parse(data);
            if (result.Success) {
                var classhtml = "";
                if (result.stuNum > 0) {
                    classhtml += "<div class=\"nr1\">";
                   // classhtml += "<p id=\"" + mc[i].id + "\">" + mc[i].name + "</p>";
                    classhtml += "<ul>";
                    classhtml += "<li><a href=\"#\"><span>趣配音</span><div class=\"percentage\"><span class=\"dd_s\"></span></div><b><i class=\"i_num\">" + result.stuNum + "</i>/<em class=\"em_num\">" + result.classStuNum + "</em><img src=\"../../AppTheme/images/xiugai.png\" alt=\"\"/></b></a></li>";
                    classhtml += "</ul>";
                    classhtml += "</div>";
                } else {
                    classhtml += "<div class=\"nr3\">";
                  //  classhtml += "<p id=\"" + mc[i].id + "\">" + mc[i].name + "</p>";
                    classhtml += " <h2>暂无记录</h2>";
                    classhtml += "</div>";
                }
                $("#ht").html(classhtml);
              
            }
        }
    });
}

var LR_ClassListInit;

$(function () {
    LR_ClassListInit = new LR_ClassListInit();
    LR_ClassListInit.Init();
});
