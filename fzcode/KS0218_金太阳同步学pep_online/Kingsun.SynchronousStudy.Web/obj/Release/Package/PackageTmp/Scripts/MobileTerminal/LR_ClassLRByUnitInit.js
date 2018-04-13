
var LR_ClassLRByUint = function () {
    var Current = this;
    this.StuNum = Common.QueryString.GetValue("StuNum"); //班级总人数
    this.ClassID = Common.QueryString.GetValue("ClassID"); //班级ID
    this.UserID = Common.QueryString.GetValue("UserID"); //班级ID
    this.time = Common.QueryString.GetValue("Time");     //查询时间
    this.GradeID = Common.QueryString.GetValue("GradeID");
    this.Init = function () {
        //Current.ChangeSel($("#value2").val());
    };

    //切换选择条件，读取数据
    this.ChangeSel = function (unitId) {
        //alert(unitId);
        if (unitId != "" && unitId != undefined && unitId != null && Current.StuNum != undefined
               && Current.ClassID != undefined && Current.UserID != undefined && Current.time != undefined) {
            var obj = { ClassID: (Current.ClassID == undefined ? "" : Current.ClassID), UnitId: unitId, time: (Current.time == undefined ? "" : Current.time) };
            $.post("../../Handler/LR_WeChatHandler.ashx?queryKey=GetVideoDetailsCount", obj, function (data) {
                if (data) {
                    var result = JSON.parse(data);
                    if (result.Success) {
                        if (result.Data != "0") {

                            var sURL = "../LearningReportInfo/LR_ClassInfo.aspx?&ClassID=" + Current.ClassID + "&StuNum=" + Current.StuNum + "&LrNum=" + result.Data + "&BookID="
                                        + $("#value1").val().split('_')[1] + "&UserID=" + Current.UserID + "&Times=" + Current.time + "&GradeID=" + Current.GradeID ;
                            var shtml = "<li><a href=\"" + sURL + "\"><span>趣配音</span>"
                                        + "<div class=\"percentage\"><span class=\"dd_s\"></span></div>"
                                        + "<b><i class=\"i_num\">" + result.Data + "</i>/<em class=\"em_num\">" + Current.StuNum + "</em>"
                                        + "<img src=\"../images/xiugai.png\" alt=\"\" /></b>"
                                        + "</a></li>";
                            $("#ul_ClassInfo").html(shtml);
                            ChangeSpWidth(); ////改变完成度显示
                        } else {
                            $("#div_LR").html("<div class=\"no_nr\" id=\"divNClass\" ><img src=\"../images/xie.png\" alt=\"\" /><p>请根据您布置的内容选择相应的目录</p></div>");
                            $("#divlr").html("");
                            $("#ul_ClassInfo").html("");
                        }
                    }

                }
            });
        }
    };
};


