var ClassListInit = function () {
    var current = this;
    this.SchoolID = ''; //学校ID
    this.UserID = ''; //用户ID
    this.DeleteIDStr = [];
    this.Telephone = '';
    this.LogTime = 0;
    this.ClassNum = 0;
    this.trueName = "";
    this.vName = "";
    this.selectNum = 0;

    this.Init = function () {
        current.UserID = window.Common.QueryString.GetValue("UserID");
        current.APPID = window.Common.QueryString.GetValue("AppID");

        current.GetClassList();


    };

    //获取老师班级列表
    this.GetClassList = function () {
        current.DeleteIDStr = [];
        var obj = { UserID: current.UserID, SubjectID: 3 };
        $.post("../Handler/WeChatHandler.ashx?queryKey=GetUserClassByUserId", obj, function (data) {
            if (data) {
                var result = JSON.parse(data); //eval("(" + data + ")")
                if (result.Success) {
                    var classList = result.ClassList;
                    current.ClassNum = classList.length;

                    if (classList.length <= 0) {
                        window.location.href = "CreateClass.aspx?UserID=" + current.UserID;
                        return;
                    }

                    current.DeleteIDStr = [];
                    current.SchoolID = classList[0].SchoolID;
                    var html = "";
                    for (var i = 0, length = classList.length; i < length; i++) {
                        html += '  <li><a href="Members.aspx?ClassID=' + classList[i].Id + '&ClassName=' + encodeURI(encodeURI(classList[i].ClassName)) + '&UserID=' + current.UserID + '"><span>' + classList[i].ClassName + '</span><em><i>' + classList[i].StudentNum + '</i>人</em></a></li>';
                    }
                    $("#cList").html('');
                    $(html).prependTo("#cList");
                } else {
                    //$(".classList").html('');
                    ////判断是否为首次登陆
                    //if (current.LogTime === 0) {
                    //window.location.href = "CreateClass.aspx?UserID=" + current.UserID;
                    //}
                }
            } else {
                popup("发送请求失败，请重新发送");
            }
        });
    };

    $("#InviteStu").click(function () {
        var obj = { UserID: current.UserID, APPID: current.APPID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=GetUserInfoByUserId", obj, function (data) {
            if (data) {
                var result = JSON.parse(data); //eval("(" + data + ")");
                if (result.Success) {
                    $("#teaName_1").html(result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName);
                    $("#teaName_2").html(result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName);
                }
            }
        });
    });

    $("#creatClass").click(function () {
        var obj = { UserID: current.UserID, APPID: current.APPID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=GetUserInfoByUserId", obj, function (data) {
            if (data) {
                var result = JSON.parse(data);//eval("(" + data + ")");
                if (result.Success) {
                    current.SchoolID = result.UserInfo.SchoolID;
                    if (result.UserInfo.SchoolID != null && result.UserInfo.SchoolID !== "" && result.UserInfo.SchoolID != 0) {
                        window.location.href = "CreateClass.aspx?UserID=" + current.UserID + "&SchoolID=" + current.SchoolID;
                    } else {
                        window.location.href = "PerfectData.aspx?UserID=" + current.UserID;
                    }
                }
            }
        });
    });

};
var classListInit;
$(function () {
    classListInit = new ClassListInit();
    classListInit.Init();
});