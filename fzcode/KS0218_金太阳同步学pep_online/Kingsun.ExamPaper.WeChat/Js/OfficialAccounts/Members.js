var MembersInit = function () {
    var Current = this;
    this.ClassID = ''; //班级ID
    this.DeleteIDStr = [];

    this.Init = function () {
        Current.UserID = Common.QueryString.GetValue("UserID");
        Current.ClassID = Common.QueryString.GetValue("ClassID");
        Current.ClassName = decodeURI(Common.QueryString.GetValue("ClassName"));
        document.title = Current.ClassName;
        Current.GetStuList();
    };

    //获取班级学生信息
    this.GetStuList = function () {
        Current.DeleteIDStr = [];
        var obj = { ClassID: Current.ClassID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=getStuListByClassID", obj, function(data) {
            var result = eval("(" + data + ")");
            if (result.Success) {
                var html = '';
                var studentList = result.ClassList;
                $("#studentNum").html("(<em>" + studentList.length + "</em>人）");
                for (var i = 0, length = studentList.length; i < length; i++) {
                    html += '<li><img src="images/tou.png" alt="" /><span>' + (studentList[i].TrueName == "" ? studentList[i].UserName : studentList[i].TrueName) + '</span></li>';
                }
                $("#studentList").html('');
                $(html).prependTo("#studentList");

                init();
                Current.CancelEdit();
            } else {
                $("#studentNum").html('');
                $("#studentNum").html("(<em>0</em>人)");
            }
        });
    }


    $("#InviteStu").click(function () {
        var obj = { UserID: Current.UserID, APPID: Current.APPID };
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
}

var membersinit;
$(function () {
    membersinit = new MembersInit();
    membersinit.Init();
});