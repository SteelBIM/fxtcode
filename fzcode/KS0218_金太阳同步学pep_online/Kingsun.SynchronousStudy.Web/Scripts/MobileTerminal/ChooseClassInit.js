/// <reference path="../../../AppTheme/js/jquery-1.11.2.min.js" />

var ChooseClassInit = function () {
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

    function popup(aaa) {
        var black = document.createElement("div");
        var black1 = document.createElement("div");
        var black2 = document.createElement("div");
        var black3 = document.createElement("p");
        var black4 = document.createTextNode(aaa);
        black.className = 'zong';
        black1.className = 'zhezhao';
        black2.className = 'hezi';
        black3.appendChild(black4);
        black2.appendChild(black3);
        black.appendChild(black1);
        black.appendChild(black2);
        document.body.appendChild(black);
        black1.onclick = function () {
            black.parentNode.removeChild(black);

        }
    };

    this.Init = function () {
        Current.UserID = Common.QueryString.GetValue("UserID");
        Current.SchoolID = Common.QueryString.GetValue("SchoolID");
        Current.GetUserClassList();
        //Current.InitStandBook();


    };


    $("#close").click(function () {
        //调用移动端接口
        var data = {
            ////传递的参数json
            //"data": {
            //    "MessageStr": MessageStr

            //}
        };
        //调用移动端的方法
        window.WebViewJavascriptBridge.callHandler(
            'finish', data, function (responseData) {

            }
        );
    });

    //获取用户已有班级信息
    this.GetUserClassList = function () {
        var obj = { UserID: Current.UserID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=queryClassList", obj, function (data) {
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
        $(".main2 .show").css("display","block");
        if (demo1.getArry().length <= 0) {
            popup("请选择要创建的班级！");
            $(".main2 .show").css("display", "none");
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
                    $(".main2 .show").css("display", "none");
                    return false;
                }
            }
        }

        var obj = { UserID: Current.UserID, SchoolID: Current.SchoolID, ClassStr: classStr };
        $.post("../Handler/WeChatHandler.ashx?queryKey=teaBindClass", obj, function (data) {
            if (data) {
                var result = JSON.parse(data); //eval("(" + data + ")");

                if (result.Success) {
                    ////调用移动端接口
                    //var mdata = {
                    //    //传递的参数json
                    //    "data": {
                    //        "Count": result.Count
                    //    }
                    //};
                    ////调用移动端的方法
                    //window.WebViewJavascriptBridge.callHandler(
                    //    'GetClassCount', mdata, function (responseData) {
                    //    }
                    //);

                    setTimeout(window.location.href = "ClassList.aspx?UserID=" + Current.UserID + "&SchoolID=" + Current.SchoolID,1000);
                } else {
                    popup(result.Msg);
                    $(".main2 .show").css("display", "none");
                    return false;
                }
            }
        });
    });
}

var chooseClassInit;
$(function () {
    chooseClassInit = new ChooseClassInit();
    chooseClassInit.Init();
});