var PerfectDataInit = function () {
    var Current = this;

    this.Init = function () {
        $("#txtName").val('');
        Current.Type = Common.QueryString.GetValue("Type");
        Current.UserID = Common.QueryString.GetValue("UserID");
        Current.schoolID = Current.getQueryString("schoolID");
        Current.schoolName = Current.getQueryString("schoolName");
        Current.UserID = Current.getQueryString("UserId");
        Current.TrueName = Current.getQueryString("TrueName");

        Current.InitArea();

        //跳转到选择界面
        $("#div_school").click(function () {
            window.location.href = "../ClassManagement/SelectSchool.aspx?TrueName=" + $("#txtName").val() + "&UserId=" + Current.UserID;
        });

    };

    //加载数据
    this.InitArea = function () {
        if (Current.schoolID != "" && Current.schoolID != "undefined" && Current.schoolID != null) {
            $("#schoolID").val(Current.schoolID);
            $("#school").val(Current.schoolName);
        }
        if (Current.TrueName != "" && Current.TrueName != "undefined" && Current.TrueName != null) {
            $("#txtName").val(Current.TrueName);
        }
    }

    this.getQueryString = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) {
            return decodeURI(r[2]);
        } else {
            return null;
        }
    }

    $("#next").click(function () {
        var name = $.trim($("#txtName").val());
        var reg = /^[\u4e00-\u9fa5][a-zA-Z0-9\u4e00-\u9fa5]+$/;
        if (name == '') {
            popup("姓名不能为空");
            return false;
        }
        if (!reg.test(name)) {
            popup("姓名要以汉字开头，中文或数字结尾");
            return false;
        }
        if (!$("#schoolID").val()) {
            popup("请选择学校");
            return false;
        }
        Current.UserName = name;
        Current.SchoolName = $("#school").val();
        Current.SchoolID = $("#schoolID").val();
        var info = { UserID: Current.UserID, TrueName: name, SchoolID: Current.SchoolID, SchoolName: Current.SchoolName };
        $.post("../Handler/WeChatHandler.ashx?queryKey=UpdateUserInfo", info, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    window.location.href = "CreateClass.aspx?SchoolID=" + Current.SchoolID + "&UserID=" + Current.UserID;
                } else {
                    popup(result.Msg);
                    return false;
                }
            }
        });

    });
}

var perfectdataInit;
$(function () {
    perfectdataInit = new PerfectDataInit();
    perfectdataInit.Init();
});