/// <reference path="../../../AppTheme/js/jquery-1.11.2.min.js" />

var AddInformationInit = function () {
    var Current = this;
    this.Type = "";
    this.provinceID = "";
    this.BookArr = [];
    this.ProvinceArr = [];
    this.getBool = false; //用来标识是否获取位置信息

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
        $("#name").val('');
        Current.Type = Common.QueryString.GetValue("Type");
        Current.UserID = Common.QueryString.GetValue("UserID");
        Current.provinceID = Current.getQueryString("provinceID");
        Current.cityID = Current.getQueryString("cityID");
        Current.districtID = Current.getQueryString("districtID");
        Current.schoolID = Current.getQueryString("schoolID");
        Current.TrueName = Current.getQueryString("TrueName");
        // Current.editionID = Current.getQueryString("editionID");

        Current.InitArea();

        if (Current.TrueName == "" || Current.TrueName == "undefined" || Current.TrueName == null) {
            Current.GetUserInfo();
        }
        //跳转到班级选择界面
        $("#div_province").click(function () {
            window.location.href = Constant.classInfo_Url + "StudyReportManagement/SelectSchool.aspx?UserID=" + Current.UserID + "&TrueName=" + $.trim($("#name").val());
        });
        $("#div_city").click(function () {
            window.location.href = Constant.classInfo_Url + "StudyReportManagement/SelectSchool.aspx?provinceID=" + Current.provinceID + "&UserID=" + Current.UserID + "&TrueName=" + $.trim($("#name").val());
        });
        $("#div_district").click(function () {
            window.location.href = Constant.classInfo_Url + "StudyReportManagement/SelectSchool.aspx?provinceID=" + Current.provinceID + "&UserID=" + Current.UserID + "&cityID=" + Current.cityID + "&TrueName=" + $.trim($("#name").val());
        });
        $("#div_school").click(function () {
            window.location.href = Constant.classInfo_Url + "StudyReportManagement/SelectSchool.aspx?provinceID=" + Current.provinceID + "&UserID=" + Current.UserID + "&cityID=" + Current.cityID + "&districtID=" + Current.districtID + "&TrueName=" + $.trim($("#name").val());
        });

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


    this.getQueryString = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) {
            return decodeURI(r[2]);
        } else {
            return null;
        }
    }

    //通过用户ID获取用户信息
    this.GetUserInfo = function () {
        var obj = { UserID: Current.UserID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=GetUserName", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    $("#name").val(result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName);
                    Current.UserTrueName = result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName;
                } else {
                    $("#name").val('');
                }
            }
        });
    }

    //加载数据
    this.InitArea = function () {
        if (Current.provinceID != "" && Current.provinceID != "undefined" && Current.provinceID != null) {
            $("#provinceID").val(Current.provinceID.split('_')[1]);
            $("#province").val(Current.provinceID.split('_')[0]);
        }
        if (Current.cityID != "" && Current.cityID != "undefined" && Current.cityID != null) {
            $("#cityID").val(Current.cityID.split('_')[1]);
            $("#city").val(Current.cityID.split('_')[0]);
        }
        if (Current.districtID != "" && Current.districtID != "undefined" && Current.districtID != null) {
            $("#districtID").val(Current.districtID.split('_')[1]);
            $("#district").val(Current.districtID.split('_')[0]);
        }
        if (Current.schoolID != "" && Current.schoolID != "undefined" && Current.schoolID != null) {
            $("#schoolID").val(Current.schoolID.split('_')[1]);
            $("#school").val(Current.schoolID.split('_')[0]);
        }
        if (Current.TrueName != "" && Current.TrueName != "undefined" && Current.TrueName != null) {
            $("#name").val(Current.TrueName);
        }

        //if (Current.editionID != "" && Current.editionID != "undefined" && Current.editionID != null) {
        //    $("#editionID").val(Current.editionID.split('_')[1]);
        //    $("#edition").val(Current.editionID.split('_')[0]);
        //}
    }

    //点击学段添加背景色
    $("#stage").click(function () {
        if ($(this).hasClass("on")) {
            $(this).removeClass("on");
        } else {
            $(this).addClass("on");
        }
    });

    //点击科目添加背景色
    $("#subject").click(function () {
        if ($(this).hasClass("on")) {
            $(this).removeClass("on");
        } else {
            $(this).addClass("on");
        }
    });

    $("#next").click(function () {
        var name = $.trim($("#name").val());
        var reg = /^[\u4e00-\u9fa5][a-zA-Z0-9\u4e00-\u9fa5]+$/;
        if (name == '') {
            popup("姓名不能为空");
            return false;
        }
        if (!reg.test(name)) {
            popup("姓名要以汉字开头，中文或数字结尾");
            return;
        }
        if (!$("#stage").hasClass("on")) {
            popup("请选择学段");
            return false;
        }
        if (!$("#subject").hasClass("on")) {
            popup("请选择学科");
            return false;
        }
        if (!$("#schoolID").val()) {
            popup("请选择学校");
            return false;
        }
        //if (!$("#editionID").val()) {
        //    popup("请选择教材版本");
        //    return false;
        //}
        Current.UserName = name;
        Current.SchoolName = $("#school").val();
        Current.SchoolID = $("#schoolID").val();
        var info = { UserID: Current.UserID, TrueName: name, SchoolID: Current.SchoolID, SchoolName: Current.SchoolName };
        $.post("../Handler/WeChatHandler.ashx?queryKey=UpdateUserInfo", info, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    window.location.href = "ChooseClass.aspx?SchoolID=" + Current.SchoolID + "&UserID=" + Current.UserID;
                } else {
                    popup(result.Msg);
                    return false;
                }
            }
        });

    });
}


var addInformationInit;
$(function () {
    addInformationInit = new AddInformationInit();
    addInformationInit.Init();
});