var CreateClassInit = function () {
    var Current = this;
    this.UserID = '';
    this.SchoolID = 0;
    this.EditionID = 0;
    this.TeacherName = "";
    this.BookArr = []; //MOD数据库书本信息
    this.GradeArr = []; //MOD数据库年级信息
    this.GradeNum = 1;//当前添加年级个数
    this.ClassNum = 0;//当前添加班级个数
    this.AddGradeArr = [];//当前添加年级信息
    this.AddClassArr = [];//当前添加班级信息
    this.UserClassArr = [];//老师已经绑定班级信息

    this.Init = function () {
        Current.UserID = Common.QueryString.GetValue("UserID");
        Current.SchoolID = Common.QueryString.GetValue("SchoolID");
        Current.GetUserClassList();

    };

    //获取用户已有班级信息
    this.GetUserClassList = function () {
        var obj = { UserID: Current.UserID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=GetUserClassByUserId", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    var classArr = result.ClassList;
                    for (var i = 0, length = classArr.length; i < length; i++) {
                        Current.UserClassArr.push(classArr[i].ClassName);
                    }
                }
            }
        });
    }


    //绑定班级
    $("#nextStep").click(function () {
        var demo1 = new demo();

        if (demo1.getArry().length <= 0) {
            popup("请选择要创建的班级！");
            return false;
        }

        var classStr = "";
        for (var i = 0; i < demo1.getArry().length; i++) {
            if (i == demo1.getArry().length - 1) {
                classStr += demo1.getArry()[i];
            } else {
                classStr += demo1.getArry()[i] + ',';
            }
        }

        if (Current.UserClassArr.length > 0) {
            for (var i = 0; i < demo1.getArry().length; i++) {
                if (Current.UserClassArr.indexOf(demo1.getArry()[i]) > -1) {
                    popup(demo1.getArry()[i] + "已经跟您绑定过了");
                    return false;
                }
            }
        }

        var obj = { UserID: Current.UserID, SchoolID: Current.SchoolID, ClassStr: classStr };
        $.post("../Handler/WeChatHandler.ashx?queryKey=teaBindClass", obj, function (data) {
            if (data) {
                var result = JSON.parse(data);
                if (result.Success) {
                    window.location.href = "ClassList.aspx?UserID=" + Current.UserID + "&SchoolID=" + Current.SchoolID;
                } else {
                    popup(result.Msg);
                    return false;
                }
            }
        });
    });

}

var createClassInit;
$(function () {
    createClassInit = new CreateClassInit();
    createClassInit.Init();
})