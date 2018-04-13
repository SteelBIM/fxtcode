var ReportLists = function () {
    var Current = this;
    this.UserID = Common.QueryString.GetValue("UserID");
    this.EditionID = Common.QueryString.GetValue("UserID");
    this.Init = function () {
        $("#divUserID").html(Current.UserID);
        $("#divEditionID").html(Current.EditionID);
        //获取班级列表

        Current.GetClassList();

    };

    //获取班级列表 
    this.GetClassList = function () {
        if (Current.UserID != undefined) {
            var obj = { UserID: Current.UserID, ClassIDs: "" };//"33299631"
            $.post("../../Handler/LR_WeChatHandler.ashx?queryKey=queryclasslist", obj, function (data) {
                if (data) {
                    var result = JSON.parse(data);
                    if (result.Success) {
                        //加载下拉班级列表  <li>一年级1班</li>
                        var classhtml = "<ul class=\"timelist\">";
                        $.each(result.ClassList, function () {
                            classhtml += "<li id=\"" + this.Id + "\" ClassNum=\"" + this.ClassNum + "\" GradeID=\"" + this.GradeId + "\">" + this.ClassName + "</li>";
                        });

                        classhtml += "</ul>";
                        if (result.ClassList.length > 6) {
                            classhtml += "<a>收起</a>";
                        }
                        $("#divClass").html(classhtml);
                        $("#span").html(result.ClassList[0].ClassName);
                        $("#span").attr("classid", result.ClassList[0].Id);
                        //获取当前日期学习数据 -- 在班级列表加载完成后
                        // Current.GetLearningReport(Current.sTime);

                        changeClass();
                    }
                    else {
                        $("#divNClass").show();
                    }
                }
            });
        }
        else {
            $("#divNClass").show();
        }
    };
}

var reportListsInit;
$(function () {
    reportListsInit = new ReportLists();
    reportListsInit.Init();
});

