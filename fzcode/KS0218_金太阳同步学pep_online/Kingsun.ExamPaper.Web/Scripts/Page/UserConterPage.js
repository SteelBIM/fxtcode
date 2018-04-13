/// <reference path="../../App_Themes/js/jquery-1.10.2.min.js" />

var tempTimeStatic = 1;//计时器变量 1=无计时器，2=计时器正在计时，此状态下重新打开修改手机 不用清空文本框内容

$(function () {
    //$("#diaUpdPho").removeAttr("style").dialog({ title: '修改手机', width: 500, height: 250, closed: true, modal: true });
    //GetUserManager 获取学生信息
    $("#liStuTask").removeClass("on");
    GetDataFun("GetUserManager", "", function (data) {
        if (data) {
            if (data.Data.TrueName == undefined || data.Data.TrueName == "undefined" || data.Data.TrueName == "" || data.Data.TrueName == null) {
                $("#spUserName").html(data.Data.UserName + "同学");
            }
            else {
                $("#spUserName").html((data.Data.TrueName.length > 9 ? data.Data.TrueName.substring(0, 9) : data.Data.TrueName) + "同学");
                $("#tdName").html(data.Data.TrueName);
            }
            $("#pUserID").html("ID:" + data.Data.UserName);
            $("#tdUserID").html(data.Data.UserName);
            $("#tdUserPho").html(data.Data.SecurityPhone);
            $("#stuImg").attr("src", "http://weixintest.kingsun.cn/TengxunImg.aspx?userid=" + data.Data.UserID);
            $(".userMsg img").attr("src", data.Data.AvatarUrl);
            $("#tdSSTAGEName").html(data.Data.Stage);
            //班级 学段
            if (data.Data.UserClass) {
                $("#tdClassName").html(data.Data.UserClass.ClassName);
                //$("#tdSSTAGEName").html(GetSSTAGEName(data.Data.UserClass.GradeID));
            }

            //教师列表
            if (data.Data.TeacherList != null && data.Data.TeacherList != undefined) {
                var teacherHtml = "";
                $.each(data.Data.TeacherList, function (i) {
                    if (data.Data.TeacherList[i] != undefined) {
                        var tName = "";
                        if (data.Data.TeacherList[i].TrueName == undefined || data.Data.TeacherList[i].TrueName == "undefined"
                                    || data.Data.TeacherList[i].TrueName == "" || data.Data.TeacherList[i].TrueName == null) {
                            tName = data.Data.TeacherList[i].UserName;
                        }
                        else {
                            tName = data.Data.TeacherList[i].TrueName;
                        }

                        teacherHtml = teacherHtml + "<tr><td width=\"100px\"><span>姓名：</span></td>";
                        teacherHtml = teacherHtml + "<td width=\"100px\">" + tName + "</td><td width=\"60px\"><span class=\"sp\">ID：</span></td>";
                        teacherHtml = teacherHtml + "<td width=\"100px\">" + data.Data.TeacherList[i].UserName + "</td>";
                        teacherHtml = teacherHtml + "<td width=\"200px\">" + data.Data.TeacherList[i].SubjectName + "</td></tr>";
                    }
                });
                $("#tbTeacher").html(teacherHtml);
            }
        }
    });

    //修改手机
    $("#aUpdPho").click(function () {
        if (tempTimeStatic == 1) {
            $("#txtPhone").val("请输入手机号").css("color", "#888");
        }
        //$("#txtPhone").val("请输入手机号").css("color", "#888");
        $("#txtCode").val("请输入短信验证码").css("color", "#888");
        $("#aSavePho").removeClass("on");

        Common.addHandler(document.getElementById("txtPhone"), "keyup", checkPhone);
        //$("#diaUpdPho").dialog("open");
        var tempHtmlUpdate = $("#diaUpdPho")[0];
        art.dialog({
            id: 'digUpdPho',
            width: '520px',
            padding: '0',
            content: tempHtmlUpdate,
            title: '修改手机',
            lock: true
        });

    });
    //修改密码
    $("#aUpdPassword").click(function () {


        //还原密码输入框的初始值 并隐藏
        $("#txtNewPwd").val("").hide();
        $("#txtNewPwd2").val("").hide();
        $("#txtOldPwd").val("").hide();

        //显示输入提示文本框
        $("#txtOldShowPwd").show();
        $("#txtNewShowPwd").show();
        $("#txtNewShowPwd2").show();

        $("#aTitlePwd").html("");
        $("#aSavePwd").removeClass("on");

        var tempHtmlUpdate = $("#diaPassword")[0];
        art.dialog({
            id: 'digUpdPwd',
            width: '480px',
            padding: '0',
            content: tempHtmlUpdate,
            title: '修改密码',
            lock: true
        });
    });

    //绑定手机号输入框的事件
    $("#txtPhone").bind("blur", function () {
        blurPhone(this);
        SetSavePhoClass(); //验证是否需要启用保存按钮
    }).bind("click", function () {
        clickPhone(this);
    });
    //************************
    //手机号相关 验证
    //绑定验证码的输入事件
    $("#txtCode").bind("blur", function () {
        blurCode(this);
        SetSavePhoClass(); //验证是否需要启用保存按钮
    }).bind("click", function () {
        clickCode(this);
    }).bind("keyup", function () {
        SetSavePhoClass(); //验证是否需要启用保存按钮
    });

    //绑定获取验证码的事件
    $("#getCode").bind("click", function () {
        sendVerifyCode(this);
    }).bind("keyup", function () {
        SetSavePhoClass(); //验证是否需要启用保存按钮
    });

    //设置修改手机按钮的点击可用
    function SetSavePhoClass() {
        var pho = Common.TrimSpace($("#txtPhone").val());
        var code = Common.TrimSpace($("#txtCode").val());
        if (pho != "" && pho != "请输入手机号" && code != "" && code != "请输入短信验证码") {
            $("#aSavePho").addClass("on");
        }
        else {
            $("#aSavePho").removeClass("on");
        }
    }
    //手机号相关 验证结束
    //************************




    //************************
    //密码相关 验证
    //隐藏/显示密码框或文本框
    $("#txtOldShowPwd").bind("focus", function () {
        $(this).hide();
        $("#txtOldPwd").show().focus();
    });
    $("#txtNewShowPwd").bind("focus", function () {
        $(this).hide();
        $("#txtNewPwd").show().focus();
    });
    $("#txtNewShowPwd2").bind("focus", function () {
        $(this).hide();
        $("#txtNewPwd2").show().focus();
    });


    //绑定旧密码输入框的事件
    $("#txtOldPwd").bind("blur", function () {
        blurOldPwd(this);
        SetSavePwdClass(); //验证是否需要启用保存按钮
    }).bind("keyup", function () {
        $("#txtOldPwd").val($("#txtOldPwd").val().replace(/\s/g, ''));
        SetSavePwdClass(); //验证是否需要启用保存按钮
    }).bind("keydown", function () {

        //oldPwdKeyDown();
    });
    //绑定新密码输入框的事件_1
    $("#txtNewPwd").bind("blur", function () {
        blurNewPwd(this);
        SetSavePwdClass(); //验证是否需要启用保存按钮
    }).bind("keyup", function () {  //按键结束  //判断是否与第一次新密码相同
        $("#txtNewPwd").val($("#txtNewPwd").val().replace(/\s/g, ''));

        var PwdNew1 = $("#txtNewPwd").val().replace("/^\s+|\s+$/", "");
        var PwdNew2 = $("#txtNewPwd2").val().replace("/^\s+|\s+$/", "");
        if (PwdNew2 != "" && PwdNew2 != "确认新密码") {
            //alert(PwdNew1 + "____" + PwdNew2);
            if (PwdNew1 != PwdNew2) { //第二次密码与第一次密码不相等
                $("#aTitlePwd").html("两次输入密码不一样");
                $("#aSavePwd").removeClass("on");
            }
            else {
                $("#aTitlePwd").html("");
            }
        }
        SetSavePwdClass(); //验证是否需要启用保存按钮
    });

    //绑定新密码输入框的事件_2
    $("#txtNewPwd2").bind("blur", function () {
        blurNewPwd_2(this);
        SetSavePwdClass(); //验证是否需要启用保存按钮
    }).bind("keydown", function (event) { //键盘按下开始

    }).bind("keyup", function () {  //按键结束  //判断是否与第一次新密码相同
        $("#txtNewPwd2").val($("#txtNewPwd2").val().replace(/\s/g, ''));

        var PwdNew1 = $("#txtNewPwd").val().replace("/^\s+|\s+$/", "");
        var PwdNew2 = $("#txtNewPwd2").val().replace("/^\s+|\s+$/", "");
        if (PwdNew1 != "" && PwdNew1 != "请输入新密码") {
            //alert(PwdNew1 + "____" + PwdNew2);
            if (PwdNew1 != PwdNew2) { //第二次密码与第一次密码不相等
                $("#aTitlePwd").html("两次输入密码不一样");
                $("#aSavePwd").removeClass("on");
            }
            else {
                $("#aTitlePwd").html("");
            }
        }
        SetSavePwdClass(); //验证是否需要启用保存按钮
    });
    //密码相关 验证结束
    //************************

    //保存手机号
    $("#aSavePho").click(function () {
        if ($(this).attr("class") == "aBtnUserCenter btn1") {
            return;
        }
        SavePho();
    });
    //取消修改手机号
    $("#aExitPho").click(function () {
        art.dialog({ id: "digUpdPho" }).close();
    });

    //保存密码
    $("#aSavePwd").click(function () {
        if ($(this).attr("class") == "btSavePwd btn1") {
            return;
        }
        SavePassword();
    });

    //取消修改密码
    $("#aExitPwd").click(function () {
        art.dialog({ id: "digUpdPwd" }).close();
    });
});

//设置修改密码按钮的点击可用
function SetSavePwdClass() {
    var PwdOld = $("#txtOldPwd").val().replace(/\s/g, '');
    var PwdNew = $("#txtNewPwd").val().replace(/\s/g, '');
    var PwdNew2 = $("#txtNewPwd2").val().replace(/\s/g, '');
    if (PwdOld != "" && PwdOld != "请输入旧密码" && PwdNew != "" && PwdNew != "请输入新密码" && PwdNew2 != "" && PwdNew2 != "确认新密码") {
        if (PwdNew == PwdNew2) {
            //$("#aSavePwd").attr("class", "btSavePwd on");
            $("#aSavePwd").addClass("on");
        }
    }
    else {
        //$("#aSavePwd").attr("class", "btSavePwd");
        $("#aSavePwd").removeClass("on");

    }
}

//*****************************************************
//*****************************************************
//修改手机相关方法开始
//点击手机输入框
function clickPhone(obj) {
    if (Common.TrimSpace($(obj).val()) == "请输入手机号") {
        $(obj).val("");
    }
    $(obj).css("color", "#000");
}
//移开手机输入框
function blurPhone(obj) {
    if (Common.TrimSpace($(obj).val()) == "") {
        $(obj).val("请输入手机号");
        $(obj).css("color", "#888");
    }
}
//实时检测号码的输入
function checkPhone() {
    if (Common.Validate.IsMobileNo($("#txtPhone").val())) {
        $("#getCode").attr("class", "getCode");//获取验证码按钮可点击
    } else {
        $("#getCode").attr("class", "getCode on");//获取验证码按钮不可点击
    }
}
//点击验证码输入框
function clickCode(obj) {
    if (Common.TrimSpace($(obj).val()) == "请输入短信验证码") {
        $(obj).val("");
    }
    $(obj).css("color", "#000");
}
//移开验证码输入框
function blurCode(obj) {
    if (Common.TrimSpace($(obj).val()) == "") {
        $(obj).val("请输入短信验证码");
        $(obj).css("color", "#888");
    }
}

//发送验证码
function sendVerifyCode() {
    if ($("#getCode").attr("class") == "getCode on") {
        return;
    }
    $("#getCode").attr("class", "getCode on");//获取验证码按钮不可点击

    var phone = Common.TrimSpace($("#txtPhone").val());
    //var phone = "18219170231";
    if (Common.Validate.IsMobileNo(phone)) {
        $.post("?action=SendVerifyCode&Rand=" + Math.random(), { SecurityPhone: phone }, function (result) {
            if (result) {
                result = eval("(" + result + ")");//JSON.parse
                alert(result.Message);
                clock(); //发送验证码计时器
            } else {
                alert("验证码发送失败，请重试！");
                $("#getCode").attr("class", "getCode");//获取验证码按钮可点击
            }
        });
    } else {
        alert("请输入合法的手机号！");
        $("#getCode").attr("class", "getCode");//获取验证码按钮可点击
    }
}

//修改手机号保存事件
function SavePho() {
    var verifyCode = Common.TrimSpace($("#txtCode").val());
    var securityPhone = Common.TrimSpace($("#txtPhone").val());
    if (Common.Validate.IsMobileNo(securityPhone) && verifyCode != "") {
        $.post("?action=SavePho&Rand=" + Math.random(), { SecurityPhone: securityPhone, VerifyCode: verifyCode }, function (result) {
            if (result) {
                result = eval("(" + result + ")");//JSON.parse
                if (result.Success) {
                    $("#tdUserPho").html(securityPhone);
                    alert("绑定成功!");
                    //location.reload();
                    //关闭弹窗
                    art.dialog({ id: "digUpdPho" }).close();
                } else {
                    alert(result.Message);
                    $("#txtCode").focus();
                }
            } else {
                alert("验证码校验失败，请检查后重试！");
                $("#txtCode").focus();
            }
        });
    } else {
        alert("请输入合法的手机号和验证码！");
    }
}

//发送验证码计时器
function clock() {
    tempTimeStatic = 2;
    var validCode = true;
    var time = 60;
    //获取验证码清除绑定事件 直到倒计时结束添加绑定事件
    $("#getCode").unbind("click");
    //验证码倒计时
    if (validCode) {
        validCode = false;
        $("#getCode").attr("class", "getCode on");
        var t = setInterval(function () {
            time--;
            $("#getCode").html(time + "秒后重新获取");
            if (time == 0) {
                tempTimeStatic = 1;
                clearInterval(t); //停止计时器
                $("#getCode").html("获取验证码");
                validCode = true;
                $("#getCode").attr("class", "getCode");
                $("#getCode").bind("click", sendVerifyCode);
                return;
            }
        }, 1000)
    }
}

//修改手机相关方法结束
//*****************************************************
//*****************************************************



//*****************************************************
//*****************************************************
//修改密码相关方法开始
//点击旧密码输入框
function clickOldPwd(obj) {
    //if (Common.TrimSpace($(obj).val()) == "请输入旧密码") {
    //    $(obj).val("");
    //    //$(obj).attr("type", "password");
    //}
    //$(obj).css("color", "#000");
}
//移开旧密码输入框
function blurOldPwd(obj) {
    if (Common.TrimSpace($(obj).val()) == "") {
        //$(obj).attr("type", "text");
        //$(obj).val("请输入旧密码");
        //$(obj).css("color", "#888");
        $(obj).hide();
        $("#txtOldShowPwd").show();

    }
}
//点击新密码输入框-1***********
function clickNewPwd(obj) {
    //if (Common.TrimSpace($(obj).val()) == "请输入新密码") {
    //    $(obj).val("");
    //    //$(obj).attr("type", "password");
    //}
    //$(obj).css("color", "#000");
}
//移开新密码输入框-1
function blurNewPwd(obj) {
    if (Common.TrimSpace($(obj).val()) == "") {
        //$(obj).attr("type", "text");
        //$(obj).val("请输入新密码");
        //$(obj).css("color", "#888");
        $(obj).hide();
        $("#txtNewShowPwd").show();

    }
}

//点击新密码输入框-2******************
function clickNewPwd_2(obj) {
    //if (Common.TrimSpace($(obj).val()) == "确认新密码") {
    //    $(obj).val("");
    //    //$(obj).attr("type", "password");
    //}
    //$(obj).css("color", "#000");
}
//移开新密码输入框-2
function blurNewPwd_2(obj) {
    if (Common.TrimSpace($(obj).val()) == "") {
        //$(obj).attr("type", "text");
        //$(obj).val("确认新密码");
        //$(obj).css("color", "#888");
        $(obj).hide();
        $("#txtNewShowPwd2").show();

    }
}

//修改密码保存事件
function SavePassword() {
    var PwdOld = $("#txtOldPwd").val().replace(/\s/g, '');
    var PwdNew = $("#txtNewPwd").val().replace(/\s/g, '');
    var PwdNew2 = $("#txtNewPwd2").val().replace(/\s/g, '');

    if (PwdOld != "" && PwdOld != "请输入旧密码" && PwdNew != "" && PwdNew != "请输入新密码" && PwdNew2 != "" && PwdNew2 != "确认新密码") {
        //判断密码长度
        if (PwdNew.length >= 6 && PwdNew.length <= 16) {
            $.post("?action=SavePassWord&Rand=" + Math.random(), { PwdOld: PwdOld, PwdNew: PwdNew }, function (result) {
                if (result) {
                    result = eval("(" + result + ")");//JSON.parse
                    if (result.Success) {
                        alert("修改成功!");
                        //关闭弹窗
                        art.dialog({ id: "digUpdPwd" }).close();
                    } else {
                        alert(result.Message);

                    }
                } else {
                    alert("验证码校验失败，请检查后重试！");
                }
            });
        }
        else {
            $("#txtNewPwd").focus();
            $("#aTitlePwd").html("密码长度为6-16位。");
        }
    } else {
        alert("请输入旧密码和新密码！");
    }
}
//修改密码相关方法结束
//*****************************************************
//*****************************************************



//根据年级数字获取学段名称
function GetSSTAGEName(GradeID) {
    var SSTAGEName = "";
    switch (GradeID) {
        case 1:
        case 2:
        case 3:
        case 4:
        case 5:
        case 6:
            SSTAGEName = "小学";
            break;
        case 7:
        case 8:
        case 9:
            SSTAGEName = "初中";
            break;
        default:
            break;
    }
    return SSTAGEName;
}

//读取数据
function GetDataFun(funName, obj, callback) {
    var async_Sign = true;
    if (typeof callback == "function") {
        async_Sign = true;
    } else {
        async_Sign = false;
    }
    var obj = null;
    $.post("?action=" + funName + "&rand=" + Math.random(), obj, function (data) {
        data = EvalData(data);
        if (async_Sign) {
            callback(data);
        } else {
            obj = response;
        }
    });
    return obj;
}

//数据转换json
function EvalData(data) {
    return eval("(" + data + ")");
};

//旧密码输入事件
function oldPwdKeyDown() {
    event = event ? event : window.event;
    //判断输入的是空格
    if (event.keyCode == 32) {
        return "";
    }
    else {
        //return true;
        return String.fromCharCode(event.keyCode);
    }
}


////弹窗用法
//var tempHtmlUpdate = $("#diaUpdPho")[0];
//art.dialog({
//    content: tempHtmlUpdate,
//    title: '修改手机',
//    lock: true,
//    ok: function () {
//        //var info = Current.getUpdateValue();
//        //if (info != null || info != undefined) {

//        //}
//        //else {
//        //    return false;
//        //}
//        //return false;
//    },
//    cancelVal: '取消',
//    cancel: true //为true等价于function(){}
//});


//Common.showMsg = function (type, msg) {
//    art.dialog({
//        id: 'msg',
//        icon: type,
//        content: msg
//    }).time(3);
//}
//////关闭弹窗 ID=弹窗ID
//Common.closeDialog = function (ID) {
//    art.dialog({ id: ID }).close();
//    //art.dialog({ id: ID }).time(3);
//}