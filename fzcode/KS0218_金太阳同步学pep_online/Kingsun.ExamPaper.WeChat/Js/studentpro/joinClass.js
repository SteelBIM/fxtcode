var JoinClassInit = function () {
    var Current = this;
    this.UserID = '';
    this.SchoolID = 0;
    this.ClassID = '';

    this.Init = function () {
        $("#name").val('');
        Current.ID = '';
        Current.UserID = Common.QueryString.GetValue("UserID");
        Current.GetUserInfo();
        //Current.GetClassList();
    };

    //发送验证码
    $("#getCheckCode").click(myclick = function () {
        var validCode = true;
        var telephone = $.trim($("#telephone").val());
        var pattern = /^(0|86|17951)?(13[0-9]|15[012356789]|17[0678]|18[0-9]|14[57])[0-9]{8}$/;
        if (!pattern.test(telephone)) {
            popup("请输入正确的手机号");
            return false;
        }
        Current.Telephone = telephone;
        var obj = { Telephone: telephone };
        $.post("../Handler/WeChatHandler.ashx?queryKey=sendCode", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    Current.CheckCode = result.CheckCode;
                    popup("验证码已发送，请注意查收！");
                    var time = 300;
                    $("#getCheckCode").css("backgroundColor", "#C7C6C6");
                    var getCode = $('#getCheckCode');
                    //获取验证码清除绑定事件 直到倒计时结束添加绑定事件
                    getCode.unbind("click");
                    //验证码倒计时
                    if (validCode) {
                        validCode = false;
                        var t = setInterval(function () {
                            time--;
                            getCode.html(time + "秒后重新获取");
                            if (time == 0) {
                                clearInterval(t);
                                $('#getCheckCode').css("backgroundColor", "#1C9EEA");
                                getCode.html("获取验证码");
                                validCode = true;
                                getCode.bind("click", myclick);
                            }
                        }, 1000);
                    }
                } else {
                    popup(result.Msg);
                }
            } else {
                popup("发送请求失败，请重新发送");
            }
        });
    });


    $("#next").click(function () {
        var obj = { Telephone: $.trim($("#telephone").val()), Type: 26, Code: $.trim($('#verifyCode').val()) };
        $.post("../Handler/WeChatHandler.ashx?queryKey=LoginByPhone", obj, function (data) {
            var result = eval("(" + data + ")");
            if (result.Success) {
                Current.UserID = result.UserID;
               
                    $(".joinclass .content").css("display", "none");
                    $(".joinclass .content1").css("display", "block");
                    $(".joinclass .content2").css("display", "none");
               
            } else {
                popup(result.Msg);
                return false;
            }
        });

    });

    //通过用户ID获取用户信息
    this.GetUserInfo = function () {
        var obj = { UserID: Current.UserID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=GetUserInfoByUserId", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    $("#teaName_1").html(result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName);
                } else {
                    $("#name").html('');
                }
            }
        });
    }

    //获取老师班级列表
    this.GetClassList = function () {
        var obj = { UserID: Current.UserID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=GetUserClassByUserId", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.Success) {
                    var classList = result.ClassList;
                    Current.SchoolID = classList[0].SchoolID;
                    var html = "";
                    for (var i = 0, length = classList.length; i < length; i++) {
                        html += '<li id="' + classList[i].ID + '">' + classList[i].ClassName + '</li>';
                    }
                    $("#classList").html('');
                    $(html).prependTo("#classList");

                    $("#classList li").click(function () {
                        var id = $(this).attr("id");
                        Current.ClassID = id;
                        var liArr = $("#classList li");
                        for (var i = 0, length = liArr.length; i < length; i++) {
                            if ($(liArr[i]).hasClass('on')) {
                                $(liArr[i]).removeClass('on');
                            }
                        }
                        $(this).addClass("on");
                    });
                }
            } else {
                popup("发送请求失败，请重新发送");
            }
        });
    }

    //确认你绑定
    $("#sure").click(function () {
        if (Current.ClassID == '') {
            popup("请先选择需要绑定的班级");
            return false;
        }
        var obj = { ClassID: Current.ClassID, StudentID: Current.StudentID, UserID: Current.UserID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=StudentBindClass", obj, function (data) {
            if (data) {
                var result = JSON.parse(data); //eval("(" + data + ")");
                if (result.Success) {
                   
                } else {
                    popup("绑定失败，请重试");
                    return false;
                }
            }
        });
    });
}


var joinClassInit;
$(function () {
    joinClassInit = new JoinClassInit();
    joinClassInit.Init();
});